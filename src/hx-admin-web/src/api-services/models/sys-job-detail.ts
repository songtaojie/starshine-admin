import { JobCreateTypeEnum } from './job-create-type-enum';
import { BasePageInput } from './base/base-page-input';

/**
 * 
 * @export
 * @interface AddJobDetailInput
 */
 export interface AddJobDetailInput {
    
    /**
     * 组名称
     * @type {string}
     * @memberof AddJobDetailInput
     */
    groupName?: string | null;
    /**
     * 作业类型FullName
     * @type {string}
     * @memberof AddJobDetailInput
     */
    jobType?: string | null;
    /**
     * 程序集Name
     * @type {string}
     * @memberof AddJobDetailInput
     */
    assemblyName?: string | null;
    /**
     * 描述信息
     * @type {string}
     * @memberof AddJobDetailInput
     */
    description?: string | null;
    /**
     * 是否并行执行
     * @type {boolean}
     * @memberof AddJobDetailInput
     */
    concurrent?: boolean;
    /**
     * 是否扫描特性触发器
     * @type {boolean}
     * @memberof AddJobDetailInput
     */
    includeAnnotations?: boolean;
    /**
     * 额外数据
     * @type {string}
     * @memberof AddJobDetailInput
     */
    properties?: string | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof AddJobDetailInput
     */
    updatedTime?: Date | null;
    /**
     * 
     * @type {JobCreateTypeEnum}
     * @memberof AddJobDetailInput
     */
    createType?: JobCreateTypeEnum;
    /**
     * 脚本代码
     * @type {string}
     * @memberof AddJobDetailInput
     */
    scriptCode?: string | null;
    /**
     * 作业Id
     * @type {string}
     * @memberof AddJobDetailInput
     */
    jobId: string;
}


/**
 * 
 * @export
 * @interface UpdateJobDetailInput
 */
 export interface UpdateJobDetailInput extends AddJobDetailInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateJobDetailInput
     */
    id: number;
}

/**
 * 
 * @export
 * @interface DeleteJobDetailInput
 */
 export interface DeleteJobDetailInput {
    /**
     * 作业Id
     * @type {string}
     * @memberof DeleteJobDetailInput
     */
    jobId?: string | null;
}


/**
 * 
 * @export
 * @interface JobDetailInput
 */
 export interface JobDetailInput {
    /**
     * 作业Id
     * @type {string}
     * @memberof JobDetailInput
     */
    jobId?: string | null;
}

/**
 * 
 * @export
 * @interface PageJobInput
 */
 export interface PageJobInput extends BasePageInput{
    /**
     * 作业Id
     * @type {string}
     * @memberof PageJobInput
     */
    jobId?: string | null;
    /**
     * 描述信息
     * @type {string}
     * @memberof PageJobInput
     */
    description?: string | null;
}


/**
 * 系统作业信息表
 * @export
 * @interface SysJobDetail
 */
export interface SysJobDetail {
    /**
     * 雪花Id
     * @type {number}
     * @memberof SysJobDetail
     */
    id?: number;
    /**
     * 作业Id
     * @type {string}
     * @memberof SysJobDetail
     */
    jobId: string;
    /**
     * 组名称
     * @type {string}
     * @memberof SysJobDetail
     */
    groupName?: string | null;
    /**
     * 作业类型FullName
     * @type {string}
     * @memberof SysJobDetail
     */
    jobType?: string | null;
    /**
     * 程序集Name
     * @type {string}
     * @memberof SysJobDetail
     */
    assemblyName?: string | null;
    /**
     * 描述信息
     * @type {string}
     * @memberof SysJobDetail
     */
    description?: string | null;
    /**
     * 是否并行执行
     * @type {boolean}
     * @memberof SysJobDetail
     */
    concurrent?: boolean;
    /**
     * 是否扫描特性触发器
     * @type {boolean}
     * @memberof SysJobDetail
     */
    includeAnnotations?: boolean;
    /**
     * 额外数据
     * @type {string}
     * @memberof SysJobDetail
     */
    properties?: string | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysJobDetail
     */
    updatedTime?: Date | null;
    /**
     * 
     * @type {JobCreateTypeEnum}
     * @memberof SysJobDetail
     */
    createType?: JobCreateTypeEnum;
    /**
     * 脚本代码
     * @type {string}
     * @memberof SysJobDetail
     */
    scriptCode?: string | null;
}
