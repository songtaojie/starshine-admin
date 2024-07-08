using Starshine.Admin.IService;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Menu;

namespace Starshine.Admin.Core.Service;

/// <summary>
/// 系统菜单服务
/// </summary>
public class SysMenuService : BaseService<SysMenu>, ISysMenuService
{
    private readonly UserManager _userManager;
    private readonly ISysRoleMenuService _sysRoleMenuService;
    private readonly ISysUserRoleService _sysUserRoleService;
    private readonly ICache _cache;

    public SysMenuService(UserManager userManager,
        ISqlSugarRepository<SysMenu> sysMenuRep,
        ISysRoleMenuService sysRoleMenuService,
        ISysUserRoleService sysUserRoleService,
        ICache cache):base(sysMenuRep)
    {
        _userManager = userManager;
        _sysRoleMenuService = sysRoleMenuService;
        _sysUserRoleService = sysUserRoleService;
        _cache = cache;
    }

    /// <summary>
    /// 获取登录菜单树
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<MenuOutput>> GetLoginMenuTree()
    {
        if (_userManager.IsSuperAdmin)
        {
            var menuList = await _rep.AsQueryable()
                .Where(u => u.Type != MenuTypeEnum.Btn && u.Status == StatusEnum.Enable)
                .OrderBy(u => u.Sort).ToTreeAsync(u => u.Children, u => u.Pid, 0);
            return menuList.Adapt<List<MenuOutput>>();
        }
        else
        {
            var menuIdList = await GetMenuIdList();
            var menuTree = await _rep.AsQueryable()
                .Where(u => u.Status == StatusEnum.Enable)
                .OrderBy(u => u.Sort).ToTreeAsync(u => u.Children, u => u.Pid, 0, menuIdList.Select(d => (object)d).ToArray());
            DeleteBtnFromMenuTree(menuTree);
            return menuTree.Adapt<List<MenuOutput>>();
        }
    }

