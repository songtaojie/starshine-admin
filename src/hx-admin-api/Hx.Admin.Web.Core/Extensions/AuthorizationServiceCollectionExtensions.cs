// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;

namespace Hx.Admin.Web.Core.Extensions;

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

        // 1.使用基于角色的授权，仅仅在api上配置，第一步：[Authorize(Roles = "admin")]，第二步：配置统一认证服务，第三步：开启中间件
        services.AddAuthorization(c =>
        {
            c.AddPolicy(ConstInfo.SuperAdminPolicy, policy => policy.RequireRole(ConstInfo.SuperAdminPolicy, ConstInfo.ClientPolicy));
            c.AddPolicy(ConstInfo.AdminPolicy, policy => policy.RequireRole(ConstInfo.AdminPolicy));
            c.AddPolicy(ConstInfo.ClientPolicy, policy => policy.RequireRole(ConstInfo.ClientPolicy));
        });
        // 第二种方式、自定义复杂的策略授权
        //读取配置文件
        JwtSettings jwtSetting = AppSettings.GetConfig<JwtSettings>("JwtSettings");
        SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSetting.SecretKey));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        // 角色与接口的权限要求参数
        var permissionRequirement = new RouteAuthorizationRequirement(
            ClaimTypes.Role,//基于角色的授权
            jwtSetting.Issuer,//发行人
            jwtSetting.Audience,//听众
            signingCredentials,//签名凭据
            expiration: TimeSpan.FromSeconds(60 * 60)//接口的过期时间
            );
        services.AddAuthorization()
            .AddOptions<AuthorizationOptions>()
            .Configure<IOptions<JWTSettingsOptions>>((options, jWTSettingsOptions) =>
            {

            });

        // 注入权限处理器
        services.AddScoped<IAuthorizationHandler, RouteAuthorizationHandler>();
        services.AddSingleton(permissionRequirement);
    }
}
