using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Dict;
using Hx.Common;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 系统字典类型服务
/// </summary>
public interface ISysDictTypeService : IBaseService<SysDictType>, IScopedDependency
{

    /// <summary>
    /// 获取字典类型分页列表
    /// </summary>
    /// <returns></returns>
    Task<PagedListResult<PageDictTypeOutput>> GetPage(PageDictTypeInput input);

    /// <summary>
    /// 获取字典类型列表
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ListDictTypeOutput>> GetList();

    /// <summary>
    /// 修改字典类型状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SetStatus(SetDictTypeStatusInput input);
}