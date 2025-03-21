using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Admin.EntityFrameworkCore.Modeling
{
    internal static class StarshinePermissionManagementDbContextModelBuilderExtensions
    {
        public static void ConfigurePermissionManagement(this ModelBuilder builder)
        {
            Check.NotNull(builder, "builder");
            builder.Entity(delegate (EntityTypeBuilder<PermissionGrant> b)
            {
                b.ToTable(AbpPermissionManagementDbProperties.DbTablePrefix + "PermissionGrants", AbpPermissionManagementDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property((PermissionGrant x) => x.Name).HasMaxLength(PermissionDefinitionRecordConsts.MaxNameLength).IsRequired();
                b.Property((PermissionGrant x) => x.ProviderName).HasMaxLength(PermissionGrantConsts.MaxProviderNameLength).IsRequired();
                b.Property((PermissionGrant x) => x.ProviderKey).HasMaxLength(PermissionGrantConsts.MaxProviderKeyLength).IsRequired();
                b.HasIndex((PermissionGrant x) => new { x.TenantId, x.Name, x.ProviderName, x.ProviderKey }).IsUnique();
                b.ApplyObjectExtensionMappings();
            });
            if (builder.IsHostDatabase())
            {
                builder.Entity(delegate (EntityTypeBuilder<PermissionGroupDefinitionRecord> b)
                {
                    b.ToTable(AbpPermissionManagementDbProperties.DbTablePrefix + "PermissionGroups", AbpPermissionManagementDbProperties.DbSchema);
                    b.ConfigureByConvention();
                    b.Property((PermissionGroupDefinitionRecord x) => x.Name).HasMaxLength(PermissionGroupDefinitionRecordConsts.MaxNameLength).IsRequired();
                    b.Property((PermissionGroupDefinitionRecord x) => x.DisplayName).HasMaxLength(PermissionGroupDefinitionRecordConsts.MaxDisplayNameLength).IsRequired();
                    b.HasIndex((PermissionGroupDefinitionRecord x) => new { x.Name }).IsUnique();
                    b.ApplyObjectExtensionMappings();
                });
                builder.Entity(delegate (EntityTypeBuilder<PermissionDefinitionRecord> b)
                {
                    b.ToTable(AbpPermissionManagementDbProperties.DbTablePrefix + "Permissions", AbpPermissionManagementDbProperties.DbSchema);
                    b.ConfigureByConvention();
                    b.Property((PermissionDefinitionRecord x) => x.GroupName).HasMaxLength(PermissionGroupDefinitionRecordConsts.MaxNameLength).IsRequired();
                    b.Property((PermissionDefinitionRecord x) => x.Name).HasMaxLength(PermissionDefinitionRecordConsts.MaxNameLength).IsRequired();
                    b.Property((PermissionDefinitionRecord x) => x.ParentName).HasMaxLength(PermissionDefinitionRecordConsts.MaxNameLength);
                    b.Property((PermissionDefinitionRecord x) => x.DisplayName).HasMaxLength(PermissionDefinitionRecordConsts.MaxDisplayNameLength).IsRequired();
                    b.Property((PermissionDefinitionRecord x) => x.Providers).HasMaxLength(PermissionDefinitionRecordConsts.MaxProvidersLength);
                    b.Property((PermissionDefinitionRecord x) => x.StateCheckers).HasMaxLength(PermissionDefinitionRecordConsts.MaxStateCheckersLength);
                    b.HasIndex((PermissionDefinitionRecord x) => new { x.Name }).IsUnique();
                    b.HasIndex((PermissionDefinitionRecord x) => new { x.GroupName });
                    b.ApplyObjectExtensionMappings();
                });
            }

            builder.TryConfigureObjectExtensions<PermissionManagementDbContext>();
        }
    }
}
