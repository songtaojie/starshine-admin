import { AxiosResponse, AxiosRequestConfig } from 'axios';
// Some imports not used depending on template conditions
// @ts-ignore
import {  BaseAPI } from '../base';
import { AddNoticeInput,AdminResult,AdminResultPagedListResult,SysNotice,SysNoticeUser } from '../models';
import { DeleteNoticeInput } from '../models';
import { NoticeInput } from '../models';
import { PageNoticeInput } from '../models';
import { UpdateNoticeInput } from '../models';

/**
 * SysNoticeApi - object-oriented interface
 * @export
 * @class SysNoticeApi
 * @extends {BaseAPI}
 */
export class SysNoticeApi extends BaseAPI {
    /**
     * 
     * @summary 增加通知公告
     * @param {AddNoticeInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysNoticeApi
     */
    public async addSysNotice(body?: AddNoticeInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysNotice/add`;
        return this.PostVoid({api,data:body,...options});
    }
    /**
     * 
     * @summary 删除通知公告
     * @param {DeleteNoticeInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysNoticeApi
     */
    public async deleteSysNotice(body?: DeleteNoticeInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysNotice/delete`;
        return this.DeleteVoid({api,...options});
    }
    /**
     * 
     * @summary 获取通知公告分页列表
     * @param {PageNoticeInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysNoticeApi
     */
    public async getSysNoticePage(body?: PageNoticeInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultPagedListResult<SysNotice>>> {
        const api = `/api/sysNotice/page`;
        return this.PageAdminResult<SysNotice>({api,params:body,...options});
    }
    /**
     * 
     * @summary 获取接收的通知公告
     * @param {string} [title] 标题
     * @param {NoticeTypeEnum} [type] 类型（1通知 2公告）
     * @param {number} [page] 当前页码
     * @param {number} [pageSize] 页码容量
     * @param {string} [field] 排序字段
     * @param {string} [order] 排序方向
     * @param {string} [descStr] 降序排序
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysNoticeApi
     */
    public async getReceivedPage(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultPagedListResult<SysNoticeUser>>> {
        const api = `/api/sysNotice/pageReceived`;
        return this.PageAdminResult<SysNoticeUser>({api,...options});
    }
    /**
     * 
     * @summary 发布通知公告
     * @param {NoticeInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysNoticeApi
     */
    public async publicSysNotice(body?: NoticeInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysNotice/public`;
        return this.PostVoid({api,data:body,...options});
    }
    /**
     * 
     * @summary 设置通知公告已读状态
     * @param {NoticeInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysNoticeApi
     */
    public async setSysNoticeRead(body?: NoticeInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysNotice/setRead`;
        return this.PostVoid({api,data:body,...options});
    }
    /**
     * 
     * @summary 获取未读的通知公告
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysNoticeApi
     */
    public async getUnReadList(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<SysNotice>>>> {
        const api = `/api/sysNotice/unReadList`;
        return this.GetAdminResult<Array<SysNotice>>({api,...options});
    }
    /**
     * 
     * @summary 更新通知公告
     * @param {UpdateNoticeInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysNoticeApi
     */
    public async updateSysNotice(body?: UpdateNoticeInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysNotice/update`;
        return this.PostVoid({api,data:body,...options});
    }
}
