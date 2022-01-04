import axios from 'axios';
// import router from '../routerManuaConfig'
import router from '../router/index'
import store from "../store";
import Vue from 'vue';

import applicationUserManager from "../Auth/applicationusermanager";

let base = '';
// 如果是IIS部署，用这个，因为 IIS 只能是 CORS 跨域，不能代理
// let base = process.env.NODE_ENV=="production"? 'http://localhost:8081':'';


// 请求延时
axios.defaults.timeout = 20000

var storeTemp = store;
axios.interceptors.request.use(
    config => {
        var curTime = new Date()
        var expiretime = new Date(Date.parse(storeTemp.state.tokenExpire))

        if (storeTemp.state.token && (curTime < expiretime && storeTemp.state.tokenExpire)) {
            // 判断是否存在token，如果存在的话，则每个http header都加上token
            config.headers.Authorization = "Bearer " + storeTemp.state.token;
        }

        saveRefreshtime();

        return config;
    },
    err => {
        return Promise.reject(err);
    }
);

// http response 拦截器
axios.interceptors.response.use(
    response => {
        return response;
    },
    error => {
        // 超时请求处理
        var originalRequest = error.config;
        if (error.code == 'ECONNABORTED' && error.message.indexOf('timeout') != -1 && !originalRequest._retry) {

            Vue.prototype.$message({
                message: '请求超时！',
                type: 'error'
            });

            originalRequest._retry = true
            return null;
        }

        if (error.response) {
            if (error.response.status == 401) {
                var curTime = new Date()
                var refreshtime = new Date(Date.parse(window.localStorage.refreshtime))
                // 在用户操作的活跃期内
                if (window.localStorage.refreshtime && (curTime <= refreshtime)) {
                    return refreshToken({ token: window.localStorage.Token }).then((res) => {
                        if (res.success) {
                            Vue.prototype.$message({
                                message: 'refreshToken success! loading data...',
                                type: 'success'
                            });

                            store.commit("saveToken", res.response.token);

                            var curTime = new Date();
                            var expiredate = new Date(curTime.setSeconds(curTime.getSeconds() + res.response.expires_in));
                            store.commit("saveTokenExpire", expiredate);

                            error.config.__isRetryRequest = true;
                            error.config.headers.Authorization = 'Bearer ' + res.response.token;
                            return axios(error.config);
                        } else {
                            // 刷新token失败 清除token信息并跳转到登录页面
                            ToLogin()
                        }
                    });
                } else {
                    // 返回 401，并且不知用户操作活跃期内 清除token信息并跳转到登录页面
                    ToLogin()
                }

            }
            // 403 无权限
            if (error.response.status == 403) {
                Vue.prototype.$message({
                    message: '失败！该操作无权限',
                    type: 'error'
                });
                return null;
            }
            // 429 ip限流
            if (error.response.status == 429) {
                Vue.prototype.$message({
                    message: '刷新次数过多，请稍事休息重试！',
                    type: 'error'
                });
                return null;
            }
        }
        return ""; // 返回接口返回的错误信息
    }
);


export const BaseApiUrl = base;

// 登录
export const requestLogin = params => {
    return axios.get(`${base}/api/login/jwttoken3.0`, { params: params }).then(res => res.data);
};
export const requestLoginMock = params => { return axios.post(`${base}/login`, params).then(res => res.data); };

export const refreshToken = params => {
    return axios.get(`${base}/api/login/RefreshToken`, { params: params }).then(res => res.data);
};
/* MD5加密 */
export const md5Password = params => {
    return axios.get(`${base}/api/login/Md5Password`, { params: params }).then(res => res.data);
};

export const saveRefreshtime = params => {

    let nowtime = new Date();
    let lastRefreshtime = window.localStorage.refreshtime ? new Date(window.localStorage.refreshtime) : new Date(-1);
    let expiretime = new Date(Date.parse(window.localStorage.TokenExpire))

    let refreshCount = 1;//滑动系数
    if (lastRefreshtime >= nowtime) {
        lastRefreshtime = nowtime > expiretime ? nowtime : expiretime;
        lastRefreshtime.setMinutes(lastRefreshtime.getMinutes() + refreshCount);
        window.localStorage.refreshtime = lastRefreshtime;
    } else {
        window.localStorage.refreshtime = new Date(-1);
    }
};
const ToLogin = params => {

    store.commit("saveToken", "");
    store.commit("saveTokenExpire", "");
    store.commit("saveTagsData", "");
    window.localStorage.removeItem('user');
    window.localStorage.removeItem('NavigationBar');



    if (global.IS_IDS4) {
        applicationUserManager.login();
    } else {
        router.replace({
            path: "/login",
            query: { redirect: router.currentRoute.fullPath }
        });

        window.location.reload()
    }
};

