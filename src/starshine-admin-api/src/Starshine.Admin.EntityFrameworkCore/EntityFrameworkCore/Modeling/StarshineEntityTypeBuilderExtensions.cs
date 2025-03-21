using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore.ValueComparers;
using Volo.Abp.EntityFrameworkCore.ValueConverters;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Volo.Abp;
using Microsoft.EntityFrameworkCore;

namespace Starshine.Admin.EntityFrameworkCore.Modeling
{
    internal static class StarshineEntityTypeBuilderExtensions
    {
        public static void ConfigureByConvention(this EntityTypeBuilder b)
        {
            b.TryConfigureConcurrencyStamp();
            b.TryConfigureExtraProperties();
            b.TryConfigureObjectExtensions();
            b.TryConfigureMayHaveCreator();
            b.TryConfigureMustHaveCreator();
            b.TryConfigureSoftDelete();
            b.TryConfigureDeletionTime();
            b.TryConfigureDeletionAudited();
            b.TryConfigureCreationTime();
            b.TryConfigureLastModificationTime();
            b.TryConfigureModificationAudited();
            b.TryConfigureMultiTenant();
        }

        public static void ConfigureConcurrencyStamp<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasConcurrencyStamp
        {
            b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
        }

        public static void TryConfigureConcurrencyStamp(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IHasConcurrencyStamp>())
            {
                b.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                    .IsConcurrencyToken()
                    .HasMaxLength(ConcurrencyStampConsts.MaxLength);
            }
        }

        public static void ConfigureExtraProperties<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasExtraProperties
        {
            b.As<EntityTypeBuilder>().TryConfigureExtraProperties();
        }

        public static void TryConfigureExtraProperties(this EntityTypeBuilder b)
        {
            if (!b.Metadata.ClrType.IsAssignableTo<IHasExtraProperties>())
            {
                return;
            }

            b.Property<ExtraPropertyDictionary>(nameof(IHasExtraProperties.ExtraProperties))
                .HasConversion(new ExtraPropertiesValueConverter(b.Metadata.ClrType))
                .Metadata.SetValueComparer(new ExtraPropertyDictionaryValueComparer());

            b.TryConfigureObjectExtensions();
        }

        public static void ConfigureObjectExtensions<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasExtraProperties
        {
            b.As<EntityTypeBuilder>().TryConfigureObjectExtensions();
        }

