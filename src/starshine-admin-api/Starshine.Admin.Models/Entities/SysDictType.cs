namespace Starshine.Admin.Models;

/// <summary>
/// 系统字典类型表
/// </summary>
[SugarTable(null, "系统字典类型表")]
[SystemTable]
public class SysDictType : AuditedEntityBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", IsNullable = true, Length = 64)]
    public virtual string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(ColumnDescription = "编码", IsNullable = true, Length = 64)]
    public string Code { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; } = 100;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注",IsNullable =true, Length = 256)]
    public string? Remark { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public StatusEnum Status { get; set; } = StatusEnum.Enable;
}