export const getUserByToken = params => {
    return axios.get(`${base}/api/user/getInfoByToken`, { params: params }).then(res => res.data);
};


export function testapi2 () {
    console.log('api is ok.')
}

export const testapi = pa => {
    console.log('api is ok.')
}

//图片管理
//上传图片
export const uploadPicUrl = `${base}/api/Img/images/Upload/Pic`
//删除图片
export const removePic = params => {
    return axios.delete(`${base}/api/Img/images/Delete`, { params: params });
};

// 用户管理
export const getUserListPage = params => {
    return axios.post(`${base}/api/user/get`, params);
};
export const removeUser = params => {
    return axios.delete(`${base}/api/user/delete`, { params: params });
};
export const updatePerson = params => {
    return axios.put(`${base}/api/user/UpdatePerson`, params);
};
export const editUser = params => {
    return axios.put(`${base}/api/user/put`, params);
};
export const addUser = params => {
    return axios.post(`${base}/api/user/post`, params);
};
export const batchRemoveUser = params => {
    return axios.delete(`${base}/api/Claims/BatchDelete`, { params: params });//没做
};

// 角色管理
export const getAllRole = params => {
    return axios.get(`${base}/api/role/getAll`, { params: params });
};
export const getRoleListPage = params => {
    return axios.post(`${base}/api/role/get`, params);
};
export const removeRole = params => {
    return axios.delete(`${base}/api/role/delete`, { params: params });
};
export const editRole = params => {
    return axios.put(`${base}/api/role/put`, params);
};
export const addRole = params => {
    return axios.post(`${base}/api/role/post`, params);
};

// 物料管理
export const getMaterialListPage = params => {
    return axios.get(`${base}/api/Material/get`, { params: params });
};
export const removeMaterial = params => {
    return axios.delete(`${base}/api/Material/delete`, { params: params });
};
export const editMaterial = params => {
    return axios.put(`${base}/api/Material/put`, params);
};
export const addMaterial = params => {
    return axios.post(`${base}/api/Material/post`, params);
};

// 接口模块管理
export const getModuleListPage = params => {
    return axios.post(`${base}/api/module/get`, params);
};
export const getAllModuleList = params => {
    return axios.get(`${base}/api/module/getAll`, { params: params });
};
export const removeModule = params => {
    return axios.delete(`${base}/api/module/delete`, { params: params });
};
export const editModule = params => {
    return axios.put(`${base}/api/module/put`, params);
};
export const addModule = params => {
    return axios.post(`${base}/api/module/post`, params);
};

// 菜单模块管理
export const getPermissionListPage = params => {
    return axios.get(`${base}/api/permission/get`, { params: params });
};
export const getPermissionTreeTable = params => {
    return axios.get(`${base}/api/permission/GetTreeTable`, { params: params });
};
export const removePermission = params => {
    return axios.delete(`${base}/api/permission/delete`, { params: params });
};
export const editPermission = params => {
    return axios.put(`${base}/api/permission/put`, params);
};
export const addPermission = params => {
    return axios.post(`${base}/api/permission/post`, params);
};
export const getPermissionTree = params => {
    return axios.get(`${base}/api/permission/getpermissiontree`, { params: params });
};
export const getPermissionIds = params => {
    return axios.get(`${base}/api/permission/GetPermissionIdByRoleId`, { params: params });
};

export const addRolePermission = params => {
    return axios.post(`${base}/api/permission/Assign`, params);
};
export const getNavigationBar = params => {
    return axios.get(`${base}/api/permission/GetNavigationBar`, { params: params }).then(res => res.data);
};

// 配置工单管理方法 
//分页获取工单管理列表 
export const getWorkOrderListPage = params => {
    return axios.post(`${base}/api/Wip_WorkOrder/get`, params);
};
//删除工单管理 
export const removeWorkOrder = params => {
    return axios.delete(`${base}/api/Wip_WorkOrder/delete`, { params: params });
};
//编辑工单管理 
export const editWorkOrder = params => {
    return axios.put(`${base}/api/Wip_WorkOrder/put`, params);
};
//添加工单管理 
export const addWorkOrder = params => {
    return axios.post(`${base}/api/Wip_WorkOrder/post`, params);
};
// 配置工件数据方法 
//分页获取工件数据列表 
export const getWorkPieceListPage = params => {
    return axios.post(`${base}/api/Wip_WorkPiece/get`, params);
};
//获取单个工件的详细数据 
export const getWorkPieceProdRecordDetails = params => {
    return axios.get(`${base}/api/Wip_WorkPiece/getDetails`, { params: params });
};
//删除工件数据 
export const removeWorkPiece = params => {
    return axios.delete(`${base}/api/Wip_WorkPiece/delete`, { params: params });
};
//编辑工件数据 
export const editWorkPiece = params => {
    return axios.put(`${base}/api/Wip_WorkPiece/put`, params);
};
//添加工件数据 
export const addWorkPiece = params => {
    return axios.post(`${base}/api/Wip_WorkPiece/post`, params);
};

