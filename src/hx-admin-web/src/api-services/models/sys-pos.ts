import { StatusEnum } from './enums/status-enum';

/**
 * 
 * @export
 * @interface AddPosInput
 */
export interface AddPosInput {
    /**
     * 编码
     * @type {string}
     * @memberof AddPosInput
     */
    code: string ;
    /**
     * 排序
     * @type {number}
     * @memberof AddPosInput
     */
    sort?: number;
    /**
     * 备注
     * @type {string}
     * @memberof AddPosInput
     */
    remark?: string | null;
    /**
     * 
     * @type {StatusEnum}
     * @memberof AddPosInput
     */
    status?: StatusEnum;
    /**
     * 名称
     * @type {string}
     * @memberof AddPosInput
     */
    name: string;
}
/**
 * 
 * @export
 * @interface UpdatePosInput
 */
export interface UpdatePosInput extends AddPosInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdatePosInput
     */
    id?: number;
}


/**
 * 系统职位表
 * @export
 * @interface SysPos
 */
export interface SysPos extends UpdatePosInput {
 
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysPos
     */
    updateTime?: Date | null;
}
