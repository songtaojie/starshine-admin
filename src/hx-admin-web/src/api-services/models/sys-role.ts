import { BasePageInput } from './base/base-page-input';
import { DataScopeEnum } from './enums/data-scope-enum';
import { StatusEnum } from './enums/status-enum';

/**
 * 
 * @export
 * @interface AddRoleInput
 */
export interface AddRoleInput {
    /**
     * 编码
     * @type {string}
     * @memberof AddRoleInput
     */
    code: string | null;
    /**
     * 排序
     * @type {number}
     * @memberof AddRoleInput
     */
    sort?: number;
    /**
     * 
     * @type {DataScopeEnum}
     * @memberof AddRoleInput
     */
    dataScope?: DataScopeEnum;
    /**
     * 备注
     * @type {string}
     * @memberof AddRoleInput
     */
    remark?: string | null;
    /**
     * 
     * @type {StatusEnum}
     * @memberof AddRoleInput
     */
    status?: StatusEnum;
    /**
     * 名称
     * @type {string}
     * @memberof AddRoleInput
     */
    name: string;
    /**
     * 菜单Id集合
     * @type {Array<number>}
     * @memberof AddRoleInput
     */
    menuIdList?: Array<number> | null;
}

/**
 * 
 * @export
 * @interface UpdateRoleInput
 */
export interface UpdateRoleInput extends AddRoleInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateRoleInput
     */
    id: number;
}


/**
 * 
 * @export
 * @interface PageRoleInput
 */
export interface PageRoleInput extends BasePageInput {
    /**
     * 名称
     * @type {string}
     * @memberof PageRoleInput
     */
    name?: string | null;
    /**
     * 编码
     * @type {string}
     * @memberof PageRoleInput
     */
    code?: string | null;
}

/**
 * 
 * @export
 * @interface RoleInput
 */
export interface RoleInput {
    /**
     * 主键Id
     * @type {number}
     * @memberof RoleInput
     */
    id: number;
    /**
     * 
     * @type {StatusEnum}
     * @memberof RoleInput
     */
    status?: StatusEnum;
}

/**
 * 授权角色菜单
 * @export
 * @interface RoleMenuInput
 */
export interface RoleMenuInput {
    /**
     * 主键Id
     * @type {number}
     * @memberof RoleMenuInput
     */
    id: number;
    /**
     * 菜单Id集合
     * @type {Array<number>}
     * @memberof RoleMenuInput
     */
    menuIdList?: Array<number> | null;
}

/**
 * 授权角色机构
 * @export
 * @interface RoleOrgInput
 */
export interface RoleOrgInput {
    /**
     * 主键Id
     * @type {number}
     * @memberof RoleOrgInput
     */
    id: number;
    /**
     * 数据范围
     * @type {number}
     * @memberof RoleOrgInput
     */
    dataScope?: number;
    /**
     * 机构Id集合
     * @type {Array<number>}
     * @memberof RoleOrgInput
     */
    orgIdList?: Array<number> | null;
}

/**
 * 系统角色表
 * @export
 * @interface SysRole
 */
export interface SysRole extends UpdateRoleInput {

    /**
     * 创建时间
     * @type {Date}
     * @memberof SysRole
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysRole
     */
    updateTime?: Date | null;
}
