/* tslint:disable */
/* eslint-disable */

import { StatusEnum } from './enums/status-enum';
import { BasePageInput } from './base/base-page-input';
/**
 * 
 * @export
 * @interface PageConfigInput
 */
export interface PageConfigInput extends BasePageInput {
    /**
     * 名称
     * @type {string}
     * @memberof PageConfigInput
     */
    name?: string | null;
    /**
     * 编码
     * @type {string}
     * @memberof PageConfigInput
     */
    code?: string | null;
    /**
     * 分组编码
     * @type {string}
     * @memberof PageConfigInput
     */
    groupCode?: string | null;
}

/**
 * 
 * @export
 * @interface AddConfigInput
 */
export interface AddConfigInput {
    /**
     * 名称
     * @type {string}
     * @memberof AddConfigInput
     */
    name: string;
    /**
     * 编码
     * @type {string}
     * @memberof AddConfigInput
     */
    code?: string | null;
    /**
     * 属性值
     * @type {string}
     * @memberof AddConfigInput
     */
    value?: string | null;
    /**
     * 
     * @type {StatusEnum}
     * @memberof AddConfigInput
     */
    sysFlag?: StatusEnum;
    /**
     * 分组编码
     * @type {string}
     * @memberof AddConfigInput
     */
    groupCode?: string | null;
    /**
     * 排序
     * @type {number}
     * @memberof AddConfigInput
     */
    sort?: number;
    /**
     * 备注
     * @type {string}
     * @memberof AddConfigInput
     */
    remark?: string | null;
}

/**
 * 
 * @export
 * @interface UpdateConfigInput
 */
export interface UpdateConfigInput extends AddConfigInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateConfigInput
     */
    id: number;
}

/**
 * 系统参数配置表
 * @export
 * @interface SysConfig
 */
export interface SysConfig {
    /**
     * 雪花Id
     * @type {number}
     * @memberof SysConfig
     */
    id?: number;
    /**
     * 创建时间
     * @type {Date}
     * @memberof SysConfig
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysConfig
     */
    updateTime?: Date | null;
    /**
     * 创建者Id
     * @type {number}
     * @memberof SysConfig
     */
    createUserId?: number | null;
    /**
     * 修改者Id
     * @type {number}
     * @memberof SysConfig
     */
    updateUserId?: number | null;
    /**
     * 软删除
     * @type {boolean}
     * @memberof SysConfig
     */
    isDelete?: boolean;
    /**
     * 名称
     * @type {string}
     * @memberof SysConfig
     */
    name: string;
    /**
     * 编码
     * @type {string}
     * @memberof SysConfig
     */
    code?: string | null;
    /**
     * 属性值
     * @type {string}
     * @memberof SysConfig
     */
    value?: string | null;
    /**
     * 
     * @type {StatusEnum}
     * @memberof SysConfig
     */
    sysFlag?: StatusEnum;
    /**
     * 分组编码
     * @type {string}
     * @memberof SysConfig
     */
    groupCode?: string | null;
    /**
     * 排序
     * @type {number}
     * @memberof SysConfig
     */
    orderNo?: number;
    /**
     * 备注
     * @type {string}
     * @memberof SysConfig
     */
    remark?: string | null;
}
