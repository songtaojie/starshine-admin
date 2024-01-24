import { AccountTypeEnum } from './enums/account-type-enum';
import { CardTypeEnum } from './enums/card-type-enum';
import { CultureLevelEnum } from './enums/culture-level-enum';
import { GenderEnum } from './enums/gender-enum';
import { StatusEnum } from './enums/status-enum';
import { SysOrg } from './sys-org';
import { SysPos } from './sys-pos';
import { SysUserExtOrg } from './sys-user-ext-org';
import { BasePageInput } from './base/base-page-input';
/**
 * 
 * @export
 * @interface AddUserInput
 */
export interface AddUserInput {
    /**
     * 昵称
     * @type {string}
     * @memberof AddUserInput
     */
    nickName?: string | null;
    /**
     * 头像
     * @type {string}
     * @memberof AddUserInput
     */
    avatar?: string | null;
    /**
     * 
     * @type {GenderEnum}
     * @memberof AddUserInput
     */
    sex?: GenderEnum;
    /**
     * 年龄
     * @type {number}
     * @memberof AddUserInput
     */
    age?: number;
    /**
     * 出生日期
     * @type {Date}
     * @memberof AddUserInput
     */
    birthday?: Date | null;
    /**
     * 民族
     * @type {string}
     * @memberof AddUserInput
     */
    nation?: string | null;
    /**
     * 手机号码
     * @type {string}
     * @memberof AddUserInput
     */
    phone?: string | null;
    /**
     * 
     * @type {CardTypeEnum}
     * @memberof AddUserInput
     */
    cardType?: CardTypeEnum;
    /**
     * 身份证号
     * @type {string}
     * @memberof AddUserInput
     */
    idCardNum?: string | null;
    /**
     * 邮箱
     * @type {string}
     * @memberof AddUserInput
     */
    email?: string | null;
    /**
     * 地址
     * @type {string}
     * @memberof AddUserInput
     */
    address?: string | null;
    /**
     * 
     * @type {CultureLevelEnum}
     * @memberof AddUserInput
     */
    cultureLevel?: CultureLevelEnum;
    /**
     * 政治面貌
     * @type {string}
     * @memberof AddUserInput
     */
    politicalOutlook?: string | null;
    /**
     * 毕业院校
     * @type {string}
     * @memberof AddUserInput
     */
    college?: string | null;
    /**
     * 办公电话
     * @type {string}
     * @memberof AddUserInput
     */
    officePhone?: string | null;
    /**
     * 紧急联系人
     * @type {string}
     * @memberof AddUserInput
     */
    emergencyContact?: string | null;
    /**
     * 紧急联系人电话
     * @type {string}
     * @memberof AddUserInput
     */
    emergencyPhone?: string | null;
    /**
     * 紧急联系人地址
     * @type {string}
     * @memberof AddUserInput
     */
    emergencyAddress?: string | null;
    /**
     * 个人简介
     * @type {string}
     * @memberof AddUserInput
     */
    introduction?: string | null;
    /**
     * 排序
     * @type {number}
     * @memberof AddUserInput
     */
    orderNo?: number;
    /**
     * 
     * @type {StatusEnum}
     * @memberof AddUserInput
     */
    status?: StatusEnum;
    /**
     * 备注
     * @type {string}
     * @memberof AddUserInput
     */
    remark?: string | null;
    /**
     * 
     * @type {AccountTypeEnum}
     * @memberof AddUserInput
     */
    accountType?: AccountTypeEnum;
    /**
     * 机构Id
     * @type {number}
     * @memberof AddUserInput
     */
    orgId?: number;
    /**
     * 
     * @type {SysOrg}
     * @memberof AddUserInput
     */
    sysOrg?: SysOrg;
    /**
     * 职位Id
     * @type {number}
     * @memberof AddUserInput
     */
    posId?: number;
    /**
     * 
     * @type {SysPos}
     * @memberof AddUserInput
     */
    sysPos?: SysPos;
    /**
     * 工号
     * @type {string}
     * @memberof AddUserInput
     */
    jobNum?: string | null;
    /**
     * 职级
     * @type {string}
     * @memberof AddUserInput
     */
    posLevel?: string | null;
    /**
     * 入职日期
     * @type {Date}
     * @memberof AddUserInput
     */
    joinDate?: Date | null;
    /**
     * 最新登录Ip
     * @type {string}
     * @memberof AddUserInput
     */
    lastLoginIp?: string | null;
    /**
     * 最新登录地点
     * @type {string}
     * @memberof AddUserInput
     */
    lastLoginAddress?: string | null;
    /**
     * 最新登录时间
     * @type {Date}
     * @memberof AddUserInput
     */
    lastLoginTime?: Date | null;
    /**
     * 最新登录设备
     * @type {string}
     * @memberof AddUserInput
     */
    lastLoginDevice?: string | null;
    /**
     * 电子签名
     * @type {string}
     * @memberof AddUserInput
     */
    signature?: string | null;
    /**
     * 账号
     * @type {string}
     * @memberof AddUserInput
     */
    account: string;
    /**
     * 真实姓名
     * @type {string}
     * @memberof AddUserInput
     */
    realName: string;
    /**
     * 角色集合
     * @type {Array<number>}
     * @memberof AddUserInput
     */
    roleIdList?: Array<number> | null;
    /**
     * 扩展机构集合
     * @type {Array<SysUserExtOrg>}
     * @memberof AddUserInput
     */
    extOrgIdList?: Array<SysUserExtOrg> | null;
}

