using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Admin.EntityFrameworkCore
{
    /// <summary>
    /// DbContextOptionsBuilder扩展
    /// </summary>
    public static class DbContextOptionsBuilderExtension
    {
        /// <summary>
        /// 动态选择数据库
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="optionsBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder<TContext> UseDynamicSql<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, IConfiguration configuration)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseDynamicSql((DbContextOptionsBuilder)optionsBuilder, configuration);

        /// <summary>
        /// 动态选择数据库
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentException"></exception>
        public static DbContextOptionsBuilder UseDynamicSql(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            var dbType = configuration.GetConnectionString("DbType");
            var connectionString = ConnectionStringParser.ParseConnectionString(configuration.GetConnectionString(ConnectionStrings.DefaultConnectionStringName));
            switch (dbType?.ToLower())
            {
                case "mysql":
                    optionsBuilder.UseMySql(ServerVersion.AutoDetect(connectionString));
                    break;
                case "npgsql":
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                default:
                    optionsBuilder.UseSqlite(connectionString);
                    break;
            }
            return optionsBuilder;
        }

        /// <summary>
        /// 动态选择数据库
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentException"></exception>
        public static AbpDbContextOptions UseDynamicSql(this AbpDbContextOptions options, IConfiguration configuration)
        {
            var dbType = configuration.GetConnectionString("DbType");
            switch (dbType?.ToLower())
            {
                case "mysql":
                    options.UseMySQL();
                    break;
                case "postgresql":
                    options.UseNpgsql();
                    break;
                default:
                    options.UseSqlite();
                    break;
            }
            return options;
        }

        
    }
}
