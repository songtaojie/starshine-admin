// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Models.ViewModels.Menu;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Models;
/// <summary>
/// 配置菜单对象映射
/// </summary>
public class SysMenuMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<SysMenu, MenuOutput>()
            .Map(t => t.Meta.Title, o => o.Title)
            .Map(t => t.Meta.Icon, o => o.Icon)
            .Map(t => t.Meta.IsIframe, o => o.IsIframe)
            .Map(t => t.Meta.IsLink, o => o.OutLink)
            .Map(t => t.Meta.IsHide, o => o.IsHide)
            .Map(t => t.Meta.IsKeepAlive, o => o.IsKeepAlive)
            .Map(t => t.Meta.IsAffix, o => o.IsAffix);
    }
}
