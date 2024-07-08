import { AxiosResponse, AxiosRequestConfig } from 'axios';
import { BaseAPI } from '../base';
import { LoginInput,LoginOutput,LoginUserOutput,AdminResult } from '../models';
/**
 * SysAuthApi - object-oriented interface
 * @export
 * @class SysAuthApi
 * @extends {BaseAPI}
 */
export class SysAuthApi extends BaseAPI {

    /**
     * 
     * @summary 获取验证码
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysAuthApi
     */
    public async getCaptcha(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<any>>> {
        return this.GetAdminResult<any>({api:'/api/sys-auth/getcaptcha', ...options});
    }
    /**
     * 
     * @summary 获取系统配置
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysAuthApi
     */
    public async getSystemConfig(options?: AxiosRequestConfig) : Promise<AxiosResponse<any>> {
        return this.GetAdminResult<any>({api:'/api/sys-auth/systemconfig',...options});
    }
    /**
     * 用户名/密码：superadmin/123456
     * @summary 登录系统
     * @param {LoginInput} body 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysAuthApi
     */
    public async login(body: LoginInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<LoginOutput>>> {
        return this.PostAdminResult<LoginOutput>({api:'/api/sys-auth/login',data :body, ...options});
    }
    /**
     * 
     * @summary 退出系统
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysAuthApi
     */
    public async logout(options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        return this.Post<void>({api:'/api/sys-auth/logout',...options}).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 获取刷新Token
     * @param {string} accessToken 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysAuthApi
     */
    public async getRefreshToken(accessToken: string, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<String>>> {
        const api = `/api/sys-auth/refreshtoken/${ encodeURIComponent(String(accessToken))}`;
        return this.GetAdminResult<String>({api,...options});
    }
   
    /**
     * 
     * @summary 获取登录账号
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysAuthApi
     */
    public async getUserInfo(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<LoginUserOutput>>> {
        const api = `/api/sys-auth/userinfo`;
        return this.GetAdminResult<LoginUserOutput>({api,...options});
    }
}
