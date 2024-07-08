using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Menu;
using Starshine.DependencyInjection;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统用户角色服务
/// </summary>
public interface ISysUserRoleService : IBaseService<SysUserRole>, IScopedDependency
{

    /// <summary>
    /// 授权用户角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task GrantUserRole(UserRoleInput input);

    /// <summary>
    /// 根据角色Id删除用户角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task DeleteUserRoleByRoleId(long roleId);

    /// <summary>
    /// 根据用户Id删除用户角色
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteUserRoleByUserId(long userId);

    /// <summary>
    /// 根据用户Id获取角色集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<SysRole>> GetUserRoleList(long userId);

    /// <summary>
    /// 根据用户Id获取角色Id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<long>> GetUserRoleIdList(long userId);
}