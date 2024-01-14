// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Notice;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.OnlineUser;

namespace Hx.Admin.Web.Entry.Controllers;


public class SysOnlineUserController : AdminControllerBase
{
    private readonly ISysOnlineUserService _service;
    public SysOnlineUserController(ISysOnlineUserService service)
    {
        _service = service;
    }

    /// <summary>
    /// <see cref="ISysOnlineUserService.GetPage(PageOnlineUserInput)"/>
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<SysOnlineUser>> GetPage([FromQuery] PageOnlineUserInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// <see cref="ISysOnlineUserService.ForceOffline(SysOnlineUser)"/>
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task ForceOffline(SysOnlineUser user)
    {
        await _service.ForceOffline(user);
    }
}