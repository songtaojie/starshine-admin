import { NoticeStatusEnum } from './notice-status-enum';
import { NoticeTypeEnum } from './notice-type-enum';
import { BasePageInput } from './base/base-page-input';

/**
 * 
 * @export
 * @interface AddNoticeInput
 */
export interface AddNoticeInput {
    /**
     * 标题
     * @type {string}
     * @memberof AddNoticeInput
     */
    title: string;
    /**
     * 内容
     * @type {string}
     * @memberof AddNoticeInput
     */
    content: string;
    /**
     * 
     * @type {NoticeTypeEnum}
     * @memberof AddNoticeInput
     */
    type?: NoticeTypeEnum;
    /**
     * 
     * @type {NoticeStatusEnum}
     * @memberof AddNoticeInput
     */
    status?: NoticeStatusEnum;
}

/**
 * 
 * @export
 * @interface UpdateNoticeInput
 */
export interface UpdateNoticeInput extends AddNoticeInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateNoticeInput
     */
    id: number;
}


/**
 * 
 * @export
 * @interface PageNoticeInput
 */
export interface PageNoticeInput extends BasePageInput {
    /**
     * 标题
     * @type {string}
     * @memberof PageNoticeInput
     */
    title?: string | null;
    /**
     * 
     * @type {NoticeTypeEnum}
     * @memberof PageNoticeInput
     */
    type?: NoticeTypeEnum;
}

/**
 * 系统通知公告表
 * @export
 * @interface SysNotice
 */
export interface SysNotice {
    /**
     * 雪花Id
     * @type {number}
     * @memberof SysNotice
     */
    id?: number;
    /**
     * 创建时间
     * @type {Date}
     * @memberof SysNotice
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysNotice
     */
    updateTime?: Date | null;
    /**
     * 创建者Id
     * @type {number}
     * @memberof SysNotice
     */
    createUserId?: number | null;
    /**
     * 修改者Id
     * @type {number}
     * @memberof SysNotice
     */
    updateUserId?: number | null;
    /**
     * 软删除
     * @type {boolean}
     * @memberof SysNotice
     */
    isDelete?: boolean;
    /**
     * 标题
     * @type {string}
     * @memberof SysNotice
     */
    title: string;
    /**
     * 内容
     * @type {string}
     * @memberof SysNotice
     */
    content: string;
    /**
     * 
     * @type {NoticeTypeEnum}
     * @memberof SysNotice
     */
    type?: NoticeTypeEnum;
    /**
     * 发布人Id
     * @type {number}
     * @memberof SysNotice
     */
    publicUserId?: number;
    /**
     * 发布人姓名
     * @type {string}
     * @memberof SysNotice
     */
    publicUserName?: string | null;
    /**
     * 发布机构Id
     * @type {number}
     * @memberof SysNotice
     */
    publicOrgId?: number;
    /**
     * 发布机构名称
     * @type {string}
     * @memberof SysNotice
     */
    publicOrgName?: string | null;
    /**
     * 发布时间
     * @type {Date}
     * @memberof SysNotice
     */
    publicTime?: Date | null;
    /**
     * 撤回时间
     * @type {Date}
     * @memberof SysNotice
     */
    cancelTime?: Date | null;
    /**
     * 
     * @type {NoticeStatusEnum}
     * @memberof SysNotice
     */
    status?: NoticeStatusEnum;
}
