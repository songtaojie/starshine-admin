using Hx.Admin.Models;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 系统用户扩展机构服务
/// </summary>
public interface ISysUserExtOrgService : IBaseService<SysUserExtOrg>, IScopedDependency
{

    /// <summary>
    /// 获取用户扩展机构集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<SysUserExtOrg>> GetUserExtOrgList(long userId);

    /// <summary>
    /// 更新用户扩展机构
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="extOrgList"></param>
    /// <returns></returns>
    Task UpdateUserExtOrg(long userId, List<SysUserExtOrg> extOrgList);

    /// <summary>
    /// 根据机构Id集合删除扩展机构
    /// </summary>
    /// <param name="orgIdList"></param>
    /// <returns></returns>
    Task DeleteUserExtOrgByOrgIdList(IEnumerable<long> orgIdList);

    /// <summary>
    /// 根据用户Id删除扩展机构
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteUserExtOrgByUserId(long userId);

    /// <summary>
    /// 根据机构Id判断是否有用户
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns></returns>
    Task<bool> HasUserOrg(long orgId);

    /// <summary>
    /// 根据职位Id判断是否有用户
    /// </summary>
    /// <param name="posId"></param>
    /// <returns></returns>
    Task<bool> HasUserPos(long posId);
}