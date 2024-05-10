﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Org;
using Hx.Admin.Models.ViewModels.Pos;
using Hx.Admin.Models.ViewModels;
using Hx.Admin.Models.ViewModels.Logs;
using Hx.Admin.Models;
using Hx.Admin.Serilog.Attributes;

namespace Hx.Admin.Web.Entry.Controllers;

/// <summary>
/// 系统日志
/// </summary>
public class SysLogController : AdminControllerBase
{
    private readonly ISysLogVisService _service;
    private readonly ISysLogOpService _sysLogOpService;

    /// <summary>
    /// 系统日志
    /// </summary>
    /// <param name="service"></param>
    /// <param name="sysLogOpService"></param>
    public SysLogController(ISysLogVisService service, 
        ISysLogOpService sysLogOpService)
    {
        _service = service;
        _sysLogOpService = sysLogOpService;
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
}