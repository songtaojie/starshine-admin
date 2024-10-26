/* tslint:disable */
/* eslint-disable */

/**
 * 枚举实体
 * @export
 * @interface EnumEntity
 */
export interface EnumEntity {
    /**
     * 枚举的描述
     * @type {string}
     * @memberof EnumEntity
     */
    describe?: string | null;
    /**
     * 枚举名称
     * @type {string}
     * @memberof EnumEntity
     */
    name?: string | null;
    /**
     * 枚举对象的值
     * @type {number}
     * @memberof EnumEntity
     */
    value?: number;
}