// 基础数据-码表集管理
export const getCodeSetListPage = params => {
    return axios.post(`${base}/api/Base_CodeSets/get`, params);
};
export const removeCodeSet = params => {
    return axios.delete(`${base}/api/Base_CodeSets/delete`, { params: params });
};
export const editCodeSet = params => {
    return axios.put(`${base}/api/Base_CodeSets/put`, params);
};
export const addCodeSet = params => {
    return axios.post(`${base}/api/Base_CodeSets/post`, params);
};
export const getAllCodeSets = params => {
    return axios.get(`${base}/api/Base_CodeSets/getAll`, params);
};
//公共
export const getCodeItemsBySetCodeForPublic = params => {
    return axios.get(`${base}/api/LESPublic/getCodeItemsBySetCode`, { params: params });
};
//通过代码编码获取代码项对象
export const getCodeItemsByCode = params => {
    return axios.get(`${base}/api/Base_CodeItems/GetCodeItemsByCode`, { params: params });
};
//获取枚举列表
export const getEnumberList = params => {
    return axios.get(`${base}/api/LESPublic/GetEnumberList`, { params: params });
};

// 基础数据-码表项管理
export const getCodeItemsBySetCode = params => {
    return axios.get(`${base}/api/Base_CodeItems/getCodeItemsBySetCode`, { params: params });
};
//查询工序列表，使用权限，通过代码集编码获取代码项集合
export const getCodeItemsBySetCodeForGX = params => {
    return axios.get(`${base}/api/Base_CodeItems/GetCodeItemsBySetCodeForGX`, { params: params });
};
export const getCodeItemsBySetId = params => {
    return axios.get(`${base}/api/Base_CodeItems/GetCodeItemsBySetId`, { params: params });
};
export const getCodeItemListPage = params => {
    return axios.post(`${base}/api/Base_CodeItems/get`, params);
};
export const removeCodeItem = params => {
    return axios.delete(`${base}/api/Base_CodeItems/delete`, { params: params });
};
export const editCodeItem = params => {
    return axios.put(`${base}/api/Base_CodeItems/put`, params);
};
export const addCodeItem = params => {
    return axios.post(`${base}/api/Base_CodeItems/post`, params);
};
// 配置托盘管理方法 
export const getSalverListPage = params => {
    return axios.post(`${base}/api/Base_Salver/get`, params);
};
export const removeSalver = params => {
    return axios.delete(`${base}/api/Base_Salver/delete`, { params: params });
};
export const editSalver = params => {
    return axios.put(`${base}/api/Base_Salver/put`, params);
};
export const addSalver = params => {
    return axios.post(`${base}/api/Base_Salver/post`, params);
};
// 配置站点管理方法 
export const getStationListPage = params => {
    return axios.post(`${base}/api/Base_Station/get`, params);
};
export const getSingleStation = params => {
    return axios.get(`${base}/api/Base_Station/GetSingle`, { params: params });
};
export const getMaterialsCodeItemsByProcessAndStation = params => {
    return axios.get(`${base}/api/Base_Station/GetMaterialsCodeItemsByProcessAndStation`, { params: params });
};
export const removeStation = params => {
    return axios.delete(`${base}/api/Base_Station/delete`, { params: params });
};
export const editStation = params => {
    return axios.put(`${base}/api/Base_Station/put`, params);
};
export const addStation = params => {
    return axios.post(`${base}/api/Base_Station/post`, params);
};
export const updateStationIsFull = params => {
    return axios.put(`${base}/api/Base_Station/UpdateIsFull`, params);
};
export const updateStationIsLock = params => {
    return axios.put(`${base}/api/Base_Station/UpdateIsLock`, params);
};
export const updateStationIsDeleted = params => {
    return axios.put(`${base}/api/Base_Station/UpdateIsDeleted`, params);
};

