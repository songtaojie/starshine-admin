// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.IService;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels;
using Starshine.Admin.Models.ViewModels.File;
using System.IO;

namespace Starshine.Admin.Web.Entry.Controllers;

/// <summary>
/// 文件服务
/// </summary>
public class SysFileController : AdminControllerBase
{
    private readonly ISysFileService _service;
    /// <summary>
    /// 文件服务
    /// </summary>
    /// <param name="service"></param>
    public SysFileController(ISysFileService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取文件分页列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageFileOutput>> GetPage([FromQuery] PageFileInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file">上传的文件</param>
    /// <param name="path">文件保存的路径</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<FileOutput> UploadFile(IFormFile file, string path)
    {
        return await _service.UploadFile(file,path);
    }

    /// <summary>
    /// 上传多文件
    /// </summary>
    /// <param name="files">上传的文件</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<FileOutput>> UploadFiles(List<IFormFile> files)
    {
        return await _service.UploadFiles(files);
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task Delete(BaseIdParam input)
    {
        await _service.DeleteFile(input);
    }

    /// <summary>
    /// 获取下载文件
    /// </summary>
    /// <param name="input">获取文件信息</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetDownloadFile(FileInput input)
    {
        var file = await _service.GetDownloadFile(input);
        return new FileStreamResult(file.Stream, "application/octet-stream") { FileDownloadName = file.FileName };
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file">上传的文件</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<FileOutput> UploadAvatar(IFormFile file)
    {
        return await _service.UploadAvatar(file);
    }

    /// <summary>
    /// 上传电子签名
    /// </summary>
    /// <param name="file">上传的文件</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<FileOutput> UploadSignature(IFormFile file)
    {
        return await _service.UploadSignature(file);
    }
}