﻿// MIT License
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

namespace Microsoft.AspNetCore.Authorization.Infrastructure;
/// <summary>
/// 路由相关的授权策略
/// </summary>
public class RouteAuthorizationRequirement : IAuthorizationRequirement
{

    /// <summary>
    /// 认证授权类型
    /// </summary>
    public string ClaimType { internal get; set; }
    /// <summary>
    /// 发行人
    /// </summary>
    public string Issuer { get; set; }
    /// <summary>
    /// 订阅人
    /// </summary>
    public string Audience { get; set; }
    /// <summary>
    /// 过期时间
    /// </summary>
    public TimeSpan Expiration { get; set; }
    /// <summary>
    /// 签名验证
    /// </summary>
    public SigningCredentials SigningCredentials { get; set; }


    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="claimType">声明类型</param>
    /// <param name="issuer">发行人</param>
    /// <param name="audience">订阅人</param>
    /// <param name="signingCredentials">签名验证实体</param>
    /// <param name="expiration">过期时间</param>
    public RouteAuthorizationRequirement(string claimType, string issuer, string audience, SigningCredentials signingCredentials, TimeSpan expiration)
    {
        ClaimType = claimType;
        Issuer = issuer;
        Audience = audience;
        Expiration = expiration;
        SigningCredentials = signingCredentials;
    }
}
