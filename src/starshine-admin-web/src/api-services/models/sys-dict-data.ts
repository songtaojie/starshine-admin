import { StatusEnum } from './enums/status-enum';
import { SysDictType } from './sys-dict-type';
import { BasePageInput } from './base/base-page-input';

/**
 * 
 * @export
 * @interface AddDictDataInput
 */
export interface AddDictDataInput {
    /**
     * 字典类型Id
     * @type {number}
     * @memberof AddDictDataInput
     */
    dictTypeId?: number;
    /**
     * 值
     * @type {string}
     * @memberof AddDictDataInput
     */
    value: string;
    /**
     * 编码
     * @type {string}
     * @memberof AddDictDataInput
     */
    code: string;
    /**
     * 排序
     * @type {number}
     * @memberof AddDictDataInput
     */
    sort?: number;
    /**
     * 备注
     * @type {string}
     * @memberof AddDictDataInput
     */
    remark?: string | null;
    /**
     * 
     * @type {StatusEnum}
     * @memberof AddDictDataInput
     */
    status?: StatusEnum;
}

/**
 * 
 * @export
 * @interface UpdateDictDataInput
 */
export interface UpdateDictDataInput extends AddDictDataInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateDictDataInput
     */
    id: number;
}

/**
 * 
 * @export
 * @interface PageDictDataInput
 */
export interface PageDictDataInput extends BasePageInput {
    /**
     * 字典类型Id
     * @type {number}
     * @memberof PageDictDataInput
     */
    dictTypeId?: number;
    /**
     * 值
     * @type {string}
     * @memberof PageDictDataInput
     */
    value?: string | null;
    /**
     * 编码
     * @type {string}
     * @memberof PageDictDataInput
     */
    code?: string | null;
}

/**
 * 
 * @export
 * @interface DictDataInput
 */
export interface DictDataInput {
    /**
     * 主键Id
     * @type {number}
     * @memberof DictDataInput
     */
    id: number;
    /**
     * 
     * @type {StatusEnum}
     * @memberof DictDataInput
     */
    status?: StatusEnum;
}


/**
 * 系统字典值表
 * @export
 * @interface SysDictData
 */
export interface SysDictData extends UpdateDictDataInput{
    /**
     * 创建时间
     * @type {Date}
     * @memberof SysDictData
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysDictData
     */
    updateTime?: Date | null;
    /**
     * 创建者Id
     * @type {number}
     * @memberof SysDictData
     */
    createUserId?: number | null;
    /**
     * 修改者Id
     * @type {number}
     * @memberof SysDictData
     */
    updateUserId?: number | null;
    /**
     * 软删除
     * @type {boolean}
     * @memberof SysDictData
     */
    isDelete?: boolean;
    
    /**
     * 
     * @type {SysDictType}
     * @memberof SysDictData
     */
    dictType?: SysDictType;
}
