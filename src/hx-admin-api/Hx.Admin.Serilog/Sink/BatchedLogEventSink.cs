// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Models;
using Mapster;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using SqlSugar;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog.Sink;
public class BatchedLogEventSink : IBatchedLogEventSink
{
    private readonly ISqlSugarClient _sqlSugarClient;
    public BatchedLogEventSink(ISqlSugarClient sqlSugarClient)
    {
        _sqlSugarClient = sqlSugarClient;
    }
    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        //await WriteSqlLog(_sqlSugarClient, batch.FilterSqlLog());
        //await WriteLogs(_sqlSugarClient, batch.FilterRemoveOtherLog());
    }

    public Task OnEmptyBatchAsync()
    {
        return Task.CompletedTask;
    }

    #region Write Log

    private async Task WriteLogs(ISqlSugarClient db, IEnumerable<LogEvent> batch)
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
                    await WriteInformationLog(db, v);
                    break;
                case LogEventLevel.Warning:
                    await WriteWarningLog(db, v);
                    break;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    await WriteErrorLog(db, v);
                    break;
            }
        }
    }

    private async Task WriteInformationLog(ISqlSugarClient db, IEnumerable<LogEvent> batch)
    {
        if (!batch.Any())
        {
            return;
        }

        //var logs = new List<GlobalInformationLog>();
        //foreach (var logEvent in batch)
        //{
        //    var log = logEvent.Adapt<GlobalInformationLog>();
        //    log.Message = logEvent.RenderMessage();
        //    log.Properties = logEvent.Properties.ToJson();
        //    log.DateTime = logEvent.Timestamp.DateTime;
        //    logs.Add(log);
        //}

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

    private async Task WriteSqlLog(ISqlSugarClient db, IEnumerable<LogEvent> batch)
    {
        if (!batch.Any())
        {
            return;
        }

        //var logs = new List<AuditSqlLog>();
        //foreach (var logEvent in batch)
        //{
        //    var log = logEvent.Adapt<AuditSqlLog>();
        //    log.Message = logEvent.RenderMessage();
        //    log.Properties = logEvent.Properties.ToJson();
        //    log.DateTime = logEvent.Timestamp.DateTime;
        //    logs.Add(log);
        //}

        //await db.AsTenant().InsertableWithAttr(logs).SplitTable().ExecuteReturnSnowflakeIdAsync();
        await Task.CompletedTask;
    }

    #endregion
}
