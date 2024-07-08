// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Starshine.JsonSerialization;

namespace Microsoft.Extensions.DependencyInjection;
public static class AdminCoreServiceCollectionExtensions
{
    public static void AddAdminCoreService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAdminOptions();
        services.AddMapsterSettings();
        services.AddControllersWithViews()
            .AddMvcOptions(options =>
            {
                options.Conventions.Add(new WebApiApplicationModelConvention());
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new SystemTextJsonDateTimeJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new SystemTextJsonNullableDateTimeJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new SystemTextJsonLongToStringJsonConverter());
            });
        // 雪花Id
        services.AddYitterIdGenerater();
        services.AddJwtAuthentication();
        services.AddAuthoriationSetup();
        services.AddCache();
        
        // 配置Nginx转发获取客户端真实IP
        // 注1：如果负载均衡不是在本机通过 Loopback 地址转发请求的，一定要加上options.KnownNetworks.Clear()和options.KnownProxies.Clear()
        // 注2：如果设置环境变量 ASPNETCORE_FORWARDEDHEADERS_ENABLED 为 True，则不需要下面的配置代码
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
        // 限流服务
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        // 即时通讯
        services.AddSignalR();
        // logo显示
        services.AddLogoDisplay();
        // 验证码
        services.AddLazyCaptcha(configuration);
        // OSS服务注册
        services.AddOSSServiceProvider(configuration);

        // 电子邮件
        services.AddFluentEmail(configuration);
        services.AddSqlSugarSetup();
        services.AddQuartzService(configuration);
    }


}
