using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Role;
using Starshine.DependencyInjection;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统角色机构服务
/// </summary>
public interface ISysRoleOrgService : IBaseService<SysRoleOrg>, IScopedDependency
{
   
    /// <summary>
    /// 授权角色机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
   Task GrantRoleOrg(RoleOrgInput input);

    /// <summary>
    /// 根据角色Id集合获取角色机构Id集合
    /// </summary>
    /// <param name="roleIdList"></param>
    /// <returns></returns>
    Task<IEnumerable<long>> GetRoleOrgIdList(IEnumerable<long> roleIdList);

    /// <summary>
    /// 根据机构Id集合删除角色机构
    /// </summary>
    /// <param name="orgIdList"></param>
    /// <returns></returns>
   Task DeleteRoleOrgByOrgIdList(IEnumerable<long> orgIdList);

    /// <summary>
    /// 根据角色Id删除角色机构
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task DeleteRoleOrgByRoleId(long roleId);
}