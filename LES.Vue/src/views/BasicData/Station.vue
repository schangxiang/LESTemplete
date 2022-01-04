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
                    @onSearch="_getStations" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="Stations"
              :height="tableHeight"
              highlight-current-row
              size="small"
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
      <el-table-column prop="StationCode"
                       label="站点编号"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="StationName"
                       label="站点名称"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="AgvStation"
                       label="AGV站点"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="StoreAreaName"
                       label="库存区域"
                       align="center">
      </el-table-column>
      <el-table-column prop="AllowLineName"
                       label="所属产线"
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
      <el-table-column prop="IsLock"
                       label="是否锁定"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.IsLock===1   ? 'danger' : 'success'"
                  disable-transitions>{{scope.row.IsLock===1 ? "是":"否"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="IsFull"
                       label="是否有货"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.IsFull ===1  ? 'danger' : 'success'"
                  disable-transitions>{{scope.row.IsFull===1 ? "是":"否"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="IsDeleted"
                       label="是否禁用"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.IsDeleted  ? 'danger' : 'success'"
                  disable-transitions>{{scope.row.IsDeleted? "是":"否"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="Str_BigMaterialStations"
                       label="可放物料"
                       width="160"
                       align="center">
      </el-table-column>
      <el-table-column prop="Remark"
                       label="备注"
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
      <el-table-column label="操作"
                       width="180"
                       v-if="isShowOperatorColumn">
        <template scope="scope">
          <el-button size="small"
                     icon="el-icon-edit"
                     type="primary"
                     v-if="isShowOperatorButton('edit')"
                     @click="handleEdit(scope.row)">编辑</el-button>

        </template>
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
               :visible.sync="addFormVisible"
               v-model="addFormVisible"
               :close-on-click-modal="false">
      <el-form :model="addForm"
               size="small"
               label-width="100px"
               ref="addForm">
        <el-form-item label="站点编号"
                      prop="StationCode"
                      :rules="[{ required: true, message: '站点编号不能为空'}]">
          <el-input v-model="addForm.StationCode"></el-input>
        </el-form-item>
        <el-form-item label="站点名称"
                      prop="StationName"
                      :rules="[{ required: true, message: '站点名称不能为空'}]">
          <el-input v-model="addForm.StationName"></el-input>
        </el-form-item>
        <el-form-item label="AGV站点"
                      prop="AgvStation"
                      :rules="[{ required: true, message: 'AGV站点不能为空'}]">
          <el-input v-model="addForm.AgvStation"></el-input>
        </el-form-item>
        <el-form-item label="库存区域"
                      prop="StoreArea"
                      :rules="[{ required: true, message: '库存区域不能为空'}]">
          <el-select v-model="addForm.StoreArea"
                     clearable
                     placeholder="请选择">
            <el-option v-for="item in storeAreaCodeItems"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="所属产线"
                      prop="AllowLine"
                      :rules="[{ required: true, message: '所属产线不能为空'}]">
          <el-select v-model="addForm.AllowLine"
                     clearable
                     placeholder="请选择">
            <el-option v-for="item in workshopCodeItems"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="所属工序"
                      prop="AllowProcedure"
                      :rules="[{ required: true, message: '所属工序不能为空'}]">
          <el-select v-model="addForm.AllowProcedure"
                     clearable
                     placeholder="请选择">
            <el-option v-for="item in procedureCodeItems"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="工位类型"
                      prop="PlaceType"
                      :rules="[{ required: true, message: '工位类型不能为空'}]">
          <el-select v-model="addForm.PlaceType"
                     clearable
                     placeholder="请选择">
            <el-option v-for="item in placeTypeItems"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="可放物料"
                      prop="BigMaterialStations">
          <el-cascader v-model="addForm.BigMaterialStations"
                       style="width:100%"
                       clearable
                       :props="cascader_props"
                       :options="bigMaterialsForCascader"></el-cascader>
        </el-form-item>
        <el-form-item label="备注"
                      prop="Remark ">
          <el-input v-model="addForm.Remark"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="cancelSaveClick"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="SaveData"
                   icon="fa fa-send"
                   :loading="addLoading">提交</el-button>
        <el-button type="primary"
                   v-if="isAdd"
                   @click.native="continueAddClick"
                   icon="el-icon-plus"
                   :loading="addLoading">继续新增</el-button>
      </div>
    </el-dialog>
    <!--更新是否锁定界面-->
    <el-dialog title="更新锁定状态"
               :visible.sync="updateIsLockVisible"
               :close-on-click-modal="false">
      <el-form :model="updateIsLockForm"
               size="small"
               label-width="80px"
               ref="updateIsLockForm">
        <el-form-item label="站点编号"
                      prop="StationCode">
          <el-input v-model="updateIsLockForm.StationCode"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="站点名称"
                      prop="StationName">
          <el-input v-model="updateIsLockForm.StationName"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label=""
                      prop="IsLock">
          <el-switch v-model="updateIsLockForm.IsLock"
                     active-text="锁定"
                     inactive-text="不锁定"
                     active-color="#13ce66"
                     inactive-color="#ff4949">
          </el-switch>
        </el-form-item>
        <el-form-item label="操作说明"
                      prop="OperationRemark"
                      :rules="[{ required: true, message: '操作说明不能为空'}]">
          <el-input v-model="updateIsLockForm.OperationRemark"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="updateIsLockVisible = false"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="do_handleIsLock"
                   icon="fa fa-send"
                   :loading="addLoading">提交</el-button>
      </div>
    </el-dialog>
    <!--更新是否禁用界面-->
    <el-dialog title="更新禁用状态"
               :visible.sync="updateIsDeletedVisible"
               :close-on-click-modal="false">
      <el-form :model="updateIsDeletedForm"
               size="small"
               label-width="80px"
               ref="updateIsDeletedForm">
        <el-form-item label="站点编号"
                      prop="StationCode">
          <el-input v-model="updateIsDeletedForm.StationCode"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="站点名称"
                      prop="StationName">
          <el-input v-model="updateIsDeletedForm.StationName"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label=""
                      prop="IsDeleted">
          <el-switch v-model="updateIsDeletedForm.IsDeleted"
                     active-text="禁用"
                     inactive-text="不禁用"
                     active-color="#13ce66"
                     inactive-color="#ff4949">
          </el-switch>
        </el-form-item>
        <el-form-item label="操作说明"
                      prop="OperationRemark"
                      :rules="[{ required: true, message: '操作说明不能为空'}]">
          <el-input v-model="updateIsDeletedForm.OperationRemark"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="updateIsDeletedVisible = false"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="do_handleIsDeleted"
                   icon="fa fa-send"
                   :loading="addLoading">提交</el-button>
      </div>
    </el-dialog>
    <!--更新是否有货界面-->
    <el-dialog title="更新有货状态"
               :visible.sync="updateIsFullVisible"
               :close-on-click-modal="false">
      <el-form :model="updateIsFullForm"
               size="small"
               label-width="80px"
               ref="updateIsFullForm">
        <el-form-item label="站点编号"
                      prop="StationCode">
          <el-input v-model="updateIsFullForm.StationCode"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="站点名称"
                      prop="StationName">
          <el-input v-model="updateIsFullForm.StationName"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label=""
                      prop="IsFull">
          <el-switch v-model="updateIsFullForm.IsFull"
                     active-text="有货"
                     inactive-text="无货"
                     active-color="#13ce66"
                     inactive-color="#ff4949">
          </el-switch>
        </el-form-item>
        <el-form-item label="操作说明"
                      prop="OperationRemark"
                      :rules="[{ required: true, message: '操作说明不能为空'}]">
          <el-input v-model="updateIsFullForm.OperationRemark"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="updateIsFullVisible = false"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="do_handleIsFull"
                   icon="fa fa-send"
                   :loading="addLoading">提交</el-button>
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
        <el-form-item label="库存区域"
                      prop="StoreArea">
          <el-input v-model="viewDetailForm.StoreArea"></el-input>
        </el-form-item>
        <el-form-item label="所属工序"
                      prop="AllowProcedure">
          <el-input v-model="viewDetailForm.AllowProcedure"></el-input>
        </el-form-item>
        <el-form-item label="工位类型"
                      prop="PlaceType">
          <el-input v-model="viewDetailForm.PlaceType"></el-input>
        </el-form-item>
        <el-form-item label="所属产线"
                      prop="AllowLine">
          <el-input v-model="viewDetailForm.AllowLine"></el-input>
        </el-form-item>
        <el-form-item label="站点编号"
                      prop="StationCode">
          <el-input v-model="viewDetailForm.StationCode"></el-input>
        </el-form-item>
        <el-form-item label="站点名称"
                      prop="StationName">
          <el-input v-model="viewDetailForm.StationName"></el-input>
        </el-form-item>
        <el-form-item label="AGV站点"
                      prop="AgvStation">
          <el-input v-model="viewDetailForm.AgvStation"></el-input>
        </el-form-item>
        <el-form-item label="备注"
                      prop="Remark">
          <el-input v-model="viewDetailForm.Remark"></el-input>
        </el-form-item>
        <el-form-item label="是否锁定"
                      prop="IsLock">
          <el-input-number v-model="viewDetailForm.IsLock"></el-input-number>
        </el-form-item>
        <el-form-item label="是否有货"
                      prop="IsFull">
          <el-input-number v-model="viewDetailForm.IsFull"></el-input-number>
        </el-form-item>
        <el-form-item label="是否删除"
                      prop="IsDeleted">
          <el-switch v-model="viewDetailForm.IsDeleted"></el-switch>
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
    <!-- 导出组件 -->
    <ToolbarExport ref="cmToolbarExport"
                   :exportFileName="exportFileName"
                   :currentPageData="currentPageData"
                   :exportColumnHeader="exportColumnHeader" />
  </section>
</template> 
 
<script> 
import util from '../../../util/date'
import { getBigMaterialsForCascader, updateStationIsDeleted, updateStationIsFull, updateStationIsLock, getStationListPage, removeStation, editStation, addStation, getCodeItemsBySetCodeForPublic } from '../../api/api';
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";
import SearchForm from "../../components/SearchForm";
import ToolbarExport from "../../components/ToolbarExport";
import { formatDate, setformOptionsNewValue } from '../../../util/tools'
import { isShowOperatorButtonCommon, isNeedShowOperatorColumn, isMobile } from '../../../util/common'

export default {
  components: { Toolbar, SearchForm, ToolbarExport },//注册子组件 
  data () {
    return {
      cascader_props: { multiple: true },
      bigMaterialsForCascader: [],
      updateIsLockVisible: false,
      updateIsLockForm: {},
      updateIsFullVisible: false,
      updateIsFullForm: {},
      updateIsDeletedVisible: false,
      updateIsDeletedForm: {},
      storeAreaCodeItems: [],
      //导出组件相关 
      exportFileName: '站点管理信息',//要导出的文件名 
      currentPageData: [],//当前页面的列表数据 
      exportColumnHeader: {
        'StationCode': '站点编号', 'StationName': '站点名称', 'AgvStation': 'AGV站点', 'AllowLineName': '所属产线', 'AllowProcedureName': '所属工序',
        'PlaceTypeName': '工位类型', 'CreateBy': '创建者', 'CreateTime': '创建时间', 'ModifyBy': '修改者', 'ModifyTime': '修改时间'
      },//当前页面列表的表头汉字和属性数组，导出用 
      //搜索框相关 
      commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准"
      searchValControlStyle: {//设置通用搜索框的长度等样式 
        width: '300px',
      },
      controlStyle: {//设置搜索控件的长度等样式
        width: '400px',
      },
      labelWidth: "90px",//显示Label的宽度
      drawerSize: "650px",//drawner宽度设置 
      searchFormInputPlaceholder: '请输入站点编号/站点名称/AGV站点',//要给子搜索组件传递的值
      searchFormInputAttrs: ['StationCode', 'StationName', 'AgvStation'],//要给子搜索组件传递的属性名 
      formOptions: [
        {
          label: '站点编号',
          prop: 'StationCode',
          element: 'el-input',
        },
        {
          label: '站点名称',
          prop: 'StationName',
          element: 'el-input',
        },
        {
          label: 'AGV站点',
          prop: 'AgvStation',
          element: 'el-input',
        },
        {
          label: '库存区域',
          prop: 'StoreArea',
          element: 'el-select',
          options: []
        },
        {
          label: '所属产线',
          prop: 'AllowLine',
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
          label: '工位类型',
          prop: 'PlaceType',
          element: 'el-select',
          options: []
        },
        {
          label: '是否禁用',
          prop: 'IsDeleted',
          element: 'el-select',
          options: [{ label: '是', value: 'true' }, { label: '否', value: 'false' }]
        },
        {
          label: '是否锁定',
          prop: 'IsLock',
          element: 'el-select',
          options: [{ label: '是', value: 1 }, { label: '否', value: 0 }]
        },
        {
          label: '是否有货',
          prop: 'IsFull',
          element: 'el-select',
          options: [{ label: '是', value: 1 }, { label: '否', value: 0 }]
        },
        {
          label: '备注',
          prop: 'Remark',
          element: 'el-input',
        },
        {
          label: '创建者',
          prop: 'CreateBy',
          element: 'el-input',
        },
        {
          label: '修改者',
          prop: 'ModifyBy',
          element: 'el-input',
        },
        {
          label: '修改时间',
          prop: 'ModifyTime',
          element: 'el-date-picker',
        },
      ],

      isShowOperatorColumn: true,// 是否显示【操作】列,true显示 false不显示
      tableHeight: window.innerHeight - 180, // 控制表格的高度

      workshopCodeItems: [],//车间的码表集合
      procedureCodeItems: [],//工序的码表集合
      placeTypeItems: [],//工位类型集合

      isAdd: false, // 是否是新增 
      dialogTitle: '',

      //查看有关 
      viewDetailVisible: false,
      viewDetailForm: {},

      filters: {
        name: ''
      },
      Stations: [],
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
      addForm: {

      }

    }
  },
  methods: {
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
      this.getStations();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getStations();
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
    _getStations () {
      this.page = 1
      this.getStations()
    },
    //获取码表集列表 
    getStations (formValue) {
      //this.total = 0 
      //this.Stations = null 
      let para = this.getPostParam('1');
      this.listLoading = true;
      getStationListPage(para).then((res) => {
        this.total = res.data.response.dataCount;
        this.Stations = res.data.response.data;
        this.listLoading = false;
      });
    },
    //通过代码集获取代码项集合 
    getCodeItemsBySetCode () {
      let para = {
        setCode: '002' // 车间码表集
      }
      getCodeItemsBySetCodeForPublic(para).then((res) => {
        this.workshopCodeItems = res.data.response;
        //给搜索框中的select控件赋值
        let propValue = "AllowLine";
        this.formOptions = setformOptionsNewValue(this.formOptions, propValue, this.workshopCodeItems);
        this.$refs.ChildSearchForm.setNewFormOptions(this.formOptions)
      });
    },
    //获取工序
    getProcedure () {
      let para = {
        setCode: 'LES_GX'
      }
      getCodeItemsBySetCodeForPublic(para).then((res) => {
        this.procedureCodeItems = res.data.response;
        //给搜索框中的select控件赋值
        let propValue = "AllowProcedure";
        this.formOptions = setformOptionsNewValue(this.formOptions, propValue, this.procedureCodeItems);
        this.$refs.ChildSearchForm.setNewFormOptions(this.formOptions)
      });
    },
    //获取库存区域
    getStoreArea () {
      let para = {
        setCode: 'KCQY'
      }
      getCodeItemsBySetCodeForPublic(para).then((res) => {
        this.storeAreaCodeItems = res.data.response;
        //给搜索框中的select控件赋值
        let propValue = "StoreArea";
        this.formOptions = setformOptionsNewValue(this.formOptions, propValue, this.storeAreaCodeItems);
        this.$refs.ChildSearchForm.setNewFormOptions(this.formOptions)
      });
    },
    //获取站点工位类型
    getStationPlaceType () {
      let para = {
        setCode: 'S001'
      }
      getCodeItemsBySetCodeForPublic(para).then((res) => {
        this.placeTypeItems = res.data.response;
        //给搜索框中的select控件赋值
        let propValue = "PlaceType";
        this.formOptions = setformOptionsNewValue(this.formOptions, propValue, this.placeTypeItems);
        this.$refs.ChildSearchForm.setNewFormOptions(this.formOptions)
      });
    },
    //获取全部列表 
    exportAllData () {
      let para = this.getPostParam('2');
      this.listLoading = true;
      getStationListPage(para).then((res) => {
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
        removeStation(para).then((res) => {

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

          this.getStations();
        });
      }).catch(() => {

      });
    },
    //显示 更新锁定状态界面 
    handleIsLock (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要修改锁定状态的一行数据！",
          type: "error"
        });
        return;
      }

      //绑定数据
      this.updateIsLockForm = {}
      this.updateIsLockForm = row
      this.updateIsLockForm.OperationRemark = ''
      this.updateIsLockForm.IsLock = row.IsLock == 1 ? true : false
      this.updateIsLockVisible = true
    },
    do_handleIsLock () {
      this.$refs.updateIsLockForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认修改锁定状态吗?', '提示', {
            type: 'warning'
          }).then(() => {
            this.listLoading = true;
            let para = {
              Id: this.updateIsLockForm.Id,
              IsLock: this.updateIsLockForm.IsLock ? 1 : 0,
              OperationRemark: this.updateIsLockForm.OperationRemark
            };
            updateStationIsLock(para).then((res) => {

              if (util.isEmt.format(res)) {
                this.listLoading = false;
                return;
              }
              this.listLoading = false;
              //NProgress.done(); 
              if (res.data.success) {
                this.$message({
                  message: '修改锁定状态成功',
                  type: 'success'
                });

              } else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
                });
              }
              this.updateIsLockVisible = false
              this.updateIsLockForm.OperationRemark = ''
              this.getStations();
            });
          }).catch(() => {
          });
        }
      });

    },
    //显示 更新禁用状态界面 
    handleIsDeleted (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要修改禁用状态的一行数据！",
          type: "warning"
        });
        return;
      }

      //绑定数据
      this.updateIsDeletedForm = {}
      this.updateIsDeletedForm = row
      this.updateIsDeletedForm.OperationRemark = ''
      this.updateIsDeletedForm.IsDeleted = row.IsDeleted
      this.updateIsDeletedVisible = true
    },
    do_handleIsDeleted () {
      this.$refs.updateIsDeletedForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认修改禁用状态吗?', '提示', {
            type: 'warning'
          }).then(() => {
            this.listLoading = true;
            let para = {
              Id: this.updateIsDeletedForm.Id,
              IsDeleted: this.updateIsDeletedForm.IsDeleted,
              OperationRemark: this.updateIsDeletedForm.OperationRemark
            };
            updateStationIsDeleted(para).then((res) => {

              if (util.isEmt.format(res)) {
                this.listLoading = false;
                return;
              }
              this.listLoading = false;
              //NProgress.done(); 
              if (res.data.success) {
                this.$message({
                  message: '修改禁用状态成功',
                  type: 'success'
                });

              } else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
                });
              }
              this.updateIsDeletedVisible = false
              this.updateIsDeletedForm.OperationRemark = ''
              this.getStations();
            });
          }).catch(() => {

          });
        }
      });

    },
    //显示 更新有货状态界面 
    handleIsFull (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要修改有货状态的一行数据！",
          type: "error"
        });
        return;
      }

      //绑定数据
      this.updateIsFullForm = {}
      this.updateIsFullForm = row
      this.updateIsFullForm.OperationRemark = ''
      this.updateIsFullForm.IsFull = row.IsFull == 1 ? true : false
      this.updateIsFullVisible = true
    },
    do_handleIsFull () {
      this.$refs.updateIsFullForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认修改有货状态吗?', '提示', {
            type: 'warning'
          }).then(() => {
            this.listLoading = true;
            let para = {
              Id: this.updateIsFullForm.Id,
              IsFull: this.updateIsFullForm.IsFull ? 1 : 0,
              OperationRemark: this.updateIsFullForm.OperationRemark
            };
            updateStationIsFull(para).then((res) => {

              if (util.isEmt.format(res)) {
                this.listLoading = false;
                return;
              }
              this.listLoading = false;
              //NProgress.done(); 
              if (res.data.success) {
                this.$message({
                  message: '修改有货状态成功',
                  type: 'success'
                });

              } else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
                });
              }
              this.updateIsFullVisible = false
              this.updateIsFullForm.OperationRemark = ''
              this.getStations();
            });
          }).catch(() => {

          });
        }
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
          type: "warning"
        });

        return;
      }
      this.isAdd = false
      this.dialogTitle = '编辑'
      this.addFormVisible = true;
      this.addForm = {
      };
      this.addForm = Object.assign({}, row);
    },
    //显示新增界面 
    handleAdd () {
      this.isAdd = true
      this.dialogTitle = '新增'
      this.addFormVisible = true;
      this.addForm = {
      };
    },
    //触发继续新增按钮
    continueAddClick () {
      this.handleAdd()
    },
    //触发取消按钮
    cancelSaveClick () {
      this.$refs['addForm'].resetFields();
      this.addFormVisible = false;
      this.getStations();
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
              addStation(para).then((res) => {

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
                  this.getStations();
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
              editStation(para).then((res) => {

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
                  this.cancelSaveClick()
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
      this.currentPageData = this.Stations
      this.$refs.cmToolbarExport.showPrintPage()
    },
    //获取大类物料，按照分级结构 
    GetBigMaterialsForCascader () {
      getBigMaterialsForCascader().then((res) => {
        if (res.data.success) {
          this.bigMaterialsForCascader = res.data.response
        } else {
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }
      });
    },
  },
  mounted () {
    this.getProcedure()
    this.getStoreArea()
    this.getCodeItemsBySetCode();
    this.getStationPlaceType();
    this.GetBigMaterialsForCascader();
    this.getStations();


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
