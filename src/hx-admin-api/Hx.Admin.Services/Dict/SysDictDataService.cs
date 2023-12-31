using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Dict;
using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统字典值服务
/// </summary>
public class SysDictDataService : BaseService<SysDictData>, ISysDictDataService
{
    public SysDictDataService(ISqlSugarRepository<SysDictData> sysDictDataRep):base(sysDictDataRep)
    {
    }

    /// <summary>
    /// 获取字典值分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedListResult<SysDictData>> Page(PageDictDataInput input)
    {
        return await _rep.AsQueryable()
            .Where(u => u.DictTypeId == input.DictTypeId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code!.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Value), u => u.Value.Contains(input.Value!.Trim()))
            .OrderBy(u => u.Sort)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取字典值列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<SysDictData>> GetList(GetDataDictDataInput input)
    {
        return await GetDictDataListByDictTypeId(input.DictTypeId);
    }

    public override async Task<bool> BeforeInsertAsync(SysDictData entity)
    {
        var isExist = await ExistAsync(u => u.Code == entity.Code && u.DictTypeId == entity.DictTypeId);
        if (isExist)
            throw new UserFriendlyException($"当前字典已存在编码为【{entity.Code}】的值");
        return await base.BeforeInsertAsync(entity);
    }

    public override async Task<bool> BeforeUpdateAsync(SysDictData entity)
    {
        var isExist = await ExistAsync(u => u.Id == entity.Id);
        if (!isExist) 
            throw new UserFriendlyException($"当前字典信息不存在");

        isExist = await ExistAsync(u => u.Code == entity.Code && u.DictTypeId == entity.DictTypeId && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"当前字典已存在编码为【{entity.Code}】的值");
        return await base.BeforeUpdateAsync(entity);
    }

    public override async Task<bool> BeforeDeleteAsync(long id)
    {
        var isExist = await ExistAsync(u => u.Id == id);
        if (!isExist)
            throw new UserFriendlyException($"当前字典信息不存在");
        return await base.BeforeDeleteAsync(id);
    }

    /// <summary>
    /// 修改字典值状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<bool> SetStatus(SetDictDataStatusInput input)
    {
        var dictData = await FirstOrDefaultAsync(u => u.Id == input.Id);
        if (dictData == null)
            throw new UserFriendlyException($"当前字典信息不存在");

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw new UserFriendlyException($"状态值异常");

        dictData.Status = (StatusEnum)input.Status;
        return await UpdateAsync(dictData);
    }

    /// <summary>
    /// 根据字典类型Id获取字典值集合
    /// </summary>
    /// <param name="dictTypeId"></param>
    /// <returns></returns>
    public async Task<List<SysDictData>> GetDictDataListByDictTypeId(long dictTypeId)
    {
        return await _rep.AsQueryable()
            .Where(u => u.DictTypeId == dictTypeId)
            .OrderBy(u => u.Sort)
            .ToListAsync();
    }

    /// <summary>
    /// 根据字典类型编码获取字典值集合
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<List<SysDictData>> GetDataList(string code)
    {
        return await _rep.Context.Queryable<SysDictType>()
            .LeftJoin<SysDictData>((a, b) => a.Id == b.DictTypeId)
            .Where((a, b) => a.Code == code && a.Status == StatusEnum.Enable && b.Status == StatusEnum.Enable)
            .Select((a, b) => b).ToListAsync();
    }

    /// <summary>
    /// 根据查询条件获取字典值集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<SysDictData>> GetDataList(QueryDictDataInput input)
    {
        return await _rep.Context.Queryable<SysDictType>()
            .LeftJoin<SysDictData>((a, b) => a.Id == b.DictTypeId)
            .Where((a, b) => a.Code == input.Code)
            .WhereIF(input.Status.HasValue, (a, b) => b.Status == (StatusEnum)input.Status!.Value)
            .Select((a, b) => b).ToListAsync();
    }

    /// <summary>
    /// 根据字典类型Id删除字典值
    /// </summary>
    /// <param name="dictTypeId"></param>
    /// <returns></returns>
    public async Task DeleteDictData(long dictTypeId)
    {
        await _rep.DeleteAsync(u => u.DictTypeId == dictTypeId);
    }
}