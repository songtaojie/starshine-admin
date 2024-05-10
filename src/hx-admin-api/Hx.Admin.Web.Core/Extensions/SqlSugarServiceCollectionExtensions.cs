// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Models;
using Hx.Admin.Serilog;
using Hx.Sdk.Core;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using SqlSugar;
using System;

namespace Microsoft.Extensions.DependencyInjection;
public static class SqlSugarServiceCollectionExtensions
{
    public static IServiceCollection AddSqlSugarSetup(this IServiceCollection services)
    {
        services.AddSqlSugar((db, provider) =>
        {
            var userManager = provider.GetRequiredService<UserManager>();
            var logger = provider.GetRequiredService<ILogger<ISqlSugarClient>>();
            var config = db.CurrentConnectionConfig;
            db.Aop.DataExecuting = (oldValue, entityInfo) =>
            {
                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    switch (entityInfo.PropertyName)
                    {
                        case nameof(FullAuditedEntityBase.Id):
                            var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                            if (id == null || (id is long longId && longId == 0))
                                entityInfo.SetValue(IdGenerater.GetNextId());
                            break;
                        case nameof(FullAuditedEntityBase.CreateTime):
                            var createTime = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue) as DateTime?;
                            if (!createTime.HasValue) entityInfo.SetValue(DateTime.Now);
                            break;
                        case nameof(FullAuditedEntityBase.UpdateTime):
                            var updateTime = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue) as DateTime?;
                            if (!updateTime.HasValue) entityInfo.SetValue(DateTime.Now);
                            break;
                    }
                    var userId = userManager.GetUserId<long>();
                    if (userId > 0)
                    {
                        if (entityInfo.PropertyName == nameof(FullAuditedEntityBase.CreatorId))
                        {
                            var creatorId = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue) as long?;
                            if (!creatorId.HasValue) entityInfo.SetValue(userId);
                        }
                        if (entityInfo.PropertyName == nameof(FullAuditedEntityBase.UpdaterId))
                        {
                            var updaterId = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue) as long?;
                            if (!updaterId.HasValue) entityInfo.SetValue(userId);
                        }
                    }
                    var orgId = userManager.GetOrgId<long>();
                    if (orgId > 0)
                    {
                        if (entityInfo.OperationType == DataFilterType.InsertByObject && entityInfo.PropertyName == nameof(IOrgIdFilter.OrgId))
                        {
                            var createOrgId = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue) as long?;
                            if(!createOrgId.HasValue) entityInfo.SetValue(orgId);
                        }
                    }
                }
                else if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                {
                    if (entityInfo.PropertyName == nameof(FullAuditedEntityBase.UpdateTime))
                        entityInfo.SetValue(DateTime.Now);
                    var userId = userManager.GetUserId<long>();
                    if (userId > 0)
                    {
                        if (entityInfo.PropertyName == nameof(FullAuditedEntityBase.UpdaterId))
                            entityInfo.SetValue(userId);
                    }
                }


            };
        });
        services.AddHostedService<SqlSugarHostedService>();
        return services;
    }
}
