using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Menu;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 系统菜单服务
/// </summary>
public interface ISysMenuService : IBaseService<SysMenu>, IScopedDependency
{

    /// <summary>
    /// 获取登录菜单树
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<MenuOutput>> GetLoginMenuTree();

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SysMenu>> GetList(MenuInput input);

    /// <summary>
    /// 增加菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task AddMenu(AddMenuInput input);

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateMenu(UpdateMenuInput input);

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteMenu(DeleteMenuInput input);


    /// <summary>
    /// 获取用户拥有按钮权限集合（缓存）
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<string?>> GetOwnBtnPermList();

    /// <summary>
    /// 获取系统所有按钮权限集合（缓存）
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<string?>> GetAllBtnPermList();

}