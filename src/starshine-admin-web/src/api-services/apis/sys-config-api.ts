import { AxiosResponse, AxiosRequestConfig } from 'axios';
import {  BaseAPI } from '../base';
import { AddConfigInput, AdminResult,SysConfig,AdminResultPagedListResult,DeleteConfigInput,PageConfigInput,UpdateConfigInput } from '../models';

/**
 * SysConfigApi - object-oriented interface
 * @export
 * @class SysConfigApi
 * @extends {BaseAPI}
 */
export class SysConfigApi extends BaseAPI {
    /**
     * 
     * @summary 增加参数配置
     * @param {AddConfigInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysConfigApi
     */
    public async addSysConfig(body?: AddConfigInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<void>>> {
        const api = `/api/sys-config/add`;
        return this.PostAdminResult<void>({api,data:body,...options});
    }
    /**
     * 
     * @summary 删除参数配置
     * @param {DeleteConfigInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysConfigApi
     */
    public async deleteSysConfig(body?: DeleteConfigInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<void>>> {
        const api = `/api/sys-config/delete`;
        return this.DeleteAdminResult({api,data:body,...options});
    }
    /**
     * 
     * @summary 获取参数配置详情
     * @param {number} id 主键Id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysConfigApi
     */
    public async getSysConfigDetail(id: number, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<SysConfig>>> {
        const api = `/api/sys-config/detail`;
        return this.GetAdminResult<SysConfig>({api,params:{id},...options});
    }
    /**
     * 
     * @summary 获取分组列表
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysConfigApi
     */
    public async getGroupList(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<string>>>> {
        const api = `/api/sys-config/getgroupList`;
        return this.GetAdminResult<Array<string>>({api,...options});
    }
    /**
     * 
     * @summary 获取参数配置列表
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysConfigApi
     */
    public async getSysConfigList(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<SysConfig>>>> {
        const api = `/api/sys-config/list`;
        return this.GetAdminResult<Array<SysConfig>>({api,...options});
    }
    /**
     * 
     * @summary 获取参数配置分页列表
     * @param {PageConfigInput} [params] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysConfigApi
     */
    public async getSysConfigPage(params?: PageConfigInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultPagedListResult<SysConfig>>> {
        const api = `/api/sys-config/getpage`;
        return this.PageAdminResult<SysConfig>({api,params,...options});
    }
    /**
     * 
     * @summary 更新参数配置
     * @param {UpdateConfigInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysConfigApi
     */
    public async updateSysConfig(body?: UpdateConfigInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<void>>> {
        const api = `/api/sys-config/update`;
        return this.PostAdminResult<void>({api,params:body,...options});
    }
}
