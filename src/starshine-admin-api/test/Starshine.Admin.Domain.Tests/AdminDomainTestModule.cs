using Volo.Abp.Modularity;

namespace Starshine.Admin;

[DependsOn(
    typeof(AdminDomainModule),
    typeof(AdminTestBaseModule)
)]
public class AdminDomainTestModule : AbpModule
{

}
