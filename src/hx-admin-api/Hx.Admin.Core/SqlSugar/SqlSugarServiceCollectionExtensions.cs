// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;
public static class SqlSugarServiceCollectionExtensions
{
    public static IServiceCollection AddSqlSugarSetup(this IServiceCollection services)
    {
        services.AddSqlSugar(_ => { }, sqlSugar =>
        {
            sqlSugar.Aop.DataExecuting = (oldValue, entityInfo) =>
            {
                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    if (entityInfo.PropertyName == "Id")
                        entityInfo.SetValue(Yitter.IdGenerator.YitIdHelper.NextId());
                    if (entityInfo.PropertyName == "CreateTime")
                        entityInfo.SetValue(DateTime.Now);
                }
                if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                {
                    if (entityInfo.PropertyName == "UpdateTime")
                        entityInfo.SetValue(DateTime.Now);
                }
            };
        });
        services.AddHostedService<SqlsugarHostedService>();
        return services;
    }
}
