using Starshine.Admin.IService;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Logs;

namespace Starshine.Admin.Core.Service;

/// <summary>
/// 系统差异日志服务
/// </summary>
public class SysLogDiffService : BaseService<SysLogDiff>, ISysLogDiffService
{

    public SysLogDiffService(ISqlSugarRepository<SysLogDiff> sysLogDiffRep):base(sysLogDiffRep)
    {
    }

    /// <summary>
    /// 获取差异日志分页列表
    /// </summary>
    /// <returns></returns>
    public async Task<PagedListResult<SysLogDiffOutput>> GetPage(PageLogInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(input.StartTime.HasValue,u => u.CreateTime >= input.StartTime)
            .WhereIF(input.EndTime.HasValue, u => u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .Select<SysLogDiffOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 清空差异日志
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Clear()
    {
        return await _rep.DeleteAsync(u => u.Id > 0);
    }

    
}