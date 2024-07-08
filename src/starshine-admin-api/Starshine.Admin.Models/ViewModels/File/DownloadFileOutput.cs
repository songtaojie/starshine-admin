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

namespace Starshine.Admin.Models.ViewModels.File;
public class DownloadFileOutput
{
    /// <summary>
    /// 文件流
    /// </summary>
    public Stream Stream { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }
}
