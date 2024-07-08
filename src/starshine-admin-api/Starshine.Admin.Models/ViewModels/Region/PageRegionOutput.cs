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

namespace Starshine.Admin.Models.ViewModels.Region;
public class PageRegionOutput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 简称
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// 组合名
    /// </summary>
    public string? MergerName { get; set; }

    /// <summary>
    /// 行政代码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? ZipCode { get; set; }

    /// <summary>
    /// 区号
    /// </summary>
    public string? CityCode { get; set; }

    /// <summary>
    /// 层级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    public string? PinYin { get; set; }
}
