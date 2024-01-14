// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Org;
using Hx.Admin.Models.ViewModels.Pos;
using Hx.Admin.Models.ViewModels.Role;

namespace Hx.Admin.Web.Entry.Controllers;


public class SysRoleController : AdminControllerBase
{
    private readonly ISysRoleService _service;
    public SysRoleController(ISysRoleService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取角色分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageRoleOutput>> GetPage([FromQuery] PageRoleInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// 增加角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> Add(AddRoleInput input)
    {
        return await _service.InsertAsync(input);
    }

    /// <summary>
    /// 编辑角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Update(UpdateRoleInput input)
    {
        await _service.UpdateAsync(input);
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task Delete(DeleteRoleInput input)
    {
        await _service.DeleteAsync(input.Id);
    }

    /// <summary>
    /// 根据角色Id获取菜单Id集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<long>> GetOwnMenuList([FromQuery]RoleInput input)
    {
        return await _service.GetOwnMenuList(input);
    }

    /// <summary>
    ///  根据角色Id获取机构Id集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<long>> GetOwnOrgList([FromQuery] RoleInput input)
    {
        return await _service.GetOwnOrgList(input);
    }

    /// <summary>
    ///  授权角色数据范围
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task GrantDataScope(RoleOrgInput input)
    {
        await _service.GrantDataScope(input);
    }

    /// <summary>
    ///  设置角色状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task SetStatus(RoleInput input)
    {
        await _service.SetStatus(input);
    }
}