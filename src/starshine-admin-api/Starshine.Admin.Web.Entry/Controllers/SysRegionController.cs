﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.IService;
using Starshine.Admin.Models.ViewModels;
using Starshine.Admin.Models.ViewModels.Org;
using Starshine.Admin.Models.ViewModels.Pos;
using Starshine.Admin.Models.ViewModels.Region;

namespace Starshine.Admin.Web.Entry.Controllers;

/// <summary>
/// 行政区域
/// </summary>
public class SysRegionController : AdminControllerBase
{
    private readonly ISysRegionService _service;
    /// <summary>
    /// 行政区域
    /// </summary>
    /// <param name="service"></param>
    public SysRegionController(ISysRegionService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取行政区域分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageRegionOutput>> GetPage([FromQuery] PageRegionInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// 获取行政区域分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<ListRegionOutput>> GetList([FromQuery] BaseIdParam input)
    {
        return await _service.GetList(input);
    }

    /// <summary>
    /// 增加行政区域
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> Add(AddRegionInput input)
    {
        return await _service.InsertAsync(input);
    }

    /// <summary>
    /// 编辑行政区域
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Update(UpdateRegionInput input)
    {
        await _service.UpdateAsync(input);
    }

    /// <summary>
    /// 删除行政区域
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task Delete(BaseIdParam input)
    {
        await _service.DeleteAsync(input.Id);
    }

    /// <summary>
    /// 同步行政区域
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task Sync()
    {
        await _service.Sync();
    }
}