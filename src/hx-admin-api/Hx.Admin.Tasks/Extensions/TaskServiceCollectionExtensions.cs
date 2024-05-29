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
