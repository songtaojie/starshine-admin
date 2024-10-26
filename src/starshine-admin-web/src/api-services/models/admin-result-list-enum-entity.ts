/* tslint:disable */
/* eslint-disable */

import { EnumEntity } from './enum-entity';
/**
 * 全局返回结果
 * @export
 * @interface AdminResultListEnumEntity
 */
export interface AdminResultListEnumEntity {
    /**
     * 状态码
     * @type {number}
     * @memberof AdminResultListEnumEntity
     */
    code?: number;
    /**
     * 类型success、warning、error
     * @type {string}
     * @memberof AdminResultListEnumEntity
     */
    type?: string | null;
    /**
     * 错误信息
     * @type {string}
     * @memberof AdminResultListEnumEntity
     */
    message?: string | null;
    /**
     * 数据
     * @type {Array<EnumEntity>}
     * @memberof AdminResultListEnumEntity
     */
    result?: Array<EnumEntity> | null;
    /**
     * 附加数据
     * @type {any}
     * @memberof AdminResultListEnumEntity
     */
    extras?: any | null;
    /**
     * 时间
     * @type {Date}
     * @memberof AdminResultListEnumEntity
     */
    time?: Date;
}
