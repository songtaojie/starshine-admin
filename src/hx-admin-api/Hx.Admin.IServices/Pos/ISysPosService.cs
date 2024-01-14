using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Pos;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 系统职位服务
/// </summary>
public interface ISysPosService : IBaseService<SysPos>, IScopedDependency
{
    /// <summary>
    /// 获取职位列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IEnumerable<ListSysPosOutput>> GetList(ListSysPosInput input);
}