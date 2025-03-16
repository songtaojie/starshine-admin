using Xunit;

namespace Starshine.Admin.EntityFrameworkCore;

[CollectionDefinition(AdminTestConsts.CollectionDefinitionName)]
public class AdminEntityFrameworkCoreCollection : ICollectionFixture<AdminEntityFrameworkCoreFixture>
{

}
