﻿namespace Starshine.Admin.Core;

/// <summary>
/// 文件上传配置选项
/// </summary>
public sealed class UploadOptions 
{
    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 大小
    /// </summary>
    public long MaxSize { get; set; }

    /// <summary>
    /// 上传格式
    /// </summary>
    public List<string> ContentType { get; set; }
}