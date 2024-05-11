// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Menu;

namespace Hx.Admin.Web.Entry.Controllers;

/// <summary>
/// 系统菜单
/// </summary>
public class SysMenuController : AdminControllerBase
{
    private readonly ISysMenuService _service;
    /// <summary>
    /// 系统菜单
    /// </summary>
    /// <param name="service"></param>
    public SysMenuController(ISysMenuService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取登录菜单树
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task Add(AddMenuInput input)
    {
       await _service.AddMenu(input);
    }
    /// <summary>
    /// 增加菜单
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task Update(UpdateMenuInput input)
    {
        await _service.UpdateMenu(input);
    }

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<MenuOutput>> GetLoginMenuTree()
    {
        return await _service.GetLoginMenuTree();
    }
    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<SysMenu>> GetList([FromQuery]MenuInput input)
    {
        return await _service.GetList(input);
    }
    /// <summary>
    /// 获取登录菜单树
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task Delete(DeleteMenuInput input)
    {
        await _service.DeleteMenu(input);
    }

    /// <summary>
    /// 获取用户拥有按钮权限集合
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<string>> GetOwnBtnPermList()
    {
       return await _service.GetOwnBtnPermList();
    }
}
