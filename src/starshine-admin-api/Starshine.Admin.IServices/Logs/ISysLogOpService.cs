using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Logs;
using Starshine.DependencyInjection;
using Magicodes.ExporterAndImporter.Excel;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统操作日志服务
/// </summary>
public interface ISysLogOpService : IBaseService<SysLogOp>, IScopedDependency
{

    /// <summary>
    /// 获取操作日志分页列表
    /// </summary>
    /// <returns></returns>
    Task<PagedListResult<SysLogOpOutput>> GetPage(PageLogInput input);

    /// <summary>
    /// 清空操作日志
    /// </summary>
    /// <returns></returns>
    Task<bool> Clear();

    /// <summary>
    /// 导出操作日志
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ExportLogDto>> GetExportListAsync(LogInput input);
}