using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Logs;
using Starshine.DependencyInjection;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统访问日志服务
/// </summary>
public interface ISysLogVisService : IBaseService<SysLogVis>, IScopedDependency
{

    /// <summary>
    /// 获取访问日志分页列表
    /// </summary>
    /// <returns></returns>
    Task<PagedListResult<SysLogVisOutput>> GetPage(PageLogInput input);

    /// <summary>
    /// 清空访问日志
    /// </summary>
    /// <returns></returns>
    Task<bool> Clear();
}