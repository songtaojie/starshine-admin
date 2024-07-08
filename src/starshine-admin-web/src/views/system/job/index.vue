<template>
	<div class="sys-job-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="state.queryParams" ref="queryForm" :inline="true">
				<el-form-item label="作业编号" prop="jobName">
					<el-input placeholder="作业编号" clearable @keyup.enter="handleQuery" v-model="state.queryParams.jobName" />
				</el-form-item>
				<el-form-item label="描述信息" prop="description">
					<el-input placeholder="描述信息" clearable @keyup.enter="handleQuery" v-model="state.queryParams.description" />
				</el-form-item>
				<el-form-item>
					<el-button-group>
						<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysJob:pageJobDetail'"> 查询 </el-button>
						<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					</el-button-group>
				</el-form-item>
				<el-form-item>
					<el-button-group style="margin: 0px 12px">
						<el-tooltip content="增加作业">
							<el-button icon="ele-CirclePlus" @click="openAddJobDetail" v-auth="'sysJob:addJobDetail'"> </el-button>
						</el-tooltip>
						<el-tooltip content="启动所有作业">
							<el-button icon="ele-VideoPlay" @click="startAllJob" />
						</el-tooltip>
						<el-tooltip content="暂停所有作业">
							<el-button icon="ele-VideoPause" @click="pauseAllJob" />
						</el-tooltip>
					</el-button-group>
					<el-button-group style="margin: 0px 12px 0px 0px">
						<el-tooltip content="强制唤醒作业调度器">
							<el-button icon="ele-AlarmClock" @click="cancelSleep" />
						</el-tooltip>
						<el-tooltip content="强制触发所有作业持久化">
							<el-button icon="ele-Connection" @click="persistAllJob" />
						</el-tooltip>
					</el-button-group>
					<el-button icon="ele-Coin" @click="openJobCluster" plain> 集群控制 </el-button>
					<el-button icon="ele-Grid" @click="openJobDashboard" plain> 任务看板 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card class="full-table" shadow="hover" style="margin-top: 8px">
			<el-table :data="state.jobData" style="width: 100%" v-loading="state.loading" border>
				<el-table-column type="expand" fixed>
					<template #default="scope">
						<el-table style="margin-left: 48px; width: calc(100% - 48px)" :data="(scope.row as JobOutput).jobTriggers" border size="small">
							<el-table-column type="index" label="序号" width="55" align="center" fixed />
							<el-table-column prop="triggerId" label="触发器编号" width="120" fixed show-overflow-tooltip />
							<el-table-column prop="triggerType" label="类型" show-overflow-tooltip />
							<!-- <el-table-column prop="assemblyName" label="程序集" show-overflow-tooltip /> -->
							<el-table-column prop="args" label="参数" show-overflow-tooltip />
							<el-table-column prop="description" label="描述" width="120" show-overflow-tooltip />
							<el-table-column prop="status" label="状态" width="100" align="center" show-overflow-tooltip>
								<template #default="scope">
									<el-tag type="warning" effect="plain" v-if="(scope.row as SysJobTrigger).status == 0"> 积压 </el-tag>
									<el-tag type="" effect="plain" v-if="(scope.row as SysJobTrigger).status == 1"> 就绪 </el-tag>
									<el-tag type="success" effect="plain" v-if="(scope.row as SysJobTrigger).status == 2"> 正在运行 </el-tag>
									<el-tag type="danger" effect="plain" v-if="(scope.row as SysJobTrigger).status == 3"> 暂停 </el-tag>
									<el-tag type="danger" effect="plain" v-if="(scope.row as SysJobTrigger).status == 4"> 阻塞 </el-tag>
									<el-tag type="" effect="plain" v-if="(scope.row as SysJobTrigger).status == 5"> 由失败进入就绪 </el-tag>
									<el-tag type="danger" effect="plain" v-if="(scope.row as SysJobTrigger).status == 6"> 归档 </el-tag>
									<el-tag type="danger" effect="plain" v-if="(scope.row as SysJobTrigger).status == 7"> 崩溃 </el-tag>
									<el-tag type="danger" effect="plain" v-if="(scope.row as SysJobTrigger).status == 8"> 超限 </el-tag>
									<el-tag type="danger" effect="plain" v-if="(scope.row as SysJobTrigger).status == 9"> 无触发时间 </el-tag>
									<el-tag type="danger" effect="plain" v-if="(scope.row as SysJobTrigger).status == 10"> 未启动 </el-tag>
									<el-tag type="danger" effect="plain" v-if="(scope.row as SysJobTrigger).status == 11"> 未知作业触发器 </el-tag>
									<el-tag type="danger" effect="plain" v-if="(scope.row as SysJobTrigger).status == 12"> 未知作业处理程序 </el-tag>
								</template>
							</el-table-column>
							<el-table-column prop="startTime" label="起始时间" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="endTime" label="结束时间" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="lastRunTime" label="最近运行时间" width="140" align="center" show-overflow-tooltip />
							<el-table-column prop="nextRunTime" label="下一次运行时间" width="140" align="center" show-overflow-tooltip />
							<el-table-column prop="numberOfRuns" label="触发次数" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="maxNumberOfRuns" label="最大触发次数" width="120" align="center" show-overflow-tooltip />
							<el-table-column prop="numberOfErrors" label="出错次数" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="maxNumberOfErrors" label="最大出错次数" width="120" align="center" show-overflow-tooltip />
							<el-table-column prop="numRetries" label="重试次数" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="retryTimeout" label="重试间隔ms" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="startNow" label="是否立即启动" width="100" align="center" show-overflow-tooltip>
								<template #default="scope">
									<el-tag v-if="(scope.row as SysJobTrigger).startNow == true"> 是 </el-tag>
									<el-tag type="info" v-else> 否 </el-tag>
								</template>
							</el-table-column>
							<el-table-column prop="runOnStart" label="是否启动时执行一次" width="150" align="center" show-overflow-tooltip>
								<template #default="scope">
									<el-tag v-if="(scope.row as SysJobTrigger).runOnStart == true"> 是 </el-tag>
									<el-tag type="info" v-else> 否 </el-tag>
								</template>
							</el-table-column>
							<el-table-column prop="resetOnlyOnce" label="是否重置触发次数" width="120" align="center" show-overflow-tooltip>
								<template #default="scope">
									<el-tag v-if="(scope.row as SysJobTrigger).resetOnlyOnce == true"> 是 </el-tag>
									<el-tag type="info" v-else> 否 </el-tag>
								</template>
							</el-table-column>
							<el-table-column prop="updatedTime" label="更新时间" width="140" align="center" show-overflow-tooltip />
							<el-table-column label="操作" width="140" align="center" show-overflow-tooltip fixed="right">
								<template #default="scope">
									<el-tooltip content="启动触发器">
										<el-button size="small" type="primary" icon="ele-VideoPlay" text @click="startTrigger(scope.row)" />
									</el-tooltip>
									<el-tooltip content="暂停触发器">
										<el-button size="small" type="primary" icon="ele-VideoPause" text @click="pauseTrigger(scope.row)" />
									</el-tooltip>
									<el-tooltip content="编辑触发器">
										<el-button size="small" type="primary" icon="ele-Edit" text @click="openEditJobTrigger(scope.row)"> </el-button>
									</el-tooltip>
									<el-tooltip content="删除触发器">
										<el-button size="small" type="danger" icon="ele-Delete" text @click="delJobTrigger(scope.row)"> </el-button>
									</el-tooltip>
								</template>
							</el-table-column>
						</el-table>
					</template>
				</el-table-column>
				<el-table-column type="index" label="序号" width="55" align="center" fixed />
				<el-table-column prop="jobName" label="作业编号" width="180" fixed>
					<template #default="scope">
						<div style="display: flex; align-items: center">
							<el-icon><timer /></el-icon>
							<span style="margin-left: 5px">{{ (scope.row as JobOutput).jobName }}</span>
						</div>
					</template>
				</el-table-column>
				<el-table-column prop="jobGroup" label="组名称" width="100" show-overflow-tooltip />
				<el-table-column prop="jobClassName" label="类型" show-overflow-tooltip />
				<el-table-column prop="description" label="描述" show-overflow-tooltip />
				<el-table-column prop="isNonConcurrent" label="执行方式" width="90" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="success" v-if="(scope.row as JobOutput).isNonConcurrent == true"> 串行 </el-tag>
						<el-tag type="warning" v-else> 并行 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="createType" label="作业创建类型" width="110" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="info" v-if="(scope.row as JobOutput).createType == JobCreateTypeEnum.NUMBER_0"> 内置 </el-tag>
						<el-tag type="warning" v-if="(scope.row as JobOutput).createType == JobCreateTypeEnum.NUMBER_1"> 脚本 </el-tag>
						<el-tag type="success" v-if="(scope.row as JobOutput).createType == JobCreateTypeEnum.NUMBER_2"> HTTP请求 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="jobData" label="额外数据" show-overflow-tooltip>
					<template #default="scope">
						<span v-if="(scope.row as JobOutput).createType != JobCreateTypeEnum.NUMBER_2"> {{ (scope.row as JobOutput).jobData }} </span>
						<div v-else style="text-align: center">
							<el-popover placement="left" :width="400" trigger="hover">
								<template #reference>
									<el-tag effect="plain" type="info"> 请求参数 </el-tag>
								</template>
								<el-descriptions title="Http 请求参数" :column="1" size="small" :border="true">
									<el-descriptions-item label="请求地址" label-align="right" label-class-name="job-index-descriptions-label-style">
										{{ getHttpJobMessage((scope.row as JobOutput).jobData).requestUri }}
									</el-descriptions-item>
									<el-descriptions-item label="请求方法" label-align="right" label-class-name="job-index-descriptions-label-style">
										{{ getHttpMethodDesc(getHttpJobMessage((scope.row as JobOutput).jobData).httpMethod) }}
									</el-descriptions-item>
									<el-descriptions-item label="请求报文体" label-align="right" label-class-name="job-index-descriptions-label-style">
										{{ getHttpJobMessage((scope.row as JobOutput).jobData).body }}
									</el-descriptions-item>
								</el-descriptions>
							</el-popover>
						</div>
					</template>
				</el-table-column>
				<el-table-column prop="updateTime_V" label="更新时间" width="160" align="center" show-overflow-tooltip />
				<el-table-column label="操作" width="170" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tooltip content="增加触发器">
							<el-button size="small" type="primary" icon="ele-CirclePlus" text @click="openAddJobTrigger(scope.row)"> </el-button>
						</el-tooltip>
						<el-tooltip content="启动作业">
							<el-button size="small" type="primary" icon="ele-VideoPlay" text @click="startJob(scope.row)" />
						</el-tooltip>
						<el-tooltip content="暂停作业">
							<el-button size="small" type="primary" icon="ele-VideoPause" text @click="pauseJob(scope.row)" />
						</el-tooltip>
						<el-tooltip content="编辑作业">
							<el-button size="small" type="primary" icon="ele-Edit" text @click="openEditJobDetail(scope.row)" v-auth="'sysJob:updateJobDetail'"> </el-button>
						</el-tooltip>
						<el-tooltip content="删除作业">
							<el-button size="small" type="danger" icon="ele-Delete" text @click="delJobDetail(scope.row)" v-auth="'sysJob:deleteJobDetail'"> </el-button>
						</el-tooltip>
					</template>
				</el-table-column>
			</el-table>
			<el-pagination
				v-model:currentPage="state.tableParams.page"
				v-model:page-size="state.tableParams.pageSize"
				:total="state.tableParams.total"
				:page-sizes="[10, 20, 50, 100]"
				small
				background
				@size-change="handleSizeChange"
				@current-change="handleCurrentChange"
				layout="total, sizes, prev, pager, next, jumper"
			/>
		</el-card>
		<EditJobDetail ref="editJobDetailRef" :title="state.editJobDetailTitle" />
		<EditJobTrigger ref="editJobTriggerRef" :title="state.editJobTriggerTitle" />
		<JobCluster ref="editJobClusterRef" />
	</div>
