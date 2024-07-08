// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Options;
using SKIT.FlurlHttpClient.Wechat.Api;
using Starshine.DependencyInjection;

namespace Starshine.Admin.Core.Service;

/// <summary>
/// 微信API客户端
/// </summary>
[ExposeServices(Pattern = DependencyInjectionPattern.Self)]
public partial class WechatApiClientManager : ISingletonDependency
{
    public readonly WechatOptions _wechatOptions;

    public WechatApiClientManager(IOptions<WechatOptions> wechatOptions)
    {
        _wechatOptions = wechatOptions.Value;
    }

    /// <summary>
    /// 微信公众号
    /// </summary>
    /// <returns></returns>
    public WechatApiClient CreateWechatClient()
    {
        if (string.IsNullOrEmpty(_wechatOptions.WechatAppId) || string.IsNullOrEmpty(_wechatOptions.WechatAppSecret))
            throw new UserFriendlyException("微信公众号配置错误");

        var wechatApiClient = new WechatApiClient(new WechatApiClientOptions()
        {
            AppId = _wechatOptions.WechatAppId,
            AppSecret = _wechatOptions.WechatAppSecret,
        });

        //wechatApiClient.Configure(settings =>
        //{
        //    settings.JsonSerializer = new SKIT.FlurlHttpClient.FlurlSystemTextJsonSerializer();
        //});

        return wechatApiClient;
    }

    /// <summary>
    /// 微信小程序
    /// </summary>
    /// <returns></returns>
    public WechatApiClient CreateWxOpenClient()
    {
        if (string.IsNullOrEmpty(_wechatOptions.WxOpenAppId) || string.IsNullOrEmpty(_wechatOptions.WxOpenAppSecret))
            throw new UserFriendlyException("微信小程序配置错误");

        var WechatApiClient = new WechatApiClient(new WechatApiClientOptions()
        {
            AppId = _wechatOptions.WxOpenAppId,
            AppSecret = _wechatOptions.WxOpenAppSecret
        });

        //WechatApiClient.Configure(settings =>
        //{
        //    settings.JsonSerializer = new FlurlNewtonsoftJsonSerializer();
        //});

        return WechatApiClient;
    }
}