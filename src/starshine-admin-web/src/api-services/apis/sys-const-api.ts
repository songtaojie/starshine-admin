import { AxiosResponse, AxiosRequestConfig } from 'axios';
// Some imports not used depending on template conditions
// @ts-ignore
import { BaseAPI } from '../base';
import { AdminResult,ConstOutput } from '../models';

/**
 * SysConstApi - object-oriented interface
 * @export
 * @class SysConstApi
 * @extends {BaseAPI}
 */
export class SysConstApi extends BaseAPI {
    /**
     * 
     * @summary 根据类名获取常量数据
     * @param {string} typeName 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysConstApi
     */
    public async apiSysConstDataTypeNameGet(typeName: string, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<ConstOutput>>>> {
        const api = `/api/sysConst/data/${encodeURIComponent(String(typeName))}`;
        return this.GetAdminResult<Array<ConstOutput>>({api,...options});
    }
    /**
     * 
     * @summary 获取所有常量列表
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysConstApi
     */
    public async apiSysConstListGet(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<ConstOutput>>>> {
        const api = `/api/sysConst/list`;
        return this.GetAdminResult<Array<ConstOutput>>({api,...options});
    }
}
