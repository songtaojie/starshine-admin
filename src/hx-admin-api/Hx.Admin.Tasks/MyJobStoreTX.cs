// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Dm;
using Hx.Admin.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Quartz.Spi;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
public class MyJobStoreTX: JobStoreTX
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<MyJobStoreTX> _logger;
    private readonly IServiceProvider _serviceProvider;
    public MyJobStoreTX(ILogger<MyJobStoreTX> logger,
        IWebHostEnvironment webHostEnvironment,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        _serviceProvider = serviceProvider;
    }
    public override async Task Initialize(ITypeLoadHelper loadHelper, ISchedulerSignaler signaler, CancellationToken cancellationToken = default)
    {
        await base.Initialize(loadHelper, signaler, cancellationToken);
        StdAdoDelegate? adoDelegate = Delegate as StdAdoDelegate;
        if (adoDelegate == null) return;
        try
        {
            int value = await ExecuteWithoutLock((ConnectionAndTransactionHolder conn) => ValidateAndCreateSchema(adoDelegate,conn, cancellationToken), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            _logger.LogInformation($"Successfully validated presence of {value} schema objects");
        }
        catch (Exception cause)
        {
            throw new SchedulerException("Database schema validation failed. Make sure you have created the database tables that Quartz requires using the database schema scripts. You can disable this check by setting quartz.jobStore.performSchemaValidation to false", cause);
        }
    }


    /// <summary>
    /// Validates the persistence schema and returns the number of validates objects.
    /// </summary>
    public virtual async Task<int> ValidateAndCreateSchema(StdAdoDelegate adoDelegate, ConnectionAndTransactionHolder conn, CancellationToken cancellationToken)
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                var quartzDb = db.AsTenant().GetConnectionScope(SqlSugarConst.Quartz_ConfigId);
            }
            await ValidateSchema(adoDelegate, conn, cancellationToken);
        }
        catch (JobPersistenceException)
        { 
            
        }
        return AllTableNames.Length;
    }

    private async Task<int> ValidateSchema(StdAdoDelegate adoDelegate, ConnectionAndTransactionHolder conn, CancellationToken cancellationToken)
    {
        foreach (var tableName in AllTableNames)
        {
            var targetTable = $"{TablePrefix}{tableName}";
            var sql = $"SELECT 1 FROM {targetTable}";
            try
            {
                using var cmd = adoDelegate.PrepareCommand(conn, sql);
                await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbException ex)
            {
                throw new JobPersistenceException($"Unable to query against table {targetTable}: " + ex.Message, ex);
            }
        }

        return AllTableNames.Length;
    }


    private async Task<int> CreateSchema(StdAdoDelegate adoDelegate, ConnectionAndTransactionHolder conn, CancellationToken cancellationToken)
    {
        

        return AllTableNames.Length;
    }


    private static readonly string[] AllTableNames = new[]
    {
        TableJobDetails,
        TableTriggers,
        TableSimpleTriggers,
        TableCronTriggers,
        TableBlobTriggers,
        TableFiredTriggers,
        TableCalendars,
        TablePausedTriggers,
        TableLocks,
        TableSchedulerState
    };
    //private virtual List<DbTableInfo> GetTableInfoList(bool isCache = true)
    //{
    //    string cacheKey = "DbMaintenanceProvider.GetTableInfoList" + this.Context.CurrentConnectionConfig.ConfigId;
    //    cacheKey = GetCacheKey(cacheKey);
    //    var result = new List<DbTableInfo>();
    //    if (isCache)
    //        result = GetListOrCache<DbTableInfo>(cacheKey, this.GetTableInfoListSql);
    //    else
    //        result = this.Context.Ado.SqlQuery<DbTableInfo>(this.GetTableInfoListSql);
    //    foreach (var item in result)
    //    {
    //        item.DbObjectType = DbObjectType.Table;
    //    }
    //    return result;
    //}
}
