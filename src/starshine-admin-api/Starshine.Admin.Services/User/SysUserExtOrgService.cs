using Starshine.Admin.IService;
using Starshine.Admin.Models;

namespace Starshine.Admin.Core.Service;

/// <summary>
/// 系统用户扩展机构服务
/// </summary>
public class SysUserExtOrgService : BaseService<SysUserExtOrg>, ISysUserExtOrgService
{

    public SysUserExtOrgService(ISqlSugarRepository<SysUserExtOrg> rep):base(rep)
    {
    }

    /// <summary>
    /// 获取用户扩展机构集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<SysUserExtOrg>> GetUserExtOrgList(long userId)
    {
        return await _rep.GetListAsync(u => u.UserId == userId);
    }

    /// <summary>
    /// 更新用户扩展机构
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="extOrgList"></param>
    /// <returns></returns>
    public async Task UpdateUserExtOrg(long userId, List<SysUserExtOrg> extOrgList)
    {
        await _rep.DeleteAsync(u => u.UserId == userId);

        if (extOrgList == null || extOrgList.Count < 1) return;
        extOrgList.ForEach(u =>
        {
            u.UserId = userId;
        });
        await _rep.InsertRangeAsync(extOrgList);
    }

    /// <summary>
    /// 根据机构Id集合删除扩展机构
    /// </summary>
    /// <param name="orgIdList"></param>
    /// <returns></returns>
    public async Task DeleteUserExtOrgByOrgIdList(IEnumerable<long> orgIdList)
    {
        await _rep.DeleteAsync(u => orgIdList.Contains(u.OrgId));
    }

    /// <summary>
    /// 根据用户Id删除扩展机构
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task DeleteUserExtOrgByUserId(long userId)
    {
        await _rep.DeleteAsync(u => u.UserId == userId);
    }

    /// <summary>
    /// 根据机构Id判断是否有用户
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns></returns>
    public async Task<bool> HasUserOrg(long orgId)
    {
        return await ExistAsync(u => u.OrgId == orgId);
    }

    /// <summary>
    /// 根据职位Id判断是否有用户
    /// </summary>
    /// <param name="posId"></param>
    /// <returns></returns>
    public async Task<bool> HasUserPos(long posId)
    {
        return await ExistAsync(u => u.PosId == posId);
    }
}