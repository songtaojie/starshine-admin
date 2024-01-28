using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels;
using Hx.Admin.Models.ViewModels.Org;
using Hx.Cache;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统机构服务
/// </summary>
public class SysOrgService : BaseService<SysOrg>, ISysOrgService
{
    private readonly UserManager _userManager;
    private readonly ICache _cache;
    private readonly ISysUserExtOrgService _sysUserExtOrgService;
    private readonly ISysUserRoleService _sysUserRoleService;
    private readonly ISysRoleOrgService _sysRoleOrgService;

    public SysOrgService(UserManager userManager,
        ISqlSugarRepository<SysOrg> sysOrgRep,
        ICache cache,
        ISysUserExtOrgService sysUserExtOrgService,
        ISysUserRoleService sysUserRoleService,
        ISysRoleOrgService sysRoleOrgService):base(sysOrgRep)
    {
        _userManager = userManager;
        _cache = cache;
        _sysUserExtOrgService = sysUserExtOrgService;
        _sysUserRoleService = sysUserRoleService;
        _sysRoleOrgService = sysRoleOrgService;
    }

    /// <summary>
    /// 获取机构列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ListOrgOutput>> GetList(ListOrgInput input)
    {
        var orgIdList = await GetUserOrgIdList();

        var query = _rep.AsQueryable().OrderBy(u => u.Sort).OrderByDescending(u => u.UpdateTime);

        // 条件筛选可能造成无法构造树（列表数据）
        if (!string.IsNullOrWhiteSpace(input.Name) || !string.IsNullOrWhiteSpace(input.Code))
        {
            return await query.WhereIF(orgIdList.Any(), u => orgIdList.Contains(u.Id))
                .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
                .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code!.Contains(input.Code))
                .Select(u => new ListOrgOutput
                {
                    Id = u.Id,
                    Code = u.Code,
                    Name = u.Name,
                    Pid = u.Pid,
                    Remark = u.Remark,
                    Sort = u.Sort,
                    Status = u.Status,
                    UpdateTime = u.UpdateTime ?? u.CreateTime,
                })
                .ToListAsync();
        }

