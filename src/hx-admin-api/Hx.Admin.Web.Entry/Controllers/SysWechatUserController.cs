// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Menu;
using Hx.Admin.Models.ViewModels.User;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Wechat;

namespace Hx.Admin.Web.Entry.Controllers;

/// <summary>
/// 微信用户
/// </summary>
public class SysWechatUserController : AdminControllerBase
{
    private readonly ISysWechatUserService _service;
    /// <summary>
    /// 微信用户
    /// </summary>
    /// <param name="service"></param>
    public SysWechatUserController(ISysWechatUserService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取用户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageSysWechatUserOutput>> GetPage([FromQuery] PageWechatUserInput input)
    {
        return await _service.GetPage(input);
    }

}