// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
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
    private readonly DynamicJobCompiler _dynamicJobCompiler;
    public DbJobPersistence(IServiceProvider serviceProvider, DynamicJobCompiler dynamicJobCompiler)
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
            switch (dbDetail.CreateType)
            {
                case JobCreateTypeEnum.Script:
                    jobType = _dynamicJobCompiler.BuildJob(dbDetail.ScriptCode!);
                    if (jobType != null)
                    {
                        JobKey jobKey = new JobKey(dbDetail.JobName, dbDetail.JobGroup);
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

            // 动态构建的 jobType 的程序集名称为随机名称，需重新设置
            dbDetail.AssemblyName = jobType.Assembly.FullName!.Split(',')[0];
            var jobBuilder = JobBuilder.Create(jobType).LoadFrom(dbDetail);

            // 强行设置为不扫描 IJob 实现类 [Trigger] 特性触发器，否则 SchedulerBuilder.Create 会再次扫描，导致重复添加同名触发器
            jobBuilder.SetIncludeAnnotations(false);

            // 获取作业的所有数据库的触发器加入到作业中
            var dbTriggers = await db.Queryable<SysJobTrigger>().Where(u => u.JobId == jobBuilder.JobId).ToListAsync();
            var triggerBuilders = dbTriggers.Select(u => TriggerBuilder.Create(u.TriggerId).LoadFrom(u).Updated());
            var schedulerBuilder = SchedulerBuilder.Create(jobBuilder, triggerBuilders.ToArray());

            // 标记更新
            schedulerBuilder.Updated();

            allJobs.Add(schedulerBuilder);
        }
    }
}
