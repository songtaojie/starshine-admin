using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Starshine.Admin.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class AdminDbContextFactory : IDesignTimeDbContextFactory<StarshineAdminDbContext>
{
    public StarshineAdminDbContext CreateDbContext(string[] args)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        AdminEfCoreEntityExtensionMappings.Configure();
        
        var builder = new DbContextOptionsBuilder<StarshineAdminDbContext>()
            //.UseNpgsql(configuration.GetConnectionString("Default"))
            .UseSqlite(GetDefaultConnectionString());

        return new StarshineAdminDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Starshine.Admin.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }

    public string? GetDefaultConnectionString()
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrEmpty(connectionString)) return connectionString;

        // 动态替换解决方案根路径  
        if (connectionString.Contains("%SOLUTIONROOT%"))
        {
            var slnPath = GetSolutionDirectoryPath();
            connectionString = connectionString.Replace("%SOLUTIONROOT%", slnPath);
        }
        return connectionString;
    }

    private string? GetSolutionDirectoryPath()
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
