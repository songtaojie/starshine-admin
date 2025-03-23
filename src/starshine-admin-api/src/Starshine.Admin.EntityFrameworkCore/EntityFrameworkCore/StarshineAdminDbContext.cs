﻿using Microsoft.EntityFrameworkCore;
using Starshine.Admin.EntityFrameworkCore.Modeling;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Starshine.Admin.EntityFrameworkCore;

[ReplaceDbContext(typeof(Volo.Abp.Identity.EntityFrameworkCore.IIdentityDbContext))]
[ReplaceDbContext(typeof(Volo.Abp.TenantManagement.EntityFrameworkCore.ITenantManagementDbContext))]
[ReplaceDbContext(typeof(Volo.Abp.SettingManagement.EntityFrameworkCore.ISettingManagementDbContext))]
[ReplaceDbContext(typeof(Volo.Abp.FeatureManagement.EntityFrameworkCore.IFeatureManagementDbContext))]
[ReplaceDbContext(typeof(Volo.Abp.PermissionManagement.EntityFrameworkCore.IPermissionManagementDbContext))]
[ReplaceDbContext(typeof(Volo.Abp.OpenIddict.EntityFrameworkCore.IOpenIddictDbContext))]
[ReplaceDbContext(typeof(Volo.Abp.AuditLogging.EntityFrameworkCore.IAuditLoggingDbContext))]
[ReplaceDbContext(typeof(Volo.Abp.BackgroundJobs.EntityFrameworkCore.IBackgroundJobsDbContext))]
[ConnectionStringName(ConnectionStrings.DefaultConnectionStringName)]
public class StarshineAdminDbContext(DbContextOptions<StarshineAdminDbContext> options) : AbpDbContext<StarshineAdminDbContext>(options),
    Volo.Abp.Identity.EntityFrameworkCore.IIdentityDbContext,
    Volo.Abp.TenantManagement.EntityFrameworkCore.ITenantManagementDbContext,
    Volo.Abp.SettingManagement.EntityFrameworkCore.ISettingManagementDbContext,
    Volo.Abp.FeatureManagement.EntityFrameworkCore.IFeatureManagementDbContext,
    Volo.Abp.PermissionManagement.EntityFrameworkCore.IPermissionManagementDbContext,
    Volo.Abp.OpenIddict.EntityFrameworkCore.IOpenIddictDbContext,
    Volo.Abp.AuditLogging.EntityFrameworkCore.IAuditLoggingDbContext,
    Volo.Abp.BackgroundJobs.EntityFrameworkCore.IBackgroundJobsDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region IIdentityDbContext
    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    #endregion

    #region ITenantManagementDbContext
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    #endregion


    #region ISettingManagementDbContext
    public DbSet<Setting> Settings { get; set; }
    public DbSet<SettingDefinitionRecord> SettingDefinitionRecords { get; set; }

    #endregion

    #region IFeatureManagementDbContext
    public DbSet<FeatureGroupDefinitionRecord> FeatureGroups { get; set; }

    public DbSet<FeatureDefinitionRecord> Features { get; set; }

    public DbSet<FeatureValue> FeatureValues { get; set; }

    #endregion

    #region IPermissionManagementDbContext

    public DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; set; }

    public DbSet<PermissionDefinitionRecord> Permissions { get; set; }

    public DbSet<PermissionGrant> PermissionGrants { get; set; }


    #endregion

    #region IOpenIddictDbContext
    public DbSet<OpenIddictApplication> Applications { get; set; }

    public DbSet<OpenIddictAuthorization> Authorizations { get; set; }

    public DbSet<OpenIddictScope> Scopes { get; set; }

    public DbSet<OpenIddictToken> Tokens { get; set; }
    #endregion

    #region IAuditLoggingDbContext
    public DbSet<AuditLog> AuditLogs { get; set; }
    #endregion

    #region IBackgroundJobsDbContext
    public DbSet<BackgroundJobRecord> BackgroundJobs { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();
    }
}
