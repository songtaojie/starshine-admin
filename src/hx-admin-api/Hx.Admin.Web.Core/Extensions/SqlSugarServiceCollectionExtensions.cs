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
                            var id = IdGenerater.GetNextId();
                            entityInfo.SetValue(id);
                            break;
                        case nameof(FullAuditedEntityBase.CreateTime):
                            entityInfo.SetValue(DateTime.Now);
                            break;
                        case nameof(FullAuditedEntityBase.UpdateTime):
                            entityInfo.SetValue(DateTime.Now);
                            break;
                    }
                    var userId = userManager.GetUserId<long>();
                    if (userId > 0)
                    {
                        if (entityInfo.PropertyName == nameof(FullAuditedEntityBase.CreatorId))
                            entityInfo.SetValue(userId);
                        if (entityInfo.PropertyName == nameof(FullAuditedEntityBase.UpdaterId))
                            entityInfo.SetValue(userId);
                    }
                    var orgId = userManager.GetOrgId<long>();
                    if (orgId > 0)
                    {
                        if (entityInfo.OperationType == DataFilterType.InsertByObject)
                        {
                            if (entityInfo.PropertyName == nameof(IOrgIdFilter.OrgId))
                                entityInfo.SetValue(orgId);
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
        //services.AddHostedService<SqlSugarHostedService>();
        return services;
    }
}
