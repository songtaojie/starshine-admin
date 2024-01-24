import { AxiosResponse} from 'axios';
import { BaseAPI, RequiredError } from '../base';
import { AddDictDataInput,AdminResult,AdminResultPagedListResult,SysDictData,BaseIdInput } from '../models';
import { DictDataInput } from '../models';
import { PageDictDataInput } from '../models';
import { StatusEnum } from '../models';
import { UpdateDictDataInput } from '../models';

/**
 * SysDictDataApi - object-oriented interface
 * @export
 * @class SysDictDataApi
 * @extends {BaseAPI}
 */
export class SysDictDataApi extends BaseAPI {
    /**
     * 
     * @summary 增加字典值
     * @param {AddDictDataInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictDataApi
     */
    public async addSysDictData(data?: AddDictDataInput) : Promise<AxiosResponse<AdminResult<number>>> {
        const api = `/api/sys-dict/adddictdata`;
        return this.PostAdminResult<number>({api,data});
    }
    /**
     * 
     * @summary 根据字典类型编码获取字典值集合
     * @param {string} code 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictDataApi
     */
    public async getSysDictDataListByCode(code: string) : Promise<AxiosResponse<AdminResult<Array<SysDictData>>>> {
        const api = `/api/sys-dict/dataList/${encodeURIComponent(code)}`;
        return this.GetAdminResult<Array<SysDictData>>({api});
    }
    /**
     * 
     * @summary 根据查询条件获取字典值集合
     * @param {string} code 编码
     * @param {number} [status] 状态
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictDataApi
     */
    public async getSysDictDataList(code: string, status?: StatusEnum ) : Promise<AxiosResponse<AdminResult<Array<SysDictData>>>> {
        if (code === null || code === undefined) {
            throw new RequiredError('code','Required parameter code was null or undefined when calling getSysDictDataList.');
        }
        const api = `/api/sys-dict/dataList`;
        const params = {code,status};
        return this.GetAdminResult<Array<SysDictData>>({api,params});
    }
    /**
     * 
     * @summary 删除字典值
     * @param {BaseIdInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictDataApi
     */
    public async deleteSysDictData(data?: BaseIdInput) : Promise<AxiosResponse<AdminResult<boolean>>> {
        const api = `/api/sys-dict/deletedictdata`;
        return this.DeleteAdminResult<boolean>({api,data});
    }
  
    /**
     * 
     * @summary 获取字典值分页列表
     * @param {PageDictDataInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictDataApi
     */
    public async getSysDictDataPage(params?: PageDictDataInput) : Promise<AxiosResponse<AdminResultPagedListResult<SysDictData>>> {
        const api = `/api/sys-dict/getdictdatapage`;
        return this.PageAdminResult<SysDictData>({api,params});
    }
    /**
     * 
     * @summary 修改字典值状态
     * @param {DictDataInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictDataApi
     */
    public async setSysDictDataStatus(data?: DictDataInput) : Promise<AxiosResponse<AdminResult<boolean>>> {
        const api = `/api/sys-dict/setdictdatastatus`;
        return this.PostAdminResult<boolean>({api,data});
    }
    /**
     * 
     * @summary 更新字典值
     * @param {UpdateDictDataInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictDataApi
     */
    public async updateSysDictData(data?: UpdateDictDataInput) : Promise<AxiosResponse<AdminResult<boolean>>> {
        const api = `/api/sys-dict/updatedictdata`;
        return this.PostAdminResult<boolean>({api,data});
    }
}
