<template>
  <section>
    <!--工具条-->
    <el-col :span="24"
            class="toolbar"
            style="padding-bottom: 0px;">
      <el-form :inline="true"
               @submit.native.prevent>
        <toolbar :buttonList="buttonList"
                 @callFunction="callFunction">
        </toolbar>
        <!-- 搜索 -->
        <SearchForm :formOptions="formOptions"
                    :commonSearchOptionSet="commonSearchOptionSet"
                    :drawerSize="drawerSize"
                    :labelWidth="labelWidth"
                    :controlStyle="controlStyle"
                    :searchValControlStyle="searchValControlStyle"
                    :searchFormInputPlaceholder="searchFormInputPlaceholder"
                    :searchFormInputAttrs="searchFormInputAttrs"
                    ref="ChildSearchForm"
                    @onSearch="_getCallOrders" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="CallOrders"
              :max-height="tableHeight"
              highlight-current-row
              size="small"
              :height="tableHeight"
              @current-change="selectCurrentRow"
              v-loading="addLoading"
              @selection-change="selsChange"
              style="width: 99%;">
      <el-table-column type="index"
                       :index="indexMethod"
                       label="#"
                       align="center"
                       width="50">
      </el-table-column>
      <el-table-column prop="CallOrderTaskNo"
                       label="叫料任务号"
                       width="160"
                       align="left">
        <template slot-scope="scope">
          <a @click="showDetailForTaskPhaseList(scope.row)"
             target="_blank"
             style="text-decoration:underline;cursor: pointer;color:blue;font-weight:bold;">
            {{scope.row.CallOrderTaskNo}}
          </a>
        </template>
      </el-table-column>
      <el-table-column prop="BurdenWorkNo"
                       width="120"
                       label="呼叫配料任务号"
                       align="center">
      </el-table-column>
      <el-table-column prop="BurdenWorkNoActual"
                       width="120"
                       label="实际配料任务号"
                       align="center">
      </el-table-column>
      <el-table-column prop="StatusName"
                       label="状态"
                       align="center">
      </el-table-column>
      <el-table-column prop="LesTaskModeName"
                       label="呼叫模式"
                       width="120"
                       align="center">
      </el-table-column>
      <el-table-column prop="AgvTaskNo"
                       label="第一个AGV任务"
                       width="160"
                       align="left">
        <template slot-scope="scope">
          <a @click="showDetailForAgvTaskPhaseList(scope.row.AgvTaskNo)"
             target="_blank"
             style="text-decoration:underline;cursor: pointer;color:blue;font-weight:bold;">
            {{scope.row.AgvTaskNo}}
          </a>
        </template>
      </el-table-column>
      <el-table-column prop="AgvTaskNo2"
                       label="第二个AGV任务"
                       width="160"
                       align="left">
        <template slot-scope="scope">
          <a @click="showDetailForAgvTaskPhaseList(scope.row.AgvTaskNo2)"
             target="_blank"
             style="text-decoration:underline;cursor: pointer;color:blue;font-weight:bold;">
            {{scope.row.AgvTaskNo2}}
          </a>
        </template>
      </el-table-column>
      <el-table-column prop="MaterialCode"
                       label="物料编号"
                       align="center">
      </el-table-column>
      <el-table-column prop="MaterialName"
                       label="物料名称"
                       align="center">
      </el-table-column>
      <el-table-column prop="StationCode"
                       label="呼叫站点号"
                       align="center">
      </el-table-column>
      <el-table-column prop="StationName"
                       label="呼叫站点名"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="DeliveryStationCode"
                       label="送货站点号"
                       align="center">
      </el-table-column>
      <el-table-column prop="DeliveryStationName"
                       label="送货站点名"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="StoreAreaName"
                       label="库存区域"
                       align="center">
      </el-table-column>
      <el-table-column prop="AllowProcedureName"
                       label="所属工序"
                       align="center">
      </el-table-column>
      <el-table-column prop="PlaceTypeName"
                       label="工位类型"
                       align="center">
      </el-table-column>
      <el-table-column prop="TaskIssueTime"
                       label="下发时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
      <el-table-column prop="TaskFinishTime"
                       label="完成时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
      <el-table-column prop="CallOrderRemark"
                       label="叫料说明"
                       align="center">
      </el-table-column>
      <el-table-column prop="DataSourceName"
                       label="来源"
                       align="center">
      </el-table-column>
      <el-table-column prop="CreateBy"
                       label="创建者"
                       align="center">
      </el-table-column>
      <el-table-column prop="CreateTime"
                       label="创建时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
      <el-table-column prop="ModifyBy"
                       label="修改者"
                       align="center">
      </el-table-column>
      <el-table-column prop="ModifyTime"
                       label="修改时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
      <el-table-column prop="IsManualHandling"
                       label="是否人工处理"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.IsManualHandling ? 'danger' : 'success'"
                  disable-transitions>{{scope.row.IsManualHandling ? "是":"否"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="ManualHandlingUser"
                       label="人工处理人"
                       align="center">
      </el-table-column>
      <el-table-column prop="ManualHandlingRemark"
                       label="人工处理说明"
                       align="center">
      </el-table-column>
      <el-table-column prop="ManualHandlingTime"
                       label="人工处理时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
    </el-table>

    <!--工具条-->
    <el-col :span="24"
            style="margin:10px 10px 10px 0px;"
            class="toolbar">
      <el-pagination @size-change="handleSizeChange"
                     @current-change="handleCurrentChange"
                     :current-page="page"
                     background
                     :page-sizes="[5,10,50,100, 200, 300, 400]"
                     :page-size="pageSize"
                     layout="total, sizes, prev, pager, next, jumper"
                     :total="total">
      </el-pagination>
    </el-col>
    <!--新增/编辑界面-->
    <el-dialog :title="dialogTitle"
               :close-on-click-modal="false"
               width="60%"
               :visible.sync="addFormVisible"
               :before-close="cancelHandle">
      <el-form :model="addForm"
               size="small"
               :rules="rules"
               label-width="120px"
               ref="addForm">
        <el-row>
          <el-col :span="addFormSpan">
            <el-form-item label="呼叫模式"
                          prop="LesTaskMode"
                          :rules="[{ required: true, message: '呼叫模式不能为空'}]">
              <el-select v-model="addForm.LesTaskMode"
                         @change="changeLesTaskMode"
                         style="width:80%"
                         placeholder="请选择呼叫模式">
                <el-option v-for="item in lesTaskModeItems"
                           :key="item.value"
                           :label="item.label"
                           :value="item.value">
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="addFormSpan">
            <el-form-item label="所属工序"
                          prop="AllowProcedure"
                          :rules="[{ required: true, message: '所属工序不能为空'}]">
              <el-select v-model="addForm.AllowProcedure"
                         style="width:80%"
                         placeholder="请选择所属工序">
                <el-option v-for="item in allCodeSetsForAllowProcess"
                           :key="item.value"
                           :label="item.label"
                           @click.native="changeAllowProcess(item)"
                           :value="item.value">
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="addFormSpan">
            <el-form-item label="呼叫站点"
                          prop="StationId"
                          :rules="[{ required: true, message: '呼叫站点不能为空'}]">
              <el-select v-model="addForm.StationId"
                         style="width:80%"
                         placeholder="请选择呼叫站点">
                <el-option v-for="item in stationList"
                           :key="item.Id"
                           @click.native="changeStation(item)"
                           :label="item.StationName"
                           :value="item.Id">
                  <span style="float: left">{{ item.StationName }}</span>
                  <span style="float: right; color: #8492a6; font-size: 13px">{{ item.DisabledMsg }}</span>
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="addFormSpan">
            <el-form-item label="物料"
                          prop="MaterialCodeItemId"
                          v-if="isShowBurdenWorkNo"
                          :rules="[{ required: true, message: '物料不能为空'}]">
              <el-select v-model="addForm.MaterialCodeItemId"
                         clearable
                         style="width:80%"
                         placeholder="请选择">
                <el-option v-for="item in materialNoCodeItems"
                           :key="item.value"
                           @click.native="changeMaterialCodeItemId(item)"
                           :label="item.label"
                           :value="item.value">
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item label="配料任务号"
                      prop="BurdenWorkNo"
                      v-if="isShowBurdenWorkNo">
          <el-input v-model="addForm.BurdenWorkNo"
                    @change="changeBurdenWorkNo"></el-input>
        </el-form-item>
        <el-form-item label=""
                      v-if="false">
          <el-tag type="warning">{{getBufferStockListByWorkNoForAlertMsg}}</el-tag>
        </el-form-item>

        <el-form-item label="叫料说明"
                      prop="CallOrderRemark ">
          <el-input v-model="addForm.CallOrderRemark"></el-input>
        </el-form-item>
        <el-row v-if="isShowBurdenWorkNo">
          <el-col :span="24">
            <el-card>
              <div slot="header"
                   class="clearfix">
                <span>当前立库缓存区库存</span>
                <div style="float:right;">
                  <el-button type="success"
                             size="mini"
                             icon="el-icon-refresh-right"
                             @click.native="handleRereshTableList"
                             round>刷新</el-button>
                </div>
              </div>
              <el-table :data="tableData_QueryBufferCollect"
                        :row-class-name="tableRowClassName"
                        size="mini"
                        max-height="250"
                        style="width: 100%">
                <el-table-column prop="BurdenWorkNo"
                                 label="配料任务号">
                </el-table-column>
                <el-table-column prop="MaterialName"
                                 label="物料">
                </el-table-column>
                <el-table-column prop="Quantity"
                                 label="在库托数">
                </el-table-column>
                <el-table-column prop="IsPickFinishName"
                                 label="分拣状态">
                </el-table-column>
                <el-table-column prop="PickInStockTaskRemark"
                                 label="备注">
                </el-table-column>
              </el-table>
            </el-card>

          </el-col>
        </el-row>

      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="cancelHandle"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="saveData"
                   :loading="addLoading"
                   icon="fa fa-send">提交</el-button>
      </div>
    </el-dialog>
    <!--取消任务界面-->
    <el-dialog title="取消AGV任务"
               :visible.sync="cancelAgvTaskVisible"
               :close-on-click-modal="false">
      <el-form :model="cancelAgvTaskForm"
               size="small"
               label-width="120px"
               ref="cancelAgvTaskForm">
        <el-form-item label="叫料任务号"
                      prop="CallOrderTaskNo">
          <el-input v-model="cancelAgvTaskForm.CallOrderTaskNo"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="配料任务号"
                      prop="BurdenWorkNo">
          <el-input v-model="cancelAgvTaskForm.BurdenWorkNo"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="呼叫模式"
                      prop="LesTaskModeName">
          <el-input v-model="cancelAgvTaskForm.LesTaskModeName"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="StatusName">
          <el-input v-model="cancelAgvTaskForm.StatusName"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="取消说明"
                      prop="OperationRemark"
                      :rules="[{ required: true, message: '取消说明不能为空'}]">
          <el-input v-model="cancelAgvTaskForm.OperationRemark"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="cancelAgvTaskVisible = false"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="do_handleCancelAgvTask"
                   icon="fa fa-send"
                   :loading="addLoading">提交</el-button>
      </div>
    </el-dialog>
    <!--强制完成AGV任务界面-->
    <el-dialog title="强制完成AGV任务"
               :visible.sync="CoerceFinishAgvTaskVisible"
               :close-on-click-modal="false">
      <el-form :model="coerceFinishAgvTaskForm"
               size="small"
               label-width="120px"
               ref="coerceFinishAgvTaskForm">
        <el-form-item label="叫料任务号"
                      prop="CallOrderTaskNo">
          <el-input v-model="coerceFinishAgvTaskForm.CallOrderTaskNo"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="配料任务号"
                      prop="BurdenWorkNo">
          <el-input v-model="coerceFinishAgvTaskForm.BurdenWorkNo"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="呼叫模式"
                      prop="LesTaskModeName">
          <el-input v-model="coerceFinishAgvTaskForm.LesTaskModeName"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="StatusName">
          <el-input v-model="coerceFinishAgvTaskForm.StatusName"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="强制完成说明"
                      prop="OperationRemark"
                      :rules="[{ required: true, message: '强制完成说明不能为空'}]">
          <el-input v-model="coerceFinishAgvTaskForm.OperationRemark"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="CoerceFinishAgvTaskVisible = false"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="do_handleCoerceFinishAgvTask"
                   icon="fa fa-send"
                   :loading="addLoading">提交</el-button>
      </div>
    </el-dialog>
    <!--详细界面(AGV任务阶段列表)-->
    <ShowAgvTaskPhaseList ref="ShowAgvTaskPhaseListMethod"></ShowAgvTaskPhaseList>
    <!--详细界面（任务阶段列表）-->
    <ShowTaskPhaseList ref="ShowTaskPhaseListMethod"></ShowTaskPhaseList>
    <!-- 导出组件 -->
    <ToolbarExport ref="cmToolbarExport"
                   :exportFileName="exportFileName"
                   :currentPageData="currentPageData"
                   :exportColumnHeader="export_columnHeader" />
  </section>
</template> 
 
<script> 
import util from '../../../util/date'
import { getMaterialsCodeItemsByProcessAndStation, getSingleStation, coerceFinishAgvTask, cancelAgvTask, getBufferStockCollectGroupMaterialList, getCodeItemsByCode, getCodeItemsBySetCodeForGX, getBufferStockListByWorkNoForAlert, getTaskPhaseList, validateCreateCallOrder, getStationsByProcedure, getEnumberList, getCodeItemsBySetCode, getCallOrderListPage, removeCallOrder, addCallOrder } from '../../api/api';
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";
import SearchForm from "../../components/SearchForm";
import ToolbarExport from "../../components/ToolbarExport";
import ShowTaskPhaseList from "./LESComponents/ShowTaskPhaseList";
import ShowAgvTaskPhaseList from "./LESComponents/ShowAgvTaskPhaseList";
import { formatDate, setformOptionsNewValue } from '../../../util/tools'
import { validateIsUseDefaultBurdenWorkNo, validateNewBurdenWorkNo } from '../../../util/LesUtil'
import { isShowOperatorButtonCommon, isNeedShowOperatorColumn, isMobile } from '../../../util/common'


export default {
  components: { ShowAgvTaskPhaseList, ShowTaskPhaseList, Toolbar, SearchForm, ToolbarExport },//注册子组件 
  data () {
    return {
      addFormSpan: 12,
      CoerceFinishAgvTaskVisible: false,
      coerceFinishAgvTaskForm: [],
      cancelAgvTaskVisible: false,
      cancelAgvTaskForm: [],
      rules: {
        BurdenWorkNo: [{
          validator: (rule, newBurdenWorkNo, callback) => {
            var msg = validateNewBurdenWorkNo(this.addForm.MaterialCodeItemId, this.addForm.AllowProcedureName, newBurdenWorkNo)
            if (msg != '') {
              callback(new Error(msg));
            }
            callback()
          }, trigger: 'blur'
        }]
      },
      tableData_QueryBufferCollect: [],
      currSelectAllowProcess: {},//当前选择的工序对象
      isNeedSetDisabled: false,
      isShowBurdenWorkNo: false,
      detailShow: false,
      taskPhaseList: [],
      stationList: [],
      lesTaskModeItems: [],
      materialNoCodeItems: [],//大类物料清单
      allCodeSetsForAllowProcess: [],
      //导出组件相关 
      exportFileName: '叫料工单信息',//要导出的文件名 
      currentPageData: [],//当前页面的列表数据 
      export_columnHeader: {
        'CallOrderTaskNo': '呼叫任务号', 'LesTaskModeName': '呼叫模式',
        'BurdenWorkNo': '呼叫配料任务号',
        'BurdenWorkNoActual': '实际配料任务号',
        'AgvTaskNo': 'AGV1任务号', 'AgvTaskNo2': 'AGV2任务号',
        'AllowProcedureName': '所属工序', 'AllowLineName': '所属产线',
        'StationCode': '叫料站点编号', 'StationName': '叫料站点',
        'DeliveryStationCode': '送货站点编号', 'DeliveryStationName': '送货站点',
        'CallOrderRemark': '叫料说明',
        'StatusName': '状态',
        'TaskIssueTime': '下发时间', 'TaskFinishTime': '完成时间',
        'IsManualHandling': '是否人工处理', 'ManualHandlingUser': '人工处理人',
        'ManualHandlingRemark': '人工处理说明', 'ManualHandlingTime': '人工处理时间',
        'MaterialCode': '物料编号', 'MaterialName': '物料名称',
        'MaterialClassifyName': '物料分类',
        'CargoTypeName': '物料类型',
        'DataSourceName': '数据来源',
        'CreateBy': '创建者', 'CreateTime': '创建时间', 'ModifyBy': '修改者',
        'ModifyTime': '修改时间',
      },//当前页面列表的表头汉字和属性数组，导出用  
      //搜索框相关 
      commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准" 
      searchValControlStyle: {//设置通用搜索框的长度等样式  
        width: '300px',
      },
      controlStyle: {//设置高级搜索控件的长度等样式  
        width: '250px',
      },
      labelWidth: "120px",//显示Label的宽度 
      drawerSize: "99%",//drawner宽度设置 
      searchFormInputPlaceholder: '请输入叫料任务号/配料任务号/呼叫模式/呼叫站点号/呼叫站点名/创建者/修改者',//要给子搜索组件传递的值 
      searchFormInputAttrs: ['CallOrderTaskNo', 'BurdenWorkNo', 'LesTaskModeName', 'StationCode', 'StationName', 'OperationRemark', 'CreateBy', 'ModifyBy'],//要给子搜索组件传递的属性名 
      formOptions: [
        {
          label: '呼叫模式',
          prop: 'LesTaskMode',
          element: 'el-select',
          options: []
        },
        {
          label: '数据来源',
          prop: 'DataSource',
          element: 'el-select',
          options: []
        },
        {
          label: '呼叫配料任务号',
          prop: 'BurdenWorkNo',
          element: 'el-input',
        },
        {
          label: '实际配料任务号',
          prop: 'BurdenWorkNoActual',
          element: 'el-input',
        },
        {
          label: '呼叫站点号',
          prop: 'StationCode',
          element: 'el-input',
        },
        {
          label: '呼叫站点名',
          prop: 'StationName',
          element: 'el-input',
        },
        {
          label: '工单号',
          prop: 'OrderNo',
          element: 'el-input',
        },
        {
          label: '状态',
          prop: 'Status',
          element: 'el-select',
          options: []
        },
        {
          label: '所属工序',
          prop: 'AllowProcedure',
          element: 'el-select',
          options: [] //先默认为空，再通过请求获取数据
        },
        {
          label: '下发时间',
          prop: 'TaskIssueTime',
          type: 'datetimerange',
          element: 'el-date-picker',
        },
        {
          label: '完成时间',
          prop: 'TaskFinishTime',
          type: 'datetimerange',
          element: 'el-date-picker',
        },
        {
          label: '创建者',
          prop: 'CreateBy',
          element: 'el-input',
        },
        {
          label: '创建时间',
          prop: 'CreateTime',
          type: 'datetimerange',
          element: 'el-date-picker',
        },
        {
          label: '修改者',
          prop: 'ModifyBy',
          element: 'el-input',
        },
        {
          label: '修改时间',
          prop: 'ModifyTime',
          type: 'datetimerange',
          element: 'el-date-picker',
        }
      ],

      isShowOperatorColumn: true,// 是否显示【操作】列,true显示 false不显示 
      tableHeight: window.innerHeight - 180, // 控制表格的高度 

      isAdd: false, // 是否是新增 
      dialogTitle: '',

      filters: {
        name: ''
      },
      CallOrders: [],
      roles: [],
      total: 0,
      buttonList: [],
      currentRow: null,
      page: 1,
      pageSize: 10,
      addLoading: false,
      sels: [],//列表选中列 

      addFormVisible: false,//新增界面是否显示 
      addLoading: false,
      //新增界面数据 
      init_addForm: {
        AllowProcedureName: '',
        StationId: '',
        MaterialCodeItemId: '',
        BurdenWorkNo: '',
        CallOrderRemark: ''
      },
      addForm: {
      },
      getBufferStockListByWorkNoForAlertMsg: '',
      default_FJGXMRPLRWH: '', //风机工序的默认配料任务号
    }
  },
  methods: {
    getEnumberListForTaskStatus () {
      let para = {
        category: 'LesTaskStatusEnum'
      }
      getEnumberList(para).then((res) => {
        //给搜索框中的select控件赋值
        let propValue = "Status";
        this.formOptions = setformOptionsNewValue(this.formOptions, propValue, res.data.response);
        this.$refs.ChildSearchForm.setNewFormOptions(this.formOptions)
      });
    },
    //显示 AGV任务阶段列表
    showDetailForAgvTaskPhaseList (agvTaskNo) {
      this.$refs.ShowAgvTaskPhaseListMethod.showDetailForAgvTaskPhaseList(agvTaskNo)
    },
    //显示 强制完成任务任务 界面 
    handleCoerceFinishAgvTask (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要强制完成任务的一行数据！",
          type: "error"
        });
        return;
      }

      //绑定数据
      this.coerceFinishAgvTaskForm = {}
      this.coerceFinishAgvTaskForm = row
      this.coerceFinishAgvTaskForm.OperationRemark = ''
      this.CoerceFinishAgvTaskVisible = true
    },
    do_handleCoerceFinishAgvTask () {
      debugger
      this.$confirm('确认要强制完成该任务吗?', '提示', {
        type: 'warning'
      }).then(() => {
        this.addLoading = true;
        let para = {
          LesTaskId: this.coerceFinishAgvTaskForm.Id,
          BussinessTableName: 'Les_CallOrder',
          Remark: this.coerceFinishAgvTaskForm.OperationRemark
        };
        coerceFinishAgvTask(para).then((res) => {

          if (util.isEmt.format(res)) {
            this.addLoading = false;
            return;
          }
          this.addLoading = false;
          //NProgress.done(); 
          if (res.data.success) {
            this.$message({
              message: '强制完成AGV任务成功',
              type: 'success'
            });
            this.CoerceFinishAgvTaskVisible = false
            this.coerceFinishAgvTaskForm.OperationRemark = ''
            this.getCallOrders();
          } else {
            this.$message({
              message: res.data.msg,
              type: 'error'
            });
          }
        });
      }).catch(() => {

      });
    },
    //显示 取消AGV任务 界面 
    handleCancelAgvTask (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要取消任务的一行数据！",
          type: "error"
        });
        return;
      }

      //绑定数据
      this.cancelAgvTaskForm = {}
      this.cancelAgvTaskForm = row
      this.cancelAgvTaskForm.OperationRemark = ''
      this.cancelAgvTaskVisible = true
    },
    do_handleCancelAgvTask () {
      this.$confirm('确认要取消任务吗?', '提示', {
        type: 'warning'
      }).then(() => {
        this.addLoading = true;
        let para = {
          LesTaskId: this.cancelAgvTaskForm.Id,
          BussinessTableName: 'Les_CallOrder',
          Remark: this.cancelAgvTaskForm.OperationRemark
        };
        cancelAgvTask(para).then((res) => {

          if (util.isEmt.format(res)) {
            this.addLoading = false;
            return;
          }
          this.addLoading = false;
          //NProgress.done(); 
          if (res.data.success) {
            this.$message({
              message: '取消任务成功',
              type: 'success'
            });
            this.cancelAgvTaskVisible = false
            this.cancelAgvTaskForm.OperationRemark = ''
            this.getCallOrders();
          } else {
            this.$message({
              message: res.data.msg,
              type: 'error'
            });
          }
        });
      }).catch(() => {

      });
    },
    //addForm赋初始值
    setInitAddForm () {
      this.addForm = Object.assign({}, this.init_addForm)
    },
    tableRowClassName ({ row, rowIndex }) {
      //如果已分拣完成
      if (row.IsPickFinish) {
        return 'success-row'
      }
      return ''
    },
    getCodeItemsByCodeForFJGXMRPLRWH () {
      let para = {
        code: 'FJGXMRPLRWH' //风机工序的默认配料任务号
      }
      getCodeItemsByCode(para).then((res) => {
        if (res.data.success) {
          this.default_FJGXMRPLRWH = res.data.response.name
        } else {
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }
      });
    },
    changeBurdenWorkNo (value) {
      //alert(value)
      if (this.addForm.LesTaskMode == 3
        || this.addForm.LesTaskMode == 4
        || this.addForm.LesTaskMode == 5) {
        //this._getBufferStockListByWorkNoForAlert()
      }
    },
    //选择 呼叫模式
    changeLesTaskMode (value) {
      // 余料或空托回库 = 1,
      // 呼叫物料 = 2,
      // 余料或空托回库并呼叫物料 = 3
      if (value == 1) {
        this.isNeedSetDisabled = false
        this.isShowBurdenWorkNo = false
        this.addForm.MaterialCodeItemId = ''
        this.addForm.BurdenWorkNo = ' '//赋值一个空格
      }
      else {
        this.isNeedSetDisabled = true
        this.isShowBurdenWorkNo = true
        this.addForm.BurdenWorkNo = ''//赋值空值
        this.restMaterials() //重新获取物料列表
        this.changeAllowProcess(this.currSelectAllowProcess)
      }
    },
    //显示 LES任务阶段列表
    showDetailForTaskPhaseList (selRow) {
      this.$refs.ShowTaskPhaseListMethod.showDetailForTaskPhaseList(selRow.CallOrderTaskNo)
    },
    //获取呼叫模式
    getEnumberListForCallOrderStatusEnum () {
      let para = {
        category: 'LesTaskModeEnum'
      }
      getEnumberList(para).then((res) => {
        this.lesTaskModeItems = res.data.response
        //给搜索框中的select控件赋值
        let propValue = "LesTaskMode";
        this.formOptions = setformOptionsNewValue(this.formOptions, propValue, res.data.response);
        this.$refs.ChildSearchForm.setNewFormOptions(this.formOptions)
      });

      //同时获取 数据来源
      para = {
        category: 'DataSourceEnum'
      }
      getEnumberList(para).then((res) => {
        //给搜索框中的select控件赋值
        let propValue = "DataSource";
        this.formOptions = setformOptionsNewValue(this.formOptions, propValue, res.data.response);
        this.$refs.ChildSearchForm.setNewFormOptions(this.formOptions)
      });
    },
    // 查询工序列表，使用权限，通过代码集编码获取代码项集合
    _getCodeItemsBySetCodeForGX: function () {
      let para = {
      }
      getCodeItemsBySetCodeForGX(para).then(res => {
        if (res.data.success) {
          this.allCodeSetsForAllowProcess = res.data.response


          //给搜索框中的select控件赋值
          let propValue = "AllowProcedure";
          this.formOptions = setformOptionsNewValue(this.formOptions, propValue, this.allCodeSetsForAllowProcess);
          this.$refs.ChildSearchForm.setNewFormOptions(this.formOptions)
        }
        else {
          this.$notify.error({
            title: '错误',
            message: res.msg
          })
        }
      }).catch(err => {
        console.log(err)
      })
    },
    //选择 工序
    changeAllowProcess: function (selVal) {
      this.addForm.StationId = ''
      this.addForm.MaterialCodeItemId = ''

      this.addForm.AllowProcedureName = selVal.name
      this.currSelectAllowProcess = selVal
      this.reLoadStationList(selVal.value)
      this.restMaterials()
    },
    //选择物料
    changeMaterialCodeItemId: function (selVal) {
      //如果选择的是 风机工序下的某些物料，4
      let isUserDefault = validateIsUseDefaultBurdenWorkNo(selVal.name)
      if (isUserDefault) {
        this.addForm.BurdenWorkNo = this.default_FJGXMRPLRWH
      } else {
        this.addForm.BurdenWorkNo = ''
      }
      this._getBufferStockCollectGroupMaterialList(false)
      //顺便执行
      this.changeBurdenWorkNo()
    },
    /*
     * 重新加载站点列表
     * procedureCodeItemId：工序码表的ID
    */
    reLoadStationList (procedureCodeItemId) {
      this.stationList = []
      this._getStationsByProcedure(procedureCodeItemId)
    },
    // 根据工序获取工序下的站点列表
    _getStationsByProcedure: function (procedureCodeItemId) {
      let para = {
        procedureCodeItemId: procedureCodeItemId,
        isNeedSetDisabled: this.isNeedSetDisabled
      }
      getStationsByProcedure(para).then(res => {
        if (res.data.success) {
          this.stationList = res.data.response
          if (this.stationList.length == 1 && this.stationList[0].Disabled == false) {//验证如果只有一个站点并且是可用的，就默认选中
            var item = this.stationList[0]
            this.addForm.StationId = item.Id
            this.changeStation(item) //主动触发事件
          }
        }
        else {
          this.$notify.error({
            title: '错误',
            message: res.msg
          })
        }
      }).catch(err => {
        console.log(err)
      })
    },
    // 重置 物料大类清单
    restMaterials: function () {
      this.addForm.MaterialCodeItemId = ''
      this.materialNoCodeItems = []
    },
    // 根据工序的码表编号和站点展示 可以看到的物料列表
    changeStation: function (selVal) {
      let stationId = selVal.Id
      if (stationId == 0 || stationId == '') {
        this.$message({
          message: '请选择站点',
          type: 'warning'
        });
        return
      }
      if (this.addForm.AllowProcedureName == '') {
        this.$message({
          message: '请选择工序',
          type: 'warning'
        });
        return
      }
      this.materialNoCodeItems = []
      let para = {
        processSetCode: this.addForm.AllowProcedureName, // 工序
        stationId: stationId // 工序
      }
      getMaterialsCodeItemsByProcessAndStation(para).then(res => {
        if (res.data.success) {
          this._doMaterials(res.data.response)
        }
        else {
          this.$notify.error({
            title: '错误',
            message: res.msg
          })
        }
      }).catch(err => {
        console.log(err)
      })
    },
    _doMaterials (data) {
      this.materialNoCodeItems = data

      if (this.materialNoCodeItems.length == 1) {//验证如果只有一个物料，就默认选中
        var item = this.materialNoCodeItems[0]
        this.addForm.MaterialCodeItemId = item.value
        this.changeMaterialCodeItemId(item) //主动调用物料改变事件
      }
    },
    //是否显示某个操作按钮 
    isShowOperatorButton (flag) {
      return isShowOperatorButtonCommon(this.isShowOperatorColumn, flag, this.buttonList)
    },
    // 设置索引 
    indexMethod (index) {
      return (this.page - 1) * this.pageSize + 1 + index
    },
    // 时间格式化 
    formatterDateTime (row, column, cellValue, index) {
      if (cellValue === null || cellValue === '') {
        return ''
      }
      var NewDtime = new Date(Date.parse(cellValue))
      return formatDate(NewDtime, 'yyyy-MM-dd hh:mm:ss')
    },
    selectCurrentRow (val) {
      this.currentRow = val;
    },
    callFunction (item) {
      this.filters = {
        name: item.search
      };
      this[item.Func].apply(this, item);
    },
    handleCurrentChange (val) {
      this.page = val;
      this.getCallOrders();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getCallOrders();
    },
    /** 
     * 获取请求参数 
     * flag:标记，1代表普通分页查询，2代表不分页，获取全部数据 
     */
    getPostParam (flag) {
      let para = Object.assign({}, this.$refs.ChildSearchForm.getFormData());
      para.page = this.page
      para.pageSize = this.pageSize
      if (flag === '2') { // 全部导出 
        para.page = 1
        para.pageSize = 100000
      }
      return para
    },
    _getCallOrders () {
      this.page = 1
      this.getCallOrders()
    },
    //获取列表 
    getCallOrders (formValue) {
      //this.total = 0 
      //this.CallOrders = null 
      let para = this.getPostParam('1');
      this.addLoading = true;
      getCallOrderListPage(para).then((res) => {
        if (res.data.success) {
          this.total = res.data.response.dataCount;
          this.CallOrders = res.data.response.data;
        } else {
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }
        this.addLoading = false;
      });
    },
    //获取全部列表 
    exportAllData () {
      let para = this.getPostParam('2');
      this.addLoading = true;
      getCallOrderListPage(para).then((res) => {
        var allData = res.data.response.data;
        this.$refs.cmToolbarExport.export2Excel(allData);
        this.addLoading = false;
      });
    },
    //删除 
    handleDel (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要删除的一行数据！",
          type: "error"
        });
        return;
      }
      this.$confirm('确认删除该记录吗?', '提示', {
        type: 'warning'
      }).then(() => {
        this.addLoading = true;
        let para = { id: row.Id };
        removeCallOrder(para).then((res) => {

          if (util.isEmt.format(res)) {
            this.addLoading = false;
            return;
          }
          this.addLoading = false;
          //NProgress.done(); 
          if (res.data.success) {
            this.$message({
              message: '删除成功',
              type: 'success'
            });

          } else {
            this.$message({
              message: res.data.msg,
              type: 'error'
            });
          }

          this.getCallOrders();
        });
      }).catch(() => {

      });
    },
    //显示编辑界面 
    handleEdit (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要编辑的一行数据！",
          type: "error"
        });

        return;
      }
      this.isAdd = false
      this.dialogTitle = '编辑'
      this.addFormVisible = true;
      this.addForm = Object.assign({}, row);
    },
    //显示新增界面 
    handleAdd () {
      this.isAdd = true
      this.dialogTitle = '新增'
      this.addFormVisible = true
      this.setInitAddForm()
    },
    //取消操作
    cancelHandle (done) {
      this.setInitAddForm()
      this.addFormVisible = false

      done()
    },
    // 更新数据 
    saveData (formName) {
      this.submitDisabled = true
      this.$refs.addForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认提交吗？', '提示', {}).then(() => {
            this.addLoading = true;
            let para = Object.assign({}, this.addForm);
            this.validate_saveData(para)

          });
        }
      });
    },
    // 验证数据 
    validate_saveData (para) {
      validateCreateCallOrder(para).then((res) => {
        if (util.isEmt.format(res)) {
          this.addLoading = false;
          return;
        }
        if (res.data.success) {
          this.do_saveData(para)
        }
        else {
          this.addLoading = false;
          if (res.data.response == 'ALERT') {
            var msg = '<strong><span style="color:red;font-size:28px;">'
              + res.data.msg + '</span></i>  <span style="color:green;font-size:20px;">您确认要继续提交吗？</span></span></strong>';
            this.$confirm(msg, '提示', { dangerouslyUseHTMLString: true }).then(() => {
              this.do_saveData(para)
            });
          } else {
            this.addLoading = false;
            this.$message({
              message: res.data.msg,
              type: 'error'
            });
          }
        }
      });
    },
    // 更新数据 
    do_saveData (para) {
      this.addLoading = true
      addCallOrder(para).then((res) => {
        if (util.isEmt.format(res)) {
          this.addLoading = false;
          return;
        }
        if (res.data.success) {
          this.addLoading = false;
          //NProgress.done(); 
          this.$message({
            message: res.data.msg,
            type: 'success'
          });
          this.$refs['addForm'].resetFields();
          this.addFormVisible = false;
          this.getCallOrders();
        }
        else {
          this.addLoading = false;
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }

      });
    },

    _getBufferStockListByWorkNoForAlert () {
      if (this.addForm.BurdenWorkNo === '') {
        // this.$message({
        //   message: '请输入配料任务号！',
        //   type: 'warning'
        // });
        return
      }
      if (this.addForm.MaterialCodeItemId === '') {
        // this.$message({
        //   message: '请选择物料！',
        //   type: 'warning'
        // });
        return
      }
      var para = {
        BurdenWorkNo: this.addForm.BurdenWorkNo,
        MaterialCodeItemId: this.addForm.MaterialCodeItemId
      }
      this.getBufferStockListByWorkNoForAlertMsg = ''
      getBufferStockListByWorkNoForAlert(para).then((res) => {
        if (res.data.success) {
          this.getBufferStockListByWorkNoForAlertMsg = res.data.msg
        }
        else {
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }
      });
    },

    handleRereshTableList () {
      this._getBufferStockCollectGroupMaterialList(true)
    },
    //获取分组查询立库缓存区的物料列表
    _getBufferStockCollectGroupMaterialList (isShowAlert) {
      if (this.addForm.MaterialCodeItemId === '') {
        // this.$message({
        //   message: '请选择物料！',
        //   type: 'warning'
        // });
        return
      }
      var para = {
        MaterialCodeItemId: this.addForm.MaterialCodeItemId,
        AllowProcedure: this.addForm.AllowProcedure
      }
      getBufferStockCollectGroupMaterialList(para).then((res) => {
        if (res.data.success) {
          this.tableData_QueryBufferCollect = res.data.response
          if (isShowAlert) {
            this.$message({
              message: "刷新成功!",
              type: 'success'
            });
          }
        }
        else {
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }
      });
    },
    selsChange: function (sels) {
      this.sels = sels;
    },
    //导出 
    handleExport () {
      this.currentPageData = this.CallOrders
      this.$refs.cmToolbarExport.showPrintPage()
    },
    // 叫料
    handleCallMaterial () {
      this.isAdd = true
      this.dialogTitle = '缺料呼叫'
      this.addFormVisible = true
      this.tableData_QueryBufferCollect = []
      if (this.allCodeSetsForAllowProcess.length == 1) {//验证如果只有一个工序，就默认选中
        var item = this.allCodeSetsForAllowProcess[0]
        this.addForm.AllowProcedure = item.value
        this.changeAllowProcess(item)
      }
    },
  },
  mounted () {
    if (isMobile()) {
      this.drawerSize = "99%"//drawner宽度设置 
      this.addFormSpan = 24
    } else {
      this.drawerSize = "550px"//drawner宽度设置 
    }
    this.getEnumberListForTaskStatus()
    this.setInitAddForm()
    this.getCodeItemsByCodeForFJGXMRPLRWH()
    this.getEnumberListForCallOrderStatusEnum()
    this._getCodeItemsBySetCodeForGX()
    this.getCallOrders();

    let routers = window.localStorage.router
      ? JSON.parse(window.localStorage.router)
      : [];
    this.buttonList = getButtonList(this.$route.path, routers);
    let isShow = isNeedShowOperatorColumn(this.buttonList);//控制是否要显示【操作】列 
    this.isShowOperatorColumn = isShow;
  }
}

</script> 
 <style>
/*设置优先级!important 并且指向那一个td*/
.el-table .success-row > td {
  background-color: #ace2b3 !important;
}

.el-table .warning-row > td {
  background-color: #dc8a8a !important;
}
</style>
<style lang="scss" scoped>
</style> 