        if (input.Id > 0)
        {
            return await query.WhereIF(orgIdList.Any(), u => orgIdList.Contains(u.Id))
                .Select(u=> new ListOrgOutput
                { 
                    Id = u.Id,
                    Code = u.Code,
                    Name = u.Name,
                    Pid = u.Pid,
                    Remark = u.Remark,
                    Sort = u.Sort,
                    Status = u.Status,
                    UpdateTime = u.UpdateTime ?? u.CreateTime,
                })
                .ToChildListAsync(u => u.Pid, input.Id, true);
        }
        else
        {
            return _userManager.IsSuperAdmin 
                ? await query.Select<ListOrgOutput>(u => new ListOrgOutput
                    {
                        Id = u.Id,
                        Code = u.Code,
                        Name = u.Name,
                        Pid = u.Pid,
                        Remark = u.Remark,
                        Sort = u.Sort,
                        Status = u.Status,
                        UpdateTime = u.UpdateTime ?? u.CreateTime,
                    }) .ToTreeAsync(u => u.Children, u => u.Pid, 0, u => u.Id) 
                : await query.Select(u => new ListOrgOutput
                    {
                        Id = u.Id,
                        Code = u.Code,
                        Name = u.Name,
                        Pid = u.Pid,
                        Remark = u.Remark,
                        Sort = u.Sort,
                        Status = u.Status,
                        UpdateTime = u.UpdateTime ?? u.CreateTime,
                    }).ToTreeAsync(u => u.Children, u => u.Pid, 0, orgIdList.Select(d => (object)d).ToArray());
        }
    }

    /// <summary>
    /// 增加机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddOrg(AddOrgInput input)
    {
        var isExist = await ExistAsync(u => u.Name == input.Name);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{input.Name}】的机构");
        isExist = await ExistAsync(u => u.Code == input.Code);
        if (isExist)
            throw new UserFriendlyException($"已存在编码为【{input.Code}】的机构");

        var orgIdList = await GetUserOrgIdList();
        if (!_userManager.IsSuperAdmin)
        {
            // 新增机构父Id不是0，则进行权限校验
            if (input.Pid != 0)
            {
                // 新增机构的父机构不在自己的数据范围内
                if (!orgIdList.Any() || !orgIdList.Contains(input.Pid))
                    throw new UserFriendlyException("没有权限操作机构");
            }
            // 删除当前用户的机构缓存
            _cache.Remove(CacheConst.KeyOrgIdList + _userManager.UserId);
        }
        var sysOrg = input.Adapt<SysOrg>();
        await _rep.InsertAsync(sysOrg);
        return sysOrg.Id;
    }

    /// <summary>
    /// 更新机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateOrg(UpdateOrgInput input)
    {
        if (input.Pid != 0)
        {
            var isExistPOrg = await ExistAsync(u => u.Id == input.Pid);
            if (!isExistPOrg) throw new UserFriendlyException("上级机构不存在");
        }
        if (input.Id == input.Pid)
            throw new UserFriendlyException("当前机构Id不能与上级机构Id相同");

        var isExist = await ExistAsync(u => u.Name == input.Name  && u.Id != input.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{input.Name}】的机构");
        isExist = await ExistAsync(u =>u.Code == input.Code && u.Id != input.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在编号为【{input.Code}】的机构");

        // 父Id不能为自己的子节点
        var childIdList = await GetChildIdListWithSelfById(input.Id);
        if (childIdList.Contains(input.Pid))
            throw new UserFriendlyException("当前机构Id不能与上级机构Id相同");

        // 是否有权限操作此机构
        var dataScopes = await GetUserOrgIdList();
        if (!_userManager.IsSuperAdmin && (!dataScopes.Any() || !dataScopes.Contains(input.Id)))
            throw new UserFriendlyException("没有权限操作机构");

        await _rep.Context.Updateable(input.Adapt<SysOrg>()).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task DeleteOrg(BaseIdParam input)
    {
        var sysOrg = await FirstOrDefaultAsync(u => u.Id == input.Id);

        // 是否有权限操作此机构
        if (!_userManager.IsSuperAdmin)
        {
            var dataScopes = await GetUserOrgIdList();
            if (!dataScopes.Any() || !dataScopes.Contains(sysOrg.Id))
                throw new UserFriendlyException("没有权限操作机构");
        }

        // 若机构有用户则禁止删除
        var orgHasUser = await _rep.Change<SysUser>()
            .AnyAsync(u => u.OrgId == input.Id);
        if (orgHasUser)  throw new UserFriendlyException("该机构下有用户，请先取消用户关联机构");

        // 若扩展机构有用户则禁止删除
        var hasExtOrgEmp = await _sysUserExtOrgService.HasUserOrg(sysOrg.Id);
        if (hasExtOrgEmp)
            throw new UserFriendlyException("附属机构下有用户，请先取消用户关联附属机构");

        // 若子机构有用户则禁止删除
        var orgTreeList = await _rep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id, true);
        var orgIdList = orgTreeList.Select(u => u.Id).ToList();

        // 若子机构有用户则禁止删除
        var cOrgHasEmp = await _rep.Change<SysUser>()
            .AnyAsync(u => orgIdList.Contains(u.OrgId));
        if (cOrgHasEmp) 
           throw new UserFriendlyException("下级机构下有用户禁止删除");

        // 级联删除机构子节点
        await _rep.DeleteAsync(u => orgIdList.Contains(u.Id));

        // 级联删除角色机构数据
        await _sysRoleOrgService.DeleteRoleOrgByOrgIdList(orgIdList);

        // 级联删除用户机构数据
        await _sysUserExtOrgService.DeleteUserExtOrgByOrgIdList(orgIdList);
    }

    /// <summary>
    /// 根据用户Id获取机构Id集合
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<long>> GetUserOrgIdList()
    {
        if (_userManager.IsSuperAdmin)
            return new List<long>();

        var userId = _userManager.GetUserId<long>();
        var orgId = _userManager.GetOrgId<long>();
        var orgIdList = _cache.Get<List<long>>($"{CacheConst.KeyOrgIdList}{userId}");
        if (orgIdList == null || orgIdList.Count < 1)
        {
            var orgList1 = await _sysUserExtOrgService.GetUserExtOrgList(userId);
            var orgList2 = await GetUserRoleOrgIdList(userId);
            orgIdList = orgList1.Select(u => u.OrgId).Union(orgList2).ToList();
            if (!orgIdList.Contains(orgId))
                orgIdList.Add(orgId);
            _cache.Set($"{CacheConst.KeyOrgIdList}{userId}", orgIdList); // 存缓存
        }
        return orgIdList;
    }

    /// <summary>
    /// 获取用户角色机构Id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<List<long>> GetUserRoleOrgIdList(long userId)
    {
        var roleList = await _sysUserRoleService.GetUserRoleList(userId);
        if (!roleList.Any())
            return new List<long>(); // 空机构Id集合

        return await GetUserOrgIdList(roleList);
    }

    /// <summary>
    /// 根据角色Id集合获取机构Id集合
    /// </summary>
    /// <param name="roleList"></param>
    /// <returns></returns>
    private async Task<List<long>> GetUserOrgIdList(IEnumerable<SysRole> roleList)
    {
        // 按最大范围策略设定(如果同时拥有ALL和SELF的权限，则结果ALL)
        int strongerDataScopeType = (int)DataScopeEnum.Self;

        // 数据范围拥有的角色集合
        var customDataScopeRoleIdList = new List<long>();
        if (roleList != null && roleList.Any())
        {
            foreach(var role in roleList) 
            {
                if (role.DataScope == DataScopeEnum.Define)
                    customDataScopeRoleIdList.Add(role.Id);
                else if ((int)role.DataScope <= strongerDataScopeType)
                    strongerDataScopeType = (int)role.DataScope;
            }
        }

        // 根据角色集合获取机构集合
        var orgIdList1 = await _sysRoleOrgService.GetRoleOrgIdList(customDataScopeRoleIdList);
        // 根据数据范围获取机构集合
        var orgIdList2 = await GetOrgIdListByDataScope(strongerDataScopeType);

        // 缓存当前用户最大角色数据范围
        _cache.Set(CacheConst.KeyMaxDataScopeType + _userManager.UserId, strongerDataScopeType);

        // 并集机构集合
        return orgIdList1.Union(orgIdList2).ToList();
    }

    /// <summary>
    /// 根据数据范围获取机构Id集合
    /// </summary>
    /// <param name="dataScope"></param>
    /// <returns></returns>
    private async Task<List<long>> GetOrgIdListByDataScope(int dataScope)
    {
        var orgId = _userManager.GetOrgId<long>();
        var orgIdList = new List<long>();
        // 若数据范围是全部，则获取所有机构Id集合
        if (dataScope == (int)DataScopeEnum.All)
        {
            orgIdList = await _rep.AsQueryable().Select(u => u.Id).ToListAsync();
        }
        // 若数据范围是本部门及以下，则获取本节点和子节点集合
        else if (dataScope == (int)DataScopeEnum.DeptChild)
        {
            orgIdList = (await GetChildIdListWithSelfById(orgId)).ToList();
        }
        // 若数据范围是本部门不含子节点，则直接返回本部门
        else if (dataScope == (int)DataScopeEnum.Dept)
        {
            orgIdList.Add(orgId);
        }
        return orgIdList;
    }

    /// <summary>
    /// 根据节点Id获取子节点Id集合(包含自己)
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    public async Task<IEnumerable<long>> GetChildIdListWithSelfById(long pid)
    {
        var orgTreeList = await _rep.AsQueryable().ToChildListAsync(u => u.Pid, pid, true);
        return orgTreeList.Select(u => u.Id).ToList();
    }
}