        public static void TryConfigureObjectExtensions(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IHasExtraProperties>())
            {
                ObjectExtensionManager.Instance.ConfigureEfCoreEntity(b);
            }
        }

        public static void ApplyObjectExtensionMappings(this EntityTypeBuilder b)
        {
            ObjectExtensionManager.Instance.ConfigureEfCoreEntity(b);
        }

        public static void ConfigureSoftDelete<T>(this EntityTypeBuilder<T> b)
            where T : class, ISoftDelete
        {
            b.As<EntityTypeBuilder>().TryConfigureSoftDelete();
        }

        public static void TryConfigureSoftDelete(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
            {
                b.Property(nameof(ISoftDelete.IsDeleted))
                    .IsRequired()
                    .HasDefaultValue(false);
            }
        }

        public static void ConfigureDeletionTime<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasDeletionTime
        {
            b.As<EntityTypeBuilder>().TryConfigureDeletionTime();
        }

        public static void TryConfigureDeletionTime(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IHasDeletionTime>())
            {
                b.TryConfigureSoftDelete();

                b.Property(nameof(IHasDeletionTime.DeletionTime))
                    .IsRequired(false);
            }
        }

        public static void ConfigureMayHaveCreator<T>(this EntityTypeBuilder<T> b)
            where T : class, IMayHaveCreator
        {
            b.As<EntityTypeBuilder>().TryConfigureMayHaveCreator();
        }

        public static void TryConfigureMayHaveCreator(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IMayHaveCreator>())
            {
                b.Property(nameof(IMayHaveCreator.CreatorId))
                    .IsRequired(false);
            }
        }

        public static void ConfigureMustHaveCreator<T>(this EntityTypeBuilder<T> b)
            where T : class, IMustHaveCreator
        {
            b.As<EntityTypeBuilder>().TryConfigureMustHaveCreator();
        }

        public static void TryConfigureMustHaveCreator(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IMustHaveCreator>())
            {
                b.Property(nameof(IMustHaveCreator.CreatorId))
                    .IsRequired();
            }
        }

        public static void ConfigureDeletionAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IDeletionAuditedObject
        {
            b.As<EntityTypeBuilder>().TryConfigureDeletionAudited();
        }

        public static void TryConfigureDeletionAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IDeletionAuditedObject>())
            {
                b.TryConfigureDeletionTime();

                b.Property(nameof(IDeletionAuditedObject.DeleterId))
                    .IsRequired(false);
            }
        }

        public static void ConfigureCreationTime<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasCreationTime
        {
            b.As<EntityTypeBuilder>().TryConfigureCreationTime();
        }

        public static void TryConfigureCreationTime(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IHasCreationTime>())
            {
                b.Property(nameof(IHasCreationTime.CreationTime))
                    .IsRequired();
            }
        }

        public static void ConfigureCreationAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, ICreationAuditedObject
        {
            b.As<EntityTypeBuilder>().TryConfigureCreationAudited();
        }

        public static void TryConfigureCreationAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<ICreationAuditedObject>())
            {
                b.As<EntityTypeBuilder>().TryConfigureCreationTime();
                b.As<EntityTypeBuilder>().TryConfigureMayHaveCreator();
            }
        }

        public static void ConfigureLastModificationTime<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasModificationTime
        {
            b.As<EntityTypeBuilder>().TryConfigureLastModificationTime();
        }

        public static void TryConfigureLastModificationTime(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IHasModificationTime>())
            {
                b.Property(nameof(IHasModificationTime.LastModificationTime))
                    .IsRequired(false);
            }
        }

        public static void ConfigureModificationAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IModificationAuditedObject
        {
            b.As<EntityTypeBuilder>().TryConfigureModificationAudited();
        }

        public static void TryConfigureModificationAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IModificationAuditedObject>())
            {
                b.TryConfigureLastModificationTime();

                b.Property(nameof(IModificationAuditedObject.LastModifierId))
                    .IsRequired(false);
            }
        }

        public static void ConfigureAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IAuditedObject
        {
            b.As<EntityTypeBuilder>().TryConfigureAudited();
        }

        public static void TryConfigureAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IAuditedObject>())
            {
                b.As<EntityTypeBuilder>().TryConfigureCreationAudited();
                b.As<EntityTypeBuilder>().TryConfigureModificationAudited();
            }
        }

        public static void ConfigureFullAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IFullAuditedObject
        {
            b.As<EntityTypeBuilder>().TryConfigureFullAudited();
        }

        public static void TryConfigureFullAudited(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IFullAuditedObject>())
            {
                b.As<EntityTypeBuilder>().TryConfigureAudited();
                b.As<EntityTypeBuilder>().TryConfigureDeletionAudited();
            }
        }

        public static void ConfigureMultiTenant<T>(this EntityTypeBuilder<T> b)
            where T : class, IMultiTenant
        {
            b.As<EntityTypeBuilder>().TryConfigureMultiTenant();
        }

        public static void TryConfigureMultiTenant(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IMultiTenant>())
            {
                b.Property(nameof(IMultiTenant.TenantId))
                    .IsRequired(false);
            }
        }

        public static void ConfigureCreationAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
            where T : class
        {
            b.As<EntityTypeBuilder>().TryConfigureCreationAudited();
            b.As<EntityTypeBuilder>().TryConfigureExtraProperties();
            b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
        }

        public static void ConfigureAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
            where T : class
        {
            b.As<EntityTypeBuilder>().TryConfigureAudited();
            b.As<EntityTypeBuilder>().TryConfigureExtraProperties();
            b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
        }

        public static void ConfigureFullAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
            where T : class
        {
            b.As<EntityTypeBuilder>().TryConfigureFullAudited();
            b.As<EntityTypeBuilder>().TryConfigureExtraProperties();
            b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
        }
    }

}
