using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Config;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统参数配置服务
/// </summary>
public class SysConfigService :BaseService<SysConfig>, ISysConfigService
{
    public SysConfigService(ISqlSugarRepository<SysConfig> rep):base(rep)
    {
    }

    /// <summary>
    /// 获取参数配置分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedListResult<PageConfigOutput>> GetPage(PageConfigInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code!.Contains(input.Code))
            .WhereIF(!string.IsNullOrWhiteSpace(input.GroupCode), u => u.GroupCode!.Equals(input.GroupCode))
            .OrderBy(u => u.Sort)
            .Select(u => new PageConfigOutput
            { 
                Id = u.Id,
                Name = u.Name,
                Code = u.Code,
                GroupCode = u.GroupCode,
                Sort = u.Sort,
                SysFlag = u.SysFlag,
                Remark = u.Remark,
                Value = u.Value
            })
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取参数配置列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ListConfigOutput>> GetList()
    {
        return await _rep.AsQueryable()
            .OrderBy(u => u.Sort)
            .Select<ListConfigOutput>()
            .ToListAsync();
    }

    public override async Task<bool> BeforeInsertAsync(SysConfig entity)
    {
        var isExist = await _rep.AsQueryable()
           .AnyAsync(u => u.Name == entity.Name);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{entity.Name}】的参数配置");
        isExist = await _rep.AsQueryable()
            .AnyAsync(u => u.Code == entity.Code);
        if (isExist)
            throw new UserFriendlyException($"已存在编码为【{entity.Code}】的参数配置");
        return await base.BeforeInsertAsync(entity);
    }

    public override async Task<bool> BeforeUpdateAsync(SysConfig entity)
    {
        var isExist = await _rep.AsQueryable()
           .AnyAsync(u => u.Name == entity.Name && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{entity.Name}】的参数配置");
        isExist = await _rep.AsQueryable()
            .AnyAsync(u => u.Code == entity.Code && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在编码为【{entity.Code}】的参数配置");
        return await base.BeforeUpdateAsync(entity);
    }

    /// <summary>
    /// 删除参数配置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<bool> DeleteConfig(DeleteConfigInput input)
    {
        var config = await _rep.AsQueryable()
            .Where(u => u.Id == input.Id)
            .Select(u => new {u.Id,u.SysFlag })
            .FirstAsync();
        if (config == null) throw new UserFriendlyException("当前配置信息不存在");
        if (config.SysFlag == YesNoEnum.Y) // 禁止删除系统参数
            throw new UserFriendlyException("禁止删除系统参数");
        return await DeleteAsync(config.Id);
    }

    /// <summary>
    /// 获取参数配置详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<DetailConfigOutput> GetDetail(long id)
    {
        return await _rep.AsQueryable()
            .Where(u => u.Id == id)
            .Select<DetailConfigOutput>()
            .FirstAsync();
    }

    /// <summary>
    /// 获取参数配置值
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<T?> GetConfigValue<T>(string code)
    {
        var config = await _rep.GetFirstAsync(u => u.Code == code);
        var value = config != null ? config.Value : default;
        if (string.IsNullOrWhiteSpace(value)) return default;
        return (T)Convert.ChangeType(value, typeof(T));
    }

    /// <summary>
    /// 获取分组列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string?>> GetGroupList()
    {
        return await _rep.AsQueryable().GroupBy(u => u.GroupCode).Select(u => u.GroupCode).ToListAsync();
    }

    /// <summary>
    /// 获取 Token 过期时间
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetTokenExpire()
    {
        var tokenExpireStr = await GetConfigValue<string>(CommonConst.SysTokenExpire);
        _ = int.TryParse(tokenExpireStr, out var tokenExpire);
        return tokenExpire == 0 ? 20 : tokenExpire;
    }

    /// <summary>
    /// 获取 RefreshToken 过期时间
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetRefreshTokenExpire()
    {
        var refreshTokenExpireStr = await GetConfigValue<string>(CommonConst.SysRefreshTokenExpire);
        _ = int.TryParse(refreshTokenExpireStr, out var refreshTokenExpire);
        return refreshTokenExpire == 0 ? 40 : refreshTokenExpire;
    }
}