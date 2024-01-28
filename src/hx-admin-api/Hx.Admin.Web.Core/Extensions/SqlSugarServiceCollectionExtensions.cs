// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Sdk.Core;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hx.Admin.Serilog;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection;
public static class SqlSugarServiceCollectionExtensions
{
    public static IServiceCollection AddSqlSugarSetup(this IServiceCollection services)
    {
        services.AddSqlSugar(_ => { }, (db, provider) =>
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
                using (var mutiLogContext = new MutiLogContext()
                    .PushProperty(LogContextConst.LogSource, db.CurrentConnectionConfig.ConfigId)
                    .PushProperty(LogContextConst.SugarActionType, db.SugarActionType))
                {
                    Log.Information($"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetNativeSql(sql, pars)}\r\n");
                    //logger.LogInformation($"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetNativeSql(sql, pars)}\r\n");
                }
                    
            };
            db.Aop.OnError = ex =>
            {
                if (ex.Parametres == null) return;
                using (var mutiLogContext = new MutiLogContext()
                    .PushProperty(LogContextConst.LogSource, db.CurrentConnectionConfig.ConfigId)
                    .PushProperty(LogContextConst.SugarActionType, db.SugarActionType))
                {
                    logger.LogError($"【{DateTime.Now}——错误SQL】\r\n {UtilMethods.GetNativeSql(ex.Sql, ex.Parametres as SugarParameter[])} \r\n");
                }
            };
        });
        services.AddHostedService<SqlSugarHostedService>();
        return services;
    }
}
