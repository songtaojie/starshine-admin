import { AxiosResponse, AxiosRequestConfig } from 'axios';
import { BaseAPI } from '../base';
import { AdminResultPagedListResult } from '../models';
import { DeleteWechatUserInput } from '../models';
import { SysWechatUser } from '../models';
import { WechatUserInput } from '../models';


/**
 * SysWechatUserApi - object-oriented interface
 * @export
 * @class SysWechatUserApi
 * @extends {BaseAPI}
 */
export class SysWechatUserApi extends BaseAPI {
    /**
     * 
     * @summary 增加微信用户
     * @param {SysWechatUser} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysWechatUserApi
     */
    public async addSysWechatUser(data?: SysWechatUser, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysWechatUser/add`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 删除微信用户
     * @param {DeleteWechatUserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysWechatUserApi
     */
    public async deleteSysWechatUser(data?: DeleteWechatUserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysWechatUser/delete`;
        return this.DeleteVoid({api,data,...options});
    }
    /**
     * 
     * @summary 获取微信用户列表
     * @param {WechatUserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysWechatUserApi
     */
    public async getSysWechatUserPage(params?: WechatUserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultPagedListResult<SysWechatUser>>> {
        const api = `/api/sysWechatUser/page`;
        return this.PageAdminResult<SysWechatUser>({api,params,...options});
    }
    /**
     * 
     * @summary 更新微信用户
     * @param {SysWechatUser} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysWechatUserApi
     */
    public async updateSysWechatUser(data?: SysWechatUser, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysWechatUser/update`;
        return this.PostVoid({api,data,...options});
    }
}
