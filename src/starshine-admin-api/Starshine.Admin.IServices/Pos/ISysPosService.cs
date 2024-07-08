using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Pos;
using Starshine.DependencyInjection;

namespace Starshine.Admin.IService;

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