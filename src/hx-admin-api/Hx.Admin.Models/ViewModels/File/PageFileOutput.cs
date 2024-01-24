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

namespace Hx.Admin.Models.ViewModels.File;
public class PageFileOutput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 提供者
    /// </summary>
    public string? Provider { get; set; }

    /// <summary>
    /// 仓储名称
    /// </summary>
    public string? BucketName { get; set; }

    /// <summary>
    /// 文件名称（上传时名称）
    /// </summary>文件名称
    public string? FileName { get; set; }

    /// <summary>
    /// 文件后缀
    /// </summary>
    public string? Suffix { get; set; }

    /// <summary>
    /// 存储路径
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// 文件大小KB
    /// </summary>
    public string? SizeKb { get; set; }

    /// <summary>
    /// 文件大小信息-计算后的
    /// </summary>
    public string? SizeInfo { get; set; }

    /// <summary>
    /// 外链地址-OSS上传后生成外链地址方便前端预览
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
}
