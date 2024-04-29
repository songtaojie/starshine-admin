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
    private readonly ILogger _logger;
    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get(int i)
    {
        _logger.LogInformation("测试接口"+i);
        return "ok";
    }
}
