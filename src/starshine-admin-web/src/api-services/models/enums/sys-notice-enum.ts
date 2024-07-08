/**
 * 通知公告状类型枚举<br />&nbsp;通知 NOTICE = 1<br />&nbsp;公告 ANNOUNCEMENT = 2<br />
 * @export
 * @enum {string}
 */
export enum NoticeTypeEnum {
  NUMBER_1 = 1,
  NUMBER_2 = 2
}

/**
 * 通知公告状态枚举<br />&nbsp;草稿 DRAFT = 0<br />&nbsp;发布 PUBLIC = 1<br />&nbsp;撤回 CANCEL = 2<br />&nbsp;删除 DELETED = 3<br />
 * @export
 * @enum {string}
 */
export enum NoticeStatusEnum {
  NUMBER_0 = 0,
  NUMBER_1 = 1,
  NUMBER_2 = 2,
  NUMBER_3 = 3
}

/**
 * 通知公告用户状态枚举<br />&nbsp;未读 UNREAD = 0<br />&nbsp;已读 READ = 1<br />
 * @export
 * @enum {string}
 */
export enum NoticeUserStatusEnum {
  NUMBER_0 = 0,
  NUMBER_1 = 1
}