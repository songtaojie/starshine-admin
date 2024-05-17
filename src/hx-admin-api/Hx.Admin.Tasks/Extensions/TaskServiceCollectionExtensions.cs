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
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Quartz.Spi;
using Hx.Admin.Core;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Task服务注册
/// </summary>
public static class TaskServiceCollectionExtensions
{
    public static IServiceCollection AddQuartzService(this IServiceCollection services, IConfiguration configuration)
    {

        var jobTypes = App.EffectiveTypes.Where(t => typeof(IJob).IsAssignableFrom(t));

        services.AddQuartz();

        services.Configure<QuartzOptions,IOptions<DbSettingsOptions>>((quartzOptions,dbOptions) =>
        {
            var dbConfig = dbOptions.Value.ConnectionConfigs?.FirstOrDefault(r => r.ConfigId.ToString() == SqlSugarConst.Quartz_ConfigId);
            if(dbConfig == null)
                throw new ArgumentNullException(nameof(dbConfig));
            var config = SchedulerBuilder.Create();
            config.UsePersistentStore<MyJobStoreTX>(x =>
            {
                x.UseGenericDatabase(GetProvider(dbConfig.DbType), ado =>
                {
                    ado.ConnectionString = dbConfig.ConnectionString;
                });
                x.UseNewtonsoftJsonSerializer();
                x.PerformSchemaValidation = false;
            });
            string[] allKeys = config.Properties.AllKeys!;
            foreach (string text in allKeys)
            {
                if (text != null)
                {
                    quartzOptions[text] = config.Properties[text];
                }
            }
        });
        services.AddSingleton<IJobStore, MyJobStoreTX>();
        services.AddSingleton<ISchedulerListener, SampleSchedulerListener>();
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
