using Hx.Admin.Core;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.File;
using Hx.Common;
using Hx.Common.DependencyInjection;
using Hx.Sqlsugar;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OnceMi.AspNetCore.OSS;

namespace Hx.Admin.IService;

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
    Task<PagedListResult<SysFile>> GetPage(PageFileInput input);

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
    Task DeleteFile(DeleteFileInput input);

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
}