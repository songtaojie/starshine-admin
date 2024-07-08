namespace Starshine.Admin.Models.ViewModels.Role;

/// <summary>
/// 授权角色菜单
/// </summary>
public class RoleMenuInput : BaseIdParam
{
    /// <summary>
    /// 菜单Id集合
    /// </summary>
    public List<long> MenuIdList { get; set; }
}