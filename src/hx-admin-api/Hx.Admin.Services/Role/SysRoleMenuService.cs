using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Role;
using Hx.Cache;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统角色菜单服务
/// </summary>
public class SysRoleMenuService : BaseService<SysRoleMenu>, ISysRoleMenuService
{
    private readonly ICache _cache;

    public SysRoleMenuService(ISqlSugarRepository<SysRoleMenu> sysRoleMenuRep,
        ICache cache):base(sysRoleMenuRep)
    {
        _cache = cache;
    }

    /// <summary>
    /// 根据角色Id集合获取菜单Id集合
    /// </summary>
    /// <param name="roleIdList"></param>
    /// <returns></returns>
    public async Task<IEnumerable<long>> GetRoleMenuIdList(IEnumerable<long> roleIdList)
    {
        return await _rep.AsQueryable()
            .Where(u => roleIdList.Contains(u.RoleId))
            .Select(u => u.MenuId).ToListAsync();
    }

    /// <summary>
    /// 授权角色菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task GrantRoleMenu(RoleMenuInput input)
    {
        await _rep.DeleteAsync(u => u.RoleId == input.Id);
        var menus = input.MenuIdList.Select(u => new SysRoleMenu
        {
            RoleId = input.Id,
            MenuId = u
        }).ToList();
        await _rep.InsertAsync(menus);

        // 清除缓存
        _cache.RemoveByPrefix(CacheConst.KeyMenu);
        _cache.RemoveByPrefix(CacheConst.KeyPermission);
    }

    /// <summary>
    /// 根据菜单Id集合删除角色菜单
    /// </summary>
    /// <param name="menuIdList"></param>
    /// <returns></returns>
    public async Task DeleteRoleMenuByMenuIdList(IEnumerable<long> menuIdList)
    {
        await _rep.DeleteAsync(u => menuIdList.Contains(u.MenuId));
    }

    /// <summary>
    /// 根据角色Id删除角色菜单
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task DeleteRoleMenuByRoleId(long roleId)
    {
        await _rep.DeleteAsync(u => u.RoleId == roleId);
    }
}