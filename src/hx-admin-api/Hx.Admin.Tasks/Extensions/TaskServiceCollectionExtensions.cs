// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Configuration;
using Quartz.AspNetCore;
using Quartz;
using Hx.Admin.Tasks;
using Hx.Sqlsugar;
using Hx.Admin.Core;
using Elastic.Clients.Elasticsearch;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Hx.Admin.Tasks.JobStore;
using Quartz.Impl.AdoJobStore;
using Quartz.Util;
using Microsoft.Extensions.Options;
using System.Runtime.Intrinsics.Arm;
using SqlSugar;
using Hx.Admin.Models;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Task服务注册
/// </summary>
public static class TaskServiceCollectionExtensions
{
    public static IServiceCollection AddQuartzService(this IServiceCollection services, IConfiguration configuration)
    {
        IEnumerable<DbConnectionConfig>? dbConnectionConfigs = new List<DbConnectionConfig>();
        configuration.GetSection("DbSettings:ConnectionConfigs").Bind(dbConnectionConfigs);
        var dbConfig = dbConnectionConfigs.FirstOrDefault(r => r.ConfigId?.ToString() == SqlSugarConst.Quartz_ConfigId);
        if (dbConfig == null)
            throw new ArgumentNullException(nameof(dbConfig));
      
        services.Configure<QuartzOptions,IServiceProvider>((quartzOptions, provider) =>
        {
            quartzOptions.ScanToBuilders();

            using var scope = provider.CreateScope();
            var sqlSugarClient = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
            var db = sqlSugarClient.AsTenant().GetConnectionScope(SqlSugarConst.Quartz_ConfigId);
            // 获取数据库所有通过脚本创建的作业
            var allDbScriptJobs = await db.Queryable<SysJobDetail>().Where(u => u.CreateType != JobCreateTypeEnum.BuiltIn).ToListAsync();
            foreach (var dbDetail in allDbScriptJobs)
            {
                // 动态创建作业
                Type jobType;
                switch (dbDetail.CreateType)
                {
                    case JobCreateTypeEnum.Script:
                        jobType = dynamicJobCompiler.BuildJob(dbDetail.ScriptCode);
                        break;

                    case JobCreateTypeEnum.Http:
                        jobType = typeof(HttpJob);
                        break;

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

            //var jobKey = new JobKey("options-custom-job", "custom");
            //quartzOptions.AddJob<LogJob>(j => j.WithIdentity(jobKey));
            //quartzOptions.AddTrigger(trigger => trigger
            //    .WithIdentity("options-custom-trigger", "custom")
            //    .ForJob(jobKey)
            //    .WithSimpleSchedule(s=>s.WithIntervalInSeconds(1000)));
            //if (!string.IsNullOrWhiteSpace(dep.Value.CronSchedule))
            //{
            //    var jobKey = new JobKey("options-custom-job", "custom");
            //    options.AddJob<ExampleJob>(j => j.WithIdentity(jobKey));
            //    options.AddTrigger(trigger => trigger
            //        .WithIdentity("options-custom-trigger", "custom")
            //        .ForJob(jobKey)
            //        .WithCronSchedule(dep.Value.CronSchedule));
            //}
        });
        services.AddQuartz(quartzOptions =>
        {
            //quartzOptions.ScanToBuilders();
            quartzOptions.UsePersistentStore(x =>
            {
                x.UseDatabase(dbConfig);
                x.UseNewtonsoftJsonSerializer();
                x.PerformSchemaValidation = true;
            });
        });
        // ASP.NET Core hosting
        services.AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
        return services;
    }

    private static void UseDatabase(this SchedulerBuilder.PersistentStoreOptions storeOptions,DbConnectionConfig dbCOnfig)
    {
        string driverDelegateType = string.Empty;
        switch (dbCOnfig.DbType)
        { 
            case SqlSugar.DbType.MySql:
                storeOptions.UseMySql(dbCOnfig.ConnectionString);
                break;
            case SqlSugar.DbType.PostgreSQL:
                storeOptions.UsePostgres(dbCOnfig.ConnectionString);
                break;
            case SqlSugar.DbType.SqlServer:
                storeOptions.UseSqlServer(dbCOnfig.ConnectionString);
                break;
            case SqlSugar.DbType.Sqlite:
                storeOptions.UseMicrosoftSQLite(dbCOnfig.ConnectionString);
                driverDelegateType = typeof(HxSQLiteDelegate).AssemblyQualifiedNameWithoutVersion();
                break;
            case SqlSugar.DbType.MySqlConnector:
                storeOptions.UseMySqlConnector(x=>
                { 
                    x.ConnectionString = dbCOnfig.ConnectionString;
                });
                break;
            case SqlSugar.DbType.Oracle:
                storeOptions.UseOracle(dbCOnfig.ConnectionString);
                break;
            default:
                throw new NotImplementedException("不支持的DbType");
        }
        storeOptions.SetProperty("quartz.jobStore.driverDelegateType", driverDelegateType);
    }
}
