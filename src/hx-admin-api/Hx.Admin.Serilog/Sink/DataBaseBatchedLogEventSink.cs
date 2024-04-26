// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Models;
using Hx.Sqlsugar;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using SqlSugar;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog.Sink;
public class DataBaseBatchedLogEventSink : IBatchedLogEventSink
{
    private readonly IServiceProvider _serviceProvider;
    public DataBaseBatchedLogEventSink(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        batch = batch.FilterWriteToDbLog();
        //await WriteLogs(batch.FilterNotSqlLog());
        await WriteLogs(batch.FilterSqlLog());
    }

    public Task OnEmptyBatchAsync()
    {
        return Task.CompletedTask;
    }

    #region Write Log

    private async Task WriteLogs(IEnumerable<LogEvent> batch)
    {
        if (!batch.Any())
        {
            return;
        }

        var group = batch.GroupBy(s => s.Level);
        foreach (var v in group)
        {
            switch (v.Key)
            {
                case LogEventLevel.Information:
                    await WriteInformationLog(v);
                    break;
                //case LogEventLevel.Warning:
                //    await WriteWarningLog(db, v);
                //    break;
                //case LogEventLevel.Error:
                //case LogEventLevel.Fatal:
                //    await WriteErrorLog(db, v);
                //    break;
            }
        }
    }

    private async Task WriteInformationLog(IEnumerable<LogEvent> batch)
    {
        if (!batch.Any())
        {
            return;
        }

        foreach (var logEvent in batch)
        {
            Console.WriteLine("数据库日志前缀：" + JsonSerializer.Serialize(logEvent));
            var Message = logEvent.RenderMessage();
            Console.WriteLine($"数据库日志：{Message}");
            //log.Properties = logEvent.Properties.ToJson();
            //log.DateTime = logEvent.Timestamp.DateTime;
            //logs.Add(log);
        }

        //await db.AsTenant().InsertableWithAttr(logs).SplitTable().ExecuteReturnSnowflakeIdAsync();
        await Task.CompletedTask;
    }

    private async Task WriteWarningLog(ISqlSugarClient db, IEnumerable<LogEvent> batch)
    {
        if (!batch.Any())
        {
            return;
        }

        //var logs = new List<GlobalWarningLog>();
        //foreach (var logEvent in batch)
        //{
        //    var log = logEvent.Adapt<GlobalWarningLog>();
        //    log.Message = logEvent.RenderMessage();
        //    log.Properties = logEvent.Properties.ToJson();
        //    log.DateTime = logEvent.Timestamp.DateTime;
        //    logs.Add(log);
        //}

        //await db.AsTenant().InsertableWithAttr(logs).SplitTable().ExecuteReturnSnowflakeIdAsync();
        await Task.CompletedTask;
    }

    private async Task WriteErrorLog(ISqlSugarClient db, IEnumerable<LogEvent> batch)
    {
        if (!batch.Any())
        {
            return;
        }

        //var logs = new List<SysLogEx>();
        //batch.Select(logEvent => new SysLogEx
        //{
        //});
        //foreach (var logEvent in batch)
        //{

        //    var log = logEvent.Adapt<GlobalErrorLog>();
        //    log.Message = logEvent.RenderMessage();
        //    log.Properties = logEvent.Properties.ToJson();
        //    log.DateTime = logEvent.Timestamp.DateTime;
        //    logs.Add(log);
        //}

        //await db.AsTenant().InsertableWithAttr(logs).SplitTable().ExecuteReturnSnowflakeIdAsync();
        await Task.CompletedTask;
    }

    private async Task WriteSqlLog(IEnumerable<LogEvent> batch)
    {
        if (!batch.Any())
        {
            return;
        }

        //var logs = new List<AuditSqlLog>();
        foreach (var logEvent in batch)
        {
            //var log = logEvent.Adapt<AuditSqlLog>();
            var Message = logEvent.RenderMessage(null);
            Console.WriteLine(Message);
            //log.Properties = logEvent.Properties.ToJson();
            var DateTime = logEvent.Timestamp.DateTime;
            //logs.Add(log);
        }

        //await db.AsTenant().InsertableWithAttr(logs).SplitTable().ExecuteReturnSnowflakeIdAsync();
        await Task.CompletedTask;
    }
    private string GetMessage(LogEvent logEvent)
    {
        if (logEvent.MessageTemplate.Tokens.Any() && logEvent.MessageTemplate.Tokens.ElementAt(0).Length <= 20000)
        {
            return logEvent.RenderMessage(null);
        }
        return logEvent.MessageTemplate.Text;
    }
    #endregion
}
