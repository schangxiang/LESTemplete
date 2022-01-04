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
                    @onSearch="_getStockPreparationNotices" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="StockPreparationNotices"
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
      <el-table-column prop="ProcedureName"
                       label="工序"
                       align="center">
      </el-table-column>
      <el-table-column prop="BurdenWorkNo"
                       label="任务号"
                       align="center">
      </el-table-column>
      <el-table-column prop="MaterialName"
                       label="物料"
                       align="center">
      </el-table-column>
      <el-table-column prop="StatusName"
                       label="备料状态"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.StatusName==='备料取消'  ? 'danger' : 'success'"
                  disable-transitions>{{scope.row.StatusName}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="PickInStockTaskID"
                       label="分拣任务号"
                       align="center">
      </el-table-column>
      <el-table-column prop="ThingworxResult"
                       label="Thingworx返回结果"
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
        <el-form-item label="工序"
                      prop="ProcedureName"
                      :rules="[{ required: true, message: '工序不能为空'}]">
          <el-input v-model="addForm.ProcedureName"></el-input>
        </el-form-item>
        <el-form-item label="任务号"
                      prop="BurdenWorkNo"
                      :rules="[{ required: true, message: '任务号不能为空'}]">
          <el-input v-model="addForm.BurdenWorkNo"></el-input>
        </el-form-item>
        <el-form-item label="物料"
                      prop="MaterialName"
                      :rules="[{ required: true, message: '物料不能为空'}]">
          <el-input v-model="addForm.MaterialName"></el-input>
        </el-form-item>
        <el-form-item label="物料"
                      prop="StatusName"
                      :rules="[{ required: true, message: '物料不能为空'}]">
          <el-input v-model="addForm.StatusName"></el-input>
        </el-form-item>
        <el-form-item label="分拣任务号"
                      prop="PickInStockTaskID">
          <el-input v-model="addForm.PickInStockTaskID"></el-input>
        </el-form-item>
        <el-form-item label="Thingworx返回结果"
                      prop="ThingworxResult">
          <el-input v-model="addForm.ThingworxResult"></el-input>
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
        <el-form-item label="工序"
                      prop="ProcedureName">
          <el-input v-model="viewDetailForm.ProcedureName"></el-input>
        </el-form-item>
        <el-form-item label="任务号"
                      prop="BurdenWorkNo">
          <el-input v-model="viewDetailForm.BurdenWorkNo"></el-input>
        </el-form-item>
        <el-form-item label="物料"
                      prop="MaterialName">
          <el-input v-model="viewDetailForm.MaterialName"></el-input>
        </el-form-item>
        <el-form-item label="物料"
                      prop="StatusName">
          <el-input v-model="viewDetailForm.StatusName"></el-input>
        </el-form-item>
        <el-form-item label="分拣任务号"
                      prop="PickInStockTaskID">
          <el-input v-model="viewDetailForm.PickInStockTaskID"></el-input>
        </el-form-item>
        <el-form-item label="Thingworx返回结果"
                      prop="ThingworxResult">
          <el-input v-model="viewDetailForm.ThingworxResult"></el-input>
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

      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="closeViewHandle"
                   icon="fa fa-power-off">关闭</el-button>
      </div>
    </el-dialog>
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
import { getStockPreparationNoticeListPage, removeStockPreparationNotice, editStockPreparationNotice, addStockPreparationNotice } from '../../api/api';
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
      export_exportFileName: 'Thingworx 备料通知表信息',//要导出的文件名 
      export_currentPageData: [],//当前页面的列表数据 
      export_columnHeader: {
        'ProcedureName': '工序', 'BurdenWorkNo': '任务号', 'MaterialName': '物料',
        'StatusName': '备料状态', 'PickInStockTaskID': '分拣任务号', 'ThingworxResult': 'Thingworx返回结果', 'CreateBy': '创建者', 'CreateTime': '创建时间'
      },//当前页面列表的表头汉字和属性数组，导出用  

      //搜索框相关 
      c_commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准" 
      c_searchValControlStyle: {//设置通用搜索框的长度等样式  
        width: '300px',
      },
      c_searchFormInputPlaceholder: '请输入工序/任务号/物料/物料/分拣任务号/Thingworx返回结果/操作说明/创建者',//要给子搜索组件传递的值 
      c_searchFormInputAttrs: ['ProcedureName', 'BurdenWorkNo', 'MaterialName', 'StatusName', 'PickInStockTaskID', 'ThingworxResult', 'OperationRemark', 'CreateBy'],//要给子搜索组件传递的属性名 
      h_controlStyle: {//设置高级搜索控件的长度等样式  
        width: '350px',
      },
      h_labelWidth: "100px",//显示Label的宽度 
      h_drawerSize: "600px",//drawner宽度设置 
      h_formOptions: [
        {
          label: '工序',
          prop: 'ProcedureName',
          element: 'el-input',
        },
        {
          label: '任务号',
          prop: 'BurdenWorkNo',
          element: 'el-input',
        },
        {
          label: '物料',
          prop: 'MaterialName',
          element: 'el-input',
        },
        {
          label: '备料状态',
          prop: 'StatusName',
          element: 'el-input',
        },
        {
          label: '分拣任务号',
          prop: 'PickInStockTaskID',
          element: 'el-input',
        },
        {
          label: 'Thingworx返回结果',
          prop: 'ThingworxResult',
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
      StockPreparationNotices: [],
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
      }
    }
  },
  methods: {
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
      this.getStockPreparationNotices();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getStockPreparationNotices();
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
        para.pageSize = 10000
      }
      return para
    },
    //点击按钮【查询】 
    _getStockPreparationNotices () {
      this.page = 1
      this.getStockPreparationNotices()
    },
    //获取列表 
    getStockPreparationNotices (formValue) {
      //this.total = 0 
      //this.StockPreparationNotices = null 
      let para = this.getPostParam('1');
      this.listLoading = true;
      getStockPreparationNoticeListPage(para).then((res) => {
        if (res.data.success) {
          this.total = res.data.response.dataCount;
          this.StockPreparationNotices = res.data.response.data;
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
      getStockPreparationNoticeListPage(para).then((res) => {
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
        removeStockPreparationNotice(para).then((res) => {

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

          this._getStockPreparationNotices();
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
              addStockPreparationNotice(para).then((res) => {
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
                  this._getStockPreparationNotices();
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
              editStockPreparationNotice(para).then((res) => {
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
                  this._getStockPreparationNotices();
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
      this.export_currentPageData = this.StockPreparationNotices
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
    this.setInitAddForm()
    this._getStockPreparationNotices();

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
