// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
using Hx.Common.Extensions.LinqBuilder;
using Hx.Sdk.Core;
using Microsoft.AspNetCore.Hosting;
using Quartz;
using Quartz.Impl.AdoJobStore;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks.JobStore;
public class HxSQLiteDelegate: SQLiteDelegate
{
    public override async Task<int> ValidateSchema(ConnectionAndTransactionHolder conn, CancellationToken cancellationToken)
    {
        try
        {
            return await base.ValidateSchema(conn, cancellationToken);
        }
        catch(JobPersistenceException) 
        {
            await CreateSchema(conn, cancellationToken);
            return 0;
        }
    }

    private async Task CreateSchema(ConnectionAndTransactionHolder conn, CancellationToken cancellationToken)
    {
        var webHostEnvironment = App.GetService<IWebHostEnvironment>();
        var sqlFile = Path.Combine(webHostEnvironment.WebRootPath, "quartz/tables_sqlite.sql");
        if (!File.Exists(sqlFile)) return;
        var commandText = await File.ReadAllTextAsync(sqlFile).ConfigureAwait(continueOnCapturedContext: false);
        if (!string.IsNullOrEmpty(commandText))
        {
            try
            {
                using DbCommand cmd = PrepareCommand(conn, commandText);
                await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex)
            {
                throw new JobPersistenceException("Cannot create a table based on this script file " + sqlFile + ": " + ex.Message, ex);
            }
        }
    }
}
