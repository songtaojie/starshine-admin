import { AxiosResponse } from 'axios';
import {  BaseAPI } from '../base';
import { AddRegionInput,AdminResult,AdminResultPagedListResult,SysRegion } from '../models';
import { PageRegionInput,BaseIdInput } from '../models';
import { UpdateRegionInput } from '../models';

/**
 * SysRegionApi - object-oriented interface
 * @export
 * @class SysRegionApi
 * @extends {BaseAPI}
 */
export class SysRegionApi extends BaseAPI {
    /**
     * 
     * @summary 增加行政区域
     * @param {AddRegionInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRegionApi
     */
    public async addRegion(data?: AddRegionInput) : Promise<AxiosResponse<AdminResult<number>>> {
        const api = `/api/sys-region/add`;
        return this.PostAdminResult<number>({api,data});
    }
    /**
     * 
     * @summary 删除行政区域
     * @param {DeleteRegionInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRegionApi
     */
    public async deleteRegion(data?: BaseIdInput) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-region/delete`;
        return this.DeleteVoid({api,data});
    }
    /**
     * 
     * @summary 获取行政区域列表
     * @param {number} id 主键Id
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRegionApi
     */
    public async getRegionList(id: number) : Promise<AxiosResponse<AdminResult<Array<SysRegion>>>> {
        const api = `/api/sys-region/getlist`;
        return this.GetAdminResult<Array<SysRegion>>({api,params:{id}});
    }
    /**
     * 
     * @summary 获取行政区域分页列表
     * @param {PageRegionInput} [params] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRegionApi
     */
    public async getRegionPage(params?: PageRegionInput) : Promise<AxiosResponse<AdminResultPagedListResult<SysRegion>>> {
        const api = `/api/sys-region/getpage`;
        return this.PageAdminResult<SysRegion>({api,params});
    }
    /**
     * 
     * @summary 同步行政区域
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRegionApi
     */
    public async syncRegion() : Promise<AxiosResponse<void>> {
        const api = `/api/sys-region/sync`;
        return this.PostVoid({api});
    }
    /**
     * 
     * @summary 更新行政区域
     * @param {UpdateRegionInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRegionApi
     */
    public async updateRegion(data?: UpdateRegionInput) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-region/update`;
        return this.PostVoid({api,data});
    }
}
