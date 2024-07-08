using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Role;
using Starshine.DependencyInjection;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统角色服务
/// </summary>
public interface ISysRoleService : IBaseService<SysRole>, IScopedDependency
{
    /// <summary>
    /// 获取角色分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<PageRoleOutput>> GetPage(PageRoleInput input);

    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<RoleOutput>> GetList();

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteRole(DeleteRoleInput input);

    /// <summary>
    /// 授权角色数据范围
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task GrantDataScope(RoleOrgInput input);

    /// <summary>
    /// 根据角色Id获取菜单Id集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IEnumerable<long>> GetOwnMenuList(RoleInput input);

    /// <summary>
    /// 根据角色Id获取机构Id集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IEnumerable<long>> GetOwnOrgList(RoleInput input);

    /// <summary>
    /// 设置角色状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<int> SetStatus(RoleInput input);
}