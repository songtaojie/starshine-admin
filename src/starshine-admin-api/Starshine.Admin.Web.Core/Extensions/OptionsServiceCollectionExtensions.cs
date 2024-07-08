// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Options服务拓展类
    /// </summary>
    public static class OptionsServiceCollectionExtensions
    {
        /// <summary>
        /// 配置跨域
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddAdminOptions(this IServiceCollection services)
        {
            services.AddConfigureOptions<SnowIdOptions>();
            services.AddConfigureOptions<OSSProviderOptions>();
            services.AddConfigureOptions<UploadOptions>();
            services.AddConfigureOptions<WechatOptions>();
            services.AddConfigureOptions<WechatPayOptions>();
            services.AddConfigureOptions<PayCallBackOptions>();
            services.AddConfigureOptions<EmailOptions>();
            services.AddConfigureOptions<OAuthOptions>();
            services.AddConfigureOptions<CryptogramOptions>();
            services.AddConfigureOptions<JWTSettingsOptions>(); 
            //services.AddConfigurableOptions<IpRateLimitingOptions>();
            //services.AddConfigurableOptions<IpRateLimitPoliciesOptions>();
            //services.AddConfigurableOptions<ClientRateLimitingOptions>();
            //services.AddConfigurableOptions<ClientRateLimitPoliciesOptions>();
            //services.Configure<IpRateLimitOptions>(App.Configuration.GetSection("IpRateLimiting"));
            //services.Configure<IpRateLimitPolicies>(App.Configuration.GetSection("IpRateLimitPolicies"));
            //services.Configure<ClientRateLimitOptions>(App.Configuration.GetSection("ClientRateLimiting"));
            //services.Configure<ClientRateLimitPolicies>(App.Configuration.GetSection("ClientRateLimitPolicies"));

            return services;
        }
    }
}
