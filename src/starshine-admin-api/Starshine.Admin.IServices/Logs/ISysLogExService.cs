// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.IService;
using Starshine.Admin.Models.ViewModels.Logs;
using Starshine.Admin.Models;
using Starshine.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Admin.IServices.Logs;

/// <summary>
/// 系统异常日志服务
/// </summary>
public interface ISysLogExService : IBaseService<SysLogEx>, IScopedDependency
{

    /// <summary>
    /// 获取异常日志分页列表
    /// </summary>
    /// <returns></returns>
    Task<PagedListResult<SysLogExOutput>> GetPage(PageLogInput input);

    /// <summary>
    /// 清空操作日志
    /// </summary>
    /// <returns></returns>
    Task<bool> Clear();

    /// <summary>
    /// 导出操作日志
    /// </summary>
    /// <returns></returns>
    Task<List<ExportLogDto>> GetExportListAsync(LogInput input);
}