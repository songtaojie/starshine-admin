using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OnceMi.AspNetCore.OSS;
using System.Text.RegularExpressions;
using Yitter.IdGenerator;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统文件服务
/// </summary>
public class SysFileService : BaseService<SysFile>, ISysFileService
{
    private readonly UserManager _userManager;
    private readonly OSSProviderOptions _OSSProviderOptions;
    private readonly UploadOptions _uploadOptions;
    private readonly ICommonService _commonService;
    private readonly IOSSService _OSSService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public SysFileService(UserManager userManager,
        ISqlSugarRepository<SysFile> sysFileRep,
        IOptions<OSSProviderOptions> oSSProviderOptions,
        IOptions<UploadOptions> uploadOptions,
        ICommonService commonService,
        IOSSServiceFactory ossServiceFactory,
        IWebHostEnvironment webHostEnvironment) :base(sysFileRep)
    {
        _userManager = userManager;
        _OSSProviderOptions = oSSProviderOptions.Value;
        _uploadOptions = uploadOptions.Value;
        _commonService = commonService;
        _webHostEnvironment = webHostEnvironment;
        if (_OSSProviderOptions.IsEnable)
            _OSSService = ossServiceFactory.Create(Enum.GetName(_OSSProviderOptions.Provider));
    }

    /// <summary>
    /// 获取文件分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedListResult<SysFile>> GetPage(PageFileInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.FileName), u => u.FileName!.Contains(input.FileName.Trim()))
            .WhereIF(input.StartTime.HasValue, u => u.CreateTime >= input.StartTime )
            .WhereIF(input.EndTime.HasValue, u => u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<FileOutput> UploadFile(IFormFile file,  string? path)
    {
        var sysFile = await HandleUploadFile(file, path);
        return new FileOutput
        {
            Id = sysFile.Id,
            Url = sysFile.Url,  // string.IsNullOrWhiteSpace(sysFile.Url) ? _commonService.GetFileUrl(sysFile) : sysFile.Url,
            SizeKb = sysFile.SizeKb,
            Suffix = sysFile.Suffix,
            FilePath = sysFile.FilePath,
            FileName = sysFile.FileName
        };
    }

    /// <summary>
    /// 上传多文件
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    public async Task<List<FileOutput>> UploadFiles(List<IFormFile> files)
    {
        var filelist = new List<FileOutput>();
        foreach (var file in files)
        {
            filelist.Add(await UploadFile(file, ""));
        }
        return filelist;
    }

    ///// <summary>
    ///// 下载文件(文件流)
    ///// </summary>
    ///// <param name="input"></param>
    ///// <returns></returns>
    //public async Task<IActionResult> DownloadFile(FileInput input)
    //{
    //    var file = await GetFile(input);
    //    var fileName = HttpUtility.UrlEncode(file.FileName, Encoding.GetEncoding("UTF-8"));
    //    if (_OSSProviderOptions.IsEnable)
    //    {
    //        var filePath = string.Concat(file.FilePath, "/", input.Id.ToString() + file.Suffix);
    //        var stream = await (await _OSSService.PresignedGetObjectAsync(file.BucketName.ToString(), filePath, 5)).GetAsStreamAsync();
    //        return new FileStreamResult(stream.Stream, "application/octet-stream") { FileDownloadName = fileName + file.Suffix };
    //    }
    //    else
    //    {
    //        var filePath = Path.Combine(file.FilePath, input.Id.ToString() + file.Suffix);
    //        var path = Path.Combine(App.WebHostEnvironment.WebRootPath, filePath);
    //        return new FileStreamResult(new FileStream(path, FileMode.Open), "application/octet-stream") { FileDownloadName = fileName + file.Suffix };
    //    }
    //}

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task DeleteFile(DeleteFileInput input)
    {
        var file = await FirstOrDefaultAsync(u => u.Id == input.Id);
        if (file != null)
        {
            await DeleteAsync(file);

            if (_OSSProviderOptions.IsEnable)
            {
                await _OSSService.RemoveObjectAsync(file.BucketName?.ToString(), string.Concat(file.FilePath, "/", $"{input.Id}{file.Suffix}"));
            }
            else
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, file.FilePath, input.Id.ToString() + file.Suffix);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file">文件</param>
    /// <param name="savePath">路径</param>
    /// <returns></returns>
    private async Task<SysFile> HandleUploadFile(IFormFile file, string savePath)
    {
        if (file == null) 
            throw new UserFriendlyException("上传文件不能为空");

        var path = savePath;
        if (string.IsNullOrWhiteSpace(savePath))
        {
            path = _uploadOptions.Path;
            var reg = new Regex(@"(\{.+?})");
            var match = reg.Matches(path);
            match.ToList().ForEach(a =>
            {
                var str = DateTime.Now.ToString(a.ToString().Substring(1, a.Length - 2)); // 每天一个目录
                path = path.Replace(a.ToString(), str);
            });
        }

        if (!_uploadOptions.ContentType.Contains(file.ContentType))
            throw new UserFriendlyException("不支持的文件类型");

        var sizeKb = (long)(file.Length / 1024.0); // 大小KB
        if (sizeKb > _uploadOptions.MaxSize)
            throw new UserFriendlyException($"已超过最大文件限制({_uploadOptions.MaxSize}KB)");

        var suffix = Path.GetExtension(file.FileName).ToLower(); // 后缀
        if (string.IsNullOrWhiteSpace(suffix))
        {
            var contentTypeProvider = FileContentTypeUtil.GetFileExtensionContentTypeProvider();
            suffix = contentTypeProvider.Mappings.FirstOrDefault(u => u.Value == file.ContentType).Key;
        }
        if (string.IsNullOrWhiteSpace(suffix))
            throw new UserFriendlyException("不支持的文件类型");

        var newFile = new SysFile
        {
            Id = YitIdHelper.NextId(),
            // BucketName = _OSSProviderOptions.IsEnable ? _OSSProviderOptions.Provider.ToString() : "Local",
            // 阿里云对bucket名称有要求，1.只能包括小写字母，数字，短横线（-）2.必须以小写字母或者数字开头  3.长度必须在3-63字节之间
            // 无法使用Provider
            BucketName = _OSSProviderOptions.IsEnable ? _OSSProviderOptions.Bucket : "Local",
            FileName = Path.GetFileNameWithoutExtension(file.FileName),
            Suffix = suffix,
            SizeKb = sizeKb.ToString(),
            FilePath = path,
        };

        var finalName = newFile.Id + suffix; // 文件最终名称
        if (_OSSProviderOptions.IsEnable)
        {
            newFile.Provider = Enum.GetName(_OSSProviderOptions.Provider);
            var filePath = string.Concat(path, "/", finalName);
            await _OSSService.PutObjectAsync(newFile.BucketName, filePath, file.OpenReadStream());
            //  http://<你的bucket名字>.oss.aliyuncs.com/<你的object名字>
            //  生成外链地址 方便前端预览
            switch (_OSSProviderOptions.Provider)
            {
                case OSSProvider.Aliyun:
                    newFile.Url = $"{(_OSSProviderOptions.IsEnableHttps ? "https" : "http")}://{newFile.BucketName}.{_OSSProviderOptions.Endpoint}/{filePath}";
                    break;

                case OSSProvider.Minio:
                    // 获取Minio文件的下载或者预览地址
                    newFile.Url = await GetMinioPreviewFileUrl(newFile.BucketName, filePath); ;
                    break;
            }
        }
        else
        {
            newFile.Provider = ""; // 本地存储 Provider 显示为空
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var realFile = Path.Combine(filePath, finalName);
            //IDetector detector;
            using (var stream = File.Create(realFile))
            {
                await file.CopyToAsync(stream);
                //detector = stream.DetectFiletype();
            }
            //var realExt = detector.Extension; // 真实扩展名
            //// 二次校验扩展名
            //if (!string.Equals(realExt, suffix.Replace(".", ""), StringComparison.OrdinalIgnoreCase))
            //{
            //    var delFilePath = Path.Combine(App.WebHostEnvironment.WebRootPath, realFile);
            //    if (File.Exists(delFilePath))
            //        File.Delete(delFilePath);
            //    throw Oops.Oh(ErrorCodeEnum.D8001);
            //}

            // 生成外链
            newFile.Url = _commonService.GetFileUrl(newFile);
        }
        await InsertAsync(newFile);
        return newFile;
    }

    /// <summary>
    /// 获取Minio文件的下载或者预览地址
    /// </summary>
    /// <param name="bucketName">桶名</param>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    private async Task<string> GetMinioPreviewFileUrl(string bucketName, string fileName)
    {
        return await _OSSService.PresignedGetObjectAsync(bucketName, fileName, 7);
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<FileOutput> UploadAvatar([Required] IFormFile file)
    {
        var sysUserRep = _rep.Change<SysUser>();
        var user = await sysUserRep.FirstOrDefaultAsync(u => u.Id == _userManager.UserId);
        // 删除当前用户已有头像
        if (!string.IsNullOrWhiteSpace(user.Avatar))
        {
            var fileId = Path.GetFileNameWithoutExtension(user.Avatar);
            await DeleteFile(new DeleteFileInput { Id = long.Parse(fileId) });
        }

        var res = await UploadFile(file, "Avatar");
        user.Avatar = res.Url;
        await sysUserRep.Context.Updateable<SysUser>(user).UpdateColumns(u => new { u.Avatar }).ExecuteCommandAsync();
        return res;
    }

    /// <summary>
    /// 上传电子签名
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<FileOutput> UploadSignature([Required] IFormFile file)
    {
        var sysUserRep = _rep.Change<SysUser>();
        var user = await sysUserRep.FirstOrDefaultAsync(u => u.Id == _userManager.UserId);
        // 删除当前用户已有电子签名
        if (!string.IsNullOrWhiteSpace(user.Signature) && user.Signature.EndsWith(".png"))
        {
            var fileId = Path.GetFileNameWithoutExtension(user.Signature);
            await DeleteFile(new DeleteFileInput { Id = long.Parse(fileId) });
        }

        var res = await UploadFile(file, "Signature");
        user.Signature = res.Url;
        await sysUserRep.Context.Updateable<SysUser>(user).UpdateColumns(u => new { u.Signature }).ExecuteCommandAsync();
        return res;
    }
}