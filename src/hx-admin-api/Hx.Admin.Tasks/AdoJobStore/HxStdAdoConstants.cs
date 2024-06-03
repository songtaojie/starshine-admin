// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Quartz.Impl.AdoJobStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
public class HxStdAdoConstants: StdAdoConstants
{
    // TableJobDetails columns names
    public const string ColumnCreateType = "CREATE_TYPE";
    public const string ColumnScriptCode = "SCRIPT_CODE";

    public static readonly string SqlUpdateJobDetailCreateType =
       $"UPDATE {TablePrefixSubst}{TableJobDetails} SET {ColumnCreateType} = @jobCreateType WHERE {ColumnSchedulerName} = @schedulerName AND {ColumnJobName} = @jobName AND {ColumnJobGroup} = @jobGroup";

}
