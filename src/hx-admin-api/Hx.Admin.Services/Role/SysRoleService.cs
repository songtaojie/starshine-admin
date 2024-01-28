using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Role;
using System.Diagnostics.CodeAnalysis;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统角色服务
/// </summary>
public class SysRoleService : BaseService<SysRole>, ISysRoleService
{
    private readonly UserManager _userManager;
    private readonly ICache _cache;
    private readonly ISysRoleOrgService _sysRoleOrgService;
    private readonly ISysRoleMenuService _sysRoleMenuService;
    private readonly ISysOrgService _sysOrgService;
    private readonly ISysUserRoleService _sysUserRoleService;

    public SysRoleService(UserManager userManager,
        ISqlSugarRepository<SysRole> sysRoleRep,
        ICache cache,
        ISysRoleOrgService sysRoleOrgService,
        ISysRoleMenuService sysRoleMenuService,
        ISysOrgService sysOrgService,
        ISysUserRoleService sysUserRoleService):base(sysRoleRep)
    {
        _userManager = userManager;
        _cache = cache;
        _sysRoleOrgService = sysRoleOrgService;
        _sysRoleMenuService = sysRoleMenuService;
        _sysOrgService = sysOrgService;
        _sysUserRoleService = sysUserRoleService;
    }

    /// <summary>
    /// 获取角色分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedListResult<PageRoleOutput>> GetPage(PageRoleInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code!.Contains(input.Code))
            .OrderBy(u => u.Sort)
            .OrderBy(u => u.CreateTime,OrderByType.Desc)
            .Select<PageRoleOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<RoleOutput>> GetList()
    {
        return await _rep.AsQueryable().OrderBy(u => u.Sort).Select<RoleOutput>().ToListAsync();
    }

    public override async Task<bool> BeforeInsertAsync(SysRole entity)
    {
        var isExist = await ExistAsync(u => u.Name == entity.Name);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{entity.Name}】的角色");
        isExist = await ExistAsync(u => u.Code == entity.Code);
        if (isExist)
            throw new UserFriendlyException($"已存在编号为【{entity.Name}】的角色");

        return await base.BeforeInsertAsync(entity);
    }

    public override async Task<bool> BeforeUpdateAsync(SysRole entity)
    {
        var isExist = await ExistAsync(u => u.Name == entity.Name && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{entity.Name}】的角色");
        isExist = await ExistAsync(u => u.Code == entity.Code && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在编号为【{entity.Name}】的角色");

        return await base.BeforeUpdateAsync(entity);
    }


    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task DeleteRole(DeleteRoleInput input)
    {
        var sysRole = await FirstOrDefaultAsync(u => u.Id == input.Id);
        if (sysRole.Code == CommonConst.SysAdminRole)
            throw new UserFriendlyException("禁止删除管理员角色");
        await DeleteAsync(sysRole);

        // 级联删除角色机构数据
        await _sysRoleOrgService.DeleteRoleOrgByRoleId(sysRole.Id);

        // 级联删除用户角色数据
        await _sysUserRoleService.DeleteUserRoleByRoleId(sysRole.Id);

        // 级联删除角色菜单数据
        await _sysRoleMenuService.DeleteRoleMenuByRoleId(sysRole.Id);
    }

    
    /// <summary>
    /// 授权角色数据范围
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task GrantDataScope(RoleOrgInput input)
    {
        // 删除所有用户机构缓存
        _cache.RemoveByPrefix(CacheConst.KeyOrgIdList);

        var role = await FirstOrDefaultAsync(u => u.Id == input.Id);
        var dataScope = input.DataScope;
        if (!_userManager.IsSuperAdmin)
        {
            // 非超级管理员没有全部数据范围权限
            if (dataScope == (int)DataScopeEnum.All)
                throw new UserFriendlyException("无该机构权限");

            // 若数据范围自定义，则判断授权数据范围是否有权限
            if (dataScope == (int)DataScopeEnum.Define)
            {
                var grantOrgIdList = input.OrgIdList;
                if (grantOrgIdList.Count > 0)
                {
                    var orgIdList = await _sysOrgService.GetUserOrgIdList();
                    if (!orgIdList.Any())
                        throw new UserFriendlyException("无该机构权限");
                    else if (!grantOrgIdList.All(u => orgIdList.Any(c => c == u)))
                        throw new UserFriendlyException("无该机构权限");
                }
            }
        }
        role.DataScope = (DataScopeEnum)dataScope;
        await _rep.Context.Updateable(role).UpdateColumns(u => new { u.DataScope }).ExecuteCommandAsync();
        await _sysRoleOrgService.GrantRoleOrg(input);
    }

    /// <summary>
    /// 根据角色Id获取菜单Id集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IEnumerable<long>> GetOwnMenuList(RoleInput input)
    {
        return await _sysRoleMenuService.GetRoleMenuIdList(new List<long> { input.Id });
    }

    /// <summary>
    /// 根据角色Id获取机构Id集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IEnumerable<long>> GetOwnOrgList(RoleInput input)
    {
        return await _sysRoleOrgService.GetRoleOrgIdList(new List<long> { input.Id });
    }

    /// <summary>
    /// 设置角色状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<int> SetStatus(RoleInput input)
    {
        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw new UserFriendlyException("状态值异常");

        return await _rep.Context.Updateable<SysRole>()
            .SetColumns(u => u.Status == input.Status)
            .Where(u => u.Id == input.Id)
            .ExecuteCommandAsync();
    }
}