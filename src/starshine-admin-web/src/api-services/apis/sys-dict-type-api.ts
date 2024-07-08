import { AxiosResponse} from 'axios';
// Some imports not used depending on template conditions
// @ts-ignore
import { BaseAPI } from '../base';
import { AdminResult,BaseIdInput,SysDictType,AdminResultPagedListResult,SysDictData } from '../models';
import { AddDictTypeInput,UpdateDictTypeInput,DictTypeInput,PageDictTypeInput } from '../models';

/**
 * SysDictTypeApi - object-oriented interface
 * @export
 * @class SysDictTypeApi
 * @extends {BaseAPI}
 */
export class SysDictTypeApi extends BaseAPI {
    /**
     * 
     * @summary 获取字典类型分页列表
     * @param {PageDictTypeInput} [params] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictTypeApi
     */
    public async getSysDictTypePage(params?: PageDictTypeInput) : Promise<AxiosResponse<AdminResultPagedListResult<SysDictType>>> {
        const api = `/api/sys-dict/getdicttypepage`;
        return this.PageAdminResult<SysDictType>({api,params});
    }

    /**
     * 
     * @summary 添加字典类型
     * @param {AddDictTypeInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictTypeApi
     */
    public async addSysDictType(data?: AddDictTypeInput) : Promise<AxiosResponse<AdminResult<Number>>> {
        const api = `/api/sys-dict/adddicttype`;
        return this.PostAdminResult<Number>({api,data});
    }
      /**
     * 
     * @summary 更新字典类型
     * @param {UpdateDictTypeInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictTypeApi
     */
      public async updateSysDictType(data?: UpdateDictTypeInput) : Promise<AxiosResponse<AdminResult<boolean>>> {
        const api = `/api/sys-dict/updatedicttype`;
        return this.PostAdminResult<boolean>({api,data});
    }
     /**
     * 
     * @summary 删除字典类型
     * @param {BaseIdInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictTypeApi
     */
     public async deleteSysDictType(data?: BaseIdInput) : Promise<AxiosResponse<AdminResult<boolean>>> {
        const api = `/api/sys-dict/deletedicttype`;
        return this.DeleteAdminResult<boolean>({api,data});
    }

    /**
     * 
     * @summary 获取字典类型列表
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictTypeApi
     */
    public async getSysDictTypeList() : Promise<AxiosResponse<AdminResult<Array<SysDictType>>>> {
        const api = `/api/sys-dict/getdicttypelist`;
        return this.GetAdminResult<Array<SysDictType>>({api});
    }
    
    /**
     * 
     * @summary 修改字典类型状态
     * @param {DictTypeInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictTypeApi
     */
    public async setSysDictTypeStatus(data?: DictTypeInput) : Promise<AxiosResponse<AdminResult<boolean>>> {
        const api = `/api/sys-dict/setdicttypestatus`;
        return this.PostAdminResult<boolean>({api,data});
    }

    /**
     * 
     * @summary 获取字典类型-值列表
     * @param {string} code 编码
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysDictTypeApi
     */
    public async apiSysDictTypeDataListGet(code: string) : Promise<AxiosResponse<AdminResult<Array<SysDictData>>>> {
        const api = `/api/sys-dict/dataList`;
        return this.GetAdminResult<Array<SysDictData>>({api,params:{code}});
    }
  
}
