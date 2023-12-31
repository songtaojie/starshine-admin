// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Task;
/// <summary>
/// 清理日志作业任务
/// </summary>
//[JobDetail("job_log", Description = "清理操作日志", GroupName = "default", Concurrent = false)]
//[Daily(TriggerId = "trigger_log", Description = "清理操作日志")]
public class LogJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public LogJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async System.Threading.Tasks.Task Execute(IJobExecutionContext context)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var logVisRep = serviceScope.ServiceProvider.GetService<ISqlSugarRepository<SysLogVis>>();
        var logOpRep = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysLogOp>>();
        var logDiffRep = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysLogDiff>>();

        var daysAgo = 30; // 删除30天以前
        await logVisRep.AsDeleteable().Where(u => (DateTime)u.CreateTime < DateTime.Now.AddDays(-daysAgo)).ExecuteCommandAsync(stoppingToken); // 删除访问日志
        await logOpRep.AsDeleteable().Where(u => (DateTime)u.CreateTime < DateTime.Now.AddDays(-daysAgo)).ExecuteCommandAsync(stoppingToken); // 删除操作日志
        await logDiffRep.AsDeleteable().Where(u => (DateTime)u.CreateTime < DateTime.Now.AddDays(-daysAgo)).ExecuteCommandAsync(stoppingToken); // 删除差异日志
    }

}