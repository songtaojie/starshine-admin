// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
using Hx.Admin.Models;
using Hx.Common.Extensions.LinqBuilder;
using Hx.Sqlsugar;
using log4net.Util;
using Mapster;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using SqlSugar;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UAParser;
using Yitter.IdGenerator;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinExpressBusinessAccountGetAllResponse.Types;

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
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
        await WriteLogs(batch.FilterNotSqlLog());
        await WriteSqlLog(batch.FilterSqlLog(), db);
    }

    public Task OnEmptyBatchAsync()
    {
        return Task.CompletedTask;
    }

    #region Write Log

    private async Task WriteLogs(IEnumerable<LogEvent> batch, ISqlSugarClient db)
    {
        if (!batch.Any())  return;
        var sysLogExList = new List<SysLogEx>();
        foreach (var logEvent in batch)
        {
            switch (logEvent.Level)
            {
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    sysLogExList.Add(await WriteErrorLog(logEvent));
                    break;
                case LogEventLevel.Warning:
                    await WriteWarningLog(db, v);
                    break;
                case LogEventLevel.Error:
                
                    await WriteErrorLog(db, v);
                    break;
            }

            Console.WriteLine("数据库日志前缀：" + JsonSerializer.Serialize(logEvent));
            var Message = logEvent.RenderMessage();
            Console.WriteLine($"数据库日志：{Message}");
            //log.Properties = logEvent.Properties.ToJson();
            //log.DateTime = logEvent.Timestamp.DateTime;
            //logs.Add(log);
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

    private async Task<SysLogEx> WriteErrorLog(LogEvent logEvent)
    {

        var remoteIPv4 = logEvent.GetPropertyValue<string>(LogContextConst.Request_RemoteIPv4);
        var elapsedMilliseconds = logEvent.GetPropertyValue<long?>(LogContextConst.Request_ElapsedMilliseconds);
        // 获取当前操作者
        var authorizationClaims = logEvent.GetPropertyValue<IEnumerable<Claim>>(LogContextConst.Request_Claims);
        string account = "", realName = "", userId = "", tenantId = "";
        if (authorizationClaims != null)
        {
            foreach (var item in authorizationClaims)
            {
                if (item.Type == ClaimConst.Account)
                    account = item.Value;
                if (item.Type == ClaimConst.RealName)
                    realName = item.Value;
                if (item.Type == ClaimConst.UserId)
                    userId = item.Value;
            }
        }
        var sysLogEx = new SysLogEx
        {
            ControllerName = logEvent.GetPropertyValue<string>(LogContextConst.Route_Controller),
            ActionName = logEvent.GetPropertyValue<string>(LogContextConst.Route_Action),
            DisplayName = logEvent.GetPropertyValue<string>(LogContextConst.Route_DisplayName),
            Status = logEvent.GetPropertyValue<string>(LogContextConst.Response_StatusCode),
            RemoteIp = remoteIPv4,
            Elapsed = elapsedMilliseconds ?? 0,
            Account = account,
            RealName = realName,
            HttpMethod = logEvent.GetPropertyValue<string>(LogContextConst.Request_Method),
            RequestUrl = logEvent.GetPropertyValue<string>(LogContextConst.Request_FullUrl),
            ReturnResult = loggingMonitor.returnInformation == null ? null : JSON.Serialize(loggingMonitor.returnInformation),
            EventId = logMsg.EventId.Id,
            ThreadId = logMsg.ThreadId,
            TraceId = logMsg.TraceId,
            Exception = JSON.Serialize(loggingMonitor.exception),
            Message = logMsg.Message,
            CreateUserId = string.IsNullOrWhiteSpace(userId) ? 0 : long.Parse(userId),
            TenantId = string.IsNullOrWhiteSpace(tenantId) ? 0 : long.Parse(tenantId),
            LogLevel = logMsg.LogLevel
        };
        if (!string.IsNullOrEmpty(sysLogEx.RemoteIp))
        {
            (string ipLocation, double? longitude, double? latitude) = CommonUtil.GetIpAddress(remoteIPv4);
            sysLogEx.Location = ipLocation;
            sysLogEx.Longitude = longitude ?? 0;
            sysLogEx.Latitude = latitude ?? 0;
        }
        var userAgent = logEvent.GetPropertyValue<string>(LogContextConst.Request_UserAgent);
        if (!string.IsNullOrEmpty(userAgent))
        {
            var client = Parser.GetDefault().Parse(userAgent);
            sysLogEx.Browser = $"{client.UA.Family} {client.UA.Major}.{client.UA.Minor} / {client.Device.Family}";
            sysLogEx.Os = $"{client.OS.Family} {client.OS.Major} {client.OS.Minor}";
            
        }
        var actionParameters = logEvent.GetPropertyValue<IEnumerable<ParameterDescriptor>>(LogContextConst.Route_ActionParameters);
        if (actionParameters != null)
        {
            sysLogEx.RequestParam = JsonSerializer.Serialize(actionParameters);
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

    private async Task WriteSqlLog(IEnumerable<LogEvent> batch, ISqlSugarClient db)
    {
        if (!batch.Any())return;
        var sysLogAuditList = new List<SysLogAudit>(); 
        var sysLogDiffList = new List<SysLogDiff>();
        foreach (var logEvent in batch)
        {
            if (!string.IsNullOrEmpty(logEvent.MessageTemplate.Text)
                && logEvent.MessageTemplate.Text.Contains("Sys_Log_Audit", StringComparison.CurrentCultureIgnoreCase)
                && logEvent.MessageTemplate.Text.Contains("Sys_Log_Diff", StringComparison.CurrentCultureIgnoreCase))
            {
                continue;
            }
            if (logEvent.Properties.ContainsKey(SugarLogScope.LogType) && int.TryParse(logEvent.Properties[SugarLogScope.LogType].ToString(), out int logType))
            {
                var creatorId = logEvent.Properties.ContainsKey(LogContextConst.Request_UserId)
                    ? Convert.ToInt64(logEvent.Properties[LogContextConst.Request_UserId].ToString())
                    : 0;
                var parameters = logEvent.Properties.ContainsKey(SugarLogScope.SqlPars)
                    ? logEvent.Properties[SugarLogScope.SqlPars].ToString()
                    : string.Empty;
                var sql = logEvent.Properties.ContainsKey(SugarLogScope.Sql)
                    ? logEvent.Properties[SugarLogScope.Sql].ToString()
                    : string.Empty;
                
                switch (logType)
                {
                    case 1:
                    case 2:
                        sysLogAuditList.Add(new SysLogAudit
                        {
                            Id = YitIdHelper.NextId(),
                            AuditType = GetSqlAuditType(logType),
                            CreateTime = logEvent.Timestamp.DateTime,
                            CreatorId = creatorId,
                            Parameters = parameters,
                            Sql = sql
                        });
                        break;
                    case 3:
                        if (!string.IsNullOrEmpty(sql))
                        {
                            var model = JsonSerializer.Deserialize<DiffLogModel>(sql);
                            if (model != null)
                            {
                                sysLogDiffList.Add(new SysLogDiff
                                {
                                    AfterData = JsonSerializer.Serialize(model.AfterData),
                                    BeforeData = JsonSerializer.Serialize(model.BeforeData),
                                    BusinessData = JsonSerializer.Serialize(model.BusinessData),
                                    // 枚举（insert、update、delete）
                                    DiffType = model.DiffType.ToString(),
                                    Sql = model.Sql,
                                    Parameters = JsonSerializer.Serialize(model.Parameters),
                                    Elapsed = model.Time == null ? 0 : Convert.ToInt64(model.Time.Value.TotalMilliseconds),
                                    CreateTime = logEvent.Timestamp.DateTime,
                                    CreatorId = creatorId,
                                    Id = YitIdHelper.NextId()
                                });
                            }
                        }
                        break;
                }
            }
        }
        if(sysLogAuditList.Any())
            await db.Insertable(sysLogAuditList).ExecuteCommandAsync();
        if (sysLogAuditList.Any())
            await db.Insertable(sysLogDiffList).ExecuteCommandAsync();
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

    private Core.SqlAuditTypeEnum GetSqlAuditType(int logType) 
    {
        return logType switch
        {
            1=> Core.SqlAuditTypeEnum.Normal,
            2=> Core.SqlAuditTypeEnum.Error,
            _ => Core.SqlAuditTypeEnum.Normal
        };
    }

    #endregion
}
