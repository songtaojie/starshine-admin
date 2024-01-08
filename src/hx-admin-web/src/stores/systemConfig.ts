import { defineStore } from 'pinia';

/**
 * 系统配置
 */
export const useSystemConfig = defineStore('systemConfig', {
	state: (): SystemConfigState => ({
		sysConfig: {
			secondVerEnabled: false,
            captchaEnabled: false,
            watermarkEnabled: false
		},
	}),
    getters: {
        getSysConfig(state) {
            return state.sysConfig || {}
        }
    },
	actions: {
		setSysConfig(data: any) {
			this.sysConfig = data;
		},
	},
});