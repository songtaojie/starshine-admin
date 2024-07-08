namespace Starshine.Admin.Models;

/// <summary>
/// 系统审计日志表
/// </summary>
[SugarTable(null, "系统审计日志表")]
public class SysLogAudit : CreationEntityBase<long>
{
    /// <summary>
    /// Sql
    /// </summary>
    [SugarColumn(ColumnDescription = "Sql", IsNullable = true, ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? Sql { get; set; }

    /// <summary>
    /// 参数  手动传入的参数
    /// </summary>
    [SugarColumn(ColumnDescription = "参数", IsNullable = true, ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? Parameters { get; set; }
        
    /// <summary>
    /// 审计类型
    /// </summary>
    public SqlAuditTypeEnum AuditType { get; set; }
}