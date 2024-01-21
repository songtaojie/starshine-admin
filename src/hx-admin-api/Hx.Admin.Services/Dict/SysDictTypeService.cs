using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Dict;
using Hx.Common;
using Hx.Sqlsugar;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统字典类型服务
/// </summary>
public class SysDictTypeService : BaseService<SysDictType>, ISysDictTypeService
{ 
    public SysDictTypeService(ISqlSugarRepository<SysDictType> sysDictTypeRep) : base(sysDictTypeRep)
    {

    }

    /// <summary>
    /// 获取字典类型分页列表
    /// </summary>
    /// <returns></returns>
    public async Task<PagedListResult<PageDictTypeOutput>> GetPage(PageDictTypeInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code!.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name!.Trim()))
            .OrderBy(u => u.Sort)
            .Select<PageDictTypeOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取字典类型列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ListDictTypeOutput>> GetList()
    {
        return await _rep.AsQueryable()
            .Where(u => u.Status == StatusEnum.Enable)
            .OrderBy(u => u.Sort)
            .Select<ListDictTypeOutput>()
            .ToListAsync();
    }

    public override async Task<bool> BeforeInsertAsync(SysDictType entity)
    {
        var isExist = await ExistAsync(u => u.Code == entity.Code);
        if (isExist)
            throw new UserFriendlyException($"已存在编号为【{entity.Code}】的字典");
        return await base.BeforeInsertAsync(entity);
    }

    public override async Task<bool> BeforeUpdateAsync(SysDictType entity)
    {
        var isExist = await ExistAsync(u => u.Id == entity.Id);
        if (!isExist)
            throw new UserFriendlyException($"当前字典信息不存在");
        isExist = await ExistAsync(u => u.Code == entity.Code && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在编号为【{entity.Code}】的字典");
        return await base.BeforeUpdateAsync(entity);
    }

    public override async Task<bool> BeforeDeleteAsync(long id)
    {
        var dictType = await FirstOrDefaultAsync(u => u.Id == id);
        if (dictType == null)
            throw new UserFriendlyException($"当前字典信息不存在");
        return await base.BeforeDeleteAsync(id);
    }

    /// <summary>
    /// 修改字典类型状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<bool> SetStatus(SetDictTypeStatusInput input)
    {
        var dictType = await FirstOrDefaultAsync(u => u.Id == input.Id);
        if (dictType == null)
            throw new UserFriendlyException($"当前字典信息不存在");

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw new UserFriendlyException($"状态值不正确");
        dictType.Status = input.Status;
        return await UpdateAsync(dictType);
    }
}