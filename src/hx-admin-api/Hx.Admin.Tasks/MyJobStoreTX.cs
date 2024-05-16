// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Dm;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Quartz.Spi;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
public class MyJobStoreTX: JobStoreTX
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<MyJobStoreTX> _logger;
    public MyJobStoreTX(ILogger<MyJobStoreTX> logger,
        IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }
    public override async Task Initialize(ITypeLoadHelper loadHelper, ISchedulerSignaler signaler, CancellationToken cancellationToken = default)
    {
        if (Delegate is StdAdoDelegate adoDelegate)
        {
            var objectCount = await ExecuteWithoutLock<int>(conn => ValidateAndCreateSchema(adoDelegate,conn, cancellationToken), cancellationToken).ConfigureAwait(false);
        }
        await base.Initialize(loadHelper, signaler, cancellationToken);
        _logger.LogInformation("CustomJobStore has been initialized, service provider is ServiceProviderType");
    }



    /// <summary>
    /// Validates the persistence schema and returns the number of validates objects.
    /// </summary>
    public virtual async Task<int> ValidateAndCreateSchema(StdAdoDelegate adoDelegate, ConnectionAndTransactionHolder conn, CancellationToken cancellationToken)
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
            catch (Exception ex)
            {
                throw new JobPersistenceException($"Unable to query against table {targetTable}: " + ex.Message, ex);
            }
        }

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
