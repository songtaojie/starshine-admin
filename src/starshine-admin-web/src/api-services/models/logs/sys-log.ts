import { BasePageInput } from '../base/base-page-input';
/**
 *
 * @export
 * @interface PageLogInput
 */
export interface PageLogInput extends BasePageInput {
	/**
	 * 开始时间
	 * @type {Date}
	 * @memberof PageLogInput
	 */
	startTime?: Date | null;
	/**
	 * 结束时间
	 * @type {Date}
	 * @memberof PageLogInput
	 */
	endTime?: Date | null;
}
