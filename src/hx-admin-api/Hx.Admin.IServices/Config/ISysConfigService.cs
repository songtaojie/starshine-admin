using Hx.Admin.Core;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Config;
using Hx.Common;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 系统参数配置服务
/// </summary>
public interface ISysConfigService : IBaseService<SysConfig>,IScopedDependency
{

    /// <summary>
    /// 获取参数配置分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<PagedListResult<PageConfigOutput>> GetPage(PageConfigInput input);

    /// <summary>
    /// 获取参数配置列表
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<ListConfigOutput>> GetList();

    /// <summary>
    /// 删除参数配置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<bool> DeleteConfig(DeleteConfigInput input);

    /// <summary>
    /// 获取参数配置详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<DetailConfigOutput> GetDetail(long id);

    /// <summary>
    /// 获取参数配置值
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public Task<T?> GetConfigValue<T>(string code);

    /// <summary>
    /// 获取分组列表
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<string?>> GetGroupList();

    /// <summary>
    /// 获取 Token 过期时间
    /// </summary>
    /// <returns></returns>
    public Task<int> GetTokenExpire();

    /// <summary>
    /// 获取 RefreshToken 过期时间
    /// </summary>
    /// <returns></returns>
    public Task<int> GetRefreshTokenExpire();
}