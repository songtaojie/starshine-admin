using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Role;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统角色机构服务
/// </summary>
public class SysRoleOrgService : BaseService<SysRoleOrg>, ISysRoleOrgService
{

    public SysRoleOrgService(ISqlSugarRepository<SysRoleOrg> sysRoleOrgRep):base(sysRoleOrgRep)
    {
    }

    /// <summary>
    /// 授权角色机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task GrantRoleOrg(RoleOrgInput input)
    {
        await _rep.DeleteAsync(u => u.RoleId == input.Id);
        if (input.DataScope == (int)DataScopeEnum.Define)
        {
            var roleOrgs = input.OrgIdList.Select(u => new SysRoleOrg
            {
                RoleId = input.Id,
                OrgId = u
            }).ToList();
            await _rep.InsertRangeAsync(roleOrgs);
        }
    }

    /// <summary>
    /// 根据角色Id集合获取角色机构Id集合
    /// </summary>
    /// <param name="roleIdList"></param>
    /// <returns></returns>
    public async Task<IEnumerable<long>> GetRoleOrgIdList(IEnumerable<long> roleIdList)
    {
        return await _rep.AsQueryable()
            .Where(u => roleIdList.Contains(u.RoleId))
            .Select(u => u.OrgId).ToListAsync();
    }

    /// <summary>
    /// 根据机构Id集合删除角色机构
    /// </summary>
    /// <param name="orgIdList"></param>
    /// <returns></returns>
    public async Task DeleteRoleOrgByOrgIdList(IEnumerable<long> orgIdList)
    {
        await _rep.DeleteAsync(u => orgIdList.Contains(u.OrgId));
    }

    /// <summary>
    /// 根据角色Id删除角色机构
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task DeleteRoleOrgByRoleId(long roleId)
    {
        await _rep.DeleteAsync(u => u.RoleId == roleId);
    }
}