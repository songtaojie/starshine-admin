/* tslint:disable */
/* eslint-disable */
/**
 * 所有接口
 * 让 .NET 开发更简单、更通用、更流行。前后端分离架构(.NET6/Vue3)，开箱即用紧随前沿技术。<br/><a href='https://gitee.com/zuohuaijun/Admin.NET/'>https://gitee.com/zuohuaijun/Admin.NET</a>
 *
 * OpenAPI spec version: 1.0.0
 * Contact: 515096995@qq.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import globalAxios, { AxiosResponse, AxiosInstance, AxiosRequestConfig } from 'axios';
import { Configuration } from '../configuration';
// Some imports not used depending on template conditions
// @ts-ignore
import { BASE_PATH, COLLECTION_FORMATS, RequestArgs, BaseAPI, RequiredError } from '../base';
import { AdminResultSqlSugarPagedListSysOnlineUser } from '../models';
import { PageOnlineUserInput } from '../models';
import { SysOnlineUser } from '../models';
/**
 * SysOnlineUserApi - axios parameter creator
 * @export
 */
export const SysOnlineUserApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 强制下线
         * @param {SysOnlineUser} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiSysOnlineUserForceOfflinePost: async (body?: SysOnlineUser, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/sysOnlineUser/forceOffline`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 获取在线用户分页列表
         * @param {PageOnlineUserInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiSysOnlineUserPagePost: async (body?: PageOnlineUserInput, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/sysOnlineUser/page`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
    }
};

/**
 * SysOnlineUserApi - functional programming interface
 * @export
 */
export const SysOnlineUserApiFp = function(configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 强制下线
         * @param {SysOnlineUser} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysOnlineUserForceOfflinePost(body?: SysOnlineUser, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<void>>> {
            const localVarAxiosArgs = await SysOnlineUserApiAxiosParamCreator(configuration).apiSysOnlineUserForceOfflinePost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @summary 获取在线用户分页列表
         * @param {PageOnlineUserInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysOnlineUserPagePost(body?: PageOnlineUserInput, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultSqlSugarPagedListSysOnlineUser>>> {
            const localVarAxiosArgs = await SysOnlineUserApiAxiosParamCreator(configuration).apiSysOnlineUserPagePost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
    }
};

/**
 * SysOnlineUserApi - factory interface
 * @export
 */
export const SysOnlineUserApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    return {
        /**
         * 
         * @summary 强制下线
         * @param {SysOnlineUser} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysOnlineUserForceOfflinePost(body?: SysOnlineUser, options?: AxiosRequestConfig): Promise<AxiosResponse<void>> {
            return SysOnlineUserApiFp(configuration).apiSysOnlineUserForceOfflinePost(body, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 获取在线用户分页列表
         * @param {PageOnlineUserInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysOnlineUserPagePost(body?: PageOnlineUserInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultSqlSugarPagedListSysOnlineUser>> {
            return SysOnlineUserApiFp(configuration).apiSysOnlineUserPagePost(body, options).then((request) => request(axios, basePath));
        },
    };
};

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
    public async apiSysOnlineUserForceOfflinePost(body?: SysOnlineUser, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        return SysOnlineUserApiFp(this.configuration).apiSysOnlineUserForceOfflinePost(body, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 获取在线用户分页列表
     * @param {PageOnlineUserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysOnlineUserApi
     */
    public async apiSysOnlineUserPagePost(body?: PageOnlineUserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultSqlSugarPagedListSysOnlineUser>> {
        return SysOnlineUserApiFp(this.configuration).apiSysOnlineUserPagePost(body, options).then((request) => request(this.axios, this.basePath));
    }
}
