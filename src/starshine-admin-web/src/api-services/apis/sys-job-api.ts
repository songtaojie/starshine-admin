import { AxiosResponse, AxiosRequestConfig } from 'axios';
import { BaseAPI } from '../base';
import { AdminResult,SysJobCluster,SysJobTrigger,AdminResultPagedListResult,JobOutput } from '../models';

import { AddJobDetailInput,UpdateJobDetailInput,DeleteJobDetailInput,JobDetailInput } from '../models';
import { AddJobTriggerInput,UpdateJobTriggerInput,DeleteJobTriggerInput,JobTriggerInput} from '../models';
import { PageJobInput } from '../models';

/**
 * SysJobApi - object-oriented interface
 * @export
 * @class SysJobApi
 * @extends {BaseAPI}
 */
export class SysJobApi extends BaseAPI {
    /**
     * 
     * @summary 添加作业
     * @param {AddJobDetailInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async addJobDetail(data?: AddJobDetailInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/addjobdetail`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 添加触发器
     * @param {AddJobTriggerInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async addJobTrigger(data?: AddJobTriggerInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/addjobtrigger`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 强制唤醒作业调度器
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async cancelJobSleep(options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/cancelSleep`;
        return this.PostVoid({api,...options});
    }
    /**
     * 
     * @summary 删除作业
     * @param {DeleteJobDetailInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async deleteJobDetail(data?: DeleteJobDetailInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/deleteJobDetail`;
        return this.DeleteVoid({api,data,...options});
    }
    /**
     * 
     * @summary 删除触发器
     * @param {DeleteJobTriggerInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async deleteJobTrigger(data?: DeleteJobTriggerInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/deleteJobTrigger`;
        return this.DeleteVoid({api,data,...options});
    }
    /**
     * 
     * @summary 获取集群列表
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async getJobClusterList(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<SysJobCluster>>>> {
        const api = `/api/sys-job/getjobclusterlist`;
        return this.GetAdminResult<Array<SysJobCluster>>({api,...options});
    }
    /**
     * 
     * @summary 获取触发器列表
     * @param {string} [jobId] 作业Id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async getJobTriggerList(jobId?: string, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<SysJobTrigger>>>> {
        const api = `/api/sys-job/jobTriggerList`;
        const params = {jobId}
        return this.GetAdminResult<Array<SysJobTrigger>>({api,params,...options});
    }
    /**
     * 
     * @summary 获取作业分页列表
     * @param {PageJobInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async getJobDetailPage(params?: PageJobInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultPagedListResult<JobOutput>>> {
        const api = `/api/sys-job/getjobdetailpage`;
        return this.PageAdminResult<JobOutput>({api,params,...options});
    }
    /**
     * 
     * @summary 暂停所有作业
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async pauseAllJob(options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/pauseAllJob`;
        return this.PostVoid({api,...options});
    }
    /**
     * 
     * @summary 暂停作业
     * @param {JobDetailInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async pauseJob(data?: JobDetailInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/pauseJob`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 暂停触发器
     * @param {JobTriggerInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async pauseTrigger(data?: JobTriggerInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/pauseTrigger`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 强制触发所有作业持久化
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async persistAllJob(options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/persistAll`;
        return this.PostVoid({api,...options});
    }
    /**
     * 
     * @summary 启动所有作业
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async startAllJob(options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/startAllJob`;
        return this.PostVoid({api,...options});
    }
    /**
     * 
     * @summary 启动作业
     * @param {JobDetailInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async apiSysJobStartJobPost(data?: JobDetailInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/startJob`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 启动触发器
     * @param {JobTriggerInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async startTrigger(data?: JobTriggerInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/startTrigger`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 更新作业
     * @param {UpdateJobDetailInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async updateJobDetail(data?: UpdateJobDetailInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/updateJobDetail`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 更新触发器
     * @param {UpdateJobTriggerInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysJobApi
     */
    public async updateJobTrigger(data?: UpdateJobTriggerInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-job/updateJobTrigger`;
        return this.PostVoid({api,data,...options});
    }
}
