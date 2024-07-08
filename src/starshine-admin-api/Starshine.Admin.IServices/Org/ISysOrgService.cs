using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels;
using Starshine.Admin.Models.ViewModels.Org;
using Starshine.DependencyInjection;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统机构服务
/// </summary>
public interface ISysOrgService : IBaseService<SysOrg>, IScopedDependency
{
    /// <summary>
    /// 获取机构列表
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ListOrgOutput>> GetList(ListOrgInput input);

    /// <summary>
    /// 增加机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<long> AddOrg(AddOrgInput input);

    /// <summary>
    /// 更新机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateOrg(UpdateOrgInput input);

    /// <summary>
    /// 删除机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteOrg(BaseIdParam input);

    /// <summary>
    /// 根据用户Id获取机构Id集合
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<long>> GetUserOrgIdList();

    /// <summary>
    /// 根据节点Id获取子节点Id集合(包含自己)
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    Task<IEnumerable<long>> GetChildIdListWithSelfById(long pid);
}