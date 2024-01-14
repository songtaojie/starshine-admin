import { AxiosResponse,  AxiosRequestConfig } from 'axios';
// Some imports not used depending on template conditions
// @ts-ignore
import {  BaseAPI, RequiredError } from '../base';
import { AdminResult,AddOrgInput,SysOrg,UpdateOrgInput,BaseIdInput,ListOrgInput } from '../models';

/**
 * SysOrgApi - object-oriented interface
 * @export
 * @class SysOrgApi
 * @extends {BaseAPI}
 */
export class SysOrgApi extends BaseAPI {
    /**
     * 
     * @summary 增加机构
     * @param {AddOrgInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysOrgApi
     */
    public async addSysOrg(body?: AddOrgInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<number>>> {
        const api = `/api/sys-org/add`;
        return this.PostAdminResult<number>({api,data:body,...options});
    }
    /**
     * 
     * @summary 删除机构
     * @param {BaseIdInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysOrgApi
     */
    public async deleteSysOrg(body?: BaseIdInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-org/delete`;
        return this.DeleteVoid({api,data:body,...options});
    }
    /**
     * 
     * @summary 获取机构列表
     * @param {number} id 主键Id
     * @param {string} [name] 名称
     * @param {string} [code] 编码
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysOrgApi
     */
    public async getSysOrgList(params:ListOrgInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<SysOrg>>>> {
        const api = `/api/sys-org/getlist`;
        return this.GetAdminResult<Array<SysOrg>>({api,params,...options});
    }
    /**
     * 
     * @summary 更新机构
     * @param {UpdateOrgInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysOrgApi
     */
    public async updateSysOrg(body?: UpdateOrgInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-org/update`;
        return this.PostVoid({api,data:body,...options});
    }
}
