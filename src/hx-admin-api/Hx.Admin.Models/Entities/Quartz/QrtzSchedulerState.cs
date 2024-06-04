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
/// 触发器
/// </summary>
[SugarTable("QRTZ_SCHEDULER_STATE", "系统简单触发器")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzSchedulerState //: EntityBase<int>
{
    ///// <summary>
    ///// 自增id
    ///// </summary>
    //[SugarColumn(ColumnDescription = "自增id", IsIdentity = true, IsPrimaryKey = true)]
    //public override int Id { get; set; }

    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", ColumnDataType = "NVARCHAR(120)", IsNullable = false,IsPrimaryKey =true)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器分组", ColumnName = "INSTANCE_NAME", ColumnDataType = "NVARCHAR(200)", IsNullable = false,IsPrimaryKey =true)]
    public string InstanceName { get; set; }

    /// <summary>
    /// 下次触发时间
    /// </summary>
    [SugarColumn(ColumnDescription = "下次触发时间", ColumnName = "LAST_CHECKIN_TIME", ColumnDataType = "BIGINT")]
    public long LastCheckinTime { get; set; }

    /// <summary>
    /// 上次触发时间
    /// </summary>
    [SugarColumn(ColumnDescription = "上次触发时间", ColumnName = "CHECKIN_INTERVAL", ColumnDataType = "BIGINT")]
    public long CheckinInterval { get; set; }
}