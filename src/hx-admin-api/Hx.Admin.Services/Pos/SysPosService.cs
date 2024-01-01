using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Pos;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统职位服务
/// </summary>
public class SysPosService : BaseService<SysPos>, ISysPosService
{
    private readonly ISysUserExtOrgService _sysUserExtOrgService;

    public SysPosService(ISqlSugarRepository<SysPos> sysPosRep,
        ISysUserExtOrgService sysUserExtOrgService):base(sysPosRep)
    {
        _sysUserExtOrgService = sysUserExtOrgService;
    }

    /// <summary>
    /// 获取职位列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IEnumerable<SysPos>> GetList(PosInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code!.Contains(input.Code.Trim()))
            .OrderBy(u => u.Sort)
            .OrderBy(u => u.CreateTime,OrderByType.Desc)
            .ToListAsync();
    }

    public override async Task<bool> BeforeInsertAsync(SysPos entity)
    {
        var isExist = await ExistAsync(u => u.Name == entity.Name );
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{entity.Name}】的职位");
        isExist = await ExistAsync(u => u.Code == entity.Code);
        if (isExist)
            throw new UserFriendlyException($"已存在编码为【{entity.Code}】的职位");
        return await base.BeforeInsertAsync(entity);
    }

    public override async Task<bool> BeforeUpdateAsync(SysPos entity)
    {
        var isExist = await ExistAsync(u => u.Name == entity.Name && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{entity.Name}】的职位");
        isExist = await ExistAsync(u => u.Code == entity.Code && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在编码为【{entity.Code}】的职位");
        return await base.BeforeUpdateAsync(entity);
    }

    public override async Task<bool> BeforeDeleteAsync(long id)
    {
        // 该职位下是否有用户
        var hasPosEmp = await _rep.Change<SysUser>()
            .AnyAsync(u => u.PosId == id);
        if (hasPosEmp)
            throw new UserFriendlyException("该职位已绑定用户，请先删除用户");

        // 该附属职位下是否有用户
        var hasExtPosEmp = await _sysUserExtOrgService.HasUserPos(id);
        if (hasExtPosEmp)
            throw new UserFriendlyException("附属职位已绑定用户，请先删除用户");
        return await base.BeforeDeleteAsync(id);
    }
}