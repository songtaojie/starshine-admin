// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Dm;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Hx.Admin.Core;
using Hx.Admin.Models;
using Hx.Sqlsugar;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qiniu.CDN;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Quartz.Impl.AdoJobStore.Common;
using Quartz.Spi;
using SqlSugar;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Threading;

namespace Hx.Admin.Tasks.JobStore;
public class DbJobStoreTX : JobStoreTX
{
    private readonly ILogger _logger;
    public DbJobStoreTX(ILogger<DbJobStoreTX> logger)
    {
        _logger = logger;
    }

    public override async Task Initialize(ITypeLoadHelper loadHelper, ISchedulerSignaler signaler, CancellationToken cancellationToken = default)
    {
        await base.Initialize(loadHelper, signaler, cancellationToken);
    }
    protected override async Task StoreJob(ConnectionAndTransactionHolder conn, IJobDetail newJob, bool replaceExisting, CancellationToken cancellationToken = default)
    {
        await base.StoreJob(conn, newJob, replaceExisting, cancellationToken);
        await UpdateJobDetail(conn, newJob, cancellationToken);
    }
    protected override async Task StoreTrigger(ConnectionAndTransactionHolder conn, IOperableTrigger newTrigger, IJobDetail? job, bool replaceExisting, string state, bool forceState, bool recovering, CancellationToken cancellationToken = default)
    {
        await base.StoreTrigger(conn, newTrigger, job, replaceExisting, state, forceState, recovering, cancellationToken);
        await UpdateTrigger(conn, newTrigger, cancellationToken);
    }
    private async Task UpdateJobDetail(ConnectionAndTransactionHolder conn,IJobDetail newJob, CancellationToken cancellationToken)
    {
        try
        {
            if (Delegate is StdAdoDelegate adoDelegate)
            {
                var sql = $"UPDATE {TablePrefix}{TableJobDetails} SET CREATE_TYPE=@createType,UPDATE_TIME=@updateTime WHERE {ColumnSchedulerName} = @schedulerName AND {ColumnJobName} = @jobName AND {ColumnJobGroup} = @jobGroup ";
                using var cmd = adoDelegate.PrepareCommand(conn, sql);
                adoDelegate.AddCommandParameter(cmd, "schedulerName", InstanceName);
                adoDelegate.AddCommandParameter(cmd, "jobName", newJob.Key.Name);
                adoDelegate.AddCommandParameter(cmd, "jobGroup", newJob.Key.Group);
                adoDelegate.AddCommandParameter(cmd, "createType", (int)JobCreateTypeEnum.BuiltIn);
                adoDelegate.AddCommandParameter(cmd, "updateTime", DateTimeOffset.Now.UtcTicks);
                await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex, $"更新表{TablePrefix}{TableJobDetails}失败");
        }
    }

    private async Task UpdateTrigger(ConnectionAndTransactionHolder conn, IOperableTrigger newTrigger,CancellationToken cancellationToken)
    {
        try
        {
            if (Delegate is StdAdoDelegate adoDelegate)
            {
                var sql = $"UPDATE {TablePrefix}{TableTriggers} SET UPDATE_TIME=@updateTime WHERE {ColumnSchedulerName} = @schedulerName AND {ColumnTriggerName} = @triggerName AND {ColumnTriggerGroup} = @triggerGroup";
                using var cmd = adoDelegate.PrepareCommand(conn, sql);
                adoDelegate.AddCommandParameter(cmd, "schedulerName", InstanceName);
                adoDelegate.AddCommandParameter(cmd, "triggerName", newTrigger.Key.Name);
                adoDelegate.AddCommandParameter(cmd, "triggerGroup", newTrigger.Key.Group);
                adoDelegate.AddCommandParameter(cmd, "updateTime", DateTimeOffset.Now.UtcTicks);
                await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"更新表{TablePrefix}{TableTriggers}失败");
        }
    }

    protected override async Task<IReadOnlyList<SchedulerStateRecord>> ClusterCheckIn(ConnectionAndTransactionHolder conn, CancellationToken cancellationToken = default)
    {
        var failedInstances = await base.ClusterCheckIn(conn, cancellationToken);
        var state = failedInstances.Any(r=>r.SchedulerInstanceId == InstanceId)? SchedulerStateEnum.Waiting: SchedulerStateEnum.Working;
        await UpdateSchedulerState(conn, state, cancellationToken);
        return failedInstances;
    }

    public override async Task Shutdown(CancellationToken cancellationToken = default)
    {
        await ExecuteWithoutLock<bool>(conn => UpdateSchedulerState(conn, SchedulerStateEnum.Crashed, cancellationToken), cancellationToken).ConfigureAwait(false);
        await base.Shutdown(cancellationToken);
    }

    private async Task<bool> UpdateSchedulerState(ConnectionAndTransactionHolder conn, SchedulerStateEnum state, CancellationToken cancellationToken)
    {
        string sqlUpdateSchedulerState =
            $"UPDATE {TablePrefix}{TableSchedulerState} SET STATE = @state WHERE {ColumnSchedulerName} = @schedulerName AND {ColumnInstanceName} = @instanceName";
        try
        {
            if (Delegate is StdAdoDelegate adoDelegate)
            {
                using var cmd = adoDelegate.PrepareCommand(conn, sqlUpdateSchedulerState);
                adoDelegate.AddCommandParameter(cmd, "schedulerName", InstanceName);
                adoDelegate.AddCommandParameter(cmd, "instanceName", InstanceId);
                adoDelegate.AddCommandParameter(cmd, "state", (int)state);
                var result = await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"更新表{TablePrefix}{TableSchedulerState}状态失败");
        }
        return false;
    }
}
