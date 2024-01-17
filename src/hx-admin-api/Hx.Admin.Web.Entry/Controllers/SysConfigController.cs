// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Notice;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Config;

namespace Hx.Admin.Web.Entry.Controllers;

public class SysConfigController : AdminControllerBase
{
    private readonly ISysConfigService _service;
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