/**
 * 
 * @export
 * @interface UpdateUserInput
 */
export interface UpdateUserInput extends AddUserInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateUserInput
     */
    id: number;
}

/**
 * 
 * @export
 * @interface ChangePwdInput
 */
export interface ChangePwdInput {
    /**
     * 当前密码
     * @type {string}
     * @memberof ChangePwdInput
     */
    passwordOld: string;
    /**
     * 新密码
     * @type {string}
     * @memberof ChangePwdInput
     */
    passwordNew: string;
}

/**
 * 
 * @export
 * @interface PageUserInput
 */
export interface PageUserInput extends BasePageInput {
    /**
     * 账号
     * @type {string}
     * @memberof PageUserInput
     */
    account?: string | null;
    /**
     * 姓名
     * @type {string}
     * @memberof PageUserInput
     */
    realName?: string | null;
    /**
     * 手机号
     * @type {string}
     * @memberof PageUserInput
     */
    phone?: string | null;
    /**
     * 查询时所选机构Id
     * @type {number}
     * @memberof PageUserInput
     */
    orgId?: number;
}

/**
 * 
 * @export
 * @interface ResetPwdUserInput
 */
export interface ResetPwdUserInput {
    /**
     * 主键Id
     * @type {number}
     * @memberof ResetPwdUserInput
     */
    id: number;
}

/**
 * 
 * @export
 * @interface UserInput
 */
export interface UserInput {
    /**
     * 主键Id
     * @type {number}
     * @memberof UserInput
     */
    id: number;
    /**
     * 
     * @type {StatusEnum}
     * @memberof UserInput
     */
    status?: StatusEnum;
}

/**
 * 系统用户表
 * @export
 * @interface SysUserBaseInfo
 */
export interface SysUserBaseInfo {
    /**
     * 雪花Id
     * @type {number}
     * @memberof SysUserBaseInfo
     */
    id?: number;
    /**
     * 真实姓名
     * @type {string}
     * @memberof SysUserBaseInfo
     */
    realName?: string | null;
    /**
     * 昵称
     * @type {string}
     * @memberof SysUserBaseInfo
     */
    nickName?: string | null;
    /**
     * 头像
     * @type {string}
     * @memberof SysUserBaseInfo
     */
    avatar?: string | null;
    /**
     * 
     * @type {GenderEnum}
     * @memberof SysUserBaseInfo
     */
    sex?: GenderEnum;

    /**
     * 出生日期
     * @type {Date}
     * @memberof SysUserBaseInfo
     */
    birthday?: Date | null;

    /**
     * 手机号码
     * @type {string}
     * @memberof SysUserBaseInfo
     */
    phone?: string | null;
   
    /**
     * 邮箱
     * @type {string}
     * @memberof SysUserBaseInfo
     */
    email?: string | null;
    /**
     * 地址
     * @type {string}
     * @memberof SysUser
     */
    address?: string | null;
    /**
     * 个人简介
     * @type {string}
     * @memberof SysUserBaseInfo
     */
    introduction?: string | null;
 
    /**
     * 备注
     * @type {string}
     * @memberof SysUserBaseInfo
     */
    remark?: string | null;
    /**
     * 电子签名
     * @type {string}
     * @memberof SysUserBaseInfo
     */
    signature?: string | null;
}

/**
 * 系统用户表
 * @export
 * @interface SysUser
 */