// 配置库存信息方法 
export const getStockInfoCollectListPage = params => {
    return axios.post(`${base}/api/Les_StockInfo/getcollect`, params);
};
export const getStockByArea = params => {
    return axios.get(`${base}/api/Les_StockInfo/getStockByArea`, { params: params });
};
export const getStockForBufferStockVisual = params => {
    return axios.get(`${base}/api/Les_StockInfo/GetStockForBufferStockVisual`, { params: params });
};
export const getStockForLVDeMoldVisual = params => {
    return axios.get(`${base}/api/Les_StockInfo/GetStockForLVDeMoldVisual`, { params: params });
};
export const getStockForHVDeMoldVisual = params => {
    return axios.get(`${base}/api/Les_StockInfo/GetStockForHVDeMoldVisual`, { params: params });
};
export const getStockForLVDeMoldPalletVisual = params => {
    return axios.get(`${base}/api/Les_StockInfo/GetStockForLVDeMoldPalletVisual`, { params: params });
};
export const getStockForHVDeMoldPalletVisual = params => {
    return axios.get(`${base}/api/Les_StockInfo/GetStockForHVDeMoldPalletVisual`, { params: params });
};
export const getStockForDeMoldLeadVisual = params => {
    return axios.get(`${base}/api/Les_StockInfo/GetStockForDeMoldLeadVisual`, { params: params });
};
export const getStockForKCQY_GYNMZBQStockVisual = params => {
    return axios.get(`${base}/api/Les_StockInfo/GetStockForKCQY_GYNMZBQStockVisual`, { params: params });
};
export const getStockForKCQY_GYNMTPQStockVisual = params => {
    return axios.get(`${base}/api/Les_StockInfo/GetStockForKCQY_GYNMTPQStockVisual`, { params: params });
};
//获取分组查询立库缓存区的物料列表
export const getBufferStockCollectGroupMaterialList = params => {
    return axios.post(`${base}/api/Les_StockInfo/GetBufferStockCollectGroupMaterialList`, params);
};
export const removeStockInfo = params => {
    return axios.delete(`${base}/api/Les_StockInfo/delete`, { params: params });
};
//删除立库缓存区所有的空托盘和余料信息
export const deleteAllAllEmptySalverStation = params => {
    return axios.delete(`${base}/api/Les_StockInfo/DeleteAllAllEmptySalverStation`, { params: params });
};
//删除某个区域的全部库存
export const clearStockByArea = params => {
    return axios.delete(`${base}/api/Les_StockInfo/ClearStockByArea`, { params: params });
};
//根据站点ID删除库存
export const deleteStockByStationId = params => {
    return axios.delete(`${base}/api/Les_StockInfo/DeleteStockByStationId`, { params: params });
};
export const editStockInfo = params => {
    return axios.put(`${base}/api/Les_StockInfo/put`, params);
};
export const addStockInfo = params => {
    return axios.post(`${base}/api/Les_StockInfo/post`, params);
};
// 配置出库记录方法 
//分页获取出库记录列表 
export const getOutStockRecordListPage = params => {
    return axios.post(`${base}/api/Les_OutStockRecord/get`, params);
};
//删除出库记录 
export const removeOutStockRecord = params => {
    return axios.delete(`${base}/api/Les_OutStockRecord/delete`, { params: params });
};
//编辑出库记录 
export const editOutStockRecord = params => {
    return axios.put(`${base}/api/Les_OutStockRecord/put`, params);
};
//添加出库记录 
export const addOutStockRecord = params => {
    return axios.post(`${base}/api/Les_OutStockRecord/post`, params);
};
// 配置入库记录方法 
//分页获取入库记录列表 
export const getInStockRecordListPage = params => {
    return axios.post(`${base}/api/Les_InStockRecord/get`, params);
};
//删除入库记录 
export const removeInStockRecord = params => {
    return axios.delete(`${base}/api/Les_InStockRecord/delete`, { params: params });
};
//编辑入库记录 
export const editInStockRecord = params => {
    return axios.put(`${base}/api/Les_InStockRecord/put`, params);
};
//添加入库记录 
export const addInStockRecord = params => {
    return axios.post(`${base}/api/Les_InStockRecord/post`, params);
};

// 配置AGV任务方法 
//分页获取AGV任务列表 
export const getAgvTaskListPage = params => {
    return axios.post(`${base}/api/Les_AgvTask/get`, params);
};
export const getForNoFinished = params => {
    return axios.post(`${base}/api/Les_AgvTask/GetForNoFinished`, params);
};
//根据 AGV任务表的ID 或者是任务号 查找 该任务的详细阶段列表  
export const getAgvTaskPhaseList = params => {
    return axios.get(`${base}/api/Les_AgvTask/GetAgvTaskPhaseList`, { params: params });
};
//删除AGV任务 
export const removeAgvTask = params => {
    return axios.delete(`${base}/api/Les_AgvTask/delete`, { params: params });
};
//编辑AGV任务 
export const editAgvTask = params => {
    return axios.put(`${base}/api/Les_AgvTask/put`, params);
};
//添加AGV任务 
export const addAgvTask = params => {
    return axios.post(`${base}/api/Les_AgvTask/post`, params);
};

// 配置AGV异常方法 
//分页获取AGV异常列表 
export const getAgvExcepitonListPage = params => {
    return axios.post(`${base}/api/Les_AgvExcepiton/get`, params);
};
//删除AGV异常 
export const removeAgvExcepiton = params => {
    return axios.delete(`${base}/api/Les_AgvExcepiton/delete`, { params: params });
};
//编辑AGV异常 
export const editAgvExcepiton = params => {
    return axios.put(`${base}/api/Les_AgvExcepiton/put`, params);
};
//添加AGV异常 
export const addAgvExcepiton = params => {
    return axios.post(`${base}/api/Les_AgvExcepiton/post`, params);
};


