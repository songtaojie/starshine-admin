﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
using Hx.Admin.Models;
using Hx.Sqlsugar;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using SqlSugar;
using System.Security.Claims;
using System.Text.Json;
using UAParser;
using Yitter.IdGenerator;

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
        await WriteLogs(batch.FilterNotSqlLog(), db);
        await WriteSqlLog(batch.FilterSqlLog(), db);
    }

    public Task OnEmptyBatchAsync()
    {
        return Task.CompletedTask;
    }

    #region Write Log

    private async Task WriteLogs(IEnumerable<LogEvent> batch, ISqlSugarClient db)
    {
        if (!batch.Any()) return;
        var sysLogExList = new List<SysLogEx>();
        var sysLogOpList = new List<SysLogOp>();
        var sysLogVisList = new List<SysLogVis>();
        foreach (var logEvent in batch)
        {
            switch (logEvent.Level)
            {
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    sysLogExList.Add(WriteErrorLog(logEvent));
                    break;
                default:
                    var sysLogVis = WriteSysLogVis(logEvent);
                    if (sysLogVis != null)
                    {
                        sysLogVisList.Add(sysLogVis);
                    }
                    
                    sysLogOpList.Add(WriteSysLogOp(logEvent));
                    break;
            }
        }
        if (sysLogExList.Any())
            await db.Insertable(sysLogExList).ExecuteCommandAsync();
        if (sysLogOpList.Any())
            await db.Insertable(sysLogOpList).ExecuteCommandAsync();
        if (sysLogVisList.Any())
            await db.Insertable(sysLogVisList).ExecuteCommandAsync();
    }

    private SysLogVis? WriteSysLogVis(LogEvent logEvent)
    {
        var actionName = logEvent.GetPropertyValue<string>(LogContextConst.Route_Action);
        if (!string.IsNullOrEmpty(actionName) && (actionName.Equals("Login")))
        {
            var remoteIPv4 = logEvent.GetPropertyValue<string>(LogContextConst.Request_RemoteIPv4);
            var elapsedMilliseconds = logEvent.GetPropertyValue<long?>(LogContextConst.Request_ElapsedMilliseconds);
            // 获取当前操作者
            var authorizationClaims = logEvent.GetPropertyValue<IEnumerable<Claim>>(LogContextConst.Request_Claims);
            string account = "", realName = "", userId = "";
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
            var sysLogVis = new SysLogVis
            {
                ControllerName = logEvent.GetPropertyValue<string>(LogContextConst.Route_Controller),
                ActionName = logEvent.GetPropertyValue<string>(LogContextConst.Route_Action),
                DisplayName = logEvent.GetPropertyValue<string>(LogContextConst.Route_DisplayName),
                Status = logEvent.GetPropertyValue<int>(LogContextConst.Response_StatusCode).ToString(),
                RemoteIp = remoteIPv4,
                Elapsed = elapsedMilliseconds ?? 0,
                Account = account,
                RealName = realName,
                CreatorId = string.IsNullOrWhiteSpace(userId) ? 0 : long.Parse(userId),
                CreateTime = logEvent.Timestamp.DateTime,
            };
            if (!string.IsNullOrEmpty(sysLogVis.RemoteIp))
            {
                (string ipLocation, double? longitude, double? latitude) = CommonUtil.GetIpAddress(remoteIPv4);
                sysLogVis.Location = ipLocation;
                sysLogVis.Longitude = longitude ?? 0;
                sysLogVis.Latitude = latitude ?? 0;
            }
            var userAgent = logEvent.GetPropertyValue<string>(LogContextConst.Request_UserAgent);
            if (!string.IsNullOrEmpty(userAgent))
            {
                var client = Parser.GetDefault().Parse(userAgent);
                sysLogVis.Browser = $"{client.UA.Family} {client.UA.Major}.{client.UA.Minor} / {client.Device.Family}";
                sysLogVis.Os = $"{client.OS.Family} {client.OS.Major} {client.OS.Minor}";

            }
            return sysLogVis;
        }
        return null;
    }

    private SysLogOp WriteSysLogOp(LogEvent logEvent)
    {
        var remoteIPv4 = logEvent.GetPropertyValue<string>(LogContextConst.Request_RemoteIPv4);
        var elapsedMilliseconds = logEvent.GetPropertyValue<long?>(LogContextConst.Request_ElapsedMilliseconds);
        // 获取当前操作者
        var authorizationClaims = logEvent.GetPropertyValue<IEnumerable<Claim>>(LogContextConst.Request_Claims);
        string account = "", realName = "", userId = "";
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
        var sysLogOp = new SysLogOp
        {
            ControllerName = logEvent.GetPropertyValue<string>(LogContextConst.Route_Controller),
            ActionName = logEvent.GetPropertyValue<string>(LogContextConst.Route_Action),
            DisplayName = logEvent.GetPropertyValue<string>(LogContextConst.Route_DisplayName),
            Status = logEvent.GetPropertyValue<int>(LogContextConst.Response_StatusCode).ToString(),
            RemoteIp = remoteIPv4,
            Elapsed = elapsedMilliseconds ?? 0,
            Account = account,
            RealName = realName,
            HttpMethod = logEvent.GetPropertyValue<string>(LogContextConst.Request_Method),
            RequestUrl = logEvent.GetPropertyValue<string>(LogContextConst.Request_FullUrl),
            RequestParam = logEvent.GetPropertyValue<string>(LogContextConst.Route_ActionParameters),
            ReturnResult = logEvent.GetPropertyValue<string>(LogContextConst.Route_ActionResult),
            ThreadId = logEvent.GetPropertyValue<int>(LogContextConst.Request_ThreadId),
            TraceId = logEvent.GetPropertyValue<string>(LogContextConst.Request_TraceIdentifier),
            Exception = logEvent.Exception == null ? string.Empty : JsonSerializer.Serialize(logEvent.Exception),
            Message = GetMessage(logEvent),
            CreatorId = string.IsNullOrWhiteSpace(userId) ? 0 : long.Parse(userId),
            CreateTime = logEvent.Timestamp.DateTime,
        };
        if (!string.IsNullOrEmpty(sysLogOp.RemoteIp))
        {
            (string ipLocation, double? longitude, double? latitude) = CommonUtil.GetIpAddress(remoteIPv4);
            sysLogOp.Location = ipLocation;
            sysLogOp.Longitude = longitude ?? 0;
            sysLogOp.Latitude = latitude ?? 0;
        }
        var userAgent = logEvent.GetPropertyValue<string>(LogContextConst.Request_UserAgent);
        if (!string.IsNullOrEmpty(userAgent))
        {
            var client = Parser.GetDefault().Parse(userAgent);
            sysLogOp.Browser = $"{client.UA.Family} {client.UA.Major}.{client.UA.Minor} / {client.Device.Family}";
            sysLogOp.Os = $"{client.OS.Family} {client.OS.Major} {client.OS.Minor}";
        }
        return sysLogOp;
    }

    private SysLogEx WriteErrorLog(LogEvent logEvent)
    {
        var remoteIPv4 = logEvent.GetPropertyValue<string>(LogContextConst.Request_RemoteIPv4);
        var elapsedMilliseconds = logEvent.GetPropertyValue<long?>(LogContextConst.Request_ElapsedMilliseconds);
        // 获取当前操作者
        var authorizationClaims = logEvent.GetPropertyValue<IEnumerable<Claim>>(LogContextConst.Request_Claims);
        string account = "", realName = "", userId = "";
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
            Status = logEvent.GetPropertyValue<int>(LogContextConst.Response_StatusCode).ToString(),
            RemoteIp = remoteIPv4,
            Elapsed = elapsedMilliseconds ?? 0,
            Account = account,
            RealName = realName,
            HttpMethod = logEvent.GetPropertyValue<string>(LogContextConst.Request_Method),
            RequestUrl = logEvent.GetPropertyValue<string>(LogContextConst.Request_FullUrl),
            RequestParam = logEvent.GetPropertyValue<string>(LogContextConst.Route_ActionParameters),
            ReturnResult = logEvent.GetPropertyValue<string>(LogContextConst.Route_ActionResult),
            ThreadId = logEvent.GetPropertyValue<int>(LogContextConst.Request_ThreadId),
            TraceId = logEvent.GetPropertyValue<string?>(LogContextConst.Request_TraceIdentifier),
            Exception = logEvent.Exception == null ? string.Empty : JsonSerializer.Serialize(logEvent.Exception),
            Message = GetMessage(logEvent),
            CreatorId = string.IsNullOrWhiteSpace(userId) ? 0 : long.Parse(userId),
            LogLevel = GetLogLevel(logEvent.Level),
            CreateTime = logEvent.Timestamp.DateTime,
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
        return sysLogEx;
    }

    private async Task WriteSqlLog(IEnumerable<LogEvent> batch, ISqlSugarClient db)
    {
        if (!batch.Any()) return;
        var sysLogAuditList = new List<SysLogAudit>();
        var sysLogDiffList = new List<SysLogDiff>();
        foreach (var logEvent in batch)
        {
            if (!string.IsNullOrEmpty(logEvent.MessageTemplate.Text) && logEvent.MessageTemplate.Text.StartsWith("Sys_Log", StringComparison.CurrentCultureIgnoreCase))
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
        if (sysLogAuditList.Any())
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
            1 => Core.SqlAuditTypeEnum.Normal,
            2 => Core.SqlAuditTypeEnum.Error,
            _ => Core.SqlAuditTypeEnum.Normal
        };
    }


    private LogLevel GetLogLevel(LogEventLevel logEventLevel)
    {
        return logEventLevel switch
        {
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Fatal => LogLevel.Critical,
            LogEventLevel.Verbose => LogLevel.Trace,
            _ => LogLevel.Information
        };
    }
    #endregion
}
