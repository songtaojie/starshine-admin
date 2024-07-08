// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Admin.Core;
/// <summary>
/// Jwt 配置
/// </summary>
public sealed class JWTSettingsOptions:IConfigureOptions<JWTSettingsOptions>
{
    /// <summary>
    /// 验证签发方密钥
    /// </summary>
    public bool? ValidateIssuerSigningKey { get; set; }

    /// <summary>
    /// 签发方密钥
    /// </summary>
    public string IssuerSigningKey { get; set; }

    /// <summary>
    /// 验证签发方
    /// </summary>
    public bool? ValidateIssuer { get; set; }

    /// <summary>
    /// 签发方
    /// </summary>
    public string ValidIssuer { get; set; }

    /// <summary>
    /// 验证签收方
    /// </summary>
    public bool? ValidateAudience { get; set; }

    /// <summary>
    /// 签收方
    /// </summary>
    public string ValidAudience { get; set; }

    /// <summary>
    /// 验证生存期
    /// </summary>
    public bool? ValidateLifetime { get; set; }

    /// <summary>
    /// 过期时间容错值，解决服务器端时间不同步问题（秒）
    /// </summary>
    public long? ClockSkew { get; set; }

    /// <summary>
    /// 过期时间（分钟）
    /// </summary>
    public long? ExpiredTime { get; set; }

    /// <summary>
    /// 加密算法
    /// </summary>
    public string Algorithm { get; set; }

    public void Configure(JWTSettingsOptions options)
    {
        options.ValidateIssuerSigningKey ??= true;
        if (options.ValidateIssuerSigningKey == true)
        {
            options.IssuerSigningKey ??= "gbxrnrAC86I245e5scPR5qpYT8duYx/WpJJa3z+c1lx+mrk9u32uaTECAwEAAQ==";
        }
        options.ValidateIssuer ??= true;
        if (options.ValidateIssuer == true)
        {
            options.ValidIssuer ??= "hxadmin";
        }
        options.ValidateAudience ??= true;
        if (options.ValidateAudience == true)
        {
            options.ValidAudience ??= "powerby hxadmin";
        }
        options.ValidateLifetime ??= true;
        if (options.ValidateLifetime == true)
        {
            options.ClockSkew ??= 10;
        }
        options.ExpiredTime ??= 20;
        options.Algorithm ??= SecurityAlgorithms.HmacSha256;
    }
}