// 配置呼叫记录方法 
//分页获取呼叫记录列表 
export const getCallRecordListPage = params => {
    return axios.post(`${base}/api/Les_CallRecord/get`, params);
};
//删除呼叫记录 
export const removeCallRecord = params => {
    return axios.delete(`${base}/api/Les_CallRecord/delete`, { params: params });
};
//编辑呼叫记录 
export const editCallRecord = params => {
    return axios.put(`${base}/api/Les_CallRecord/put`, params);
};
//添加呼叫记录 
export const addCallRecord = params => {
    return axios.post(`${base}/api/Les_CallRecord/post`, params);
};

// 配置叫料工单方法 
//分页获取叫料工单列表 
export const getCallOrderListPage = params => {
    return axios.post(`${base}/api/Les_CallOrder/get`, params);
};
//删除叫料工单 
export const removeCallOrder = params => {
    return axios.delete(`${base}/api/Les_CallOrder/delete`, { params: params });
};
//编辑叫料工单 
export const editCallOrder = params => {
    return axios.put(`${base}/api/Les_CallOrder/put`, params);
};
//验证叫料工单 
export const validateCreateCallOrder = params => {
    return axios.post(`${base}/api/Les_CallOrder/validateCreateCallOrder`, params);
};
//根据 配料工作号 查找 已经分拣完成的，并且指定物料的库存信息 
export const getBufferStockListByWorkNoForAlert = params => {
    return axios.post(`${base}/api/Les_CallOrder/GetBufferStockListByWorkNoForAlert`, params);
};
//添加叫料工单 
export const addCallOrder = params => {
    return axios.post(`${base}/api/Les_CallOrder/post`, params);
};
// 配置回库搬运方法 
//分页获取回库搬运列表 
export const getBackToStoreListPage = params => {
    return axios.post(`${base}/api/Les_BackToStore/get`, params);
};
//删除回库搬运 
export const removeBackToStore = params => {
    return axios.delete(`${base}/api/Les_BackToStore/delete`, { params: params });
};
//编辑回库搬运 
export const editBackToStore = params => {
    return axios.put(`${base}/api/Les_BackToStore/put`, params);
};
//添加回库搬运 
export const addBackToStore = params => {
    return axios.post(`${base}/api/Les_BackToStore/post`, params);
};
// 配置产线报警方法 
//分页获取产线报警列表 
export const getLineWarningListPage = params => {
    return axios.post(`${base}/api/Wip_LineWarning/get`, params);
};
//删除产线报警 
export const removeLineWarning = params => {
    return axios.delete(`${base}/api/Wip_LineWarning/delete`, { params: params });
};
//编辑产线报警 
export const editLineWarning = params => {
    return axios.put(`${base}/api/Wip_LineWarning/put`, params);
};
//添加产线报警 
export const addLineWarning = params => {
    return axios.post(`${base}/api/Wip_LineWarning/post`, params);
};


// 日志
export const getLogs = params => {
    return axios.get(`${base}/api/Monitor/get`, { params: params });
};
export const getRequestApiinfoByWeek = params => {
    return axios.get(`${base}/api/Monitor/GetRequestApiinfoByWeek`, { params: params });
};
export const getAccessApiByDate = params => {
    return axios.get(`${base}/api/Monitor/GetAccessApiByDate`, { params: params });
};
export const getAccessApiByHour = params => {
    return axios.get(`${base}/api/Monitor/GetAccessApiByHour`, { params: params });
};
export const getServerInfo = params => {
    return axios.get(`${base}/api/Monitor/Server`, { params: params });
};
export const getAccessLogs = params => {
    return axios.get(`${base}/api/Monitor/GetAccessLogs`, { params: params });
};
export const getIds4UsersGrow = params => {
    return axios.get(`${base}/api/Monitor/GetIds4Users`, { params: params });
};
export const getActiveUsers = params => {
    return axios.get(`${base}/api/Monitor/GetActiveUsers`, { params: params });
};

// 信息采集
export const realTimeDataShow = params => {
    return axios.get(`${base}/api/wip/RealTimeDataShow`, { params: params });
};

// Task管理
export const getTaskListPage = params => {
    return axios.post(`${base}/api/TasksQz/get`, params);
};
export const removeTask = params => {
    return axios.delete(`${base}/api/TasksQz/delete`, { params: params });
};
export const editTask = params => {
    return axios.put(`${base}/api/TasksQz/put`, params);
};
export const addTask = params => {
    return axios.post(`${base}/api/TasksQz/post`, params);
};

