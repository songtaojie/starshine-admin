﻿namespace Starshine.Admin.Models;

/// <summary>
/// 系统通知公告表
/// </summary>
[SugarTable(null, "系统通知公告表")]
public class SysNotice : AuditedEntityBase
{
    /// <summary>
    /// 标题
    /// </summary>
    [SugarColumn(ColumnDescription = "标题", Length = 32)]
    public string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [SugarColumn(ColumnDescription = "内容", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string Content { get; set; }

    /// <summary>
    /// 类型（1通知 2公告）
    /// </summary>
    [SugarColumn(ColumnDescription = "类型（1通知 2公告）")]
    public NoticeTypeEnum Type { get; set; }

    /// <summary>
    /// 发布人Id
    /// </summary>
    [SugarColumn(ColumnDescription = "发布人Id")]
    public long PublicUserId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "发布人姓名",IsNullable =true, Length = 32)]
    public string? PublicUserName { get; set; }

    /// <summary>
    /// 发布机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "发布机构Id")]
    public long PublicOrgId { get; set; }

    /// <summary>
    /// 发布机构名称
    /// </summary>
    [SugarColumn(ColumnDescription = "发布机构名称", IsNullable = true, Length = 64)]
    public string? PublicOrgName { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [SugarColumn(ColumnDescription = "发布时间")]
    public DateTime? PublicTime { get; set; }

    /// <summary>
    /// 撤回时间
    /// </summary>
    [SugarColumn(ColumnDescription = "撤回时间")]
    public DateTime? CancelTime { get; set; }

    /// <summary>
    /// 状态（0草稿 1发布 2撤回 3删除）
    /// </summary>
    [SugarColumn(ColumnDescription = "状态（0草稿 1发布 2撤回 3删除）")]
    public NoticeStatusEnum Status { get; set; }
}