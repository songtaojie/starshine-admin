using Starshine.Admin.Models;
using Starshine.DependencyInjection;
using SqlSugar;

namespace Starshine.Admin.IService;

public interface ICommonService: IScopedDependency
{
    string GetHost();

    string GetFileUrl(SysFile sysFile);
}