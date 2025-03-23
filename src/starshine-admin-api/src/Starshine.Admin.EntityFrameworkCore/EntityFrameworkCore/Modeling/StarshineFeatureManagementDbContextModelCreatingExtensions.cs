﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.FeatureManagement;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Admin.EntityFrameworkCore.Modeling
{
    internal static class StarshineFeatureManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureFeatureManagement(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            if (builder.IsTenantOnlyDatabase())
            {
                return;
            }

            builder.Entity<FeatureValue>(b =>
            {
                b.ToStarshineTable(nameof(FeatureValue));

                b.Property(x => x.Name).HasMaxLength(FeatureValueConsts.MaxNameLength).IsRequired();
                b.Property(x => x.Value).HasMaxLength(FeatureValueConsts.MaxValueLength).IsRequired();
                b.Property(x => x.ProviderName).HasMaxLength(FeatureValueConsts.MaxProviderNameLength);
                b.Property(x => x.ProviderKey).HasMaxLength(FeatureValueConsts.MaxProviderKeyLength);

                b.HasIndex(x => new { x.Name, x.ProviderName, x.ProviderKey }).IsUnique();

                b.ApplyObjectExtensionMappings();
            });
            builder.Entity<FeatureGroupDefinitionRecord>(b =>
            {
                b.ToStarshineTable(nameof(FeatureGroupDefinitionRecord))
                    .ConfigureStarshineByConvention();

                b.Property(x => x.Name).HasMaxLength(FeatureGroupDefinitionRecordConsts.MaxNameLength).IsRequired();
                b.Property(x => x.DisplayName).HasMaxLength(FeatureGroupDefinitionRecordConsts.MaxDisplayNameLength).IsRequired();
                b.HasIndex(x => new { x.Name }).IsUnique();
                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<FeatureDefinitionRecord>(b =>
            {
                b.ToStarshineTable(nameof(FeatureDefinitionRecord))
                    .ConfigureStarshineByConvention();

                b.Property(x => x.GroupName).HasMaxLength(FeatureGroupDefinitionRecordConsts.MaxNameLength).IsRequired();
                b.Property(x => x.Name).HasMaxLength(FeatureDefinitionRecordConsts.MaxNameLength).IsRequired();
                b.Property(x => x.ParentName).HasMaxLength(FeatureDefinitionRecordConsts.MaxNameLength);
                b.Property(x => x.DisplayName).HasMaxLength(FeatureDefinitionRecordConsts.MaxDisplayNameLength).IsRequired();
                b.Property(x => x.Description).HasMaxLength(FeatureDefinitionRecordConsts.MaxDescriptionLength);
                b.Property(x => x.DefaultValue).HasMaxLength(FeatureDefinitionRecordConsts.MaxDefaultValueLength);
                b.Property(x => x.AllowedProviders).HasMaxLength(FeatureDefinitionRecordConsts.MaxAllowedProvidersLength);
                b.Property(x => x.ValueType).HasMaxLength(FeatureDefinitionRecordConsts.MaxValueTypeLength);

                b.HasIndex(x => new { x.Name }).IsUnique();
                b.HasIndex(x => new { x.GroupName });

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<FeatureManagementDbContext>();
        }
    }
}
