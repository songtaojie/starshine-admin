// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042


using Microsoft.AspNetCore.Authorization;

namespace Hx.Admin.Web.Entry.Controllers;

/// <summary>
/// 健康检查控制器
/// </summary>
[AllowAnonymous]
public class HealthController: AdminControllerBase
{
    private readonly ILogger _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 测试接口
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    [HttpGet]
    public string Get(int i)
    {
        _logger.LogInformation("测试接口"+i);
        return "ok";
    }
}
