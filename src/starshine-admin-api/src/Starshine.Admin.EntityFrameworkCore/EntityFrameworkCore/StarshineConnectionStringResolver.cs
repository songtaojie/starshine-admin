using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Starshine.Admin.EntityFrameworkCore
{
    [ExposeServices(typeof(IConnectionStringResolver))]
    [Dependency(ReplaceServices =true)]
    public class StarshineConnectionStringResolver : DefaultConnectionStringResolver
    {
        public StarshineConnectionStringResolver(IOptionsMonitor<AbpDbConnectionOptions> options) : base(options)
        {
        }

        public override async Task<string> ResolveAsync(string? connectionStringName = null)
        {
            var connectionString = await base.ResolveAsync(connectionStringName);
            return ConnectionStringParser.ParseConnectionString(connectionString);

        }
    }
}
