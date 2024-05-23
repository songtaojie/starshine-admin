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
        services.AddQuartz(quartzOptions =>
        {
            //quartzOptions.ScanToBuilders();
            /// Just use the name of your job that you created in the Jobs folder.
            var jobKey = new JobKey("job_log");
            //quartzOptions.AddJob<LogJob>(opts => opts.WithIdentity(jobKey));
            quartzOptions.AddJob(typeof(LogJob), jobKey, opts => opts.WithIdentity(jobKey));

            quartzOptions.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("trigger_log")
                .StartNow()
                .WithSimpleSchedule(s=>s.WithIntervalInSeconds(2))
                //This Cron interval can be described as "run every minute" (when second is zero)
            );
            //quartzOptions.UsePersistentStore(x =>
            //{
            //    x.UseGenericDatabase(GetProvider(dbConfig.DbType), ado =>
            //    {
            //        ado.ConnectionString = dbConfig.ConnectionString;
            //    });
            //    x.UseNewtonsoftJsonSerializer();
            //    x.PerformSchemaValidation = true;
            //});
            //quartzOptions.AddSchedulerListener<SampleSchedulerListener>();
        });
        // ASP.NET Core hosting
        services.AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
        return services;
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
