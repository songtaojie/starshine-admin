using Starshine.Admin.Core;
using Starshine.DependencyInjection;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统常量服务
/// </summary>
public interface ISysConstService : IScopedDependency
{
    /// <summary>
    /// 获取所有常量列表
    /// </summary>
    /// <returns></returns>
   Task<IEnumerable<ConstOutput>> GetList();

    /// <summary>
    /// 根据类名获取常量数据
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    Task<IEnumerable<ConstOutput>> GetData(string typeName);
}