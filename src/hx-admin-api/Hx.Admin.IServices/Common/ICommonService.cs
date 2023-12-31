using Hx.Admin.Models;
using Hx.Common.DependencyInjection;
using SqlSugar;

namespace Hx.Admin.IService;

public interface ICommonService: IScopedDependency
{
    Task<IEnumerable<EntityInfo>> GetEntityInfos();

    string GetHost();

    string GetFileUrl(SysFile sysFile);
}