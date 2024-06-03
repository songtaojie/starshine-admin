// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
using Quartz;
using Quartz.Impl.AdoJobStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
public class HxSqlServerDelegate : SqlServerDelegate
{
    private string schedName = null!;
    public override void Initialize(DelegateInitializationArgs args)
    {
        schedName = args.InstanceName;
        base.Initialize(args);
    }

    public override async Task<int> InsertJobDetail(ConnectionAndTransactionHolder conn, IJobDetail job, CancellationToken cancellationToken = default)
    {
        var insertResult = await base.InsertJobDetail(conn, job, cancellationToken);
        if (insertResult > 0)
        {
            using var cmd = PrepareCommand(conn, ReplaceTablePrefix(HxStdAdoConstants.SqlUpdateJobDetailCreateType));
            AddCommandParameter(cmd, "schedulerName", schedName);
            AddCommandParameter(cmd, "jobName", job.Key.Name);
            AddCommandParameter(cmd, "jobGroup", job.Key.Group);
            AddCommandParameter(cmd, "jobCreateType", (int)JobCreateTypeEnum.BuiltIn);
            await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
        return insertResult;
    }
}
