using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Dict;
using Hx.Common;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 系统字典值服务
/// </summary>
public interface ISysDictDataService : IBaseService<SysDictData>, IScopedDependency
{

    /// <summary>
    /// 获取字典值分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<SysDictData>> GetPage(PageDictDataInput input);

    /// <summary>
    /// 获取字典值列表
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SysDictData>> GetList(GetDataDictDataInput input);

    /// <summary>
    /// 修改字典值状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> SetStatus(SetDictDataStatusInput input);

    /// <summary>
    /// 根据字典类型Id获取字典值集合
    /// </summary>
    /// <param name="dictTypeId"></param>
    /// <returns></returns>
    Task<List<SysDictData>> GetDictDataListByDictTypeId(long dictTypeId);

    /// <summary>
    /// 根据字典类型编码获取字典值集合
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<List<SysDictData>> GetDataList(string code);

    /// <summary>
    /// 根据查询条件获取字典值集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<SysDictData>> GetDataList(QueryDictDataInput input);

}