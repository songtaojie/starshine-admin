// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Serilog.Sinks.PeriodicBatching;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hx.Admin.Serilog.Sink;
using Serilog.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Hx.Admin.Serilog.Extensions;
public static class LogBatchingSinkConfigurationExtension
{
    public static LoggerConfiguration WriteToLogBatching(this LoggerConfiguration loggerConfiguration, IServiceProvider provider)
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        if (!configuration.GetValue<bool>("Serilog:WriteToDb", false)) return loggerConfiguration;
        var exampleSink = new DataBaseBatchedLogEventSink(provider);

        var batchingOptions = new PeriodicBatchingSinkOptions();

        var batchingSink = new PeriodicBatchingSink(exampleSink, batchingOptions);

        return loggerConfiguration.WriteTo.Async(loggerConfig=> loggerConfig.Sink(batchingSink));
    }
}
