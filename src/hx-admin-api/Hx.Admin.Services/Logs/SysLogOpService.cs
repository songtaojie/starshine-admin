using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Logs;
using Magicodes.ExporterAndImporter.Excel;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统操作日志服务
/// </summary>
public class SysLogOpService : BaseService<SysLogOp>, ISysLogOpService
{
    public SysLogOpService(ISqlSugarRepository<SysLogOp> sysLogOpRep):base(sysLogOpRep)
    {
    }

    /// <summary>
    /// 获取操作日志分页列表
    /// </summary>
    /// <returns></returns>
    public async Task<PagedListResult<SysLogOpOutput>> GetPage(PageLogInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(input.StartTime.HasValue,u => u.CreateTime >= input.StartTime)
            .WhereIF(input.EndTime.HasValue, u => u.CreateTime <= input.EndTime)
            //.OrderBy(u => u.CreateTime, OrderByType.Desc)
            .OrderBuilder(input)
            .Select<SysLogOpOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 清空操作日志
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Clear()
    {
        return await _rep.DeleteAsync(u => u.Id > 0) > 0;
    }

    /// <summary>
    /// 导出操作日志
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ExportLogDto>> GetExportListAsync(LogInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(input.StartTime.HasValue, u => u.CreateTime >= input.StartTime)
            .WhereIF(input.EndTime.HasValue, u => u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .Select<ExportLogDto>()
            .ToListAsync();
       
    }
}