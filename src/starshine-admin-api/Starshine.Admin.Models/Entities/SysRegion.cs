﻿namespace Starshine.Admin.Models;

/// <summary>
/// 系统行政地区表
/// </summary>
[SugarTable(null, "系统行政地区表")]
public class SysRegion : EntityBase
{
    /// <summary>
    /// 父Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父Id")]
    public long Pid { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", Length = 64)]
    public string Name { get; set; }

    /// <summary>
    /// 简称
    /// </summary>
    [SugarColumn(ColumnDescription = "简称",IsNullable =true, Length = 32)]
    public string? ShortName { get; set; }

    /// <summary>
    /// 组合名
    /// </summary>
    [SugarColumn(ColumnDescription = "组合名",IsNullable =true, Length = 64)]
    public string? MergerName { get; set; }

    /// <summary>
    /// 行政代码
    /// </summary>
    [SugarColumn(ColumnDescription = "行政代码",Length = 32)]
    public string Code { get; set; }

    /// <summary>
    /// 邮政编码
    /// </summary>
    [SugarColumn(ColumnDescription = "邮政编码", IsNullable = true, Length = 6)]
    public string? ZipCode { get; set; }

    /// <summary>
    /// 区号
    /// </summary>
    [SugarColumn(ColumnDescription = "区号", IsNullable = true, Length = 6)]
    public string? CityCode { get; set; }

    /// <summary>
    /// 层级
    /// </summary>
    [SugarColumn(ColumnDescription = "层级")]
    public int Level { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    [SugarColumn(ColumnDescription = "拼音", IsNullable = true, Length = 128)]
    public string? PinYin { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    [SugarColumn(ColumnDescription = "经度")]
    public float Lng { get; set; }

    /// <summary>
    /// 维度
    /// </summary>
    [SugarColumn(ColumnDescription = "维度")]
    public float Lat { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; } = 100;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 128)]
    public string? Remark { get; set; }

    /// <summary>
    /// 机构子项
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<SysRegion> Children { get; set; }
}