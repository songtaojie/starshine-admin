// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Common;

namespace Starshine.Admin.Models;

/// <summary>
/// 代码生成表
/// </summary>
[SugarTable(null, "代码生成表")]
public class SysCodeGen : AuditedEntityBase
{
    /// <summary>
    /// 作者姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "作者姓名", IsNullable = true, Length = 32)]
    public string? AuthorName { get; set; }

    /// <summary>
    /// 是否移除表前缀
    /// </summary>
    [SugarColumn(ColumnDescription = "是否移除表前缀", IsNullable = true, Length = 8)]
    public string? TablePrefix { get; set; }

    /// <summary>
    /// 生成方式
    /// </summary>
    [SugarColumn(ColumnDescription = "生成方式", IsNullable = true, Length = 32)]
    public string? GenerateType { get; set; }

    /// <summary>
    /// 库定位器名
    /// </summary>
    [SugarColumn(ColumnDescription = "库定位器名", IsNullable = true, Length = 64)]
    public string? ConfigId { get; set; }

    /// <summary>
    /// 数据库名(保留字段)
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库库名", IsNullable = true, Length = 64)]
    public string? DbName { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库类型", IsNullable = true, Length = 64)]
    public string? DbType { get; set; }

    /// <summary>
    /// 数据库链接
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库链接", IsNullable = true, Length = 256)]
    public string? ConnectionString { get; set; }

    /// <summary>
    /// 数据库表名
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库表名", IsNullable = true, Length = 128)]
    public string? TableName { get; set; }

    /// <summary>
    /// 命名空间
    /// </summary>
    [SugarColumn(ColumnDescription = "命名空间", IsNullable = true, Length = 128)]
    public string? NameSpace { get; set; }

    /// <summary>
    /// 业务名
    /// </summary>
    [SugarColumn(ColumnDescription = "业务名", IsNullable =true, Length = 128)]
    public string? BusName { get; set; }

    /// <summary>
    /// 菜单编码
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单编码")]
    public long MenuPid { get; set; }

}