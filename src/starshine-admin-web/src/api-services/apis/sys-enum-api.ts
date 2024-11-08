/* tslint:disable */
/* eslint-disable */

import globalAxios, { AxiosResponse, AxiosInstance, AxiosRequestConfig } from 'axios';
import { Configuration } from '../configuration';
// Some imports not used depending on template conditions
// @ts-ignore
import { BASE_PATH, COLLECTION_FORMATS, RequestArgs, BaseAPI, RequiredError } from '../base';
import { AdminResultListEnumEntity } from '../models';
import { AdminResultListEnumTypeOutput } from '../models';
/**
 * SysEnumApi - axios parameter creator
 * @export
 */
export const SysEnumApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 通过实体的字段名获取相关枚举值集合（目前仅支持枚举类型）
         * @param {string} entityName 实体名称
         * @param {string} fieldName 字段名称
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiSysEnumEnumDataListByFieldGet: async (entityName: string, fieldName: string, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            // verify required parameter 'entityName' is not null or undefined
            if (entityName === null || entityName === undefined) {
                throw new RequiredError('entityName','Required parameter entityName was null or undefined when calling apiSysEnumEnumDataListByFieldGet.');
            }
            // verify required parameter 'fieldName' is not null or undefined
            if (fieldName === null || fieldName === undefined) {
                throw new RequiredError('fieldName','Required parameter fieldName was null or undefined when calling apiSysEnumEnumDataListByFieldGet.');
            }
            const localVarPath = `/api/sysEnum/enumDataListByField`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required

            if (entityName !== undefined) {
                localVarQueryParameter['EntityName'] = entityName;
            }

            if (fieldName !== undefined) {
                localVarQueryParameter['FieldName'] = fieldName;
            }

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

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 通过枚举类型获取枚举值集合
         * @param {string} enumName 枚举类型名称
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiSysEnumEnumDataListGet: async (enumName: string, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            // verify required parameter 'enumName' is not null or undefined
            if (enumName === null || enumName === undefined) {
                throw new RequiredError('enumName','Required parameter enumName was null or undefined when calling apiSysEnumEnumDataListGet.');
            }
            const localVarPath = `/api/sysEnum/enumDataList`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required

            if (enumName !== undefined) {
                localVarQueryParameter['EnumName'] = enumName;
            }

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

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 获取所有枚举类型
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiSysEnumEnumTypeListGet: async (options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/sysEnum/enumTypeList`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required

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

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
    }
};

/**
 * SysEnumApi - functional programming interface
 * @export
 */
export const SysEnumApiFp = function(configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 通过实体的字段名获取相关枚举值集合（目前仅支持枚举类型）
         * @param {string} entityName 实体名称
         * @param {string} fieldName 字段名称
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysEnumEnumDataListByFieldGet(entityName: string, fieldName: string, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultListEnumEntity>>> {
            const localVarAxiosArgs = await SysEnumApiAxiosParamCreator(configuration).apiSysEnumEnumDataListByFieldGet(entityName, fieldName, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @summary 通过枚举类型获取枚举值集合
         * @param {string} enumName 枚举类型名称
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysEnumEnumDataListGet(enumName: string, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultListEnumEntity>>> {
            const localVarAxiosArgs = await SysEnumApiAxiosParamCreator(configuration).apiSysEnumEnumDataListGet(enumName, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @summary 获取所有枚举类型
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysEnumEnumTypeListGet(options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultListEnumTypeOutput>>> {
            const localVarAxiosArgs = await SysEnumApiAxiosParamCreator(configuration).apiSysEnumEnumTypeListGet(options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
    }
};

/**
 * SysEnumApi - factory interface
 * @export
 */
export const SysEnumApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    return {
        /**
         * 
         * @summary 通过实体的字段名获取相关枚举值集合（目前仅支持枚举类型）
         * @param {string} entityName 实体名称
         * @param {string} fieldName 字段名称
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysEnumEnumDataListByFieldGet(entityName: string, fieldName: string, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultListEnumEntity>> {
            return SysEnumApiFp(configuration).apiSysEnumEnumDataListByFieldGet(entityName, fieldName, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 通过枚举类型获取枚举值集合
         * @param {string} enumName 枚举类型名称
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysEnumEnumDataListGet(enumName: string, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultListEnumEntity>> {
            return SysEnumApiFp(configuration).apiSysEnumEnumDataListGet(enumName, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 获取所有枚举类型
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiSysEnumEnumTypeListGet(options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultListEnumTypeOutput>> {
            return SysEnumApiFp(configuration).apiSysEnumEnumTypeListGet(options).then((request) => request(axios, basePath));
        },
    };
};

/**
 * SysEnumApi - object-oriented interface
 * @export
 * @class SysEnumApi
 * @extends {BaseAPI}
 */
export class SysEnumApi extends BaseAPI {
    /**
     * 
     * @summary 通过实体的字段名获取相关枚举值集合（目前仅支持枚举类型）
     * @param {string} entityName 实体名称
     * @param {string} fieldName 字段名称
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysEnumApi
     */
    public async apiSysEnumEnumDataListByFieldGet(entityName: string, fieldName: string, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultListEnumEntity>> {
        return SysEnumApiFp(this.configuration).apiSysEnumEnumDataListByFieldGet(entityName, fieldName, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 通过枚举类型获取枚举值集合
     * @param {string} enumName 枚举类型名称
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysEnumApi
     */
    public async apiSysEnumEnumDataListGet(enumName: string, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultListEnumEntity>> {
        return SysEnumApiFp(this.configuration).apiSysEnumEnumDataListGet(enumName, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 获取所有枚举类型
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysEnumApi
     */
    public async apiSysEnumEnumTypeListGet(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultListEnumTypeOutput>> {
        return SysEnumApiFp(this.configuration).apiSysEnumEnumTypeListGet(options).then((request) => request(this.axios, this.basePath));
    }
}
