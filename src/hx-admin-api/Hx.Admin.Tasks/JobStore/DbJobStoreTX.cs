// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Hosting;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Quartz.Spi;
using System.Data.Common;

namespace Hx.Admin.Tasks.JobStore;
public class DbJobStoreTX : JobStoreTX
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public DbJobStoreTX(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    public override Task Initialize(ITypeLoadHelper loadHelper, ISchedulerSignaler signaler, CancellationToken cancellationToken = default)
    {
        try
        {
            return base.Initialize(loadHelper, signaler, cancellationToken);
        }
        catch (SchedulerException)
        {
            StdAdoDelegate? adoDelegate = Delegate as StdAdoDelegate;
            if (adoDelegate == null) throw;

        }
    }

        private async Task CreateSchema(StdAdoDelegate adoDelegate, ConnectionAndTransactionHolder conn, CancellationToken cancellationToken)
        {
            var sqlFile = Path.Combine(_webHostEnvironment.WebRootPath, "quartz/tables_sqlite.sql");
            if (!File.Exists(sqlFile)) return;
            var commandText = await File.ReadAllTextAsync(sqlFile).ConfigureAwait(continueOnCapturedContext: false);
            if (!string.IsNullOrEmpty(commandText))
            {
                try
                {
                    using DbCommand cmd = adoDelegate.PrepareCommand(conn, commandText);
                    await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                catch (Exception ex)
                {
                    throw new JobPersistenceException("Cannot create a table based on this script file " + sqlFile + ": " + ex.Message, ex);
                }
            }
        }
    }
}