    /// <summary>
    /// 删除登录菜单树里面的按钮
    /// </summary>
    private void DeleteBtnFromMenuTree(List<SysMenu> menuList)
    {
        if (menuList == null) return;
        for (var i = menuList.Count - 1; i >= 0; i--)
        {
            var menu = menuList[i];
            if (menu.Type == MenuTypeEnum.Btn)
                menuList.Remove(menu);
            else if (menu.Children.Count > 0)
                DeleteBtnFromMenuTree(menu.Children);
        }
    }

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<SysMenu>> GetList( MenuInput input)
    {
        var menuIdList = _userManager.IsSuperAdmin ? new List<long>() : await GetMenuIdList();

        // 有筛选条件时返回list列表（防止构造不出树）
        if (!string.IsNullOrWhiteSpace(input.Title) || input.Type is > 0)
        {
            return await _rep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title))
                .WhereIF(input.Type is > 0, u => u.Type == input.Type)
                .WhereIF(menuIdList.Any(), u => menuIdList.Contains(u.Id))
                .OrderBy(u => u.Sort).ToListAsync();
        }

        return _userManager.IsSuperAdmin ?
            await _rep.AsQueryable().OrderBy(u => u.Sort).ToTreeAsync(u => u.Children, u => u.Pid, 0) :
            await _rep.AsQueryable()
                .OrderBy(u => u.Sort).ToTreeAsync(u => u.Children, u => u.Pid, 0, menuIdList.Select(d => (object)d).ToArray()); // 角色菜单授权时
    }

    /// <summary>
    /// 增加菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task AddMenu(AddMenuInput input)
    {
        var isExist = input.Type != MenuTypeEnum.Btn
            ? await ExistAsync(u => u.Title == input.Title)
            : await ExistAsync(u => u.Permission == input.Permission);

        if (isExist)
            throw new UserFriendlyException("已存在同名的菜单");

        // 校验菜单参数
        var sysMenu = input.Adapt<SysMenu>();
        CheckMenuParam(sysMenu);

        await InsertAsync(sysMenu);

        // 清除缓存
        DeleteMenuCache();
    }

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateMenu(UpdateMenuInput input)
    {
        var isExist = input.Type != MenuTypeEnum.Btn
            ? await ExistAsync(u => u.Title == input.Title && u.Type == input.Type && u.Id != input.Id)
            : await ExistAsync(u => u.Permission == input.Permission && u.Type == input.Type && u.Id != input.Id);
        if (isExist)
            throw new UserFriendlyException("已存在同名的菜单");

        // 校验菜单参数
        var sysMenu = input.Adapt<SysMenu>();
        CheckMenuParam(sysMenu);

        await _rep.UpdateAsync(sysMenu);

        // 清除缓存
        DeleteMenuCache();
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task DeleteMenu(DeleteMenuInput input)
    {
        var menuTreeList = await _rep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id, true);
        var menuIdList = menuTreeList.Select(u => u.Id).ToList();

        await _rep.DeleteAsync(u => menuIdList.Contains(u.Id));

        // 级联删除角色菜单数据
        await _sysRoleMenuService.DeleteRoleMenuByMenuIdList(menuIdList);

        // 清除缓存
        DeleteMenuCache();
    }

    /// <summary>
    /// 增加和编辑时检查菜单数据
    /// </summary>
    /// <param name="menu"></param>
    private static void CheckMenuParam(SysMenu menu)
    {
        var permission = menu.Permission;
        if (menu.Type == MenuTypeEnum.Btn)
        {
            menu.Name = null;
            menu.Path = null;
            menu.Component = null;
            menu.Icon = null;
            menu.Redirect = null;
            menu.OutLink = null;
            menu.IsHide = false;
            menu.IsKeepAlive = true;
            menu.IsAffix = false;
            menu.IsIframe = false;

            if (string.IsNullOrEmpty(permission))
                throw new UserFriendlyException("权限标识不能为空");
            if (!permission.Contains(':'))
                throw new UserFriendlyException("权限标识不正确");
        }
        else
        {
            menu.Permission = null;
        }
    }

    /// <summary>
    /// 获取拥有的路由权限
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string?>> GetOwnRouteList()
    {
        var userId = _userManager.UserId;
        var cacheKey = $"{CacheConst.Key_Route}{userId}";
        var permissions = _cache.Get<List<string?>>(cacheKey);
        if (permissions == null || permissions.Count == 0)
        {
            var menuIdList = _userManager.IsSuperAdmin ? new List<long>() : await GetMenuIdList();
            permissions = await _rep.AsQueryable()
                .Where(u => u.Type == MenuTypeEnum.Menu)
                .WhereIF(menuIdList.Any(), u => menuIdList.Contains(u.Id))
                .Select(u => u.Path).ToListAsync();
            _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(10));
        }
        return permissions;
    }

    /// <summary>
    /// 获取用户拥有按钮权限集合（缓存）
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string?>> GetOwnBtnPermList()
    {
        var userId = _userManager.UserId;
        var cacheKey = $"{CacheConst.KeyPermission}{userId}";
        var permissions = _cache.Get<List<string?>>(cacheKey);
        if (permissions == null || permissions.Count == 0)
        {
            var menuIdList = _userManager.IsSuperAdmin ? new List<long>() : await GetMenuIdList();
            permissions = await _rep.AsQueryable()
                .Where(u => u.Type == MenuTypeEnum.Btn)
                .WhereIF(menuIdList.Any(), u => menuIdList.Contains(u.Id))
                .Select(u => u.Permission).ToListAsync();
            _cache.Set(cacheKey, permissions,TimeSpan.FromMinutes(10));
        }
        return permissions;
    }

    /// <summary>
    /// 获取系统所有按钮权限集合（缓存）
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string?>> GetAllBtnPermList()
    {
        var cacheKey = $"{CacheConst.KeyPermission}0";
        IEnumerable<string?> permissions = _cache.Get<List<string>>(cacheKey);
        if (permissions == null || !permissions.Any())
        {
            permissions = await _rep.AsQueryable()
                .Where(u => u.Type == MenuTypeEnum.Btn)
                .Select(u => u.Permission).ToListAsync();
            _cache.Set(cacheKey, permissions);
        }
        return permissions;
    }

    /// <summary>
    /// 清除菜单和按钮缓存
    /// </summary>
    private void DeleteMenuCache()
    {
        _cache.RemoveByPrefix(CacheConst.KeyMenu);
        _cache.RemoveByPrefix(CacheConst.KeyPermission);
    }

    /// <summary>
    /// 获取当前用户菜单Id集合
    /// </summary>
    /// <returns></returns>
    private async Task<IEnumerable<long>> GetMenuIdList()
    {
        var roleIdList = await _sysUserRoleService.GetUserRoleIdList(_userManager.GetUserId<long>());
        return await _sysRoleMenuService.GetRoleMenuIdList(roleIdList);
    }
}