</template>

<script lang="ts" setup name="sysJob">
import { onMounted, onUnmounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import { useRouter } from 'vue-router';
import mittBus from '/@/utils/mitt';
import { Timer } from '@element-plus/icons-vue';
import EditJobDetail from '/@/views/system/job/component/editJobDetail.vue';
import EditJobTrigger from '/@/views/system/job/component/editJobTrigger.vue';
import JobCluster from '/@/views/system/job/component/jobCluster.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysJobApi } from '/@/api-services/api';
import { JobCreateTypeEnum, JobOutput, SysJobTrigger } from '/@/api-services/models';

const router = useRouter();
const editJobDetailRef = ref<InstanceType<typeof EditJobDetail>>();
const editJobTriggerRef = ref<InstanceType<typeof EditJobTrigger>>();
const editJobClusterRef = ref<InstanceType<typeof JobCluster>>();
const state = reactive({
	loading: false,
	jobData: [] as Array<JobOutput>,
	queryParams: {
		jobName: undefined,
		description: undefined,
	},
	tableParams: {
		page: 1,
		pageSize: 10,
		total: 0 as any,
	},
	editJobDetailTitle: '',
	editJobTriggerTitle: '',
});

onMounted(async () => {
	handleQuery();

	mittBus.on('submitRefresh', () => {
		handleQuery();
	});
});

onUnmounted(() => {
	mittBus.off('submitRefresh');
});

// 查询操作
const handleQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryParams, state.tableParams);
	var res = await getAPI(SysJobApi).getJobDetailPage(params);
	state.jobData = res.data.data?.items ?? [];
	state.tableParams.total = res.data.data?.total;
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.jobName = undefined;
	state.queryParams.description = undefined;
	handleQuery();
};

