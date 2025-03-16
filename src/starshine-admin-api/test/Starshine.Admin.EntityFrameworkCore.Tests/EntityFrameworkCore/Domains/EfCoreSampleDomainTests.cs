using Starshine.Admin.Samples;
using Xunit;

namespace Starshine.Admin.EntityFrameworkCore.Domains;

[Collection(AdminTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AdminEntityFrameworkCoreTestModule>
{

}
