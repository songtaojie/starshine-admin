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

namespace Hx.Admin.Models.Entities.Quartz;

/// <summary>
/// 触发器
/// </summary>
[SugarTable("QRTZ_CALENDARS", "日历")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzCalendars//:EntityBase<int>
{
    ///// <summary>
    ///// 自增id
    ///// </summary>
    //[SugarColumn(ColumnDescription = "自增id", IsIdentity = true, IsPrimaryKey = true)]
    //public override int Id { get; set; }

    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", ColumnDataType = "NVARCHAR(120)", IsNullable = false, IsPrimaryKey = true)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 日历名字
    /// </summary>
    [SugarColumn(ColumnDescription = "日历名字", ColumnName = "CALENDAR_NAME", ColumnDataType = "NVARCHAR(200)", IsNullable = false, IsPrimaryKey = true)]
    public string CalendarName { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    [SugarColumn(ColumnDescription = "数据", ColumnName = "CALENDAR", ColumnDataType = "BLOB", IsNullable = false)]
    public byte[] Calendar { get; set; }
}