// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Serilog.Debugging;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Starshine.Admin.Serilog.Extensions;
using Starshine.Admin.Serilog.Filters;

namespace Microsoft.Extensions.DependencyInjection;
public static class SerilogServiceCollectionExtensions
{
    public static IHostBuilder UseSerilogSetup(this IHostBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        builder.UseSerilog((context, serviceProvider, loggerConfig) =>
        {
            loggerConfig
             .ReadFrom.Configuration(context.Configuration)
             .ReadFrom.Services(serviceProvider)
             .WriteToLogBatching(serviceProvider);
        });
        builder.ConfigureServices(services => 
        { 
            services.AddMvcFilter<HttpContextLogActionFilter>(); 
        });
        //var loggerConfiguration = new LoggerConfiguration()
        //    .ReadFrom.Configuration(builder.Configuration);
        //loggerConfiguration.WriteTo;
        //    .Enrich.FromLogContext()
        //    //输出到控制台
        //    .WriteToConsole()
        //    //将日志保存到文件中
        //    .WriteToFile()
        //    //配置日志库
        //    .WriteToLogBatching();

        ////Serilog 内部日志
        //var file = File.CreateText(LogContextStatic.Combine($"SerilogDebug{DateTime.Now:yyyyMMdd}.txt"));
        //SelfLog.Enable(TextWriter.Synchronized(file));

        //host.UseSerilog();
        return builder;
    }
}
