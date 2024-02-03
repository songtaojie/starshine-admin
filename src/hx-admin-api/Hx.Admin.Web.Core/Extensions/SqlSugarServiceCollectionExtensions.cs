// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

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
            db.Aop.DataExecuting = (oldValue, entityInfo) =>
            {
                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    switch (entityInfo.PropertyName)
                    {
                        case "Id":
                            entityInfo.SetValue(Yitter.IdGenerator.YitIdHelper.NextId());
                            break;
                        case "CreateTime":
                            entityInfo.SetValue(DateTime.Now);
                            break;
                        case "UpdateTime":
                            entityInfo.SetValue(DateTime.Now);
                            break;
                    }
                    var userId = userManager.GetUserId<long>();
                    if (userId > 0)
                    {
                        if (entityInfo.PropertyName == "CreatorId")
                            entityInfo.SetValue(userId);
                        if (entityInfo.PropertyName == "UpdaterId")
                            entityInfo.SetValue(userId);
                    }
                    var orgId = userManager.GetOrgId<long>();
                    if (orgId > 0)
                    {
                        if (entityInfo.OperationType == DataFilterType.InsertByObject)
                        {
                            if (entityInfo.PropertyName == "OrgId")
                                entityInfo.SetValue(orgId);
                        }
                    }
                }
                else if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                {
                    if (entityInfo.PropertyName == "UpdateTime")
                        entityInfo.SetValue(DateTime.Now);
                    var userId = userManager.GetUserId<long>();
                    if (userId > 0)
                    {
                        if (entityInfo.PropertyName == "UpdaterId")
                            entityInfo.SetValue(userId);
                    }
                }


            };
            // 打印SQL语句
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                using var mutiLogContext = MutiLogContext.Instance.PushSqlsugarProperty(db);
                logger.LogInformation($"{UtilMethods.GetNativeSql(sql, pars)}");
            };
            db.Aop.OnError = ex =>
            {
                if (ex.Parametres == null) return;
                using var mutiLogContext = MutiLogContext.Instance.PushSqlsugarProperty(db);
                logger.LogError($"{UtilMethods.GetNativeSql(ex.Sql, ex.Parametres as SugarParameter[])} ");
            };
        });
        services.AddHostedService<SqlSugarHostedService>();
        return services;
    }
}