export const startJob = params => {
    return axios.get(`${base}/api/TasksQz/StartJob`, { params: params });
};
export const stopJob = params => {
    return axios.get(`${base}/api/TasksQz/StopJob`, { params: params });
};
export const reCovery = params => {
    return axios.get(`${base}/api/TasksQz/ReCovery`, { params: params });
};
export const executeJob = params => {
    return axios.get(`${base}/api/TasksQz/ExecuteJob`, { params: params });
};
export const pauseJob = params => {
    return axios.get(`${base}/api/TasksQz/PauseJob`, { params: params });
};
export const resumeJob = params => {
    return axios.get(`${base}/api/TasksQz/ResumeJob`, { params: params });
};
export const getTaskNameSpace = params => {
    return axios.get(`${base}/api/TasksQz/GetTaskNameSpace`, { params: params });
};

// ids4
export const getAchieveUsers_IS4 = params => {
    return axios.get(`${base}/is4api/GetAchieveUsers`, { params: params });
};

// 配置线圈托盘方法 
//分页获取线圈托盘列表 
export const getCoilSalverListPage = params => {
    return axios.post(`${base}/api/Wip_CoilSalver/get`, params);
};
//删除线圈托盘 
export const removeCoilSalver = params => {
    return axios.delete(`${base}/api/Wip_CoilSalver/delete`, { params: params });
};
//编辑线圈托盘 
export const editCoilSalver = params => {
    return axios.put(`${base}/api/Wip_CoilSalver/put`, params);
};
//添加线圈托盘 
export const addCoilSalver = params => {
    return axios.post(`${base}/api/Wip_CoilSalver/post`, params);
};
// 配置请求备料表方法 
//分页获取请求备料表列表 
export const getPrepareMatsListPage = params => {
    return axios.post(`${base}/api/Les_PrepareMats/get`, params);
};
//删除请求备料表 
export const removePrepareMats = params => {
    return axios.delete(`${base}/api/Les_PrepareMats/delete`, { params: params });
};
//编辑请求备料表 
export const editPrepareMats = params => {
    return axios.put(`${base}/api/Les_PrepareMats/put`, params);
};
//添加请求备料表 
export const addPrepareMats = params => {
    return axios.post(`${base}/api/Les_PrepareMats/post`, params);
};
//获取空库位列表
export const getEmptyStationList = params => {
    return axios.get(`${base}/api/Base_Station/GetEmptyStationList`, { params: params });
};
//用于下拉列表，展示所有的库位，并显示状态
export const getAllStationList = params => {
    return axios.get(`${base}/api/Base_Station/GetAllStationList`, { params: params });
};
//根据工序获取工序下的站点列表（显示全部）
export const getStationsByProcedure = params => {
    return axios.get(`${base}/api/Base_Station/GetStationsByProcedure`, { params: params });
};
//根据用户的工序权限，获取他下面的工序获取工序下的站点列表（显示全部）
export const getAllStationsByUserProcedure = params => {
    return axios.get(`${base}/api/Base_Station/getAllStationsByUserProcedure`, { params: params });
};
// 配置人工处理数据记录表方法 
//分页获取人工处理数据记录表列表 
export const getPersonHandlerRecordListPage = params => {
    return axios.post(`${base}/api/Les_PersonHandlerRecord/get`, params);
};
//删除人工处理数据记录表 
export const removePersonHandlerRecord = params => {
    return axios.delete(`${base}/api/Les_PersonHandlerRecord/delete`, { params: params });
};
//编辑人工处理数据记录表 
export const editPersonHandlerRecord = params => {
    return axios.put(`${base}/api/Les_PersonHandlerRecord/put`, params);
};
//添加人工处理数据记录表 
export const addPersonHandlerRecord = params => {
    return axios.post(`${base}/api/Les_PersonHandlerRecord/post`, params);
};

//获取大类物料，按照分级结构
export const getBigMaterialsForCascader = params => {
    return axios.get(`${base}/api/LESPublic/GetBigMaterialsForCascader`, { params: params });
};
//获取站点，按照 工序的 分级结构
export const getStaionsByProcedureForCascader = params => {
    return axios.get(`${base}/api/LESPublic/GetStaionsByProcedureForCascader`, { params: params });
};

