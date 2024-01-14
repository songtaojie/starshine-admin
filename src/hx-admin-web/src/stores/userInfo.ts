import { defineStore } from 'pinia';
import { Local, Session } from '/@/utils/storage';
import Watermark from '/@/utils/watermark';
import { useThemeConfig } from '/@/stores/themeConfig';
import { useSystemConfig } from '/@/stores/systemConfig';


import { getAPI } from '/@/utils/axios-utils';
import { SysAuthApi, SysConstApi } from '/@/api-services/api';

/**
 * 用户信息
 * @methods setUserInfos 设置用户信息
 */
export const useUserInfo = defineStore('userInfo', {
	state: (): UserInfosState => ({
		userInfos: {} as any,
		constList: [] as any,
	}),
	getters: {
		// 获取系统常量列表
		async getSysConstList(): Promise<any[]> {
			var res = await getAPI(SysConstApi).apiSysConstListGet();
			this.constList = res.data.result ?? [];
			return this.constList;
		},
	},
	actions: {
		async setUserInfos() {
			// 缓存用户信息
			if (Session.get('userInfo')) {
				this.userInfos = Session.get('userInfo');
			} else {
				const userInfos: any = await this.getApiUserInfo();
				this.userInfos = userInfos;
			}
		},
		// 获取当前用户信息
		getApiUserInfo() {
			return new Promise((resolve) => {
				getAPI(SysAuthApi)
					.getUserInfo()
					.then(async (res: any) => {
						if (res.data.data == null) return;
						var data = res.data.data;
						const userInfos = {
							account: data.account,
							realName: data.realName,
							avatar: data.avatar ? data.avatar : '/favicon.ico',
							address: data.address,
							signature: data.signature,
							orgId: data.orgId,
							orgName: data.orgName,
							posName: data.posName,
							roles: [],
							authBtnList: data.buttons,
							time: new Date().getTime(),
						};
						Session.set('userInfo', userInfos);

						// 读取用户配置
						const storesThemeConfig = useThemeConfig();
						const storesSystemConfig = useSystemConfig();

						// 是否设置水印
						storesThemeConfig.themeConfig.isWatermark = storesSystemConfig.sysConfig.watermarkEnabled;
						if (storesThemeConfig.themeConfig.isWatermark) Watermark.set(storesThemeConfig.themeConfig.watermarkText);
						else Watermark.del();

						Local.remove('themeConfig');
						Local.set('themeConfig', storesThemeConfig.themeConfig);
						
						resolve(userInfos);
					});
			});
		},
	},
});
