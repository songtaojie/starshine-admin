export interface AdminResultPagedListResult<T> {
    /**
     * 状态码
     * @type {number}
     * @memberof AdminResultPagedListResult
     */
    code?: number;
    /**
     * 类型success、warning、error
     * @type {string}
     * @memberof AdminResultPagedListResult
     */
    type?: string | null;
    /**
     * 错误信息
     * @type {string}
     * @memberof AdminResultPagedListResult
     */
    message?: string | null;
    /**
     * 
     * @type {PagedListResult<T>}
     * @memberof AdminResultPagedListResult
     */
    data?: PagedListResult<T>;
    /**
     * 附加数据
     * @type {any}
     * @memberof AdminResultPagedListResult
     */
    extras?: any | null;
    /**
     * 时间
     * @type {Number}
     * @memberof AdminResultPagedListResult
     */
    time?: Number;
}

/**
 * 分页泛型集合
 * @export
 * @interface PagedListResult
 */
export interface PagedListResult<T> {
    /**
     * 页码
     * @type {number}
     * @memberof PagedListResult
     */
    page?: number;
    /**
     * 页容量
     * @type {number}
     * @memberof PagedListResult
     */
    pageSize?: number;
    /**
     * 总条数
     * @type {number}
     * @memberof PagedListResult
     */
    total?: number;
    /**
     * 总页数
     * @type {number}
     * @memberof PagedListResult
     */
    totalPages?: number;
    /**
     * 当前页集合
     * @type {Array<T>}
     * @memberof PagedListResult
     */
    items?: Array<T> | null;
    /**
     * 是否有上一页
     * @type {boolean}
     * @memberof PagedListResult
     */
    hasPrevPage?: boolean;
    /**
     * 是否有下一页
     * @type {boolean}
     * @memberof PagedListResult
     */
    hasNextPage?: boolean;
}
