import { StatusEnum } from './enums/status-enum';
/**
 * 
 * @export
 * @interface AddOrgInput
 */
export interface AddOrgInput {
    /**
     * 父Id
     * @type {number}
     * @memberof AddOrgInput
     */
    pid?: number;
    /**
     * 编码
     * @type {string}
     * @memberof AddOrgInput
     */
    code: string;
    /**
     * 排序
     * @type {number}
     * @memberof AddOrgInput
     */
    sort?: number;
    /**
     * 备注
     * @type {string}
     * @memberof AddOrgInput
     */
    remark?: string | null;
    /**
     * 
     * @type {StatusEnum}
     * @memberof AddOrgInput
     */
    status?: StatusEnum;
    /**
     * 名称
     * @type {string}
     * @memberof AddOrgInput
     */
    name: string;
}

/**
 * 
 * @export
 * @interface UpdateOrgInput
 */
export interface UpdateOrgInput extends AddOrgInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateOrgInput
     */
    id: number;
}

/**
 * 系统机构表
 * @export
 * @interface SysOrg
 */
export interface SysOrg extends UpdateOrgInput {
    /**
     * 创建时间
     * @type {Date}
     * @memberof SysOrg
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysOrg
     */
    updateTime?: Date | null;

    /**
     * 机构子项
     * @type {Array<SysOrg>}
     * @memberof SysOrg
     */
    children?: Array<SysOrg> | null;
}

/**
 * 
 * @export
 * @interface ListOrgInput
 */
export interface ListOrgInput  {
    /**
     * 雪花Id
     * @type {number}
     * @memberof ListOrgInput
     */
    id: number;
    /**
     * 编码
     * @type {string}
     * @memberof ListOrgInput
     */
    code?: string | undefined;
    /**
     * 
     * @type {StatusEnum}
     * @memberof ListOrgInput
     */
    status?: StatusEnum | undefined;
    /**
     * 名称
     * @type {string}
     * @memberof ListOrgInput
     */
    name?: string  | undefined;
}