// 配置分拣入库任务方法 
//分页获取分拣入库任务列表 
export const getPickInStockTaskListPage = params => {
    return axios.post(`${base}/api/Les_PickInStockTask/get`, params);
};
//删除分拣入库任务 
export const removePickInStockTask = params => {
    return axios.delete(`${base}/api/Les_PickInStockTask/delete`, { params: params });
};
//编辑分拣入库任务 
export const editPickInStockTask = params => {
    return axios.put(`${base}/api/Les_PickInStockTask/put`, params);
};
//添加分拣入库任务 
export const addPickInStockTask = params => {
    return axios.post(`${base}/api/Les_PickInStockTask/post`, params);
};
//更新分拣入库任务是否分拣完成 
export const updateIsPickFinish = params => {
    return axios.put(`${base}/api/Les_PickInStockTask/UpdateIsPickFinish`, params);
};
// 配置角色工序查询视图方法 
//分页获取角色工序查询视图列表 
export const getProcedureRoleListPage = params => {
    return axios.post(`${base}/api/V_ProcedureRole/get`, params);
};
//删除角色工序查询视图 
export const removeProcedureRole = params => {
    return axios.delete(`${base}/api/V_ProcedureRole/delete`, { params: params });
};
//编辑角色工序查询视图 
export const editProcedureRole = params => {
    return axios.put(`${base}/api/V_ProcedureRole/put`, params);
};
//添加角色工序查询视图 
export const addProcedureRole = params => {
    return axios.post(`${base}/api/V_ProcedureRole/post`, params);
};
//根据 LES任务表的ID 查找 该任务的详细阶段列表  
export const getTaskPhaseList = params => {
    return axios.get(`${base}/api/Les_TaskPhase/Get`, { params: params });
};
//根据 LES任务号 查找 该任务的详细阶段列表  
export const getTaskPhaseListByLesTaskNo = params => {
    return axios.get(`${base}/api/Les_TaskPhase/GetTaskPhaseListByLesTaskNo`, { params: params });
};
// 配置Agv车辆方法 
//获取全部车辆
export const getAllVehicles = params => {
    return axios.get(`${base}/api/Les_Agv/GetAllVehicles`, { params: params });
};
//分页获取Agv车辆列表 
export const getAgvListPage = params => {
    return axios.post(`${base}/api/Les_Agv/get`, params);
};
//删除Agv车辆 
export const removeAgv = params => {
    return axios.delete(`${base}/api/Les_Agv/delete`, { params: params });
};
//编辑Agv车辆 
export const editAgv = params => {
    return axios.put(`${base}/api/Les_Agv/put`, params);
};
//添加Agv车辆 
export const addAgv = params => {
    return axios.post(`${base}/api/Les_Agv/post`, params);
};
// 配置用户站点绑定方法 
//分页获取用户站点绑定列表 
export const getUserStationListPage = params => {
    return axios.post(`${base}/api/V_UserStation/get`, params);
};
//删除用户站点绑定 
export const removeUserStation = params => {
    return axios.delete(`${base}/api/V_UserStation/delete`, { params: params });
};
//编辑用户站点绑定 
export const editUserStation = params => {
    return axios.put(`${base}/api/V_UserStation/put`, params);
};
//添加用户站点绑定 
export const addUserStation = params => {
    return axios.post(`${base}/api/V_UserStation/post`, params);
};

//取消AGV任务 
export const cancelAgvTask = params => {
    return axios.post(`${base}/api/LESPublic/CancelAgvTask`, params);
};
//强制完成AGV任务 
export const coerceFinishAgvTask = params => {
    return axios.post(`${base}/api/LESPublic/CoerceFinishAgvTask`, params);
};
// 配置AGV报警方法 
//分页获取AGV报警列表 
export const getAgvWarningListPage = params => {
    return axios.post(`${base}/api/Les_AgvWarning/get`, params);
};
//删除AGV报警 
export const removeAgvWarning = params => {
    return axios.delete(`${base}/api/Les_AgvWarning/delete`, { params: params });
};
//编辑AGV报警 
export const editAgvWarning = params => {
    return axios.put(`${base}/api/Les_AgvWarning/put`, params);
};
//添加AGV报警 
export const addAgvWarning = params => {
    return axios.post(`${base}/api/Les_AgvWarning/post`, params);
};
//更新全部报警为已处理 
export const updateAllWarningDone = params => {
    return axios.post(`${base}/api/Les_AgvWarning/UpdateAllWarningDone`, params);
};
//报警信息知道了 
export const updateWarningKnow = params => {
    return axios.post(`${base}/api/Les_AgvWarning/UpdateWarningKnow`, params);
};
//获取 最新的AGV报警 
export const getLastOneAgvWarning = params => {
    return axios.get(`${base}/api/Les_AgvWarning/GetLastOneAgvWarning`, { params: params });
};
// 配置高压绕线线圈任务方法 
//分页获取高压绕线线圈任务列表 
export const getHighPressureProductsTaskListPage = params => {
    return axios.post(`${base}/api/Les_HighPressureProductsTask/get`, params);
};
//删除高压绕线线圈任务 
export const removeHighPressureProductsTask = params => {
    return axios.delete(`${base}/api/Les_HighPressureProductsTask/delete`, { params: params });
};
//编辑高压绕线线圈任务 
export const editHighPressureProductsTask = params => {
    return axios.put(`${base}/api/Les_HighPressureProductsTask/put`, params);
};
//添加高压绕线线圈任务 
export const addHighPressureProductsTask = params => {
    return axios.post(`${base}/api/Les_HighPressureProductsTask/post`, params);
};
// 配置出入库记录方法 
//分页获取出入库记录列表 
export const getInOutStockRecordListPage = params => {
    return axios.post(`${base}/api/Les_InOutStockRecord/get`, params);
};
//删除出入库记录 
export const removeInOutStockRecord = params => {
    return axios.delete(`${base}/api/Les_InOutStockRecord/delete`, { params: params });
};
//编辑出入库记录 
export const editInOutStockRecord = params => {
    return axios.put(`${base}/api/Les_InOutStockRecord/put`, params);
};
//添加出入库记录 
export const addInOutStockRecord = params => {
    return axios.post(`${base}/api/Les_InOutStockRecord/post`, params);
};

