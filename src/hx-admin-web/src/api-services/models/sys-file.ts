
import { BasePageInput } from './base/base-page-input'

/**
 * 
 * @export
 * @interface PageFileInput
 */
export interface PageFileInput extends BasePageInput{
    /**
     * 文件名称
     * @type {string}
     * @memberof PageFileInput
     */
    fileName?: string | null;
    /**
     * 开始时间
     * @type {Date}
     * @memberof PageFileInput
     */
    startTime?: Date | null;
    /**
     * 结束时间
     * @type {Date}
     * @memberof PageFileInput
     */
    endTime?: Date | null;
}

/**
 * 
 * @export
 * @interface FileOutput
 */
export interface FileOutput {
    /**
     * Id
     * @type {number}
     * @memberof FileOutput
     */
    id?: number;
    /**
     * 提供者
     * @type {string}
     * @memberof FileOutput
     */
    provider?: string | null;
    /**
     * 名称
     * @type {string}
     * @memberof FileOutput
     */
    name?: string | null;
    /**
     * URL
     * @type {string}
     * @memberof FileOutput
     */
    url?: string | null;
    /**
     * 大小
     * @type {string}
     * @memberof FileOutput
     */
    sizeKb?: string | null;
    /**
     * 后缀
     * @type {string}
     * @memberof FileOutput
     */
    suffix?: string | null;
    /**
     * 路径
     * @type {string}
     * @memberof FileOutput
     */
    filePath?: string | null;
    /**
     * 文件名称
     * @type {string}
     * @memberof FileOutput
     */
    fileName?: string | null;
}

/**
 * 系统文件表
 * @export
 * @interface SysFile
 */
export interface SysFile {
    /**
     * 雪花Id
     * @type {number}
     * @memberof SysFile
     */
    id?: number;
    /**
     * 创建时间
     * @type {Date}
     * @memberof SysFile
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysFile
     */
    updateTime?: Date | null;
    /**
     * 创建者Id
     * @type {number}
     * @memberof SysFile
     */
    createUserId?: number | null;
    /**
     * 修改者Id
     * @type {number}
     * @memberof SysFile
     */
    updateUserId?: number | null;
    /**
     * 软删除
     * @type {boolean}
     * @memberof SysFile
     */
    isDelete?: boolean;
    /**
     * 提供者
     * @type {string}
     * @memberof SysFile
     */
    provider?: string | null;
    /**
     * 仓储名称
     * @type {string}
     * @memberof SysFile
     */
    bucketName?: string | null;
    /**
     * 文件名称（上传时名称）
     * @type {string}
     * @memberof SysFile
     */
    fileName?: string | null;
    /**
     * 文件后缀
     * @type {string}
     * @memberof SysFile
     */
    suffix?: string | null;
    /**
     * 存储路径
     * @type {string}
     * @memberof SysFile
     */
    filePath?: string | null;
    /**
     * 文件大小KB
     * @type {string}
     * @memberof SysFile
     */
    sizeKb?: string | null;
    /**
     * 文件大小信息-计算后的
     * @type {string}
     * @memberof SysFile
     */
    sizeInfo?: string | null;
    /**
     * 外链地址-OSS上传后生成外链地址方便前端预览
     * @type {string}
     * @memberof SysFile
     */
    url?: string | null;
}
