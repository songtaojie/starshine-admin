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

namespace Hx.Admin.Serilog.Extensions;
public static class LogBatchingSinkConfigurationExtension
{
    //public static LoggerConfiguration WriteToLogBatching(this LoggerConfiguration loggerConfiguration)
    //{
    //    //if (!AppSettings.app("AppSettings", "LogToDb").ObjToBool())
    //    //{
    //    //    return loggerConfiguration;
    //    //}

    //    //var exampleSink = new LogBatchingSink();

    //    //var batchingOptions = new PeriodicBatchingSinkOptions
    //    //{
    //    //    BatchSizeLimit = 500,
    //    //    Period = TimeSpan.FromSeconds(1),
    //    //    EagerlyEmitFirstEvent = true,
    //    //    QueueLimit = 10000
    //    //};

    //    //var batchingSink = new PeriodicBatchingSink(exampleSink, batchingOptions);

    //    return loggerConfiguration.WriteTo.Sink(batchingSink);
    //}
}
