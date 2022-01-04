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
        <SearchForm :formOptions="h_formOptions"
                    :commonSearchOptionSet="commonSearchOptionSet"
                    :drawerSize="drawerSize"
                    :labelWidth="labelWidth"
                    :controlStyle="controlStyle"
                    :searchValControlStyle="searchValControlStyle"
                    :searchFormInputPlaceholder="searchFormInputPlaceholder"
                    :searchFormInputAttrs="searchFormInputAttrs"
                    ref="ChildSearchForm"
                    @onSearch="_getStockInfos" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="StockInfos"
              :max-height="tableHeight"
              highlight-current-row
              size="small"
              :height="tableHeight"
              @current-change="selectCurrentRow"
              v-loading="listLoading"
              @selection-change="selsChange"
              style="width: 99%;">
      <el-table-column type="index"
                       :index="indexMethod"
                       label="#"
                       align="center"
                       width="50">
      </el-table-column>
      <el-table-column prop="PickInStockTaskNo"
                       label="分拣入库任务号"
                       width="160"
                       align="left">
        <template slot-scope="scope">
          <a @click="showDetailForTaskPhaseList(scope.row)"
             target="_blank"
             style="text-decoration:underline;cursor: pointer;color:blue;font-weight:bold;">
            {{scope.row.PickInStockTaskNo}}
          </a>
        </template>
      </el-table-column>
      <el-table-column prop="AgvTaskNo"
                       label="AGV任务号"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="BurdenWorkNo"
                       label="配料任务号"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="MaterialName"
                       label="物料"
                       width="120"
                       align="center">
      </el-table-column>
      <el-table-column prop="LesTaskStatusName"
                       label="任务状态"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="StationCode"
                       label="分拣点编号"
                       align="center">
      </el-table-column>
      <el-table-column prop="StationName"
                       label="分拣点名称"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="ToStationCode"
                       label="缓存点编号"
                       align="center">
      </el-table-column>
      <el-table-column prop="ToStationName"
                       label="缓存点名称"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="IsPickFinish"
                       label="是否分拣完成"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.IsPickFinish  ? 'success' : 'danger'"
                  disable-transitions>{{scope.row.IsPickFinish ? "是":"否"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="IsPersonPickInStore"
                       label="是否人工分拣入库"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.IsPersonPickInStore  ? 'success' : 'danger'"
                  disable-transitions>{{scope.row.IsPersonPickInStore ? "是":"否"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="Remark"
                       label="备注"
                       align="center">
      </el-table-column>
      <el-table-column prop="MaterialCode"
                       label="物料编号"
                       width="120"
                       align="center">
      </el-table-column>
      <el-table-column prop="CargoTypeName"
                       label="物料类型"
                       align="center">
      </el-table-column>
      <!-- <el-table-column prop="StoreAreaName"
                       label="库存区域"
                       align="center">
      </el-table-column> -->
      <!-- <el-table-column prop="AllowLineName"
                       label="所属产线"
                       align="center">
      </el-table-column> -->
      <el-table-column prop="AllowProcedureName"
                       label="所属工序"
                       align="center">
      </el-table-column>
      <!-- <el-table-column prop="PlaceTypeName"
                       label="工位类型"
                       align="center">
      </el-table-column> -->
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
      <el-table-column prop="InStockDate"
                       label="入缓存库时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
      <el-table-column prop="PreMatResult"
                       label="备料通知结果"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.PreMatResult==='备料取消'  ? 'danger' : 'success'"
                  disable-transitions>{{scope.row.PreMatResult}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="PreMatResultTime"
                       label="备料通知时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
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
    <!--  分拣入库 界面-->
    <el-dialog :title="dialogTitle"
               :visible.sync="addFormVisible"
               v-model="addFormVisible"
               :close-on-click-modal="false">
      <el-form :model="addForm"
               size="small"
               :rules="rules"
               label-width="120px"
               ref="addForm">
        <el-form-item label="入库方式"
                      prop="IsPersonPickInStore"
                      :rules="[{ required: true, message: '入库方式不能为空'}]">
          <el-switch v-model="addForm.IsPersonPickInStore"
                     @change="_getEmptyStationList"
                     active-text="人工入库"
                     inactive-text="AGV入库">
          </el-switch>
        </el-form-item>
        <el-form-item label="站点"
                      prop="StationId"
                      :rules="[{ required: true, message: '站点不能为空'}]">
          <el-select v-model="addForm.StationId"
                     clearable
                     filterable
                     placeholder="请选择">
            <el-option v-for="item in stationIdCodeItems"
                       :key="item.Id"
                       :label="item.StationName"
                       :disabled="item.Disabled"
                       :value="item.Id">
              <span style="float: left">{{ item.StationName }}</span>
              <span style="float: right; color: #8492a6; font-size: 13px">{{ item.DisabledMsg }}</span>
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="货物类型"
                      v-if="false"
                      prop="CargoType"
                      :rules="[{ required: true, message: '货物类型不能为空'}]">
          <el-select v-model="addForm.CargoType"
                     @change="changeCargoType"
                     clearable>
            <el-option v-for="item in cargoTypeData"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="所属工序"
                      prop="AllowProcedure"
                      :rules="[{ required: true, message: '所属工序不能为空'}]">
          <el-select v-model="addForm.AllowProcedure"
                     clearable
                     placeholder="请选择">
            <el-option v-for="item in allCodeSetsForAllowProcess"
                       @click.native="changeAllowProcess(item)"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="物料"
                      prop="MaterialCodeItemId"
                      :rules="[{ required: true, message: '物料不能为空'}]">
          <el-select v-model="addForm.MaterialCodeItemId"
                     clearable
                     placeholder="请选择">
            <el-option v-for="item in materialNoCodeItems"
                       :key="item.value"
                       :label="item.label"
                       @click.native="changeMaterialCodeItemId(item)"
                       :value="item.value">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="配料任务号"
                      v-if="!isInStoreForSalver"
                      prop="BurdenWorkNo">
          <el-input placeholder="请输入配料任务号！"
                    aria-placeholder=""
                    v-model="addForm.BurdenWorkNo"></el-input>
          <el-tag type="success">如果有多个任务号,中间用英文逗号(,)间隔输入</el-tag>
        </el-form-item>
        <el-form-item label="是否分拣完成"
                      prop="IsPickFinish"
                      :rules="[{ required: true, message: '是否分拣完成不能为空'}]">
          <el-switch v-model="addForm.IsPickFinish"
                     active-text="已分拣完成"
                     inactive-text="未分拣完成">
          </el-switch>
        </el-form-item>
        <el-form-item label="备注"
                      prop="Remark ">
          <el-input v-model="addForm.Remark"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="CancelSaveData"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="SaveData"
                   :loading="addLoading"
                   icon="fa fa-send">提交</el-button>
      </div>
    </el-dialog>
    <!--更新是否禁用界面-->
    <el-dialog title="更新是否分拣完成"
               :visible.sync="updateIsPickFinishVisible"
               :close-on-click-modal="false">
      <el-form :model="updateIsPickFinishForm"
               size="small"
               label-width="80px"
               ref="updateIsPickFinishForm">
        <el-form-item label="配料任务号"
                      prop="BurdenWorkNo">
          <el-input v-model="updateIsPickFinishForm.BurdenWorkNo"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="站点名称"
                      prop="StationName">
          <el-input v-model="updateIsPickFinishForm.StationName"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label=""
                      prop="IsPickFinish">
          <el-switch v-model="updateIsPickFinishForm.IsPickFinish"
                     active-text="已分拣完成"
                     inactive-text="未分拣完成"
                     active-color="#13ce66"
                     inactive-color="#ff4949">
          </el-switch>
        </el-form-item>
        <el-form-item label="操作说明"
                      prop="OperationRemark"
                      :rules="[{ required: true, message: '操作说明不能为空'}]">
          <el-input v-model="updateIsPickFinishForm.OperationRemark"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="updateIsPickFinishVisible = false"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="do_handleIsPickFinish"
                   :loading="addLoading"
                   icon="fa fa-send">提交</el-button>
      </div>
    </el-dialog>
    <!--查看界面-->
    <el-dialog title="查看"
               :visible.sync="viewDetailVisible"
               :before-close="cancelHandle">
      <el-form :model="viewDetailForm"
               size="small"
               disabled="true"
               label-width="120px"
               ref="viewDetailForm">
        <el-form-item label="配料任务号"
                      prop="BurdenWorkNo"
                      :rules="[{ required: true, message: '配料任务号不能为空'}]">
          <el-input v-model="viewDetailForm.BurdenWorkNo"></el-input>
        </el-form-item>
        <el-form-item label="是否分拣完成"
                      prop="IsPickFinish"
                      :rules="[{ required: true, message: '是否分拣完成不能为空'}]">
          <el-switch v-model="viewDetailForm.IsPickFinish"></el-switch>
        </el-form-item>
        <el-form-item label="所属工序"
                      prop="AllowProcedure"
                      :rules="[{ required: true, message: '所属工序不能为空'}]">
          <el-input v-model="viewDetailForm.AllowProcedureName"></el-input>
        </el-form-item>
        <el-form-item label="站点"
                      prop="StationName"
                      :rules="[{ required: true, message: '站点ID不能为空'}]">
          <el-input v-model="viewDetailForm.StationName"></el-input>
        </el-form-item>
        <el-form-item label="货物类型"
                      prop="CargoTypeName"
                      :rules="[{ required: true, message: '货物类型不能为空'}]">
          <el-input v-model="viewDetailForm.CargoTypeName"></el-input>
        </el-form-item>
        <el-form-item label="物料名称"
                      prop="MaterialCodeItemId">
          <el-input v-model="viewDetailForm.MaterialName"></el-input>
        </el-form-item>
        <el-form-item label="备注"
                      prop="Remark">
          <el-input v-model="viewDetailForm.Remark"></el-input>
        </el-form-item>
        <el-form-item label="货物数量"
                      prop="CargoNum">
          <el-input-number v-model="viewDetailForm.CargoNum"></el-input-number>
        </el-form-item>
        <el-form-item label="单位"
                      prop="Unit">
          <el-input v-model="viewDetailForm.Unit"></el-input>
        </el-form-item>
        <el-form-item label="重量"
                      prop="CargoWeight">
          <el-input-number v-model="viewDetailForm.CargoWeight"></el-input-number>
        </el-form-item>
        <el-form-item label="入缓存库时间"
                      prop="InStockDate">
          <el-date-picker placeholder="选择日期时间"
                          type="datetime"
                          v-model="viewDetailForm.InStockDate"></el-date-picker>
        </el-form-item>
        <el-form-item label="操作说明"
                      prop="OperationRemark">
          <el-input v-model="viewDetailForm.OperationRemark"></el-input>
        </el-form-item>
        <el-form-item label="创建者"
                      prop="CreateBy">
          <el-input v-model="viewDetailForm.CreateBy"></el-input>
        </el-form-item>
        <el-form-item label="创建时间"
                      prop="CreateTime">
          <el-date-picker placeholder="选择日期时间"
                          type="datetime"
                          v-model="viewDetailForm.CreateTime"></el-date-picker>
        </el-form-item>
        <el-form-item label="修改者"
                      prop="ModifyBy">
          <el-input v-model="viewDetailForm.ModifyBy"></el-input>
        </el-form-item>
        <el-form-item label="修改时间"
                      prop="ModifyTime">
          <el-date-picker placeholder="选择日期时间"
                          type="datetime"
                          v-model="viewDetailForm.ModifyTime"></el-date-picker>
        </el-form-item>

      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="closeViewHandle"
                   icon="fa fa-power-off">关闭</el-button>
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
        <el-form-item label="分拣入库任务号"
                      prop="PickInStockTaskNo">
          <el-input v-model="cancelAgvTaskForm.PickInStockTaskNo"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="配料任务号"
                      prop="BurdenWorkNo">
          <el-input v-model="cancelAgvTaskForm.BurdenWorkNo"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="物料名称"
                      prop="MaterialName">
          <el-input v-model="cancelAgvTaskForm.MaterialName"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="LesTaskStatusName">
          <el-input v-model="cancelAgvTaskForm.LesTaskStatusName"
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
        <el-form-item label="分拣入库任务号"
                      prop="PickInStockTaskNo">
          <el-input v-model="coerceFinishAgvTaskForm.PickInStockTaskNo"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="配料任务号"
                      prop="BurdenWorkNo">
          <el-input v-model="coerceFinishAgvTaskForm.BurdenWorkNo"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="物料名称"
                      prop="MaterialName">
          <el-input v-model="coerceFinishAgvTaskForm.MaterialName"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="LesTaskStatusName">
          <el-input v-model="coerceFinishAgvTaskForm.LesTaskStatusName"
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
    <!--详细界面（任务阶段列表）-->
    <ShowTaskPhaseList ref="ShowTaskPhaseListMethod"></ShowTaskPhaseList>
    <!-- 导出组件 -->
    <ToolbarExport ref="cmToolbarExport"
                   :exportFileName="exportFileName"
                   :currentPageData="currentPageData"
                   :exportColumnHeader="exportColumnHeader" />
  </section>
</template> 
 
<script> 
import util from '../../../util/date'
import { coerceFinishAgvTask, cancelAgvTask, getCodeItemsBySetCodeForGX, getCodeItemsByCode, getEnumberList, getEmptyStationList, updateIsPickFinish, getAllStationList, getCodeItemsBySetCode, getPickInStockTaskListPage, removeStockInfo, editStockInfo, addStockInfo } from '../../api/api';
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";
import SearchForm from "../../components/SearchForm";
import ToolbarExport from "../../components/ToolbarExport";
import ShowTaskPhaseList from "./LESComponents/ShowTaskPhaseList";
import { formatDate, setformOptionsNewValue } from '../../../util/tools'
import { validateIsUseDefaultBurdenWorkNo, validateNewBurdenWorkNoForWhole } from '../../../util/LesUtil'
import { isShowOperatorButtonCommon, isNeedShowOperatorColumn, isMobile } from '../../../util/common'


export default {
  components: { ShowTaskPhaseList, Toolbar, SearchForm, ToolbarExport },//注册子组件 
  data () {
    return {
      CoerceFinishAgvTaskVisible: false,
      coerceFinishAgvTaskForm: [],
      cancelAgvTaskVisible: false,
      cancelAgvTaskForm: [],
      rules: {
        BurdenWorkNo: [{
          validator: (rule, newBurdenWorkNo, callback) => {
            var msg = validateNewBurdenWorkNoForWhole(this.addForm.MaterialCodeItemId, this.addForm.AllowProcedureName, newBurdenWorkNo
              , this.addForm.MaterialCodeItemCode, this.addForm.MaterialCodeItemName)
            if (msg != '') {
              callback(new Error(msg));
            }
            callback()
          }, trigger: 'blur'
        }]
      },

      updateIsPickFinishVisible: false,
      updateIsPickFinishForm: {},

      stationIdCodeItems: [],
      materialNoCodeItems: [],
      allCodeSetsForAllowProcess: [],
      isInStoreForSalver: false,//是否是录入的是托盘
      //货物类型
      cargoTypeData: [
        {
          label: '物料',
          value: '1'
        },
        {
          label: '托盘',
          value: '2'
        }
      ],
      //导出组件相关 
      exportFileName: '库存信息信息',//要导出的文件名 
      currentPageData: [],//当前页面的列表数据 
      exportColumnHeader: {
        'PickInStockTaskNo': '分拣任务号', 'LesTaskStatusName': '状态',
        'BurdenWorkNo': '配料任务号',
        'PreMatResult': '备料通知结果', 'PreMatResultTime': '备料通知时间',
        'CreateBy': '创建者', 'CreateTime': '创建时间', 'ModifyBy': '修改者',
        'ModifyTime': '修改时间'
      },//当前页面列表的表头汉字和属性数组，导出用 
      //搜索框相关 
      commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准" 
      searchValControlStyle: {//设置通用搜索框的长度等样式  
        width: '300px',
      },
      controlStyle: {//设置高级搜索控件的长度等样式  
        width: '300px',
      },
      labelWidth: "90px",//显示Label的宽度 
      drawerSize: "550px",//drawner宽度设置 
      searchFormInputPlaceholder: '请输入缓存点编号/分拣入库任务号/配料任务号/物料',//要给子搜索组件传递的值 
      searchFormInputAttrs: ['ToStationCode', 'PickInStockTaskNo', 'BurdenWorkNo', 'MaterialName'],//要给子搜索组件传递的属性名 
      h_formOptions: [
        {
          label: '状态',
          prop: 'LesTaskStatus',
          element: 'el-select',
          options: []
        },
        {
          label: '所属工序',
          prop: 'AllowProcedure',
          element: 'el-select',
          options: []
        },
        {
          label: '物料编号',
          prop: 'MaterialCode',
          element: 'el-select',
          options: []
        },
        {
          label: '物料名称',
          prop: 'MaterialName',
          element: 'el-select',
          options: []
        },
        {
          label: '分拣点编号',
          prop: 'StationCode',
          element: 'el-input',
        },
        {
          label: '分拣点名称',
          prop: 'StationName',
          element: 'el-input',
        },
        {
          label: '缓存点编号',
          prop: 'ToStationCode',
          element: 'el-input',
        },
        {
          label: '缓存点名称',
          prop: 'ToStationName',
          element: 'el-input',
        },
        {
          label: '入库时间',
          prop: 'InStockDate',
          type: 'datetimerange',
          element: 'el-date-picker',
        },
        {
          label: '备料通知结果',
          prop: 'PreMatResult',
          element: 'el-input',
        },
        {
          label: '备料通知时间',
          prop: 'PreMatResultTime',
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

      //查看有关
      viewDetailVisible: false,
      viewDetailForm: {},

      filters: {
        name: ''
      },
      StockInfos: [],
      roles: [],
      total: 0,
      buttonList: [],
      currentRow: null,
      page: 1,
      pageSize: 10,
      listLoading: false,
      sels: [],//列表选中列 

      addFormVisible: false,//新增界面是否显示 
      addLoading: false,
      //新增界面数据 
      init_addForm: {
        AllowProcedureName: '',
        MaterialCodeItemId: '',
        MaterialCodeItemCode: '',
        MaterialCodeItemName: '',
        CargoType: '1', //默认为物料
        MaterialCode: '',
        BurdenWorkNo: '',
        IsPickFinish: false,
        IsPersonPickInStore: false,
        StationId: ''
      },
      addForm: {

      },
      detailShow: false,
      taskPhaseList: [],
      default_FJGXMRPLRWH: '', //风机工序的默认配料任务号
    }
  },
  methods: {
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
      this.$confirm('确认要强制完成该任务吗?', '提示', {
        type: 'warning'
      }).then(() => {
        this.addLoading = true;
        let para = {
          LesTaskId: this.coerceFinishAgvTaskForm.Id,
          BussinessTableName: 'Les_PickInStockTask',
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
          BussinessTableName: 'Les_PickInStockTask',
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
            this.getStockInfos();
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
    getEnumberListForLesTaskStatusEnum () {
      let para = {
        category: 'LesTaskStatusEnum'
      }
      getEnumberList(para).then((res) => {
        //给搜索框中的select控件赋值
        let propValue = "LesTaskStatus";
        this.h_formOptions = setformOptionsNewValue(this.h_formOptions, propValue, res.data.response);
        this.$refs.ChildSearchForm.setNewFormOptions(this.h_formOptions)
      });
    },
    //显示 LES任务阶段列表
    showDetailForTaskPhaseList (selRow) {
      this.$refs.ShowTaskPhaseListMethod.showDetailForTaskPhaseList(selRow.PickInStockTaskNo)
    },
    //显示 更新是否分拣完成界面 
    handleIsPickFinish (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要修改是否分拣完成的一行数据！",
          type: "error"
        });
        return;
      }

      //绑定数据
      this.updateIsPickFinishForm = {}
      this.updateIsPickFinishForm = row
      this.updateIsPickFinishForm.OperationRemark = ''
      this.updateIsPickFinishForm.IsPickFinish = row.IsPickFinish
      this.updateIsPickFinishVisible = true
    },
    do_handleIsPickFinish () {
      this.$confirm('确认修改是否分拣完成吗?', '提示', {
        type: 'warning'
      }).then(() => {
        this.listLoading = true;
        let para = {
          Id: this.updateIsPickFinishForm.Id,
          IsPickFinish: this.updateIsPickFinishForm.IsPickFinish,
          OperationRemark: this.updateIsPickFinishForm.OperationRemark
        };
        updateIsPickFinish(para).then((res) => {

          if (util.isEmt.format(res)) {
            this.listLoading = false;
            return;
          }
          this.listLoading = false;
          //NProgress.done(); 
          if (res.data.success) {
            this.$message({
              message: '修改是否分拣完成成功',
              type: 'success'
            });

          } else {
            this.$message({
              message: res.data.msg,
              type: 'error'
            });
          }
          this.updateIsPickFinishVisible = false
          this.updateIsPickFinishForm.OperationRemark = ''
          this.getStations();
        });
      }).catch(() => {

      });
    },

    changeAllowProcess: function (selVal) {
      this.addForm.MaterialCodeItemId = ''
      this.addForm.MaterialCodeItemCode = ''
      this.addForm.MaterialCodeItemName = ''
      this.materialNoCodeItems = []
      this.addForm.AllowProcedureName = selVal.name
      this.getMaterials(selVal.name)
    },
    //选择物料
    changeMaterialCodeItemId: function (selVal) {
      //保存值
      this.addForm.MaterialCodeItemCode = selVal.name
      this.addForm.MaterialCodeItemName = selVal.label
      //如果选择的是 风机工序下的某些物料，
      let isUserDefault = validateIsUseDefaultBurdenWorkNo(selVal.name)
      if (isUserDefault) {
        this.addForm.BurdenWorkNo = this.default_FJGXMRPLRWH
      } else {
        this.addForm.BurdenWorkNo = ''
      }
    },
    // 获取空库位的列表
    _getEmptyStationList: function () {
      this.addForm.StationId = ''
      let para = {
        isNeedStationPermission: false,
        StoreAreaCode: 'FJQ', // 立库分拣区
        InStoreSceneEnum: 1 // 1是立库分拣完成后入缓存库场景
      }
      if (this.addForm.IsPersonPickInStore) {
        //人工入库
        para.StoreAreaCode = 'StoreBuffer' //立库缓存区
        getEmptyStationList(para).then(res => {
          if (res.data.success) {
            this.stationIdCodeItems = res.data.response
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
      } else {
        getAllStationList(para).then(res => {
          if (res.data.success) {
            this.stationIdCodeItems = res.data.response
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
      }
    },
    // 获取指定工序下的物料大类清单
    getMaterials: function (processName) {
      let para = {
        setCode: processName // 工序
      }
      getCodeItemsBySetCode(para).then(res => {
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
      }
    },
    //查询工序列表，使用权限，通过代码集编码获取代码项集合 
    getCodeItemsBySetCodeForGX () {
      let para = {
      }
      getCodeItemsBySetCodeForGX(para).then((res) => {
        this.allCodeSetsForAllowProcess = res.data.response;
        //给搜索框中的select控件赋值
        let propValue = "AllowProcedure";
        this.h_formOptions = setformOptionsNewValue(this.h_formOptions, propValue, this.allCodeSetsForAllowProcess);
        this.$refs.ChildSearchForm.setNewFormOptions(this.h_formOptions)
      });
    },
    changeCargoType () {
      if (this.addForm.CargoType == '1') {
        this.isInStoreForSalver = false;
      } else {
        //选择录入托盘
        this.isInStoreForSalver = true;
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
      this.getStockInfos();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getStockInfos();
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
    _getStockInfos () {
      this.page = 1
      this.getStockInfos()
    },
    //获取列表 
    getStockInfos (formValue) {
      //this.total = 0 
      //this.StockInfos = null 
      let para = this.getPostParam('1');
      this.listLoading = true;
      getPickInStockTaskListPage(para).then((res) => {
        if (res.data.success) {
          this.total = res.data.response.dataCount;
          this.StockInfos = res.data.response.data;
        } else {
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }
        this.listLoading = false;
      });
    },
    //获取全部列表 
    exportAllData () {
      let para = this.getPostParam('2');
      this.listLoading = true;
      getPickInStockTaskListPage(para).then((res) => {
        var allData = res.data.response.data;
        this.$refs.cmToolbarExport.export2Excel(allData);
        this.listLoading = false;
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
        this.listLoading = true;
        let para = { id: row.Id };
        removeStockInfo(para).then((res) => {

          if (util.isEmt.format(res)) {
            this.listLoading = false;
            return;
          }
          this.listLoading = false;
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

          this.getStockInfos();
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
      this._getEmptyStationList() //每次都要重新获取站点
      this.isAdd = true
      this.dialogTitle = '分拣入库'
      this.addFormVisible = true;
      this.setInitAddForm()
      if (this.allCodeSetsForAllowProcess.length == 1) {//验证如果只有一个工序，就默认选中
        var item = this.allCodeSetsForAllowProcess[0]
        this.addForm.AllowProcedure = item.value
        this.changeAllowProcess(item)
      }
    },
    /******************************************[查看界面]开始*********************************************************** */
    //显示查看界面 
    handleView (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要查看的一行数据！",
          type: "warning"
        });
        return;
      }
      this.viewDetailVisible = true
      this.viewDetailForm = {
      };
      this.viewDetailForm = Object.assign({}, row);
    },
    //关闭查看界面
    closeViewHandle () {
      this.viewDetailVisible = false
    },
    /******************************************[查看界面]结束*********************************************************** */
    CancelSaveData () {
      this.setInitAddForm()
      this.addFormVisible = false
    },
    // 更新数据 
    SaveData (formName) {
      this.submitDisabled = true

      this.$refs.addForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认提交吗？', '提示', {}).then(() => {
            this.addLoading = true;
            let para = Object.assign({}, this.addForm);
            if (this.isAdd) {//新增 
              addStockInfo(para).then((res) => {

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
                  this.getStockInfos();
                }
                else {
                  this.addLoading = false;
                  this.$message({
                    message: res.data.msg,
                    type: 'error'
                  });
                }

              });
            } else {//编辑 
              editStockInfo(para).then((res) => {

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
                  this.getStockInfos();
                }
                else {
                  this.addLoading = false;
                  this.$message({
                    message: res.data.msg,
                    type: 'error'
                  });
                }
              });
            }
          });
        }
      });
    },
    selsChange: function (sels) {
      this.sels = sels;
    },
    //导出 
    handleExport () {
      this.currentPageData = this.StockInfos
      this.$refs.cmToolbarExport.showPrintPage()
    },
    //addForm赋初始值
    setInitAddForm () {
      this.addForm = Object.assign({}, this.init_addForm)
    }
  },
  mounted () {
    if (isMobile()) {
      this.drawerSize = "99%"//drawner宽度设置 
    } else {
      this.drawerSize = "550px"//drawner宽度设置 
    }

    this.setInitAddForm()
    this.getCodeItemsByCodeForFJGXMRPLRWH()
    this.getEnumberListForLesTaskStatusEnum()
    this.getCodeItemsBySetCodeForGX()
    this.getStockInfos();

    let routers = window.localStorage.router
      ? JSON.parse(window.localStorage.router)
      : [];
    this.buttonList = getButtonList(this.$route.path, routers);
    let isShow = isNeedShowOperatorColumn(this.buttonList);//控制是否要显示【操作】列 
    this.isShowOperatorColumn = isShow;
  }
}

</script> 
 
<style lang="scss" scoped>
</style> 
