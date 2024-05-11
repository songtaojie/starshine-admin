// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Logs;
using Hx.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hx.Admin.IServices.Logs;

namespace Hx.Admin.Services.Logs;
/// <summary>
/// 系统异常日志服务
/// </summary>
public class SysLogExService : BaseService<SysLogEx>, ISysLogExService
{
    public SysLogExService(ISqlSugarRepository<SysLogEx> sysLogOpRep) : base(sysLogOpRep)
    {
    }

    /// <summary>
    /// 获取操作日志分页列表
    /// </summary>
    /// <returns></returns>
    public async Task<PagedListResult<SysLogExOutput>> GetPage(PageLogInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(input.StartTime.HasValue, u => u.CreateTime >= input.StartTime)
            .WhereIF(input.EndTime.HasValue, u => u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .Select<SysLogExOutput>()
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
    public async Task<List<ExportLogDto>> GetExportListAsync(LogInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(input.StartTime.HasValue, u => u.CreateTime >= input.StartTime)
            .WhereIF(input.EndTime.HasValue, u => u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .Select<ExportLogDto>()
            .ToListAsync();

    }
}