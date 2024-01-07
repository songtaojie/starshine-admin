/**
 * 
 * @export
 * @interface BasePageInput
 */
export interface BasePageInput {
    /**
     * 当前页码
     * @type {number}
     * @memberof PageNoticeInput
     */
    page?: number | 1;
    /**
     * 页码容量
     * @type {number}
     * @memberof PageNoticeInput
     */
    pageSize?: number | 10;
    /**
     * 排序字段
     * @type {string}
     * @memberof PageNoticeInput
     */
    field?: string | null;
    /**
     * 排序方向
     * @type {string}
     * @memberof PageNoticeInput
     */
    order?: string | null;
    /**
     * 降序排序
     * @type {string}
     * @memberof PageNoticeInput
     */
    descStr?: string | 'descend';
}
