// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.Models.ViewModels.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Admin.Models.ViewModels.User;
public class AddUserInput
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required(ErrorMessage = "账号不能为空")]
    [MaxLength(32,ErrorMessage ="账号最大长度超过{1}")]
    public string Account { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [Required(ErrorMessage = "真实姓名不能为空")]
    [MaxLength(32, ErrorMessage = "真实姓名最大长度超过{1}")]
    public string RealName { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [MaxLength(32, ErrorMessage = "昵称最大长度超过{1}")]
    public string? NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [MaxLength(512, ErrorMessage = "上传头像地址最大长度超过{1}")]
    public string? Avatar { get; set; }

    /// <summary>
    /// 性别-男_1、女_2
    /// </summary>
    [SugarColumn(ColumnDescription = "性别")]
    public GenderEnum Sex { get; set; } = GenderEnum.Male;

    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 民族
    /// </summary>
    [MaxLength(32, ErrorMessage = "民族最大长度超过{1}")]
    public string? Nation { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [RegularExpression("^1[3456789][0-9]{9}$",ErrorMessage ="手机号码格式不正确")]
    public string? Phone { get; set; }

    /// <summary>
    /// 证件类型
    /// </summary>
    public CardTypeEnum CardType { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    [RegularExpression("(^\\d{15}$)|(^\\d{18}$)|(^\\d{17}(\\d|X|x)$)", ErrorMessage = "身份证号格式不正确")]
    public string? IdCard { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [RegularExpression("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$", ErrorMessage = "邮箱格式不正确")]
    public string? Email { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [MaxLength(256, ErrorMessage = "地址最大长度超过{1}")]
    public string? Address { get; set; }

    /// <summary>
    /// 文化程度
    /// </summary>
    public CultureLevelEnum CultureLevel { get; set; }

    /// <summary>
    /// 政治面貌
    /// </summary>
    [MaxLength(16, ErrorMessage = "政治面貌最大长度超过{1}")]
    public string? PoliticalOutlook { get; set; }

    /// <summary>
    /// 毕业院校
    /// </summary>COLLEGE
    [MaxLength(128, ErrorMessage = "毕业院校最大长度超过{1}")]
    public string? College { get; set; }

    /// <summary>
    /// 办公电话
    /// </summary>
    [RegularExpression("(^1[3456789][0-9]{9}$)|((^[0-9]{3,4}\\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\\([0-9]{3,4}\\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$))",ErrorMessage = "办公电话格式不正确")]
    public string? OfficePhone { get; set; }

    /// <summary>
    /// 紧急联系人
    /// </summary>
    [MaxLength(32, ErrorMessage = "紧急联系人最大长度超过{1}")]
    public string? EmergencyContact { get; set; }

    /// <summary>
    /// 紧急联系人电话
    /// </summary>
    [RegularExpression("^1[3456789][0-9]{9}$", ErrorMessage = "紧急联系人电话格式不正确")]
    public string? EmergencyPhone { get; set; }

    /// <summary>
    /// 紧急联系人地址
    /// </summary>
    [MaxLength(256, ErrorMessage = "紧急联系人地址最大长度超过{1}")]
    public string? EmergencyAddress { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    [MaxLength(512, ErrorMessage = "个人简介最大长度超过{1}")]
    public string? Introduction { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } = 100;

    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; } = StatusEnum.Enable;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(128, ErrorMessage = "个人简介最大长度超过{1}")]
    public string? Remark { get; set; }

    /// <summary>
    /// 账号类型
    /// </summary>
    public AccountTypeEnum AccountType { get; set; } = AccountTypeEnum.None;

    /// <summary>
    /// 机构Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 职位Id
    /// </summary>
    public long PosId { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    [MaxLength(32, ErrorMessage = "工号最大长度超过{1}")]
    public string? JobNum { get; set; }

    /// <summary>
    /// 职级
    /// </summary>
    [MaxLength(32, ErrorMessage = "职级最大长度超过{1}")]
    public string? PosLevel { get; set; }

    /// <summary>
    /// 入职日期
    /// </summary>
    public DateTime? JoinDate { get; set; }

    /// <summary>
    /// 角色集合
    /// </summary>
    public List<long> RoleIdList { get; set; }

    /// <summary>
    /// 扩展机构集合
    /// </summary>
    public List<UserExtOrgInput> ExtOrgIdList { get; set; }
}
