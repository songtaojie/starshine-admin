// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using COSXML.Network;
using Hx.Sdk.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hx.Admin.Web.Core.Authentication;
/// <summary>
/// jwt身份验证处理程序
/// </summary>
public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="encoder"></param>
    /// <param name="clock"></param>
    public JwtAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 如果有问题的身份验证方案将身份验证交互作为其请求流的一部分来处理，则重写此方法以处理401个挑战问题。
    /// (比如添加一个响应标头，或者将登录页面或外部登录位置的401结果更改为302。)
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = StatusCodes.Status200OK;
        base.Response.Headers.Append(HeaderNames.WWWAuthenticate, nameof(JwtAuthenticationHandler));
        JsonSerializerOptions setting = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false,
        };
        await Response.WriteAsync(JsonSerializer.Serialize(new RESTfulResult<object>
        {
            StatusCode = StatusCodes.Status401Unauthorized,
            Succeeded = false,
            Data = null,
            Message = "401 Unauthorized",
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        }, setting));
    }

    /// <summary>
    /// 处理重定向
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        base.Response.ContentType = "application/json";
        base.Response.StatusCode = StatusCodes.Status200OK;
        base.Response.Headers.Append(HeaderNames.WWWAuthenticate, nameof(JwtAuthenticationHandler));
        JsonSerializerOptions setting = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false,
        };
        await Response.WriteAsync(JsonSerializer.Serialize(new RESTfulResult<object>
        {
            StatusCode = StatusCodes.Status403Forbidden,
            Succeeded = false,
            Data = null,
            Message = "403 Forbidden",
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        }, setting));
    }
}

