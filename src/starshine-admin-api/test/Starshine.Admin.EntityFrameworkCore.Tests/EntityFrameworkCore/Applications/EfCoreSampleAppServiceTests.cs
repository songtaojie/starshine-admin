using Starshine.Admin.Samples;
using Xunit;

namespace Starshine.Admin.EntityFrameworkCore.Applications;

[Collection(AdminTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AdminEntityFrameworkCoreTestModule>
{

}
