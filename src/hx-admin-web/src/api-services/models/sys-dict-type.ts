import { StatusEnum } from './enums/status-enum';
import { BasePageInput } from './base/base-page-input';

/**
 * 
 * @export
 * @interface AddDictTypeInput
 */
export interface AddDictTypeInput {
    /**
     * 名称
     * @type {string}
     * @memberof AddDictTypeInput
     */
    name: string;
    /**
     * 编码
     * @type {string}
     * @memberof AddDictTypeInput
     */
    code: string;
    /**
     * 排序
     * @type {number}
     * @memberof AddDictTypeInput
     */
    sort?: number;
    /**
     * 备注
     * @type {string}
     * @memberof AddDictTypeInput
     */
    remark?: string | null;
    /**
     * 
     * @type {StatusEnum}
     * @memberof AddDictTypeInput
     */
    status?: StatusEnum;
}

/**
 * 
 * @export
 * @interface UpdateDictTypeInput
 */
export interface UpdateDictTypeInput extends AddDictTypeInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateDictTypeInput
     */
    id: number;
}

/**
 * 
 * @export
 * @interface PageDictTypeInput
 */
export interface PageDictTypeInput extends BasePageInput {
    /**
     * 名称
     * @type {string}
     * @memberof PageDictTypeInput
     */
    name?: string | null;
    /**
     * 编码
     * @type {string}
     * @memberof PageDictTypeInput
     */
    code?: string | null;
}

/**
 * 
 * @export
 * @interface DictTypeInput
 */
export interface DictTypeInput {
    /**
     * 主键Id
     * @type {number}
     * @memberof DictTypeInput
     */
    id: number;
    /**
     * 
     * @type {StatusEnum}
     * @memberof DictTypeInput
     */
    status?: StatusEnum;
}


/**
 * 系统字典类型表
 * @export
 * @interface SysDictType
 */
export interface SysDictType extends UpdateDictTypeInput{
    /**
     * 创建时间
     * @type {Date}
     * @memberof SysDictType
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysDictType
     */
    updateTime?: Date | null;
}
