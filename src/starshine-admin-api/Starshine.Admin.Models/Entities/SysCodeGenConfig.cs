using Starshine.Common;

namespace Starshine.Admin.Models;

/// <summary>
/// 代码生成字段配置表
/// </summary>
[SugarTable(null, "代码生成字段配置表")]
public class SysCodeGenConfig : AuditedEntityBase<long>
{
    /// <summary>
    /// 代码生成主表Id
    /// </summary>
    [SugarColumn(ColumnDescription = "主表Id")]
    public long CodeGenId { get; set; }

    /// <summary>
    /// 数据库字段名
    /// </summary>
    [SugarColumn(ColumnDescription = "字段名称", Length = 128)]
    public string ColumnName { get; set; }

    /// <summary>
    /// 字段描述
    /// </summary>
    [SugarColumn(ColumnDescription = "字段描述",IsNullable =true, Length = 128)]
    public string? ColumnComment { get; set; }

    /// <summary>
    /// .NET数据类型
    /// </summary>
    [SugarColumn(ColumnDescription = "NET数据类型", IsNullable = true, Length = 64)]
    public string? NetType { get; set; }

    /// <summary>
    /// 作用类型（字典）
    /// </summary>
    [SugarColumn(ColumnDescription = "作用类型", IsNullable = true, Length = 64)]
    public string? EffectType { get; set; }

    /// <summary>
    /// 外键实体名称
    /// </summary>
    [SugarColumn(ColumnDescription = "外键实体名称", IsNullable = true, Length = 64)]
    public string? FkEntityName { get; set; }

    /// <summary>
    /// 外键表名称
    /// </summary>
    [SugarColumn(ColumnDescription = "外键表名称", IsNullable = true, Length = 128)]
    public string? FkTableName { get; set; }

    /// <summary>
    /// 外键显示字段
    /// </summary>
    [SugarColumn(ColumnDescription = "外键显示字段", IsNullable = true, Length = 64)]
    public string? FkColumnName { get; set; }

    /// <summary>
    /// 外键显示字段.NET类型
    /// </summary>
    [SugarColumn(ColumnDescription = "外键显示字段.NET类型", IsNullable = true, Length = 64)]
    public string? FkColumnNetType { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    [SugarColumn(ColumnDescription = "字典编码", IsNullable = true, Length = 64)]
    public string? DictTypeCode { get; set; }

    /// <summary>
    /// 列表是否缩进（字典）
    /// </summary>
    [SugarColumn(ColumnDescription = "列表是否缩进", IsNullable = true, Length = 8)]
    public string? WhetherRetract { get; set; }

    /// <summary>
    /// 是否必填（字典）
    /// </summary>
    [SugarColumn(ColumnDescription = "是否必填", IsNullable = true, Length = 8)]
    public string? WhetherRequired { get; set; }

    /// <summary>
    /// 是否是查询条件
    /// </summary>
    [SugarColumn(ColumnDescription = "是否是查询条件", IsNullable = true, Length = 8)]
    public string? QueryWhether { get; set; }

    /// <summary>
    /// 查询方式
    /// </summary>
    [SugarColumn(ColumnDescription = "查询方式", IsNullable = true, Length = 16)]
    public string? QueryType { get; set; }

    /// <summary>
    /// 列表显示
    /// </summary>
    [SugarColumn(ColumnDescription = "列表显示", IsNullable = true, Length = 8)]
    public string? WhetherTable { get; set; }

    /// <summary>
    /// 增改
    /// </summary>
    [SugarColumn(ColumnDescription = "增改", IsNullable = true, Length = 8)]
    public string? WhetherAddUpdate { get; set; }

    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnDescription = "主键", IsNullable = true, Length = 8)]
    public string? ColumnKey { get; set; }

    /// <summary>
    /// 数据库中类型（物理类型）
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库中类型", IsNullable = true, Length = 64)]
    public string? DataType { get; set; }

    /// <summary>
    /// 是否通用字段
    /// </summary>
    [SugarColumn(ColumnDescription = "是否通用字段", IsNullable = true, Length = 8)]
    public string? WhetherCommon { get; set; }

    /// <summary>
    /// 显示文本字段
    /// </summary>
    [SugarColumn(ColumnDescription = "显示文本字段", IsNullable = true, ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? DisplayColumn { get; set; }

    /// <summary>
    /// 选中值字段
    /// </summary>
    [SugarColumn(ColumnDescription = "选中值字段", IsNullable = true, Length = 128)]
    public string? ValueColumn { get; set; }

    /// <summary>
    /// 父级字段
    /// </summary>
    [SugarColumn(ColumnDescription = "父级字段", IsNullable = true, Length = 128)]
    public string? PidColumn { get; set; }
}