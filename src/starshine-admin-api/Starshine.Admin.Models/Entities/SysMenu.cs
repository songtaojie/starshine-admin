﻿namespace Starshine.Admin.Models;

/// <summary>
/// 系统菜单表
/// </summary>
[SugarTable(null, "系统菜单表")]
public class SysMenu : AuditedEntityBase
{
    /// <summary>
    /// 父Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父Id")]
    public long Pid { get; set; }

    /// <summary>
    /// 菜单类型（1目录 2菜单 3按钮）
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单类型")]
    public MenuTypeEnum Type { get; set; }

    /// <summary>
    /// 路由名称
    /// </summary>
    [SugarColumn(ColumnDescription = "路由名称",IsNullable =true, Length = 64)]
    public string? Name { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [SugarColumn(ColumnDescription = "路由地址", IsNullable = true, Length = 128)]
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    [SugarColumn(ColumnDescription = "组件路径", IsNullable = true, Length = 128)]
    public string? Component { get; set; }

    /// <summary>
    /// 重定向
    /// </summary>
    [SugarColumn(ColumnDescription = "重定向", IsNullable = true, Length = 128)]
    public string? Redirect { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    [SugarColumn(ColumnDescription = "权限标识", IsNullable = true, Length = 128)]
    public string? Permission { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单名称", Length = 64)]
    public string Title { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [SugarColumn(ColumnDescription = "图标", IsNullable = true, Length = 128)]
    public string? Icon { get; set; }

    /// <summary>
    /// 是否内嵌
    /// </summary>
    [SugarColumn(ColumnDescription = "是否内嵌")]
    public bool IsIframe { get; set; }

    /// <summary>
    /// 外链链接
    /// </summary>
    [SugarColumn(ColumnDescription = "外链链接", IsNullable = true, Length = 256)]
    public string? OutLink { get; set; }

    /// <summary>
    /// 是否隐藏
    /// </summary>
    [SugarColumn(ColumnDescription = "是否隐藏")]
    public bool IsHide { get; set; }

    /// <summary>
    /// 是否缓存
    /// </summary>
    [SugarColumn(ColumnDescription = "是否缓存")]
    public bool IsKeepAlive { get; set; } = true;

    /// <summary>
    /// 是否固定
    /// </summary>
    [SugarColumn(ColumnDescription = "是否固定")]
    public bool IsAffix { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; } = 100;

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public StatusEnum Status { get; set; } = StatusEnum.Enable;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 256)]
    public string? Remark { get; set; }

    /// <summary>
    /// 菜单子项
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<SysMenu> Children { get; set; } = new List<SysMenu>();
}