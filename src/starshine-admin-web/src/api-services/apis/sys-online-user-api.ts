import { AxiosResponse,AxiosRequestConfig } from 'axios';
import {  BaseAPI } from '../base';
import { AdminResultPagedListResult,SysOnlineUser } from '../models';
import { PageOnlineUserInput } from '../models';


/**
 * SysOnlineUserApi - object-oriented interface
 * @export
 * @class SysOnlineUserApi
 * @extends {BaseAPI}
 */
export class SysOnlineUserApi extends BaseAPI {
    /**
     * 
     * @summary 强制下线
     * @param {SysOnlineUser} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysOnlineUserApi
     */
    public async forceOffline(body?: SysOnlineUser, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysOnlineUser/forceOffline`;
        return this.PostVoid({api,data:body,...options});
    }
    /**
     * 
     * @summary 获取在线用户分页列表
     * @param {PageOnlineUserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysOnlineUserApi
     */
    public async getSysOnlineUser(body?: PageOnlineUserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultPagedListResult<SysOnlineUser>>> {
        const api = `/api/sysOnlineUser/page`;
        return this.PageAdminResult({api,params:body,...options});
    }
}
