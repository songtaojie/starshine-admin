// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

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
        bool flag = await JobExists(conn, newJob.Key, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        await base.StoreJob(conn, newJob, replaceExisting, cancellationToken);
        if (!flag)
        {
           await UpdateJobDetailCreateType(conn,newJob,cancellationToken);
        }
    }

    private async Task UpdateJobDetailCreateType(ConnectionAndTransactionHolder conn,IJobDetail newJob, CancellationToken cancellationToken)
    {
        try
        {
            if (Delegate is StdAdoDelegate adoDelegate)
            {
                var sql = $"UPDATE {TablePrefix}{TableJobDetails} SET CREATE_TYPE=@createType WHERE {ColumnSchedulerName} = @schedulerName AND {ColumnJobName} = @jobName AND {ColumnJobGroup} = @jobGroup ";
                using var cmd = adoDelegate.PrepareCommand(conn, sql);
                adoDelegate.AddCommandParameter(cmd, "schedulerName", InstanceName);
                adoDelegate.AddCommandParameter(cmd, "jobName", newJob.Key.Name);
                adoDelegate.AddCommandParameter(cmd, "jobGroup", newJob.Key.Group);
                adoDelegate.AddCommandParameter(cmd, "createType", (int)JobCreateTypeEnum.BuiltIn);
                await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex, "更新JobDetails失败");
        }
    }
}
