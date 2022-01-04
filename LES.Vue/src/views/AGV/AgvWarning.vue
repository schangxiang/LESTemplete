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
        <SearchForm :commonSearchOptionSet="c_commonSearchOptionSet"
                    :searchValControlStyle="c_searchValControlStyle"
                    :searchFormInputPlaceholder="c_searchFormInputPlaceholder"
                    :searchFormInputAttrs="c_searchFormInputAttrs"
                    :formOptions="h_formOptions"
                    :drawerSize="h_drawerSize"
                    :labelWidth="h_labelWidth"
                    :controlStyle="h_controlStyle"
                    ref="ChildSearchForm"
                    @onSearch="_getAgvWarnings" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="AgvWarnings"
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
      <el-table-column prop="WarningTime"
                       label="报警时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
      <el-table-column prop="AgvName"
                       label="车辆名称"
                       width="120"
                       align="center">
      </el-table-column>
      <el-table-column prop="AgvNameDesc"
                       label="车辆"
                       align="center">
      </el-table-column>
      <el-table-column prop="WarningLocation"
                       label="报警位置"
                       align="center">
      </el-table-column>
      <el-table-column prop="WarningContent"
                       label="报警内容"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="StatusName"
                       label="状态"
                       align="center">
      </el-table-column>
      <el-table-column prop="IsKnow"
                       label="是否工人已读"
                       width="80"
                       sortable>
        <template slot-scope="scope">
          <el-tag :type="scope.row.IsKnow===1  ? 'success' : 'danger'"
                  disable-transitions>{{scope.row.IsKnow===1  ? '已读' : '未读'}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="CloseContent"
                       label="关闭报警说明"
                       align="center">
      </el-table-column>
      <el-table-column prop="FinishTime"
                       label="完成时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
      <el-table-column prop="DurationMinuteTime"
                       label="持续时间(分钟)"
                       width="100"
                       align="center">
      </el-table-column>
      <el-table-column prop="DurationTime"
                       label="持续时间"
                       width="150"
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
                       fixed="right"
                       width="180"
                       v-if="isShowOperatorColumn">
        <template scope="scope">
          <el-button size="small"
                     icon="el-icon-edit"
                     type="primary"
                     v-if="isShowOperatorButton('edit')"
                     @click="handleEdit(scope.row)">编辑</el-button>
          <el-button type="danger"
                     size="small"
                     icon="el-icon-delete"
                     v-if="isShowOperatorButton('del')"
                     @click="handleDel(scope.row)">删除</el-button>
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
               :close-on-click-modal="false"
               :before-close="cancelHandle">
      <el-form :model="addForm"
               size="small"
               label-width="120px"
               ref="addForm">
        <el-form-item label="报警时间"
                      prop="WarningTime"
                      :rules="[{ required: true, message: '报警时间不能为空'}]">
          <el-date-picker placeholder="选择日期时间"
                          type="datetime"
                          v-model="addForm.WarningTime"></el-date-picker>
        </el-form-item>
        <el-form-item label="车辆名称"
                      prop="AgvName"
                      :rules="[{ required: true, message: '车辆名称不能为空'}]">
          <el-input v-model="addForm.AgvName"></el-input>
        </el-form-item>
        <el-form-item label="车辆描述"
                      prop="AgvNameDesc">
          <el-input v-model="addForm.AgvNameDesc"></el-input>
        </el-form-item>
        <el-form-item label="报警位置"
                      prop="WarningLocation"
                      :rules="[{ required: true, message: '报警位置不能为空'}]">
          <el-input v-model="addForm.WarningLocation"></el-input>
        </el-form-item>
        <el-form-item label="报警内容"
                      prop="WarningContent"
                      :rules="[{ required: true, message: '报警内容不能为空'}]">
          <el-input v-model="addForm.WarningContent"></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="Status"
                      :rules="[{ required: true, message: '状态不能为空'}]">
          <el-input-number v-model="addForm.Status"></el-input-number>
        </el-form-item>
        <el-form-item label="状态"
                      prop="StatusName"
                      :rules="[{ required: true, message: '状态不能为空'}]">
          <el-input v-model="addForm.StatusName"></el-input>
        </el-form-item>
        <el-form-item label="是否工人已知道"
                      prop="IsKnow">
          <el-input-number v-model="addForm.IsKnow"></el-input-number>
        </el-form-item>
        <el-form-item label="关闭报警说明"
                      prop="CloseContent"
                      :rules="[{ required: true, message: '关闭报警说明不能为空'}]">
          <el-input v-model="addForm.CloseContent"></el-input>
        </el-form-item>
        <el-form-item label="完成时间"
                      prop="FinishTime">
          <el-date-picker placeholder="选择日期时间"
                          type="datetime"
                          v-model="addForm.FinishTime"></el-date-picker>
        </el-form-item>
        <el-form-item label="持续时间"
                      prop="DurationTime">
          <el-input v-model="addForm.DurationTime"></el-input>
        </el-form-item>
        <el-form-item label="操作说明"
                      prop="OperationRemark">
          <el-input v-model="addForm.OperationRemark"></el-input>
        </el-form-item>
        <el-form-item label="创建者ID"
                      prop="CreateId">
          <el-input-number v-model="addForm.CreateId"></el-input-number>
        </el-form-item>
        <el-form-item label="创建者"
                      prop="CreateBy">
          <el-input v-model="addForm.CreateBy"></el-input>
        </el-form-item>
        <el-form-item label="修改者ID"
                      prop="ModifyId">
          <el-input-number v-model="addForm.ModifyId"></el-input-number>
        </el-form-item>
        <el-form-item label="修改者"
                      prop="ModifyBy">
          <el-input v-model="addForm.ModifyBy"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="cancelHandle"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="submitDataHandle"
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
        <el-form-item label="报警时间"
                      prop="WarningTime">
          <el-date-picker placeholder="选择日期时间"
                          type="datetime"
                          v-model="viewDetailForm.WarningTime"></el-date-picker>
        </el-form-item>
        <el-form-item label="车辆名称"
                      prop="AgvName">
          <el-input v-model="viewDetailForm.AgvName"></el-input>
        </el-form-item>
        <el-form-item label="车辆描述"
                      prop="AgvNameDesc">
          <el-input v-model="viewDetailForm.AgvNameDesc"></el-input>
        </el-form-item>
        <el-form-item label="报警位置"
                      prop="WarningLocation">
          <el-input v-model="viewDetailForm.WarningLocation"></el-input>
        </el-form-item>
        <el-form-item label="报警内容"
                      prop="WarningContent">
          <el-input v-model="viewDetailForm.WarningContent"></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="Status">
          <el-input-number v-model="viewDetailForm.Status"></el-input-number>
        </el-form-item>
        <el-form-item label="状态"
                      prop="StatusName">
          <el-input v-model="viewDetailForm.StatusName"></el-input>
        </el-form-item>
        <el-form-item label="是否工人已知道"
                      prop="IsKnow">
          <el-input-number v-model="viewDetailForm.IsKnow"></el-input-number>
        </el-form-item>
        <el-form-item label="关闭报警说明"
                      prop="CloseContent">
          <el-input v-model="viewDetailForm.CloseContent"></el-input>
        </el-form-item>
        <el-form-item label="完成时间"
                      prop="FinishTime">
          <el-date-picker placeholder="选择日期时间"
                          type="datetime"
                          v-model="viewDetailForm.FinishTime"></el-date-picker>
        </el-form-item>
        <el-form-item label="持续时间"
                      prop="DurationTime">
          <el-input v-model="viewDetailForm.DurationTime"></el-input>
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
    <div>
      <el-dialog title="关闭报警"
                 :visible.sync="dialogCloseVisible"
                 width="70%">
        <el-form v-bind:model="addForm"
                 ref="addForm"
                 label-position="left"
                 label-width="100px">
          <el-row :gutter="20">
            <el-form-item label="报警时间"
                          prop="WarningTime"
                          :rules="[{ required: true, message: '报警时间不能为空'}]">
              <el-date-picker type="datetime"
                              placeholder="选择日期时间"
                              v-model="addForm.WarningTime"
                              disabled></el-date-picker>
            </el-form-item>
            <el-form-item label="车辆"
                          prop="AgvNameDesc">
              <el-input v-model="addForm.AgvNameDesc"
                        disabled></el-input>
            </el-form-item>
            <el-form-item label="报警内容"
                          prop="WarningContent"
                          :rules="[{ required: true, message: '报警内容不能为空'}]">
              <el-input v-model="addForm.WarningContent"
                        disabled></el-input>
            </el-form-item>
            <el-form-item label="报警位置"
                          prop="WarningLocation">
              <el-input disabled="true"
                        v-model="addForm.WarningLocation"></el-input>
            </el-form-item>
            <el-form-item label="处理说明"
                          prop="CloseContent"
                          :rules="[{ required: true, message: '处理说明不能为空'}]">
              <el-input type="textarea"
                        size="mini"
                        v-model="addForm.CloseContent"></el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary"
                         size="mini"
                         @click="closeWarnData"
                         :loading="closeLoading">确 定</el-button>
              <el-button size="mini"
                         @click="dialogCloseVisible = false">关 闭</el-button>
            </el-form-item>
          </el-row>
        </el-form>
      </el-dialog>
    </div>
    <dviv>
      <el-dialog title="关闭所有报警"
                 :visible.sync="closeAllDialogCloseVisible"
                 width="70%">
        <el-form v-bind:model="closeAllForm"
                 ref="closeAllForm"
                 label-position="left"
                 label-width="100px">
          <el-row :gutter="20">
            <el-form-item label="处理说明"
                          prop="CloseContent"
                          :rules="[{ required: true, message: '处理说明不能为空'}]">
              <el-input type="textarea"
                        size="mini"
                        v-model="closeAllForm.CloseContent"></el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary"
                         size="mini"
                         @click="closeAllWarnData"
                         :loading="closeLoading">确 定</el-button>
              <el-button size="mini"
                         @click="closeAllDialogCloseVisible = false">关 闭</el-button>
            </el-form-item>
          </el-row>
        </el-form>
      </el-dialog>
    </dviv>
    <!-- 导出组件 -->
    <ToolbarExport ref="cmToolbarExport"
                   :exportFileName="export_exportFileName"
                   :currentPageData="export_currentPageData"
                   :exportColumnHeader="export_columnHeader" />
    <!-- 导入Excel组件 -->
    <ToolbarImportExcel ref="tbImportExcel"
                        :importExcelCategroy="emport_excelCategroy"
                        :templeteFileDownName="emport_templeteFileDownName"></ToolbarImportExcel>
  </section>
</template> 
 
<script> 
import util from '../../../util/date'
import { updateAllWarningDone, getEnumberList, getAgvWarningListPage, removeAgvWarning, editAgvWarning, addAgvWarning } from '../../api/api';
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";
import SearchForm from "../../components/SearchForm";
import ToolbarExport from "../../components/ToolbarExport";
import ToolbarImportExcel from "../../components/ToolbarImportExcel";
import { formatDate, setformOptionsNewValue } from '../../../util/tools'
import { isShowOperatorButtonCommon, isNeedShowOperatorColumn, isMobile } from '../../../util/common'


export default {
  components: { Toolbar, SearchForm, ToolbarExport, ToolbarImportExcel },//注册子组件 
  data () {
    return {
      //导入Excel数据相关 
      emport_templeteFileDownName: '.xlsx',//文件下载名字 
      emport_excelCategroy: "",//上传文件的请求分类 
      //导出组件相关 
      export_exportFileName: 'AGV报警信息',//要导出的文件名 
      export_currentPageData: [],//当前页面的列表数据 
      export_columnHeader: {
        'WarningTime': '报警时间', 'AgvName': '车辆名称',
        'AgvNameDesc': '车辆描述', 'WarningLocation': '报警位置', 'WarningContent': '报警内容',
        'StatusName': '状态', 'CloseContent': '关闭报警说明', 'FinishTime': '完成时间',
        'DurationMinuteTime': '持续时间(分钟)', 'DurationTime': '持续时间',
        'CreateBy': '创建者', 'CreateTime': '创建时间', 'ModifyBy': '修改者',
        'ModifyTime': '修改时间'
      },//当前页面列表的表头汉字和属性数组，导出用  

      //搜索框相关 
      c_commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准" 
      c_searchValControlStyle: {//设置通用搜索框的长度等样式  
        width: '300px',
      },
      c_searchFormInputPlaceholder: '请输入车辆名称/车辆描述/报警位置/报警内容/状态/关闭报警说明/持续时间/操作说明/创建者/修改者',//要给子搜索组件传递的值 
      c_searchFormInputAttrs: ['AgvName', 'AgvNameDesc', 'WarningLocation', 'WarningContent', 'StatusName', 'CloseContent', 'DurationTime', 'OperationRemark', 'CreateBy', 'ModifyBy'],//要给子搜索组件传递的属性名 
      h_controlStyle: {//设置高级搜索控件的长度等样式  
        width: '350px',
      },
      h_labelWidth: "100px",//显示Label的宽度 
      h_drawerSize: "600px",//drawner宽度设置 
      h_formOptions: [
        {
          label: '状态',
          prop: 'Status',
          element: 'el-select',
          options: []
        },
        {
          label: '报警时间',
          prop: 'WarningTime',
          type: 'datetimerange',
          element: 'el-date-picker',
        },
        {
          label: '车辆名称',
          prop: 'AgvName',
          element: 'el-input',
        },
        {
          label: '车辆描述',
          prop: 'AgvNameDesc',
          element: 'el-input',
        },
        {
          label: '报警位置',
          prop: 'WarningLocation',
          element: 'el-input',
        },
        {
          label: '报警内容',
          prop: 'WarningContent',
          element: 'el-input',
        },
        {
          label: '关闭报警说明',
          prop: 'CloseContent',
          element: 'el-input',
        },
        {
          label: '完成时间',
          prop: 'FinishTime',
          type: 'datetimerange',
          element: 'el-date-picker',
        },
        {
          label: '持续时间',
          prop: 'DurationTime',
          element: 'el-input',
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
      AgvWarnings: [],
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
      //新增或编辑界面数据 
      addForm: {
        //注意：这里不需要写初始值，初始值都写在 init_addForm中 
      },
      init_addForm: {
        //注意：这里写初始值 
      },
      //关闭所有报警的对象
      closeAllForm: {},
      dialogCloseVisible: false,//是否显示关闭报警界面
      closeAllDialogCloseVisible: false,//是否显示关闭报警界面
    }
  },
  methods: {
    //关闭报警 
    handleClose (selRow) {
      let row = {};
      if (selRow && selRow != undefined) {
        this.currentRow = selRow;
      }
      row = this.currentRow;
      if (!row) {
        this.$message({
          message: "请选择要关闭报警的一行数据！",
          type: "error"
        });
        return;
      }
      if (row.Status == 1) {
        this.$message({
          message: "该报警已经被处理！",
          type: "error"
        });
        return;
      }
      this.dialogCloseVisible = true;
      this.addForm = row;
    },
    //关闭报警
    closeWarnData () {
      this.$refs.addForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认提交吗？', '提示', {}).then(() => {
            this.closeLoading = true;
            let para = {
              Id: this.addForm.Id,
              closeContent: this.addForm.CloseContent
            };
            editAgvWarning(para).then((res) => {
              if (util.isEmt.format(res)) {
                this.closeLoading = false;
                return;
              }
              this.closeLoading = false;
              this.dialogCloseVisible = false;
              if (res.data.success) {
                this.$message({
                  message: '关闭成功',
                  type: 'success'
                });

              } else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
                });
              }
              this.getAgvWarnings();
            });
          });
        }
      });
    },
    //点击关闭所有报警按钮
    handlecloseAllWarnData () {
      this.closeAllDialogCloseVisible = true;
    },
    //关闭所有的报警
    closeAllWarnData () {
      this.$refs.closeAllForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认提交吗？', '提示', {}).then(() => {
            this.closeLoading = true;
            let para = {
              closeContent: this.closeAllForm.CloseContent
            };
            updateAllWarningDone(para).then((res) => {
              if (util.isEmt.format(res)) {
                this.closeLoading = false;
                return;
              }
              this.closeLoading = false;
              this.closeAllDialogCloseVisible = false;
              if (res.data.success) {
                this.$message({
                  message: '关闭成功',
                  type: 'success'
                });

              } else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
                });
              }
              this.getAgvWarnings();
            });
          });
        }
      });
    },
    //获取 报警状态
    getEnumberListForAgvWarningStatusEnum () {
      let para = {
        category: 'AgvWarningStatusEnum'
      }
      getEnumberList(para).then((res) => {
        //给搜索框中的select控件赋值
        let propValue = "Status";
        this.h_formOptions = setformOptionsNewValue(this.h_formOptions, propValue, res.data.response);
        this.$refs.ChildSearchForm.setNewFormOptions(this.h_formOptions)
      });
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
      this.getAgvWarnings();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getAgvWarnings();
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
    //点击按钮【查询】 
    _getAgvWarnings () {
      this.page = 1
      this.getAgvWarnings()
    },
    //获取列表 
    getAgvWarnings (formValue) {
      //this.total = 0 
      //this.AgvWarnings = null 
      let para = this.getPostParam('1');
      this.listLoading = true;
      getAgvWarningListPage(para).then((res) => {
        if (res.data.success) {
          this.total = res.data.response.dataCount;
          this.AgvWarnings = res.data.response.data;
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
      getAgvWarningListPage(para).then((res) => {
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
          type: "warning"
        });
        return;
      }
      this.$confirm('确认删除该记录吗?', '提示', {
        type: 'warning'
      }).then(() => {
        this.listLoading = true;
        let para = { id: row.Id };
        removeAgvWarning(para).then((res) => {

          if (util.isEmt.format(res)) {
            this.listLoading = false;
            return;
          }
          this.listLoading = false;
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

          this._getAgvWarnings();
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
          type: "warning"
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
      this.addFormVisible = true;
      this.setInitAddForm()
    },
    //取消提交数据操作 
    cancelHandle (done) {
      this.setInitAddForm()
      this.addFormVisible = false

      done() //done 用于关闭 Dialog 
    },
    // 提交数据 
    submitDataHandle (formName) {
      this.submitDisabled = true

      this.$refs.addForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认提交吗？', '提示', {}).then(() => {
            this.addLoading = true;
            let para = Object.assign({}, this.addForm);
            if (this.isAdd) {//新增 
              addAgvWarning(para).then((res) => {
                if (util.isEmt.format(res)) {
                  this.addLoading = false;
                  return;
                }

                if (res.data.success) {
                  this.addLoading = false;
                  this.$message({
                    message: res.data.msg,
                    type: 'success'
                  });
                  this.$refs['addForm'].resetFields();
                  this.addFormVisible = false;
                  this._getAgvWarnings();
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
              editAgvWarning(para).then((res) => {
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
                  this._getAgvWarnings();
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
      this.export_currentPageData = this.AgvWarnings
      this.$refs.cmToolbarExport.showPrintPage()
    },
    //导入  
    handleImport () {
      this.$refs.tbImportExcel.showPage()
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
    //addForm赋初始值 
    setInitAddForm () {
      this.addForm = Object.assign({}, this.init_addForm)
    }
  },
  mounted () {
    if (isMobile()) {
      this.h_drawerSize = "99%"//drawner宽度设置 
    } else {
      this.h_drawerSize = "600px"//drawner宽度设置 
    }
    this.getEnumberListForAgvWarningStatusEnum()
    this.setInitAddForm()
    this._getAgvWarnings();

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
