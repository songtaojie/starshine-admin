/* tslint:disable */
/* eslint-disable */

import { EnumTypeOutput } from './enum-type-output';
/**
 * 全局返回结果
 * @export
 * @interface AdminResultListEnumTypeOutput
 */
export interface AdminResultListEnumTypeOutput {
    /**
     * 状态码
     * @type {number}
     * @memberof AdminResultListEnumTypeOutput
     */
    code?: number;
    /**
     * 类型success、warning、error
     * @type {string}
     * @memberof AdminResultListEnumTypeOutput
     */
    type?: string | null;
    /**
     * 错误信息
     * @type {string}
     * @memberof AdminResultListEnumTypeOutput
     */
    message?: string | null;
    /**
     * 数据
     * @type {Array<EnumTypeOutput>}
     * @memberof AdminResultListEnumTypeOutput
     */
    result?: Array<EnumTypeOutput> | null;
    /**
     * 附加数据
     * @type {any}
     * @memberof AdminResultListEnumTypeOutput
     */
    extras?: any | null;
    /**
     * 时间
     * @type {Date}
     * @memberof AdminResultListEnumTypeOutput
     */
    time?: Date;
}
