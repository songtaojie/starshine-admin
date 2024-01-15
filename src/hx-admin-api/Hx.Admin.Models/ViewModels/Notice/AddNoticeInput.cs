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

namespace Hx.Admin.Models.ViewModels.Notice;
public class AddNoticeInput 
{
    /// <summary>
    /// 标题
    /// </summary>
    [Required(ErrorMessage ="标题不能为空"), MaxLength(32,ErrorMessage ="标题长度不能超过{1}")]
    public string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [Required(ErrorMessage ="内容不能为空")]
    public string Content { get; set; }

    /// <summary>
    /// 类型（1通知 2公告）
    /// </summary>
    public NoticeTypeEnum Type { get; set; }
}
