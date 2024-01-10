// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Auth;
using Hx.Sdk.Core;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Hx.Admin.Web.Entry.Controllers;

public class SysAuthController: AdminControllerBase
{
    private readonly ISysAuthService _sysAuthService;
    public SysAuthController(ISysAuthService sysAuthService)
    {
        _sysAuthService = sysAuthService;
    }

    /// <summary>
    /// <see cref="ISysAuthService.Login"/>
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<LoginOutput> Login(LoginInput input)
    {
        return await _sysAuthService.Login(input);
    }

    /// <summary>
    /// <see cref="ISysAuthService.GetCaptcha"/>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public dynamic GetCaptcha()
    {
        return  _sysAuthService.GetCaptcha();
    }

    /// <summary>
    /// <see cref="ISysAuthService.GetLoginConfig"/>
    /// </summary>
    /// <returns></returns>
    [HttpGet,ActionName("SystemConfig")]
    public async Task<dynamic> GetSystemConfig()
    {
        return await _sysAuthService.GetSystemConfig();
    }

    /// <summary>
    /// <see cref="ISysAuthService.Logout"/>
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public void Logout()
    {
        _sysAuthService.Logout();
    }

    /// <summary>
    /// <see cref="ISysAuthService.GetRefreshToken"/>
    /// </summary>
    /// <returns></returns>
    [ActionName("RefreshToken"), HttpGet]
    public async Task<string> GetRefreshToken(string accessToken)
    {
        return await _sysAuthService.GetRefreshToken(accessToken);
    }

    /// <summary>
    /// <see cref="ISysAuthService.GetUserInfo"/>
    /// </summary>
    /// <returns></returns>
    [ActionName("UserInfo"), HttpGet]
    public async Task<LoginUserOutput> GetUserInfo()
    {
        return await _sysAuthService.GetUserInfo();
    }
}
