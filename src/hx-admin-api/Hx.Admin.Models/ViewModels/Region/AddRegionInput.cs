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

namespace Hx.Admin.Models.ViewModels.Region;
public class AddRegionInput
{
    /// <summary>
    /// 父Id
    /// </summary>
    public long Pid { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Required(ErrorMessage ="名称不能为空")]
    [MaxLength(64,ErrorMessage ="名称长度不能超过{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 简称
    /// </summary>
    [MaxLength(32, ErrorMessage = "简称长度不能超过{1}")]
    public string? ShortName { get; set; }

    /// <summary>
    /// 组合名
    /// </summary>
    [MaxLength(64, ErrorMessage = "组合名长度不能超过{1}")]
    public string? MergerName { get; set; }

    /// <summary>
    /// 行政代码
    /// </summary>
    [MaxLength(32, ErrorMessage = "行政代码长度不能超过{1}")]
    public string? Code { get; set; }

    /// <summary>
    /// 邮政编码
    /// </summary>
    [MaxLength(6, ErrorMessage = "邮政编码长度不能超过{1}")]
    public string? ZipCode { get; set; }

    /// <summary>
    /// 区号
    /// </summary>
    [MaxLength(6, ErrorMessage = "区号长度不能超过{1}")]
    public string? CityCode { get; set; }

    /// <summary>
    /// 层级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    [MaxLength(128, ErrorMessage = "拼音长度不能超过{1}")]
    public string? PinYin { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    public float Lng { get; set; }

    /// <summary>
    /// 维度
    /// </summary>
    public float Lat { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } 

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(128, ErrorMessage = "备注长度不能超过{1}")]
    public string? Remark { get; set; }

    /// <summary>
    /// 机构子项
    /// </summary>
    public List<SysRegion> Children { get; set; }
}
