using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.AuditLogging;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Admin.EntityFrameworkCore.Modeling
{
    internal static class StarshineAuditLoggingDbContextModelBuilderExtensions
    {
        internal static void ConfigureAuditLogging(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<AuditLog>(b =>
            {
                b.ToTable(AbpAuditLoggingDbProperties.DbTablePrefix + "AuditLogs", AbpAuditLoggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.ApplicationName).HasMaxLength(AuditLogConsts.MaxApplicationNameLength);
                b.Property(x => x.ClientIpAddress).HasMaxLength(AuditLogConsts.MaxClientIpAddressLength);
                b.Property(x => x.ClientName).HasMaxLength(AuditLogConsts.MaxClientNameLength);
                b.Property(x => x.ClientId).HasMaxLength(AuditLogConsts.MaxClientIdLength);
                b.Property(x => x.CorrelationId).HasMaxLength(AuditLogConsts.MaxCorrelationIdLength);
                b.Property(x => x.BrowserInfo).HasMaxLength(AuditLogConsts.MaxBrowserInfoLength);
                b.Property(x => x.HttpMethod).HasMaxLength(AuditLogConsts.MaxHttpMethodLength);
                b.Property(x => x.Url).HasMaxLength(AuditLogConsts.MaxUrlLength);
                b.Property(x => x.HttpStatusCode).HasColumnName(nameof(AuditLog.HttpStatusCode));

                b.Property(x => x.Comments).HasMaxLength(AuditLogConsts.MaxCommentsLength);
                b.Property(x => x.ExecutionDuration).HasColumnName(nameof(AuditLog.ExecutionDuration));
                b.Property(x => x.ImpersonatorTenantId).HasColumnName(nameof(AuditLog.ImpersonatorTenantId));
                b.Property(x => x.ImpersonatorUserId).HasColumnName(nameof(AuditLog.ImpersonatorUserId));
                b.Property(x => x.ImpersonatorTenantName).HasMaxLength(AuditLogConsts.MaxTenantNameLength);
                b.Property(x => x.ImpersonatorUserName).HasMaxLength(AuditLogConsts.MaxUserNameLength);
                b.Property(x => x.UserId).HasColumnName(nameof(AuditLog.UserId));
                b.Property(x => x.UserName).HasMaxLength(AuditLogConsts.MaxUserNameLength);
                b.Property(x => x.TenantId).HasColumnName(nameof(AuditLog.TenantId));
                b.Property(x => x.TenantName).HasMaxLength(AuditLogConsts.MaxTenantNameLength);

                b.HasMany(a => a.Actions).WithOne().HasForeignKey(x => x.AuditLogId).IsRequired();
                b.HasMany(a => a.EntityChanges).WithOne().HasForeignKey(x => x.AuditLogId).IsRequired();

                b.HasIndex(x => new { x.TenantId, x.ExecutionTime });
                b.HasIndex(x => new { x.TenantId, x.UserId, x.ExecutionTime });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<AuditLogAction>(b =>
            {
                b.ToTable(AbpAuditLoggingDbProperties.DbTablePrefix + "AuditLogActions", AbpAuditLoggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.AuditLogId).HasColumnName(nameof(AuditLogAction.AuditLogId));
                b.Property(x => x.ServiceName).HasMaxLength(AuditLogActionConsts.MaxServiceNameLength);
                b.Property(x => x.MethodName).HasMaxLength(AuditLogActionConsts.MaxMethodNameLength);
                b.Property(x => x.Parameters).HasMaxLength(AuditLogActionConsts.MaxParametersLength);
                b.Property(x => x.ExecutionTime).HasColumnName(nameof(AuditLogAction.ExecutionTime));
                b.Property(x => x.ExecutionDuration).HasColumnName(nameof(AuditLogAction.ExecutionDuration));

                b.HasIndex(x => new { x.AuditLogId });
                b.HasIndex(x => new { x.TenantId, x.ServiceName, x.MethodName, x.ExecutionTime });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<EntityChange>(b =>
            {
                b.ToTable(AbpAuditLoggingDbProperties.DbTablePrefix + "EntityChanges", AbpAuditLoggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.EntityTypeFullName).HasMaxLength(EntityChangeConsts.MaxEntityTypeFullNameLength).IsRequired();
                b.Property(x => x.EntityId).HasMaxLength(EntityChangeConsts.MaxEntityIdLength);
                b.Property(x => x.AuditLogId).IsRequired();
                b.Property(x => x.ChangeTime).IsRequired();
                b.Property(x => x.ChangeType).IsRequired();
                b.Property(x => x.TenantId).HasColumnName(nameof(EntityChange.TenantId));

                b.HasMany(a => a.PropertyChanges).WithOne().HasForeignKey(x => x.EntityChangeId);

                b.HasIndex(x => new { x.AuditLogId });
                b.HasIndex(x => new { x.TenantId, x.EntityTypeFullName, x.EntityId });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<EntityPropertyChange>(b =>
            {
                b.ToTable(AbpAuditLoggingDbProperties.DbTablePrefix + "EntityPropertyChanges", AbpAuditLoggingDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.NewValue).HasMaxLength(EntityPropertyChangeConsts.MaxNewValueLength);
                b.Property(x => x.PropertyName).HasMaxLength(EntityPropertyChangeConsts.MaxPropertyNameLength).IsRequired();
                b.Property(x => x.PropertyTypeFullName).HasMaxLength(EntityPropertyChangeConsts.MaxPropertyTypeFullNameLength).IsRequired();
                b.Property(x => x.OriginalValue).HasMaxLength(EntityPropertyChangeConsts.MaxOriginalValueLength);

                b.HasIndex(x => new { x.EntityChangeId });

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<AbpAuditLoggingDbContext>();
        }
    }
}
