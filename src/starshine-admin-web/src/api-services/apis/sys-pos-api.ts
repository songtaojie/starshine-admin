import  { AxiosResponse,AxiosRequestConfig } from 'axios';
import {  BaseAPI } from '../base';
import { AddPosInput,AdminResult,SysPos,BaseIdInput } from '../models';
import { UpdatePosInput } from '../models';

/**
 * SysPosApi - object-oriented interface
 * @export
 * @class SysPosApi
 * @extends {BaseAPI}
 */
export class SysPosApi extends BaseAPI {
    /**
     * 
     * @summary 增加职位
     * @param {AddPosInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysPosApi
     */
    public async addSysPos(data?: AddPosInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-pos/add`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 删除职位
     * @param {BaseIdInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysPosApi
     */
    public async deleteSysPos(data?: BaseIdInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-pos/delete`;
        return this.DeleteVoid({api,data,...options});
    }
    /**
     * 
     * @summary 获取职位列表
     * @param {string} [name] 名称
     * @param {string} [code] 编码
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysPosApi
     */
    public async getSysPosList(name?: string, code?: string, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<SysPos>>>> {
        const api = `/api/sys-pos/getlist`;
        return this.GetAdminResult<Array<SysPos>>({api,params:{
            code,name
        },...options});
    }
    /**
     * 
     * @summary 更新职位
     * @param {UpdatePosInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysPosApi
     */
    public async updateSysPos(data?: UpdatePosInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-pos/update`;
        return this.PostVoid({api,data,...options});
    }
}