// 打开新增作业页面
const openAddJobDetail = () => {
	state.editJobDetailTitle = '添加作业';
	editJobDetailRef.value?.openDialog({ concurrent: true, includeAnnotations: true, groupName: 'default', createType: JobCreateTypeEnum.NUMBER_2 });
};

// 打开编辑作业页面
const openEditJobDetail = (row: JobOutput) => {
	state.editJobDetailTitle = '编辑作业';
	editJobDetailRef.value?.openDialog(row);
};

// 删除作业
const delJobDetail = (row: JobOutput) => {
	ElMessageBox.confirm(`确定删除作业：【${row.jobName}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysJobApi).deleteJobDetail({ schedulerName:"",jobGroup:"",jobName: row.jobName });
			handleQuery();
			ElMessage.success('删除成功');
		})
		.catch(() => {});
};

// 打开新增触发器页面
const openAddJobTrigger = (row: JobOutput) => {
	state.editJobTriggerTitle = '添加触发器';
	editJobTriggerRef.value?.openDialog({ jobName: row.jobName, retryTimeout: 1000, startNow: true, runOnStart: true, resetOnlyOnce: true, triggerType: 'Furion.Schedule.PeriodTrigger' });
};

// 打开编辑触发器页面
const openEditJobTrigger = (row: SysJobTrigger) => {
	state.editJobTriggerTitle = '编辑触发器';
	editJobTriggerRef.value?.openDialog(row);
};

// 删除触发器
const delJobTrigger = (row: SysJobTrigger) => {
	ElMessageBox.confirm(`确定删除触发器：【${row.triggerId}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysJobApi).deleteJobTrigger({ jobId: row.jobId, triggerId: row.triggerId });
			handleQuery();
			ElMessage.success('删除成功');
		})
		.catch(() => {});
};

