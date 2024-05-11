// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Configuration;
using OnceMi.AspNetCore.OSS;

namespace Microsoft.Extensions.DependencyInjection;
/// <summary>
/// OSS服务注册
/// </summary>
public static class OSSServiceProviderCollectionExtensions
{
    public static void AddOSSServiceProvider(this IServiceCollection services, IConfiguration configuration)
    {
        var isEnable = configuration.GetValue("OSSProvider:IsEnable", false);
        if (!isEnable) return;
        services.AddOSSService(configuration["OSSProvider:Provider"], "OSSProvider");
    }
}