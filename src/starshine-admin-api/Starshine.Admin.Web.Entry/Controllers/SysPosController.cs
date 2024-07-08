// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.IService;
using Starshine.Admin.Models.ViewModels;
using Starshine.Admin.Models.ViewModels.Org;
using Starshine.Admin.Models.ViewModels.Pos;

namespace Starshine.Admin.Web.Entry.Controllers;

/// <summary>
/// 职位
/// </summary>
public class SysPosController : AdminControllerBase
{
    private readonly ISysPosService _service;
    /// <summary>
    /// 职位
    /// </summary>
    /// <param name="service"></param>
    public SysPosController(ISysPosService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取职位列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<ListSysPosOutput>> GetList([FromQuery] ListSysPosInput input)
    {
        return await _service.GetList(input);
    }

    /// <summary>
    /// 增加机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> Add(AddOrgInput input)
    {
        return await _service.InsertAsync<AddOrgInput>(input);
    }

    /// <summary>
    /// 增加机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Update(UpdateOrgInput input)
    {
        await _service.UpdateAsync<UpdateOrgInput>(input);
    }

    /// <summary>
    /// 删除机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task Delete(BaseIdParam input)
    {
        await _service.DeleteAsync(input.Id);
    }

}