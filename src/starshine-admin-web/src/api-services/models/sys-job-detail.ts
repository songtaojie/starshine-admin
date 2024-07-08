import { JobCreateTypeEnum } from './enums/job-create-type-enum';
import { BasePageInput } from './base/base-page-input';
import { SysJobTrigger } from './sys-job-trigger';

/**
 * 
 * @export
 * @interface AddJobDetailInput
 */
 export interface AddJobDetailInput {
    /**
     * 调度名字
     * @type {string}
     * @memberof AddJobDetailInput
     */
    schedulerName: string;
    /**
     * 作业分组
     * @type {string}
     * @memberof AddJobDetailInput
     */
    jobGroup: string;
    /**
     * 作业名字
     * @type {string}
     * @memberof AddJobDetailInput
     */
    jobName: string;
    /**
     * 描述信息
     * @type {string}
     * @memberof AddJobDetailInput
     */
    description?: string | null;
    /**
     * 是否持久化
     * @type {boolean}
     * @memberof AddJobDetailInput
     */
    isDurable?: boolean;
    /**
     * 是否非并行执行
     * @type {boolean}
     * @memberof AddJobDetailInput
     */
    isNonConcurrent?: boolean;
    /**
     * 是否扫描特性触发器
     * @type {boolean}
     * @memberof AddJobDetailInput
     */
    isUpdateData?: boolean;
    /**
     * 是否扫描特性触发器
     * @type {boolean}
     * @memberof AddJobDetailInput
     */
    requestsRecovery?: boolean;
    /**
     * 额外数据
     * @type {string}
     * @memberof AddJobDetailInput
     */
    jobData?: string | null;
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
     * 调度名字
     * @type {string}
     * @memberof AddJobDetailInput
     */
    schedulerName: string;
    /**
     * 作业分组
     * @type {string}
     * @memberof AddJobDetailInput
     */
    jobGroup: string;
    /**
     * 作业名字
     * @type {string}
     * @memberof AddJobDetailInput
     */
    jobName: string;
}


/**
 * 
 * @export
 * @interface JobDetailInput
 */
 export interface JobDetailInput {
    /**
     * 调度名字
     * @type {string}
     * @memberof AddJobDetailInput
     */
    schedulerName: string;
    /**
     * 作业分组
     * @type {string}
     * @memberof AddJobDetailInput
     */
    jobGroup: string;
    /**
     * 作业名字
     * @type {string}
     * @memberof AddJobDetailInput
     */
    jobName: string;
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


/**
 * 
 * @export
 * @interface JobOutput
 */
export interface JobOutput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof SysJobDetail
     */
    id?: number;
    /**
     * 调度名字
     * @type {string}
     * @memberof SysJobDetail
     */
    schedulerName: string;
    /**
     * 作业Id
     * @type {string}
     * @memberof SysJobDetail
     */
    jobName: string;
    /**
     * 组名称
     * @type {string}
     * @memberof SysJobDetail
     */
    jobGroup: string;
    /**
     * 作业类型FullName
     * @type {string}
     * @memberof SysJobDetail
     */
    jobClassName?: string | null;
    /**
     * 描述信息
     * @type {string}
     * @memberof SysJobDetail
     */
    description?: string | null;
    /**
     * 是否持久化
     * @type {boolean}
     * @memberof SysJobDetail
     */
    isDurable?: boolean;
    /**
     * 是否并行执行
     * @type {boolean}
     * @memberof SysJobDetail
     */
    isNonConcurrent?: boolean;
    /**
     * 是否更新数据
     * @type {boolean}
     * @memberof SysJobDetail
     */
    isUpdateData?: boolean;
    /**
     * 请求恢复
     * @type {boolean}
     * @memberof SysJobDetail
     */
    requestsRecovery?: boolean;
    /**
     * 额外数据
     * @type {string}
     * @memberof SysJobDetail
     */
    jobData?: string | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysJobDetail
     */
    updateTime?: Number | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysJobDetail
     */
    updateTime_V?: String | null;
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
    /**
     * 触发器集合
     * @type {Array<SysJobTrigger>}
     * @memberof JobOutput
     */
    jobTriggers?: Array<SysJobTrigger> | null;
}