// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Microsoft.AspNetCore.Mvc;

namespace Hx.Admin.Web.Entry.Controllers;

public class SysAuthController: AdminControllerBase
{
    private readonly ISysAuthService _sysAuthService;
    public SysAuthController(ISysAuthService sysAuthService)
    {
        _sysAuthService = sysAuthService;
    }
    [HttpGet]
    public string Get()
    {
        return "ok";
    }
    [HttpGet]
    public string GetList()
    {
        return "ok";
    }

    [HttpGet]
    public string GetPageAsync()
    {
        return "ok";
    }

    /// <summary>
    /// <see cref="ISysAuthService.GetLoginConfig"/>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName("SystemConfig")]
    public dynamic GetSystemConfig()
    {
        return _sysAuthService.GetSystemConfig();
    }
}
