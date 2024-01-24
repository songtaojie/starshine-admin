import { AxiosResponse, AxiosRequestConfig } from 'axios';

import { BaseAPI } from '../base';
import { BaseIdInput,AdminResult,AdminResultPagedListResult,IActionResult } from '../models';
import { SysFile,FileOutput } from '../models';
import { PageFileInput } from '../models';

/**
 * SysFileApi - object-oriented interface
 * @export
 * @class SysFileApi
 * @extends {BaseAPI}
 */
export class SysFileApi extends BaseAPI {
    /**
     * 
     * @summary 删除文件
     * @param {DeleteFileInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysFileApi
     */
    public async deleteFile(data?: BaseIdInput) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-file/delete`;
        return this.DeleteVoid({api,data});
    }
    /**
     * 
     * @summary 下载文件(文件流)
     * @param {FileInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysFileApi
     */
    public async downloadFile(id: number) : Promise<AxiosResponse<AdminResult<IActionResult>>> {
        const api = `/api/sys-file/getdownloadfile/${id}`;
        return this.GetAdminResult<IActionResult>({api});
    }
    /**
     * 
     * @summary 获取文件分页列表
     * @param {PageFileInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysFileApi
     */
    public async getFilePage(params?: PageFileInput) : Promise<AxiosResponse<AdminResultPagedListResult<SysFile>>> {
        const api = `/api/sys-file/getpage`;
        return this.PageAdminResult({api,params});
    }
    /**
     * 
     * @summary 上传头像
     * @param {Blob} [file] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysFileApi
     */
    public async uploadAvatar(file?: Blob, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<FileOutput>>> {
        const api = `/api/sys-file/uploadavatar`;
        return this.PostAdminResult<FileOutput>({api,data:{file},...options});
    }
    /**
     * 
     * @summary 上传文件
     * @param {Blob} [file] 
     * @param {string} [path] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysFileApi
     */
    public async uploadSysFile(file?: Blob, path?: string) : Promise<AxiosResponse<AdminResult<FileOutput>>> {
        const headers = {'Content-Type':'multipart/form-data'};
        const api = `/api/sys-file/uploadfile`;
        const data = new FormData();
        if (file !== undefined) { 
            data.append('file', file as any);
        }
        return this.PostAdminResult<FileOutput>({api,data,params:{path},headers});
    }
    /**
     * 
     * @summary 上传多文件
     * @param {Array<Blob>} [files] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysFileApi
     */
    public async uploadSysFiles(files?: Array<Blob>, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<FileOutput>>>> {
        const api = `/api/sys-file/uploadfiles`;
        return this.PostAdminResult<Array<FileOutput>>({api,data:{files},...options});
    }
    /**
     * 
     * @summary 上传电子签名
     * @param {Blob} [file] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysFileApi
     */
    public async uploadSignature(file?: Blob, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<FileOutput>>> {
        const api = `/api/sys-file/uploadsignature`;
        return this.PostAdminResult({api,data:{file},...options});
    }
}
