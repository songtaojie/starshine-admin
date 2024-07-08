namespace Starshine.Admin.Models;

/// <summary>
/// 假删除接口过滤器
/// </summary>
internal interface IDeletedFilter
{
    /// <summary>
    /// 软删除
    /// </summary>
    bool IsDeleted { get; set; }
}

/// <summary>
/// 机构Id接口过滤器
/// </summary>
public interface IOrgIdFilter
{
    /// <summary>
    /// 创建者部门Id
    /// </summary>
    long OrgId { get; set; }
}