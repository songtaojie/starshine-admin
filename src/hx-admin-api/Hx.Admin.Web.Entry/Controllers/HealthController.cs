// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042


using Microsoft.AspNetCore.Authorization;

namespace Hx.Admin.Web.Entry.Controllers;

[AllowAnonymous]
public class HealthController: AdminControllerBase
{
    public HealthController()
    { 
    }

    [HttpGet]
    public string Get()
    {
        return "ok";
    }
}
