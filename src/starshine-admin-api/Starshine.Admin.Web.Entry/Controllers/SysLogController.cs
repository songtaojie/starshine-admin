// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.IService;
using Starshine.Admin.Models.ViewModels.Logs;
using Starshine.Admin.Serilog.Attributes;
using Starshine.Admin.IServices.Logs;
using Magicodes.ExporterAndImporter.Excel;

namespace Starshine.Admin.Web.Entry.Controllers;

/// <summary>
/// 系统日志
/// </summary>
public class SysLogController : AdminControllerBase
{
    private readonly ISysLogVisService _service;
    private readonly ISysLogOpService _sysLogOpService;
    private readonly ISysLogDiffService _sysLogDiffService;
    private readonly ISysLogExService _sysLogExService;

    /// <summary>
    /// 系统日志
    /// </summary>
    /// <param name="service"></param>
    /// <param name="sysLogOpService"></param>
    /// <param name="sysLogDiffService"></param>
    /// <param name="sysLogExService"></param>
    public SysLogController(ISysLogVisService service, 
        ISysLogOpService sysLogOpService,
        ISysLogDiffService sysLogDiffService,
        ISysLogExService sysLogExService)
    {
        _service = service;
        _sysLogOpService = sysLogOpService;
        _sysLogDiffService = sysLogDiffService;
        _sysLogExService = sysLogExService;
    }

    /// <summary>
    /// 获取访问日志分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet, SkipLogging]
    public async Task<PagedListResult<SysLogVisOutput>> GetVisLogPage([FromQuery] PageLogInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// 清空访问日志
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> ClearVisLog()
    {
        return await _service.Clear();
    }

    /// <summary>
    /// 获取操作日志分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet, SkipLogging]
    public async Task<PagedListResult<SysLogOpOutput>> GetOpLogPage([FromQuery] PageLogInput input)
    {
        return await _sysLogOpService.GetPage(input);
    }

    /// <summary>
    /// 清空操作日志
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> ClearOpLog()
    {
        return await _sysLogOpService.Clear();
    }

    /// <summary>
    /// 获取操作日志分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet, SkipLogging]
    public async Task<PagedListResult<SysLogDiffOutput>> GetDiffLogPage([FromQuery] PageLogInput input)
    {
        return await _sysLogDiffService.GetPage(input);
    }

    /// <summary>
    /// 清空差异化日志
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> ClearDiffLog()
    {
        return await _sysLogDiffService.Clear();
    }

    /// <summary>
    /// 获取操作日志分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet, SkipLogging]
    public async Task<PagedListResult<SysLogExOutput>> GetExLogPage([FromQuery] PageLogInput input)
    {
        return await _sysLogExService.GetPage(input);
    }

    /// <summary>
    /// 清空异常日志
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> ClearExLog()
    {
        return await _sysLogExService.Clear();
    }

    /// <summary>
    /// 导出异常日志 🔖
    /// </summary>
    /// <returns></returns>
    [HttpGet,NonUnify]
    public async Task<IActionResult> ExportExLog([FromQuery] LogInput input)
    {
        var logExList = await _sysLogExService.GetExportListAsync(input);

        IExcelExporter excelExporter = new ExcelExporter();
        var res = await excelExporter.ExportAsByteArray(logExList);
        return new FileStreamResult(new MemoryStream(res), "application/octet-stream") { FileDownloadName = $"异常日志_{DateTime.Now:yyyyMMddHHmm}.xlsx" };
    }
}