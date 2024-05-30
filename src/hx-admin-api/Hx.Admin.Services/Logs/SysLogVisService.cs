using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Logs;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统访问日志服务
/// </summary>
public class SysLogVisService : BaseService<SysLogVis>, ISysLogVisService
{

    public SysLogVisService(ISqlSugarRepository<SysLogVis> sysLogVisRep):base(sysLogVisRep)
    {
    }

    /// <summary>
    /// 获取访问日志分页列表
    /// </summary>
    /// <returns></returns>
    public async Task<PagedListResult<SysLogVisOutput>> GetPage(PageLogInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(input.StartTime.HasValue, u => u.CreateTime >= input.StartTime)
            .WhereIF(input.EndTime.HasValue, u => u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .Select<SysLogVisOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 清空访问日志
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Clear()
    {
        return await _rep.DeleteAsync(u => u.Id > 0);
    }
}