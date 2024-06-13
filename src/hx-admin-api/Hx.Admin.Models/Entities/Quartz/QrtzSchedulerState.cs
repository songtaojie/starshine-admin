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
/// 集群调度状态
/// </summary>
[SugarTable("QRTZ_SCHEDULER_STATE", "系统简单触发器")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzSchedulerState : EntityBase<int>
{
    /// <summary>
    /// 自增id
    /// </summary>
    [SugarColumn(ColumnDescription = "自增id", IsIdentity = true, IsPrimaryKey = true)]
    public override int Id { get; set; }

    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", Length =120, IsNullable = false)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 实例名称
    /// </summary>
    [SugarColumn(ColumnDescription = "实例名称", ColumnName = "INSTANCE_NAME", Length =200, IsNullable = false)]
    public string InstanceName { get; set; }

    /// <summary>
    /// 上次检查时间
    /// </summary>
    [SugarColumn(ColumnDescription = "上次检查时间", ColumnName = "LAST_CHECKIN_TIME")]
    public long LastCheckinTime { get; set; }

    /// <summary>
    /// 集群检查频率
    /// </summary>
    [SugarColumn(ColumnDescription = "集群检查频率", ColumnName = "CHECKIN_INTERVAL")]
    public long CheckinInterval { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态",ColumnName ="STATE",IsNullable =true)]
    public SchedulerStateEnum? State { get; set; }
}