// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
using Hx.Sqlsugar;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Quartz.Spi;
using System.Data.Common;

namespace Hx.Admin.Tasks.JobStore;
public class DbJobStoreTX : JobStoreTX
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly DbSettingsOptions _dbSettingsOptions;
    public DbJobStoreTX(IWebHostEnvironment webHostEnvironment,
        IOptions<DbSettingsOptions> options)
    {
        _webHostEnvironment = webHostEnvironment;
        _dbSettingsOptions = options.Value;
    }
    public override async Task Initialize(ITypeLoadHelper loadHelper, ISchedulerSignaler signaler, CancellationToken cancellationToken = default)
    {
        try
        {
            await base.Initialize(loadHelper, signaler, cancellationToken);
        }
        catch (SchedulerException)
        {
            StdAdoDelegate? adoDelegate = Delegate as StdAdoDelegate;
            if (adoDelegate == null) throw;
            await ExecuteWithoutLock<bool>((ConnectionAndTransactionHolder conn) => CreateSchema(adoDelegate,conn, cancellationToken), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private async Task<bool> CreateSchema(StdAdoDelegate adoDelegate, ConnectionAndTransactionHolder conn, CancellationToken cancellationToken)
    {
        var sqlFile = Path.Combine(_webHostEnvironment.WebRootPath, GetSqlFile());
        if (!File.Exists(sqlFile)) return false;
        var commandText = await File.ReadAllTextAsync(sqlFile).ConfigureAwait(continueOnCapturedContext: false);
        if (!string.IsNullOrEmpty(commandText))
        {
            try
            {
                using DbCommand cmd = adoDelegate.PrepareCommand(conn, commandText);
                await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                return true;
            }
            catch (Exception ex)
            {
                throw new JobPersistenceException("Cannot create a table based on this script file " + sqlFile + ": " + ex.Message, ex);
            }
        }
        return false;
    }

    private string GetSqlFile()
    {
        var dbConfig = _dbSettingsOptions.ConnectionConfigs?.FirstOrDefault(r => r.ConfigId?.ToString() == SqlSugarConst.Quartz_ConfigId);
        if (dbConfig == null)
            throw new ArgumentNullException(nameof(dbConfig),"缺少数据库连接字符串");
        switch (dbConfig.DbType)
        { 
            case SqlSugar.DbType.Sqlite:
                return "quartz/tables_sqlite.sql";
            case SqlSugar.DbType.MySql:
            case SqlSugar.DbType.MySqlConnector:
                return "quartz/tables_mysql_innodb.sql";
            case SqlSugar.DbType.PostgreSQL:
                return "quartz/tables_postgres.sql";
            case SqlSugar.DbType.Oracle:
                return "quartz/tables_oracle.sql";
            case SqlSugar.DbType.SqlServer:
                return "quartz/tables_sqlServer.sql";
            default:
                throw new NotImplementedException("不支持的DbType");
        }
    }
}
