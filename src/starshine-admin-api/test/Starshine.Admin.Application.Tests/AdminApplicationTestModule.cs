using Volo.Abp.Modularity;

namespace Starshine.Admin;

[DependsOn(
    typeof(AdminApplicationModule),
    typeof(AdminDomainTestModule)
)]
public class AdminApplicationTestModule : AbpModule
{

}
