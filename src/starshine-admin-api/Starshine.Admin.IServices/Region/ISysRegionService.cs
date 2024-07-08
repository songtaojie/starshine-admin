﻿using AngleSharp;
using AngleSharp.Html.Dom;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels;
using Starshine.Admin.Models.ViewModels.Region;
using Starshine.DependencyInjection;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统行政区域服务
/// </summary>
public interface ISysRegionService : IBaseService<SysRegion>, IScopedDependency
{

    /// <summary>
    /// 获取行政区域分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<PageRegionOutput>> GetPage(PageRegionInput input);

    /// <summary>
    /// 获取行政区域列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IEnumerable<ListRegionOutput>> GetList(BaseIdParam input);

    /// <summary>
    /// 删除行政区域
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteRegion(BaseIdParam input);

    /// <summary>
    /// 同步行政区域
    /// </summary>
    /// <returns></returns>
    Task Sync();
}