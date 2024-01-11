// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Menu;

namespace Hx.Admin.Web.Entry.Controllers;


public class SysMenuController : AdminControllerBase
{
    private readonly ISysMenuService _service;
    public SysMenuController(ISysMenuService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取登录菜单树
    /// </summary>
    /// <returns></returns>
    [HttpGet,ActionName("LoginMenuTree")]
    public async Task<IEnumerable<MenuOutput>> GetLoginMenuTree()
    {
       return await _service.GetLoginMenuTree();
    }
}
