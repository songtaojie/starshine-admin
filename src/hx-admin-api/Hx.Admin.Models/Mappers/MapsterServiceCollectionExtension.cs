// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Models;
using Mapster;


namespace Microsoft.Extensions.DependencyInjection;
/// <summary>
/// </summary>
public static class MapsterServiceCollectionExtension
{
    /// <summary>
    /// 获取当前 HttpContext 上下文
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMapsterSettings(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(SysMenuMapper).Assembly);
        return services;
    }
}