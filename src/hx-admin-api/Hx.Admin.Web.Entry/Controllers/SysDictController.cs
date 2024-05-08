// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels;
using Hx.Admin.Models.ViewModels.Config;
using Hx.Admin.Models.ViewModels.Dict;
using Hx.Admin.Models.ViewModels.Org;

namespace Hx.Admin.Web.Entry.Controllers;

/// <summary>
///字典管理控制器
/// </summary>
public class SysDictController : AdminControllerBase
{
    private readonly ISysDictTypeService _service;
    private readonly ISysDictDataService _sysDictDataService;

    /// <summary>
    /// <see cref="SysDictController"/>
    /// </summary>
    /// <param name="service"></param>
    /// <param name="sysDictDataService"></param>
    public SysDictController(ISysDictTypeService service, ISysDictDataService sysDictDataService)
    {
        _service = service;
        _sysDictDataService = sysDictDataService;
    }
    #region 字典类型
    /// <summary>
    /// 获取字典类型分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageDictTypeOutput>> GetDictTypePage([FromQuery] PageDictTypeInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// 获取字典类型列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<ListDictTypeOutput>> GetDictTypeList()
    {
        return await _service.GetList();
    }

    /// <summary>
    /// 增加字典类型
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddDictType(AddDictTypeInput input)
    {
        return await _service.InsertAsync(input);
    }

    /// <summary>
    /// 编辑字典类型
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> UpdateDictType(UpdateDictTypeInput input)
    {
        return await _service.UpdateAsync(input);
    }

    /// <summary>
    /// 修改字典类型状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> SetDictTypeStatus(SetDictTypeStatusInput input)
    {
        return await _service.SetStatus(input);
    }
    /// <summary>
    /// 删除字典类型
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteDictType(BaseIdParam input)
    {
        return await _service.DeleteAsync(input.Id);
    }
    #endregion

    #region 字典值
    /// <summary>
    /// 获取字典值分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageDictDataOutput>> GetDictDataPage([FromQuery] PageDictDataInput input)
    {
        return await _sysDictDataService.GetPage(input);
    }

    /// <summary>
    /// 获取字典值列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<ListDictDataOutput>> GetDictDataList([FromQuery]GetDataDictDataInput input)
    {
        return await _sysDictDataService.GetList(input);
    }

    /// <summary>
    /// 增加字典值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddDictData(AddDictDataInput input)
    {
        return await _sysDictDataService.InsertAsync(input);
    }

    /// <summary>
    /// 编辑字典类型
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> UpdateDictData(UpdateDictDataInput input)
    {
        return await _sysDictDataService.UpdateAsync(input);
    }

    /// <summary>
    /// 修改字典值状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> SetDictDataStatus(SetDictDataStatusInput input)
    {
        return await _sysDictDataService.SetStatus(input);
    }
    /// <summary>
    /// 删除字典值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteDictData(BaseIdParam input)
    {
        return await _sysDictDataService.DeleteAsync(input.Id);
    }

    /// <summary>
    /// 根据查询条件获取字典值集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<ListDictDataOutput>> GetDataList([FromQuery]QueryDictDataInput input)
    {
        return await _sysDictDataService.GetDataList(input);
    }
    #endregion
}