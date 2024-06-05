// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Models;
/// <summary>
/// UseDBLocks设置为true时会使用
/// 数据库的线程同步锁。
/// </summary>
[SugarTable("QRTZ_LOCKS", "数据库的线程同步锁")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzLocks
{
    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", IsNullable = false, IsPrimaryKey = true)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 锁名称
    /// </summary>
    [SugarColumn(ColumnDescription = "锁名称", ColumnName = "LOCK_NAME", IsNullable = false, IsPrimaryKey = true)]
    public string LockName { get; set; }
}