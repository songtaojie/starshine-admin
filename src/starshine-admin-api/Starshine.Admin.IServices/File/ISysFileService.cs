using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels;
using Starshine.Admin.Models.ViewModels.File;
using Starshine.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统文件服务
/// </summary>
public interface ISysFileService : IBaseService<SysFile>, IScopedDependency
{
    /// <summary>
    /// 获取文件分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<PageFileOutput>> GetPage(PageFileInput input);

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    Task<FileOutput> UploadFile(IFormFile file, string? path);

    /// <summary>
    /// 上传多文件
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    Task<List<FileOutput>> UploadFiles(List<IFormFile> files);

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteFile(BaseIdParam input);

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<FileOutput> UploadAvatar(IFormFile file);

    /// <summary>
    /// 上传电子签名
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<FileOutput> UploadSignature(IFormFile file);

    /// <summary>
    /// 获取下载文件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<DownloadFileOutput> GetDownloadFile(FileInput input);
}