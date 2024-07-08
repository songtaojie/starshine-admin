// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Common;

namespace Starshine.Admin.Models;

/// <summary>
/// 触发器
/// </summary>
[SugarTable("QRTZ_SIMPROP_TRIGGERS", "系统简单触发器")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzSimpropTriggers:EntityBase<int>
{
    /// <summary>
    /// 自增id
    /// </summary>
    [SugarColumn(ColumnDescription = "自增id", IsIdentity = true, IsPrimaryKey = true)]
    public override int Id { get; set; }

    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME",Length =120, IsNullable = false)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 触发器名字
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器名字", ColumnName = "TRIGGER_NAME", Length =150, IsNullable = false)]
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器分组", ColumnName = "TRIGGER_GROUP", Length =150, IsNullable = false)]
    public string TriggerGroup { get; set; }

    /// <summary>
    /// 字符串属性1
    /// </summary>
    [SugarColumn(ColumnDescription = "字符串属性1", ColumnName = "STR_PROP_1", Length =512, IsNullable = true)]
    public string? StrProp1 { get; set; }

    /// <summary>
    /// 字符串属性2
    /// </summary>
    [SugarColumn(ColumnDescription = "字符串属性2", ColumnName = "STR_PROP_2", Length =512, IsNullable = true)]
    public string? StrProp2 { get; set; }

    /// <summary>
    /// 字符串属性3
    /// </summary>
    [SugarColumn(ColumnDescription = "字符串属性3", ColumnName = "STR_PROP_3", Length = 512, IsNullable = true)]
    public string? StrProp3 { get; set; }

    /// <summary>
    /// 整型属性1
    /// </summary>
    [SugarColumn(ColumnDescription = "整型属性1", ColumnName = "INT_PROP_1", IsNullable = true)]
    public int? IntProp1 { get; set; }

    /// <summary>
    /// 整型属性2
    /// </summary>
    [SugarColumn(ColumnDescription = "整型属性2", ColumnName = "INT_PROP_2", IsNullable = true)]
    public int? IntProp2 { get; set; }

    /// <summary>
    /// 长整型属性1
    /// </summary>
    [SugarColumn(ColumnDescription = "长整型属性1", ColumnName = "LONG_PROP_1", IsNullable = true)]
    public long? LongProp1 { get; set; }

    /// <summary>
    /// 长整型属性2
    /// </summary>
    [SugarColumn(ColumnDescription = "长整型属性2", ColumnName = "LONG_PROP_2", IsNullable = true)]
    public long? LongProp2 { get; set; }

    /// <summary>
    /// 浮点型属性1
    /// </summary>
    [SugarColumn(ColumnDescription = "浮点型属性1", ColumnName = "DEC_PROP_1", IsNullable = true)]
    public decimal? DecProp1 { get; set; }

    /// <summary>
    /// 浮点型属性2
    /// </summary>
    [SugarColumn(ColumnDescription = "浮点型属性2", ColumnName = "DEC_PROP_2", IsNullable = true)]
    public decimal? DecProp2 { get; set; }

    /// <summary>
    /// boolean型属性1
    /// </summary>
    [SugarColumn(ColumnDescription = "boolean型属性1", ColumnName = "BOOL_PROP_1", IsNullable = true)]
    public bool? BoolProp1 { get; set; }

    /// <summary>
    /// boolean型属性2
    /// </summary>
    [SugarColumn(ColumnDescription = "boolean型属性2", ColumnName = "BOOL_PROP_2", IsNullable = true)]
    public bool? BoolProp2 { get; set; }

    /// <summary>
    ///时区id
    /// </summary>
    [SugarColumn(ColumnDescription = "时区id", ColumnName = "TIME_ZONE_ID",Length =80, IsNullable = false)]
    public string TimeZoneId { get; set; }
}