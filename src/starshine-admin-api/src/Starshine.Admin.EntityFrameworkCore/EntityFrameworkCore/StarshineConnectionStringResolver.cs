using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            // 动态替换解决方案根路径  
            if (connectionString.Contains("%SOLUTIONROOT%"))
            {
                var slnPath = GetSolutionDirectoryPath();
                connectionString = connectionString.Replace("%SOLUTIONROOT%", slnPath);
            }
            return connectionString;

        }

        private static string? GetSolutionDirectoryPath()
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (Directory.GetParent(currentDirectory.FullName) != null)
            {
                currentDirectory = Directory.GetParent(currentDirectory.FullName);
                if (currentDirectory == null) return null;
                if (Directory.GetFiles(currentDirectory.FullName).FirstOrDefault(f => f.EndsWith(".sln")) != null)
                {
                    return currentDirectory.FullName;
                }
            }

            return null;
        }
    }
}
