namespace Starshine.Admin.Models;

/// <summary>
/// 系统作业集群表
/// </summary>
[SugarTable(null, "系统作业集群表")]
public class SysJobCluster : EntityBase
{
    /// <summary>
    /// 作业集群Id
    /// </summary>
    [SugarColumn(ColumnDescription = "作业集群Id", Length = 64)]
    public string ClusterId { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息",IsNullable =true, Length = 128)]
    public string? Description { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public int Status { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间")]
    public DateTime? UpdatedTime { get; set; }
}