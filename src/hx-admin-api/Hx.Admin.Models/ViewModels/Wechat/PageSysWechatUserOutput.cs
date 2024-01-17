// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Models.ViewModels.Wechat;
public class PageSysWechatUserOutput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 系统用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 平台类型
    /// </summary>
    public PlatformTypeEnum PlatformType { get; set; }

    /// <summary>
    /// OpenId
    /// </summary>
    public string OpenId { get; set; }

    /// <summary>
    /// 会话密钥
    /// </summary>
    public string? SessionKey { get; set; }

    /// <summary>
    /// UnionId
    /// </summary>
    public string? UnionId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public int Sex { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 省
    /// </summary>
    public string? Province { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    public string? Country { get; set; }


    /// <summary>
    /// 用户授权的作用域，使用逗号分隔
    /// </summary>
    public string? Scope { get; set; }
}
