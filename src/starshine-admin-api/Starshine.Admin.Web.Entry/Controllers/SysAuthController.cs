// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.IService;
using Starshine.Admin.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Starshine.Admin.Web.Entry.Controllers;

/// <summary>
/// 系统授权
/// </summary>
public class SysAuthController: AdminControllerBase
{
    private readonly ISysAuthService _sysAuthService;
    /// <summary>
    /// 系统授权
    /// </summary>
    /// <param name="sysAuthService"></param>
    public SysAuthController(ISysAuthService sysAuthService)
    {
        _sysAuthService = sysAuthService;
    }

    /// <summary>
    /// 系统登录
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<LoginOutput> Login(LoginInput input)
    {
        return await _sysAuthService.Login(input);
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public dynamic GetCaptcha()
    {
        return  _sysAuthService.GetCaptcha();
    }

    /// <summary>
    /// 获取登录配置
    /// </summary>
    /// <returns></returns>
    [HttpGet,ActionName("SystemConfig")]
    [AllowAnonymous]
    public async Task<dynamic> GetSystemConfig()
    {
        return await _sysAuthService.GetSystemConfig();
    }

    /// <summary>
    /// 退出系统
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public void Logout()
    {
        _sysAuthService.Logout();
    }

    /// <summary>
    /// 获取刷新token
    /// </summary>
    /// <returns></returns>
    [ActionName("RefreshToken"), HttpGet]
    public async Task<string> GetRefreshToken(string accessToken)
    {
        return await _sysAuthService.GetRefreshToken(accessToken);
    }

    /// <summary>
    /// 获取登录账号信息
    /// </summary>
    /// <returns></returns>
    [ActionName("UserInfo"), HttpGet]
    public async Task<LoginUserOutput> GetUserInfo()
    {
        return await _sysAuthService.GetUserInfo();
    }
}