export interface SysUser {
    /**
     * 雪花Id
     * @type {number}
     * @memberof SysUser
     */
    id?: number;
    /**
     * 创建时间
     * @type {Date}
     * @memberof SysUser
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysUser
     */
    updateTime?: Date | null;
   
    /**
     * 账号
     * @type {string}
     * @memberof SysUser
     */
    account: string;
    /**
     * 真实姓名
     * @type {string}
     * @memberof SysUser
     */
    realName?: string | null;
    /**
     * 昵称
     * @type {string}
     * @memberof SysUser
     */
    nickName?: string | null;
    /**
     * 头像
     * @type {string}
     * @memberof SysUser
     */
    avatar?: string | null;
    /**
     * 
     * @type {GenderEnum}
     * @memberof SysUser
     */
    sex?: GenderEnum;
    /**
     * 年龄
     * @type {number}
     * @memberof SysUser
     */
    age?: number;
    /**
     * 出生日期
     * @type {Date}
     * @memberof SysUser
     */
    birthday?: Date | null;
    /**
     * 民族
     * @type {string}
     * @memberof SysUser
     */
    nation?: string | null;
    /**
     * 手机号码
     * @type {string}
     * @memberof SysUser
     */
    phone?: string | null;
    /**
     * 
     * @type {CardTypeEnum}
     * @memberof SysUser
     */
    cardType?: CardTypeEnum;
    /**
     * 身份证号
     * @type {string}
     * @memberof SysUser
     */
    idCardNum?: string | null;
    /**
     * 邮箱
     * @type {string}
     * @memberof SysUser
     */
    email?: string | null;
    /**
     * 地址
     * @type {string}
     * @memberof SysUser
     */
    address?: string | null;
    /**
     * 
     * @type {CultureLevelEnum}
     * @memberof SysUser
     */
    cultureLevel?: CultureLevelEnum;
    /**
     * 政治面貌
     * @type {string}
     * @memberof SysUser
     */
    politicalOutlook?: string | null;
    /**
     * 毕业院校
     * @type {string}
     * @memberof SysUser
     */
    college?: string | null;
    /**
     * 办公电话
     * @type {string}
     * @memberof SysUser
     */
    officePhone?: string | null;
    /**
     * 紧急联系人
     * @type {string}
     * @memberof SysUser
     */
    emergencyContact?: string | null;
    /**
     * 紧急联系人电话
     * @type {string}
     * @memberof SysUser
     */
    emergencyPhone?: string | null;
    /**
     * 紧急联系人地址
     * @type {string}
     * @memberof SysUser
     */
    emergencyAddress?: string | null;
    /**
     * 个人简介
     * @type {string}
     * @memberof SysUser
     */
    introduction?: string | null;
    /**
     * 排序
     * @type {number}
     * @memberof SysUser
     */
    orderNo?: number;
    /**
     * 
     * @type {StatusEnum}
     * @memberof SysUser
     */
    status?: StatusEnum;
    /**
     * 备注
     * @type {string}
     * @memberof SysUser
     */
    remark?: string | null;
    /**
     * 
     * @type {AccountTypeEnum}
     * @memberof SysUser
     */
    accountType?: AccountTypeEnum;
    /**
     * 机构Id
     * @type {number}
     * @memberof SysUser
     */
    orgId?: number;
    /**
     * 
     * @type {SysOrg}
     * @memberof SysUser
     */
    sysOrg?: SysOrg;
    /**
     * 职位Id
     * @type {number}
     * @memberof SysUser
     */
    posId?: number;
    /**
     * 
     * @type {SysPos}
     * @memberof SysUser
     */
    sysPos?: SysPos;
    /**
     * 工号
     * @type {string}
     * @memberof SysUser
     */
    jobNum?: string | null;
    /**
     * 职级
     * @type {string}
     * @memberof SysUser
     */
    posLevel?: string | null;
    /**
     * 入职日期
     * @type {Date}
     * @memberof SysUser
     */
    joinDate?: Date | null;
    /**
     * 最新登录Ip
     * @type {string}
     * @memberof SysUser
     */
    lastLoginIp?: string | null;
    /**
     * 最新登录地点
     * @type {string}
     * @memberof SysUser
     */
    lastLoginAddress?: string | null;
    /**
     * 最新登录时间
     * @type {Date}
     * @memberof SysUser
     */
    lastLoginTime?: Date | null;
    /**
     * 最新登录设备
     * @type {string}
     * @memberof SysUser
     */
    lastLoginDevice?: string | null;
    /**
     * 电子签名
     * @type {string}
     * @memberof SysUser
     */
    signature?: string | null;
}


