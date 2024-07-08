import { AxiosResponse} from 'axios';

import { BaseAPI, RequiredError } from '../base';
import { AdminResult} from '../models';

/**
 * SysCacheApi - object-oriented interface
 * @export
 * @class SysCacheApi
 * @extends {BaseAPI}
 */
export class SysCacheApi extends BaseAPI {
    /**
     * 
     * @summary 根据键名前缀删除缓存
     * @param {string} prefixKey 键名前缀
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysCacheApi
     */
    public async removeCacheByPrefixKey(prefixKey: string) : Promise<AxiosResponse<AdminResult<Number>>> {
        if (prefixKey === null || prefixKey === undefined) {
            throw new RequiredError('prefixKey','Required parameter prefixKey was null or undefined when calling removeCacheByPrefixKey.');
        }
        const api = `/api/sys-cache/removebyprefixkey`;
        return this.DeleteAdminResult<Number>({api,data:{prefixKey}});
    }
    /**
     * 
     * @summary 删除缓存
     * @param {string} key 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysCacheApi
     */
    public async removeCacheByKey(key: string) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-cache/remove`;
        return this.DeleteVoid({api,data:{key}});
    }
    /**
     * 
     * @summary 获取缓存键名集合
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysCacheApi
     */
    public async getSysCacheKeyList() : Promise<AxiosResponse<AdminResult<Array<string>>>> {
        const api = `/api/sys-cache/getkeylist`;
        return this.GetAdminResult<Array<string>>({api});
    }
    /**
     * 
     * @summary 获取缓存值
     * @param {string} key 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysCacheApi
     */
    public async getSysCacheValue(key: string) : Promise<AxiosResponse<AdminResult<any>>> {
        const api = `/api/sys-cache/getvalue/${encodeURIComponent(key)}`;
        return this.GetAdminResult<any>({api});
    }
}
