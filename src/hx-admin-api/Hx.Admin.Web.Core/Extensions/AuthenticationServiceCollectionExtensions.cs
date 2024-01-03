// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
using Hx.Admin.Web.Core.Authentication;
using Hx.Cache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Nest;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;
public static class AuthenticationServiceCollectionExtensions
{
    /// <summary>
    /// 添加jwt的授权认证方案
    /// </summary>
    /// <param name="services"></param>
    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        // 开启Bearer认证
        services.Configure<JwtBearerOptions, IOptions<JWTSettingsOptions>>((options, jWTSettingsOptions) =>
        {
            options.TokenValidationParameters = JWTEncryption.CreateTokenValidationParameters(jWTSettingsOptions.Value);
            options.Events = new JwtBearerEvents()
            {
                OnMessageReceived = context => MessageReceived(context),
                OnAuthenticationFailed = context => AuthenticationFailed(context)
            };
        });
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = nameof(JwtAuthenticationHandler);
            options.DefaultForbidScheme = nameof(JwtAuthenticationHandler);
        })
        .AddJwtBearer()
        .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>(nameof(JwtAuthenticationHandler), o => { });
    }

    private static async Task MessageReceived(MessageReceivedContext context)
    {
        //signalr token获取
        var accessToken = context.Request.Query["access_token"];
        // If the request is for our hub...
        var path = context.HttpContext.Request.Path;
        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
        {
            // Read the token out of the query string
            context.Token = accessToken;
        }
        await Task.CompletedTask;
    }

    /// <summary>
    /// 认证失败时
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static async Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        var jwtSettingsOptions =  context.HttpContext.RequestServices.GetService<IOptions<JWTSettingsOptions>>();
        var jwtSettings = jwtSettingsOptions.Value;
        string token = context.Request.Headers[HeaderNames.Authorization];
        if (string.IsNullOrEmpty(token)) token = context.Request.Query["access_token"];
        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = token.Substring("Bearer ".Length).Trim();
        }
        var jwtToken = JWTEncryption.ReadJwtToken(token);
        bool existCacheToken = false;
        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
        {
            //var cache = context.HttpContext.RequestServices?.GetService<ICache>();
            //if (cache != null)
            //{
            //    var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            //    var cacheToken = cache.Get(string.Format(CacheConst.AuthToken_Key, userId));
            //    if (!string.IsNullOrEmpty(cacheToken))
            //    {
            //        JWTEncryption.Validate(cacheToken);
            //        TokenValidationParameters tokenValidationParameters = context.Options.TokenValidationParameters.Clone();
            //        var securityTokenValidator = new MyJwtSecurityTokenHandler(redisCache);
            //        var principal = securityTokenValidator.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            //        context.Principal = principal;
            //        context.Success();
            //        existCacheToken = true;
            //    }
            //}
            context.Response.Headers.Add("Token-Expired", "true");
        }
        if (!existCacheToken)
        {
            if (jwtToken.Issuer != jwtSettings.ValidIssuer)
            {
                context.Response.Headers.Add("Token-Error-Issuer", "true");
            }

            if (jwtToken.Audiences.FirstOrDefault() != jwtSettings.ValidAudience)
            {
                context.Response.Headers.Add("Token-Error-Audience", "true");
            }
        }
        await Task.CompletedTask;
    }
}
