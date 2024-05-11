// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Notice;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Org;
using Hx.Admin.Models.ViewModels;

namespace Hx.Admin.Web.Entry.Controllers;

/// <summary>
/// 机构
/// </summary>
public class SysOrgController : AdminControllerBase
{
    private readonly ISysOrgService _service;
    /// <summary>
    /// 机构
    /// </summary>
    /// <param name="service"></param>
    public SysOrgController(ISysOrgService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取机构列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<ListOrgOutput>> GetList([FromQuery] ListOrgInput input)
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
        return await _service.AddOrg(input);
    }

    /// <summary>
    /// 增加机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Update(UpdateOrgInput input)
    {
        await _service.UpdateOrg(input);
    }

    /// <summary>
    /// 删除机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task Delete(BaseIdParam input)
    {
        await _service.DeleteOrg(input);
    }
}