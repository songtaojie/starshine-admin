// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Configuration;
using Quartz.AspNetCore;
using Quartz;
using Hx.Admin.Tasks;
using Hx.Sdk.Core;
using Hx.Common.Extensions;
using System.Reflection;
using Hx.Sqlsugar;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Task服务注册
/// </summary>
public static class TaskServiceCollectionExtensions
{
    public static IServiceCollection AddQuartzService(this IServiceCollection services, IConfiguration configuration)
    {

        var jobTypes = App.EffectiveTypes.Where(t => typeof(IJob).IsAssignableFrom(t));

        services.AddQuartz(q =>
        {
            //// base Quartz scheduler, job and trigger configuration
            //if (jobTypes.Any())
            //{
            //    foreach (var jobType in jobTypes)
            //    {
            //        var jobDetail = jobType.GetCustomAttribute<JobDetailAttribute>();
            //        if (jobDetail == null) continue;
            //        if(string.IsNullOrWhiteSpace(jobDetail.JobId))
            //            jobDetail.JobId = jobType.FullName!;
            //        // Create a "key" for the job
            //        var jobKey = new JobKey(jobDetail.JobId, jobDetail.GroupName);
            //        // Register the job with the DI container
            //        q.AddJob<LogJob>(opts =>
            //        {
            //            opts.WithIdentity(jobKey).WithDescription(jobDetail.Description);
            //        });
            //        var jobTriggerList = jobType.GetCustomAttributes<TriggerAttribute>(true);
            //        if (jobTriggerList.Any())
            //        {
            //            int index = 0;
            //            foreach (var jobTrigger in jobTriggerList)
            //            {
            //                if (string.IsNullOrWhiteSpace(jobTrigger.TriggerId))
            //                    jobTrigger.TriggerId = $"{jobDetail.JobId}-trigger-{index}";
            //                // Create a trigger for the job
            //                q.AddTrigger(opts =>
            //                {
            //                    opts.ForJob(jobKey)
            //                        .WithIdentity(jobTrigger.TriggerId)
            //                        .WithDescription(jobTrigger.Description);
            //                    if (jobTrigger.StartNow)
            //                    {
            //                        opts.StartNow();
            //                    }
            //                    else if(jobTrigger.RuntimeStartTime.HasValue)
            //                    {
            //                        opts.StartAt(jobTrigger.RuntimeStartTime.Value);
            //                    }
            //                    if (jobTrigger.RuntimeEndTime.HasValue)
            //                    {
            //                        opts.EndAt(jobTrigger.RuntimeEndTime.Value);
            //                    }
            //                    if (jobTrigger.TriggerType == TriggerTypeEnum.Corn && jobTrigger is CronTriggerAttribute cronTrigger)
            //                    {
            //                        var configCron = configuration[$"Quartz:{jobType.Name}"];
            //                        if (string.IsNullOrWhiteSpace(configCron))
            //                        {
            //                            configCron = cronTrigger.Cron;
            //                        }
            //                        if (!string.IsNullOrWhiteSpace(configCron))
            //                        {
            //                            opts.WithCronSchedule(configCron);
            //                        }
            //                    }
            //                    else if (jobTrigger.TriggerType == TriggerTypeEnum.Simple && jobTrigger is PeriodTriggerAttribute periodTrigger)
            //                    {
            //                        var interval = jobTrigger.TriggerArgs?.FirstOrDefault() as long?;
            //                        if (interval.HasValue && interval > 100)
            //                        {
            //                            opts.WithSimpleSchedule(s => s.WithInterval(TimeSpan.FromMilliseconds(interval.Value)));
            //                        }
            //                    }
            //                }); 
            //                index++;
            //            }
            //        }
            //        else
            //        {
            //            // Create a trigger for the job
            //            q.AddTrigger(opts =>
            //            {
            //                opts.ForJob(jobKey).StartNow().WithIdentity($"{jobDetail.JobId}-trigger");
            //                var configCron = configuration[$"Quartz:{jobType.Name}"];
            //                if (!string.IsNullOrWhiteSpace(configCron))
            //                {
            //                    opts.WithCronSchedule(configCron);
            //                }
            //            });
            //        }
            //    }
            //}
            //持久化
            var dbConfig =  configuration.GetValue<DbConnectionConfig>("DbSettings:QuartzConnectionConfig");
            if (dbConfig == null)
            {
                dbConfig = new DbConnectionConfig
                {
                    DbType = SqlSugar.DbType.Sqlite,
                    ConnectionString = "DataSource=./HxAdmin.db"
                };
            }
            q.UsePersistentStore<MyJobStoreTX>(x =>
            {
                x.UseGenericDatabase(GetProvider(dbConfig.DbType), ado =>
                {
                    ado.ConnectionString = dbConfig.ConnectionString;
                });
                x.UseNewtonsoftJsonSerializer();
            });
            q.AddSchedulerListener<SampleSchedulerListener>();
        });
        
        // ASP.NET Core hosting
        services.AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
        return services;
    }


    /// <summary>
    /// 检查字段域 非 Null 非空数组
    /// </summary>
    /// <param name="fields">字段值</param>
    private static void CheckCronExpression(params object[] fields)
    {
        // 空检查
        if (fields == null || fields.Length == 0) throw new ArgumentNullException(nameof(fields));

        // 检查 fields 只能是 int, long，string 和非 null 类型
        if (fields.Any(f => f == null || (f.GetType() != typeof(int) && f.GetType() != typeof(long) && f.GetType() != typeof(string)))) 
            throw new InvalidOperationException("Invalid Cron expression.");
    }

    private static string GetProvider(SqlSugar.DbType dbType)
    {
        return dbType switch
        {
            SqlSugar.DbType.MySql => "MySql",
            SqlSugar.DbType.PostgreSQL => "Npgsql",
            SqlSugar.DbType.SqlServer => "SqlServer",
            SqlSugar.DbType.Sqlite => "SQLite-Microsoft",
            SqlSugar.DbType.MySqlConnector => "MySqlConnector",
            SqlSugar.DbType.Oracle => "OracleODPManaged",
            _ => throw new InvalidOperationException("无效的数据库操作类型")
        };
    }
}
