/* eslint-disable */
import * as axios from 'axios';
// 扩展 axios 数据返回类型，可自行扩展
declare module 'axios' {
	// export interface AxiosResponse<T = any> {
	// 	status: number;
	// 	statusText:string;
	// 	data: T;
	// 	message: string;
	// 	type?: string;
	// 	[key: string]: T;
	// }
	export interface AxiosRequestConfig<T = any> {
		api?: string;
	}
}
