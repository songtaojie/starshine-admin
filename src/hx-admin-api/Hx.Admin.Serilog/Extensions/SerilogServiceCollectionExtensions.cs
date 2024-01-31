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
using Hx.Admin.Serilog.Extensions;

namespace Hx.Admin.Serilog;
public static class SerilogServiceCollectionExtensions
{
    public static IHostBuilder UseSerilogSetup(this IHostBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        builder.UseSerilog((context, services, loggerConfig) =>
        {
            loggerConfig
             .ReadFrom.Configuration(context.Configuration)
             .ReadFrom.Services(services)
             .Enrich.FromLogContext()
            .WriteToConsole()
            //将日志保存到文件中
            .WriteToFile()
            //配置日志库
            .WriteToLogBatching();//输出到控制台

        });
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration);
        loggerConfiguration.WriteTo;
            .Enrich.FromLogContext()
            //输出到控制台
            .WriteToConsole()
            //将日志保存到文件中
            .WriteToFile()
            //配置日志库
            .WriteToLogBatching();

        //var option = App.GetOptions<SeqOptions>();
        ////配置Seq日志中心
        //if (option.Enabled)
        //{
        //    var address = option.Address;
        //    var apiKey = option.ApiKey;
        //    if (!address.IsNullOrEmpty())
        //    {
        //        loggerConfiguration =
        //            loggerConfiguration.WriteTo.Seq(address, restrictedToMinimumLevel: LogEventLevel.Verbose,
        //                apiKey: apiKey, eventBodyLimitBytes: 10485760);
        //    }
        //}

        //Log.Logger = loggerConfiguration.CreateLogger();

        ////Serilog 内部日志
        //var file = File.CreateText(LogContextStatic.Combine($"SerilogDebug{DateTime.Now:yyyyMMdd}.txt"));
        //SelfLog.Enable(TextWriter.Synchronized(file));

        //host.UseSerilog();
        return builder;
    }
}
