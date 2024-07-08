import { AxiosResponse } from 'axios';
import { BaseAPI } from '../base';
import { AdminResult } from '../models';

/**
 * SysServerApi - object-oriented interface
 * @export
 * @class SysServerApi
 * @extends {BaseAPI}
 */
export class SysServerApi extends BaseAPI {
    /**
     * 
     * @summary 获取框架主要程序集
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysServerApi
     */
    public async getServerAssemblyList() : Promise<AxiosResponse<AdminResult<any>>> {
        const api = `/api/sys-server/getassemblylist`;
        return this.GetAdminResult<any>({api,});
    }
    /**
     * 
     * @summary 获取服务器配置信息
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysServerApi
     */
    public async getServerBase() : Promise<AxiosResponse<AdminResult<any>>> {
        const api = `/api/sys-server/getserverbase`;
        return this.GetAdminResult<any>({api});
    }
    /**
     * 
     * @summary 获取服务器磁盘信息
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysServerApi
     */
    public async getServerDisk() : Promise<AxiosResponse<AdminResult<any>>> {
        const api = `/api/sys-server/getserverdisk`;
        return this.GetAdminResult<any>({api});
    }
    /**
     * 
     * @summary 获取服务器使用信息
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysServerApi
     */
    public async getServerUsed() : Promise<AxiosResponse<AdminResult<any>>> {
        const api = `/api/sys-server/getserverused`;
        return this.GetAdminResult<any>({api});
    }
}