//获取首页数据  
export const getWelcomeData = params => {
    return axios.get(`${base}/api/LESPublic/GetWelcomeData`, { params: params });
};
export const getWelcomeDataForPie = params => {
    return axios.get(`${base}/api/LESPublic/GetWelcomeDataForPie`, { params: params });
};
export const productYieldByTimeRange = params => {
    return axios.get(`${base}/api/LESPublic/ProductYieldByTimeRange`, { params: params });
};
export const getAgvForUseRatioPie = params => {
    return axios.get(`${base}/api/LESPublic/GetAgvForUseRatioPie`, { params: params });
};
//增加库存 
export const addStoreByStation = params => {
    return axios.post(`${base}/api/LESPublic/AddStoreByStation`, params);
};

// 配置异常日志表方法 
//分页获取异常日志表列表 
export const getSysExceptionInfoListPage = params => {
    return axios.post(`${base}/api/Base_SysExceptionInfo/get`, params);
};
//删除异常日志表 
export const removeSysExceptionInfo = params => {
    return axios.delete(`${base}/api/Base_SysExceptionInfo/delete`, { params: params });
};
//编辑异常日志表 
export const editSysExceptionInfo = params => {
    return axios.put(`${base}/api/Base_SysExceptionInfo/put`, params);
};
//添加异常日志表 
export const addSysExceptionInfo = params => {
    return axios.post(`${base}/api/Base_SysExceptionInfo/post`, params);
};

// 配置请求日志表方法 
//分页获取请求日志表列表 
export const getHttpRequestRecordListPage = params => {
    return axios.post(`${base}/api/HttpRequestRecord/get`, params);
};
//删除请求日志表 
export const removeHttpRequestRecord = params => {
    return axios.delete(`${base}/api/HttpRequestRecord/delete`, { params: params });
};
//编辑请求日志表 
export const editHttpRequestRecord = params => {
    return axios.put(`${base}/api/HttpRequestRecord/put`, params);
};
//添加请求日志表 
export const addHttpRequestRecord = params => {
    return axios.post(`${base}/api/HttpRequestRecord/post`, params);
};

// 配置Agv车辆状态历史记录方法 
//分页获取Agv车辆状态历史记录列表 
export const getAgvStatusHistoryListPage = params => {
    return axios.post(`${base}/api/Les_AgvStatusHistory/get`, params);
};
//删除Agv车辆状态历史记录 
export const removeAgvStatusHistory = params => {
    return axios.delete(`${base}/api/Les_AgvStatusHistory/delete`, { params: params });
};
//编辑Agv车辆状态历史记录 
export const editAgvStatusHistory = params => {
    return axios.put(`${base}/api/Les_AgvStatusHistory/put`, params);
};
//添加Agv车辆状态历史记录 
export const addAgvStatusHistory = params => {
    return axios.post(`${base}/api/Les_AgvStatusHistory/post`, params);
};
// 配置Thingworx呼叫任务方法 
//分页获取Thingworx呼叫任务列表 
export const getThingworx_CallOrderListPage = params => {
    return axios.post(`${base}/api/Thingworx_CallOrder/get`, params);
};

// 配置Thingworx 备料通知表方法 
//分页获取Thingworx 备料通知表列表 
export const getStockPreparationNoticeListPage = params => {
    return axios.post(`${base}/api/Thingworx_StockPreparationNotice/get`, params);
};
//删除Thingworx 备料通知表 
export const removeStockPreparationNotice = params => {
    return axios.delete(`${base}/api/Thingworx_StockPreparationNotice/delete`, { params: params });
};
//编辑Thingworx 备料通知表 
export const editStockPreparationNotice = params => {
    return axios.put(`${base}/api/Thingworx_StockPreparationNotice/put`, params);
};
//添加Thingworx 备料通知表 
export const addStockPreparationNotice = params => {
    return axios.post(`${base}/api/Thingworx_StockPreparationNotice/post`, params);
}; 
