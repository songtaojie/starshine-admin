﻿namespace Starshine.Admin.Models;

/// <summary>
/// 系统参数配置表
/// </summary>
[SugarTable(null, "系统参数配置表")]
public class SysConfig : AuditedEntityBase
{
    [SugarColumn(ColumnDescription = "Id", IsPrimaryKey =true)]
    public override long Id { get => base.Id; set => base.Id = value; }
    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", Length = 64)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(ColumnDescription = "编码",IsNullable =true, Length = 64)]
    public string? Code { get; set; }

    /// <summary>
    /// 属性值
    /// </summary>
    [SugarColumn(ColumnDescription = "属性值", IsNullable = true, Length = 64)]
    public string? Value { get; set; }

    /// <summary>
    /// 是否是内置参数（Y-是，N-否）
    /// </summary>
    [SugarColumn(ColumnDescription = "是否是内置参数")]
    public YesNoEnum SysFlag { get; set; }

    /// <summary>
    /// 分组编码
    /// </summary>
    [SugarColumn(ColumnDescription = "分组编码", IsNullable = true, Length = 64)]
    public string? GroupCode { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; } = 100;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 256)]
    public string? Remark { get; set; }
}