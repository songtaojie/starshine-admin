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
[SugarTable("QRTZ_BLOB_TRIGGERS", "系统Blob触发器")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzBlobTriggers//:EntityBase<int>
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
    /// 触发器名字
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器名字", ColumnName = "TRIGGER_NAME", ColumnDataType = "NVARCHAR(200)",IsNullable = false, IsPrimaryKey = true)]
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器分组", ColumnName = "TRIGGER_GROUP", ColumnDataType = "NVARCHAR(200)", IsNullable = false, IsPrimaryKey = true)]
    public string TriggerGroup { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    [SugarColumn(ColumnDescription = "数据", ColumnName = "BLOB_DATA", ColumnDataType = "BLOB", IsNullable = true)]
    public byte[] BlobData { get; set; }
}