// 改变页面容量
const handleSizeChange = (val: number) => {
	state.tableParams.pageSize = val;
	handleQuery();
};

// 改变页码序号
const handleCurrentChange = (val: number) => {
	state.tableParams.page = val;
	handleQuery();
};

// 启动所有作业
const startAllJob = async () => {
	await getAPI(SysJobApi).startAllJob();
	ElMessage.success('启动所有作业');
};

// 暂停所有作业
const pauseAllJob = async () => {
	await getAPI(SysJobApi).pauseAllJob();
	ElMessage.success('暂停所有作业');
};

// 启动某个作业
const startJob = async (row: JobOutput) => {
	await getAPI(SysJobApi).apiSysJobStartJobPost({ schedulerName:row.schedulerName,jobGroup:row.jobGroup,jobName: row.jobName });
	ElMessage.success('启动作业');
};

// 暂停某个作业
const pauseJob = async (row: JobOutput) => {
	await getAPI(SysJobApi).pauseJob({ schedulerName:row.schedulerName,jobGroup:row.jobGroup,jobName: row.jobName });
	ElMessage.success('暂停作业');
};

// 启动触发器
const startTrigger = async (row: SysJobTrigger) => {
	await getAPI(SysJobApi).startTrigger({ jobId: row.jobId, triggerId: row.triggerId });
	ElMessage.success('启动触发器');
};

// 暂停触发器
const pauseTrigger = async (row: SysJobTrigger) => {
	await getAPI(SysJobApi).pauseTrigger({ jobId: row.jobId, triggerId: row.triggerId });
	ElMessage.success('暂停触发器');
};

// 强制唤醒作业调度器
const cancelSleep = async () => {
	await getAPI(SysJobApi).cancelJobSleep();
	ElMessage.success('强制唤醒作业调度器');
};

// 强制触发所有作业持久化
const persistAllJob = async () => {
	await getAPI(SysJobApi).persistAllJob();
	ElMessage.success('强制触发所有作业持久化');
};

// 打开集群控制页面
const openJobCluster = () => {
	editJobClusterRef.value?.openDrawer();
};

// 打开任务看板
const openJobDashboard = () => {
	router.push({
		path: '/platform/job/dashboard',
	});
};

// 根据任务属性获取 HttpJobMessage
const getHttpJobMessage = (properties: string | undefined | null): HttpJobMessage => {
	if (properties === undefined || properties === null || properties === '') return {};

	const propData = JSON.parse(properties);
	const httpJobMessageNet = JSON.parse(propData['HttpJob']); // 后端大写开头的 HttpJobMessage

	return {
		requestUri: httpJobMessageNet.RequestUri,
		httpMethod: JSON.stringify(httpJobMessageNet.HttpMethod),
		body: httpJobMessageNet.Body,
	};
};

// 获取请求方法的对应描述
const getHttpMethodDesc = (httpMethodStr: string | undefined | null): string => {
	if (httpMethodStr === undefined || httpMethodStr === null || httpMethodStr === '') return '';

	for (const key in editJobDetailRef.value?.httpMethodDef) {
		if (editJobDetailRef.value?.httpMethodDef[key] === httpMethodStr) return key;
	}
	return '';
};
</script>

<style>
/* 此样式不能为 scoped */
.job-index-descriptions-label-style {
	width: 80px;
}
</style>
