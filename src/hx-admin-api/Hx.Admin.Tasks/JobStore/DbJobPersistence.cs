// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
using Hx.Admin.IServices;
using Hx.Admin.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
public class DbJobPersistence
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDynamicJobCompiler _dynamicJobCompiler;
    public DbJobPersistence(IServiceProvider serviceProvider, 
        IDynamicJobCompiler dynamicJobCompiler)
    {
        _serviceProvider = serviceProvider;
        _dynamicJobCompiler = dynamicJobCompiler;
    }

    public async Task LoadDbJob(QuartzOptions quartzOptions)
    { 
        using var scope = _serviceProvider.CreateScope();
        var sqlSugarClient = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
        var db = sqlSugarClient.AsTenant().GetConnectionScope(SqlSugarConst.Quartz_ConfigId);
        // 获取数据库所有通过脚本创建的作业
        var allDbScriptJobs = await db.Queryable<QrtzJobDetails>().Where(u => u.CreateType != JobCreateTypeEnum.BuiltIn).ToListAsync();
        foreach (var dbDetail in allDbScriptJobs)
        {
            // 动态创建作业
            Type? jobType;
            JobKey? jobKey = null;
            switch (dbDetail.CreateType)
            {
                case JobCreateTypeEnum.Script:
                    jobType = _dynamicJobCompiler.BuildJob(dbDetail.ScriptCode!);
                    if (jobType != null)
                    {
                        jobKey = new JobKey(dbDetail.JobName, dbDetail.JobGroup);
                        quartzOptions.AddJob(jobType, jobBuilder =>
                        {
                            
                            jobBuilder.WithIdentity(jobKey)
                                .WithDescription(dbDetail.Description);
                            dbDetail.IsDurable = dbDetail.IsDurable;
                            if (dbDetail.IsNonConcurrent)
                            {
                                jobBuilder.DisallowConcurrentExecution(false);
                            }
                        });
                    }
                    break;

                //case JobCreateTypeEnum.Http:
                //    jobType = typeof(HttpJob);
                //    break;

                default:
                    throw new NotSupportedException();
            }

            // 获取作业的所有数据库的触发器加入到作业中
            var dbTriggers = await db.Queryable<QrtzTriggers>().Where(u =>u.SchedulerName == dbDetail.SchedulerName && u.JobGroup == dbDetail.JobGroup && u.JobName == dbDetail.JobName).ToListAsync();
            dbTriggers.ForEach(dbTrigger =>
            {
                quartzOptions.AddTrigger(triggerBuilder =>
                {
                    triggerBuilder.ForJob(jobKey!)
                                .WithIdentity(dbTrigger.TriggerName, dbTrigger.TriggerGroup ?? dbTrigger.JobGroup)
                                .WithDescription(dbTrigger.Description);
                    if (dbTrigger.StartTime > 0)
                    { 
                        
                    }
                    if (dbTrigger .StartNow)
                    {
                        triggerBuilder.StartNow();
                    }
                    else if (jobtrigger.RuntimeStartTime.HasValue)
                    {
                        triggerBuilder.StartAt(jobtrigger.RuntimeStartTime.Value);
                    }
                    if (jobtrigger.RuntimeEndTime.HasValue)
                    {
                        triggerBuilder.EndAt(jobtrigger.RuntimeEndTime.Value);
                    }
                    if (jobtrigger.TriggerType == TriggerTypeEnum.Corn && jobtrigger is CronTriggerAttribute cronTrigger)
                    {
                        triggerBuilder.WithCronSchedule(cronTrigger.Cron!);
                    }
                    else if (jobtrigger.TriggerType == TriggerTypeEnum.Simple && jobtrigger is PeriodTriggerAttribute periodTrigger)
                    {
                        var interval = jobtrigger.TriggerArgs?.FirstOrDefault() as long?;
                        if (interval.HasValue && interval > 100)
                        {
                            triggerBuilder.WithSimpleSchedule(s =>
                            {
                                s.WithInterval(TimeSpan.FromMilliseconds(interval.Value)).RepeatForever();
                            });
                        }
                    }
                });
            });

            var triggerBuilders = dbTriggers.Select(u => TriggerBuilder.Create(u.TriggerId).LoadFrom(u).Updated());
            var schedulerBuilder = SchedulerBuilder.Create(jobBuilder, triggerBuilders.ToArray());

            // 标记更新
            schedulerBuilder.Updated();

            allJobs.Add(schedulerBuilder);
        }
    }
}
