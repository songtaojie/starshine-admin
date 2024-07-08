using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Logs;
using Starshine.DependencyInjection;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统差异日志服务
/// </summary>
public interface ISysLogDiffService : IBaseService<SysLogDiff>, IScopedDependency
{
    /// <summary>
    /// 获取差异日志分页列表
    /// </summary>
    /// <returns></returns>
    Task<PagedListResult<SysLogDiffOutput>> GetPage(PageLogInput input);

    /// <summary>
    /// 清空差异日志
    /// </summary>
    /// <returns></returns>
    Task<bool> Clear();
}