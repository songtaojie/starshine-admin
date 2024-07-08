// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Authorization;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// JWT 授权服务拓展类
/// </summary>
public static class AuthorizationServiceCollectionExtensions
{
    /// <summary>
    /// 添加权限
    /// </summary>
    /// <param name="services"></param>
    public static void AddAuthoriationSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        // 1、使用基于角色的授权，仅仅在api上配置，第一步：[Authorize(Roles = "admin")]，第二步：配置统一认证服务，第三步：开启中间件
        // 2、第二种方式、自定义复杂的策略授权
        ////读取配置文件
        //JwtSettings jwtSetting = AppSettings.GetConfig<JwtSettings>("JwtSettings");
        //SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSetting.SecretKey));
        //var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
       
        services.Configure<AuthorizationOptions,IOptions<JWTSettingsOptions>>((options, jWTSettingsOptions) =>
            {
                options.AddPolicy(CommonConst.SuperAdminPolicy, policy => policy.RequireRole(CommonConst.SuperAdminPolicy, CommonConst.ClientPolicy));
                options.AddPolicy(CommonConst.AdminPolicy, policy => policy.RequireRole(CommonConst.AdminPolicy));
                options.AddPolicy(CommonConst.ClientPolicy, policy => policy.RequireRole(CommonConst.ClientPolicy));
                // 角色与接口的权限要求参数
                var jWTSettings = jWTSettingsOptions.Value;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTSettings.IssuerSigningKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var minute = jWTSettings?.ExpiredTime ?? 20;
                var permissionRequirement = new RouteAuthorizationRequirement(
                    ClaimTypes.Role,//基于角色的授权
                    jWTSettings.ValidIssuer,//发行人
                    jWTSettings.ValidAudience,//听众
                    credentials,//签名凭据
                    expiration: TimeSpan.FromMinutes(minute)//接口的过期时间
                    );

                options.AddPolicy(CommonConst.PermissionPolicy,
                        policy => policy.Requirements.Add(permissionRequirement));
            }).AddAuthorization();

        // 注入权限处理器
        services.AddScoped<IAuthorizationHandler, RouteAuthorizationHandler>();
        //services.AddSingleton(permissionRequirement);
    }
}
