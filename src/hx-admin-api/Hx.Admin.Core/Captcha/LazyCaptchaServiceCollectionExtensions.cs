﻿using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Generator;
using Lazy.Captcha.Core.Storage;
using SkiaSharp;

namespace Hx.Admin.Core;

public static class LazyCaptchaServiceCollectionExtensions
{
    /// <summary>
    /// 验证码初始化
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">配置</param>
    public static void AddLazyCaptcha(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddCaptcha(configuration);
        services.AddScoped<ICaptcha, Captcha.RandomCaptcha>();
    }
}