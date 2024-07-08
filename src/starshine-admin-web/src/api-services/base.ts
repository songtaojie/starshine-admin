import { Configuration } from "./configuration";
import globalAxios, { AxiosResponse,AxiosRequestConfig, AxiosInstance } from 'axios';
import { AdminResult, AdminResultPagedListResult } from './models/base';
export const BASE_PATH = "/".replace(/\/+$/, "");

/**
 *
 * @export
 */
export const COLLECTION_FORMATS = {
    csv: ",",
    ssv: " ",
    tsv: "\t",
    pipes: "|",
};

/**
 *
 * @export
 * @interface RequestArgs
 */
export interface RequestArgs {
    url: string;
    options: AxiosRequestConfig;
}

/**
 *
 * @export
 * @class BaseAPI
 */
export class BaseAPI {
    protected configuration: Configuration | undefined;

    constructor(configuration?: Configuration, protected basePath: string = BASE_PATH, protected axios: AxiosInstance = globalAxios) {
        if (configuration) {
            this.configuration = configuration;
            this.basePath = configuration.basePath || this.basePath;
        }
    }

    /**
         * 
         * @summary 删除
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
    async Delete<Tout>(options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<Tout>>> {
        const newOptions = {method: 'DELETE', ...options}
        const localVarAxiosArgs = await ApiAxiosParamCreator(this.configuration).BuildParam(newOptions);
        return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
            const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
            return axios.request(axiosRequestArgs);
        };
    }
    /**
     * 
     * @summary 删除
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async DeleteVoid(options?: AxiosRequestConfig):  Promise<AxiosResponse<void>> {
        return this.Delete<void>(options).then((request) => request(this.axios, this.basePath))
    }

    /**
     * 
     * @summary 删除
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async DeleteAdminResult<Tout>(options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResult<Tout>>> {
        return this.Delete<AdminResult<Tout>>(options).then((request) => request(this.axios, this.basePath))
    }

    /**
     * 
     * @summary 获取分页列表
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async Page<Tout>(options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultPagedListResult<Tout>>>> {
        const newOptions = {method: 'GET', ...options}
        const localVarAxiosArgs = await ApiAxiosParamCreator(this.configuration).BuildParam(newOptions);
        return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
            const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
            return axios.request(axiosRequestArgs);
        };
    }
    /**
     * 
     * @summary 获取分页列表
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
   async PageAdminResult<Tout>(options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultPagedListResult<Tout>>> {
     return this.Page<Tout>(options).then((request) => request(this.axios, this.basePath))
   }

    /**
     * 
     * @summary 更新
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async Put<Tout>(options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResult<Tout>>>> {
        const newOptions = {method: 'PUT', ...options}
        const localVarAxiosArgs = await ApiAxiosParamCreator(this.configuration).BuildParam(newOptions);
        return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
            const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
            return axios.request(axiosRequestArgs);
        };
    }
    /**
     * 
     * @summary 更新
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async PutAdminResult<Tout>(options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResult<Tout>>> {
        return this.Put<Tout>(options).then((request) => request(this.axios, this.basePath))
    }

    /**
     * 
     * @summary 获取
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async Get<Tout>(options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<Tout>>> {
        const newOptions = {method: 'GET', ...options}
        const localVarAxiosArgs = await ApiAxiosParamCreator(this.configuration).BuildParam(newOptions);
        return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
            const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
            return axios.request(axiosRequestArgs);
        };
    }
    /**
     * 
     * @summary 获取
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async GetAdminResult<Tout>(options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResult<Tout>>> {
        return this.Get<AdminResult<Tout>>(options).then((request) => request(this.axios, this.basePath))
    }
    /**
     * 
     * @summary 更新
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async Post<Tout>(options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<Tout>>> {
        const newOptions = {method: 'POST', ...options}
        const localVarAxiosArgs = await ApiAxiosParamCreator(this.configuration).BuildParam(newOptions);
        return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
            const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
            return axios.request(axiosRequestArgs);
        };
    }

    /**
     * 
     * @summary 更新
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async PostVoid(options?: AxiosRequestConfig): Promise<AxiosResponse<void>> {
        return this.Post<void>(options).then((request) => request(this.axios, this.basePath))
    }

    /**
     * 
     * @summary 更新
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     */
    async PostAdminResult<Tout>(options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResult<Tout>>> {
        return this.Post<AdminResult<Tout>>(options).then((request) => request(this.axios, this.basePath))
    }
}

/**
 *
 * @export
 * @class RequiredError
 * @extends {Error}
 */
export class RequiredError extends Error {
    name: "RequiredError" = "RequiredError";
    constructor(public field: string, msg?: string) {
        super(msg);
    }
}


export const ApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 构建参数
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        BuildParam: async (options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(options.api || '', 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {...options.params} as any;
            options.params = null
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            // authentication Bearer required

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                if(localVarQueryParameter[key] != undefined) {
                    query.set(key, localVarQueryParameter[key]);
                }
            }
            
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            let needsSerialization;
            if(options.data instanceof FormData) {
                needsSerialization = false;
            }else {
                needsSerialization = (typeof options.data !== "string") || (localVarRequestOptions.headers != undefined && localVarRequestOptions.headers['Content-Type'] === 'application/json');
            }
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(options.data  !== undefined ? options.data  : {}) : (options.data  || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                // url: localVarUrlObj.pathname + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        } 
    }
}

