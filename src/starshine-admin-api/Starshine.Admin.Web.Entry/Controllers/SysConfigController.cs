// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.IService;
using Starshine.Admin.Models.ViewModels.Notice;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Config;

namespace Starshine.Admin.Web.Entry.Controllers;

/// <summary>
/// 系统配置
/// </summary>
public class SysConfigController : AdminControllerBase
{
    private readonly ISysConfigService _service;
    /// <summary>
    /// 系统配置
    /// </summary>
    /// <param name="service"></param>
    public SysConfigController(ISysConfigService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取参数配置分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageConfigOutput>> GetPage([FromQuery] PageConfigInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// 获取分组列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<string>> GetGroupList()
    {
        return await _service.GetGroupList();
    }
}