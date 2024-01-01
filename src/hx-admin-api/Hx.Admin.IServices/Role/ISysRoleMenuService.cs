using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Role;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 系统角色菜单服务
/// </summary>
public interface ISysRoleMenuService : IBaseService<SysRoleMenu>, IScopedDependency
{

    /// <summary>
    /// 根据角色Id集合获取菜单Id集合
    /// </summary>
    /// <param name="roleIdList"></param>
    /// <returns></returns>
    Task<IEnumerable<long>> GetRoleMenuIdList(IEnumerable<long> roleIdList);

    /// <summary>
    /// 授权角色菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task GrantRoleMenu(RoleMenuInput input);

    /// <summary>
    /// 根据菜单Id集合删除角色菜单
    /// </summary>
    /// <param name="menuIdList"></param>
    /// <returns></returns>
    Task DeleteRoleMenuByMenuIdList(IEnumerable<long> menuIdList);
    /// <summary>
    /// 根据角色Id删除角色菜单
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task DeleteRoleMenuByRoleId(long roleId);
}