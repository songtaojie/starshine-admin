using Volo.Abp.Modularity;

namespace Starshine.Admin;

public abstract class AdminApplicationTestBase<TStartupModule> : AdminTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
