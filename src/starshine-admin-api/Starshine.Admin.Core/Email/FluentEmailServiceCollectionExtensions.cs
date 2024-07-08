// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Starshine.Admin.Core;
using System.Net;
using System.Net.Mail;

namespace Microsoft.Extensions.DependencyInjection;
public static class FluentEmailServiceCollectionExtensions
{
    public static IServiceCollection AddFluentEmail(this IServiceCollection services,IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        services.AddFluentEmail(configuration["Email:DefaultFromEmail"], configuration["Email:DefaultFromName"])
            .AddSmtpSender();
        return services;
    }
    private static FluentEmailServicesBuilder AddSmtpSender(this FluentEmailServicesBuilder builder)
    {
        builder.Services.AddSingleton<ISender>((s) =>
        {
            var options = s.GetService<IOptions<EmailOptions>>();
            var emailOption = options.Value;
            return new SmtpSender(new SmtpClient(emailOption.Host, emailOption.Port)
            {
                EnableSsl = emailOption.EnableSsl,
                UseDefaultCredentials = emailOption.UseDefaultCredentials,
                Credentials = new NetworkCredential(emailOption.UserName, emailOption.Password)
            });
        });
        return builder;
    }
}
