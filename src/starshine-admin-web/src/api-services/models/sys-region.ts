/* tslint:disable */
/* eslint-disable */

import { BasePageInput } from './base/base-page-input';


/**
 * 
 * @export
 * @interface AddRegionInput
 */
export interface AddRegionInput {
    
    /**
     * 父Id
     * @type {number}
     * @memberof AddRegionInput
     */
    pid?: number;
    /**
     * 简称
     * @type {string}
     * @memberof AddRegionInput
     */
    shortName?: string | null;
    /**
     * 组合名
     * @type {string}
     * @memberof AddRegionInput
     */
    mergerName?: string | null;
    /**
     * 行政代码
     * @type {string}
     * @memberof AddRegionInput
     */
    code?: string | null;
    /**
     * 邮政编码
     * @type {string}
     * @memberof AddRegionInput
     */
    zipCode?: string | null;
    /**
     * 区号
     * @type {string}
     * @memberof AddRegionInput
     */
    cityCode?: string | null;
    /**
     * 层级
     * @type {number}
     * @memberof AddRegionInput
     */
    level?: number;
    /**
     * 拼音
     * @type {string}
     * @memberof AddRegionInput
     */
    pinYin?: string | null;
    /**
     * 经度
     * @type {number}
     * @memberof AddRegionInput
     */
    lng?: number;
    /**
     * 维度
     * @type {number}
     * @memberof AddRegionInput
     */
    lat?: number;
    /**
     * 排序
     * @type {number}
     * @memberof AddRegionInput
     */
    sort?: number;
    /**
     * 备注
     * @type {string}
     * @memberof AddRegionInput
     */
    remark?: string | null;
    /**
     * 机构子项
     * @type {Array<SysRegion>}
     * @memberof AddRegionInput
     */
    children?: Array<SysRegion> | null;
    /**
     * 名称
     * @type {string}
     * @memberof AddRegionInput
     */
    name: string;
}

/**
 * 
 * @export
 * @interface UpdateRegionInput
 */
export interface UpdateRegionInput extends AddRegionInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateRegionInput
     */
    id: number;
}

/**
 * 
 * @export
 * @interface PageRegionInput
 */
export interface PageRegionInput extends BasePageInput {
    /**
     * 父节点Id
     * @type {number}
     * @memberof PageRegionInput
     */
    pid?: number;
    /**
     * 名称
     * @type {string}
     * @memberof PageRegionInput
     */
    name?: string | null;
    /**
     * 编码
     * @type {string}
     * @memberof PageRegionInput
     */
    code?: string | null;
}


/**
 * 系统行政地区表
 * @export
 * @interface SysRegion
 */
export interface SysRegion {
    /**
     * 雪花Id
     * @type {number}
     * @memberof SysRegion
     */
    id?: number;
    /**
     * 父Id
     * @type {number}
     * @memberof SysRegion
     */
    pid?: number;
    /**
     * 名称
     * @type {string}
     * @memberof SysRegion
     */
    name: string;
    /**
     * 简称
     * @type {string}
     * @memberof SysRegion
     */
    shortName?: string | null;
    /**
     * 组合名
     * @type {string}
     * @memberof SysRegion
     */
    mergerName?: string | null;
    /**
     * 行政代码
     * @type {string}
     * @memberof SysRegion
     */
    code?: string | null;
    /**
     * 邮政编码
     * @type {string}
     * @memberof SysRegion
     */
    zipCode?: string | null;
    /**
     * 区号
     * @type {string}
     * @memberof SysRegion
     */
    cityCode?: string | null;
    /**
     * 层级
     * @type {number}
     * @memberof SysRegion
     */
    level?: number;
    /**
     * 拼音
     * @type {string}
     * @memberof SysRegion
     */
    pinYin?: string | null;
    /**
     * 经度
     * @type {number}
     * @memberof SysRegion
     */
    lng?: number;
    /**
     * 维度
     * @type {number}
     * @memberof SysRegion
     */
    lat?: number;
    /**
     * 排序
     * @type {number}
     * @memberof SysRegion
     */
    orderNo?: number;
    /**
     * 备注
     * @type {string}
     * @memberof SysRegion
     */
    remark?: string | null;
    /**
     * 机构子项
     * @type {Array<SysRegion>}
     * @memberof SysRegion
     */
    children?: Array<SysRegion> | null;
}
