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
            quartzOptions.ScanToBuilders();
            quartzOptions.UsePersistentStore(x =>
            {
                x.UseDatabase(dbConfig);
                x.UseNewtonsoftJsonSerializer();
                x.PerformSchemaValidation = true;
            });
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
