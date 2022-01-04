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
                    @onSearch="getAgvTasks" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="AgvTasks"
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
      <el-table-column prop="TaskNo"
                       label="AGV任务号"
                       width="160"
                       align="left">
        <template slot-scope="scope">
          <a @click="showDetailForAgvTaskPhaseList(scope.row)"
             target="_blank"
             style="text-decoration:underline;cursor: pointer;color:blue;font-weight:bold;">
            {{scope.row.TaskNo}}
          </a>
        </template>
      </el-table-column>
      <el-table-column prop="LesTaskNo"
                       label="LES任务号"
                       width="160"
                       align="left">
        <template slot-scope="scope">
          <a @click="showDetailForTaskPhaseList(scope.row)"
             target="_blank"
             style="text-decoration:underline;cursor: pointer;color:blue;font-weight:bold;">
            {{scope.row.LesTaskNo}}
          </a>
        </template>
      </el-table-column>
      <el-table-column prop="BurdenWorkNo"
                       label="配料任务号"
                       width="120"
                       align="center">
      </el-table-column>
      <el-table-column prop="SourceAgvStation"
                       label="起点编号"
                       width="100"
                       align="center">
      </el-table-column>
      <el-table-column prop="SourcePlaceName"
                       label="起点名称"
                       width="100"
                       align="center">
      </el-table-column>
      <el-table-column prop="ToPlaceAgvStation"
                       label="目标点编号"
                       width="100"
                       align="center">
      </el-table-column>
      <el-table-column prop="ToPlaceName"
                       label="目标点名称"
                       width="100"
                       align="center">
      </el-table-column>
      <el-table-column prop="TaskTypeName"
                       label="任务类型"
                       align="center">
      </el-table-column>

      <el-table-column prop="TaskStatusName"
                       label="任务状态"
                       align="center">
      </el-table-column>
      <el-table-column prop="AgvNameDesc"
                       label="执行车辆"
                       align="center">
      </el-table-column>
      <el-table-column prop="AgvStateName"
                       label="AGV状态"
                       align="center">
      </el-table-column>
      <el-table-column prop="AgvTakeCargoStateName"
                       label="AGV取货状态"
                       align="center">
      </el-table-column>
      <el-table-column prop="AllowProcedure_CodeItem_Name"
                       label="所属工序"
                       align="center">
      </el-table-column>
      <el-table-column prop="ExecuteSequence"
                       label="执行顺序"
                       align="center">
      </el-table-column>
      <el-table-column prop="MaterialCode"
                       label="物料号"
                       align="center">
      </el-table-column>
      <el-table-column prop="MaterialName"
                       label="物料名称"
                       width="200"
                       align="center">
      </el-table-column>
      <el-table-column prop="AllowProcedure_CodeItem_Name"
                       label="所属工序"
                       align="center">
      </el-table-column>

      <el-table-column prop="TaskCreateTime"
                       label="创建时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
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
      <el-table-column prop="CreateBy"
                       label="创建者"
                       align="center">
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
      <el-table-column prop="TaskDesc"
                       label="任务描述"
                       align="center">
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
    <!--详细界面(AGV任务阶段列表)-->
    <ShowAgvTaskPhaseList ref="ShowAgvTaskPhaseListMethod"></ShowAgvTaskPhaseList>
    <!--详细界面（任务阶段列表）-->
    <ShowTaskPhaseList ref="ShowTaskPhaseListMethod"></ShowTaskPhaseList>
    <!-- 导出组件 -->
    <ToolbarExport ref="cmToolbarExport"
                   :exportFileName="export_exportFileName"
                   :currentPageData="export_currentPageData"
                   :exportColumnHeader="export_columnHeader" />
  </section>
</template> 
 
<script> 
import util from '../../../util/date'
import { getAgvTaskListPage, removeAgvTask, editAgvTask, addAgvTask, getEnumberList } from '../../api/api';
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";
import SearchForm from "../../components/SearchForm";
import ToolbarExport from "../../components/ToolbarExport";
import ShowAgvTaskPhaseList from "./LESComponents/ShowAgvTaskPhaseList";
import ShowTaskPhaseList from "./LESComponents/ShowTaskPhaseList";
import { formatDate, setformOptionsNewValue } from '../../../util/tools'
import { isShowOperatorButtonCommon, isNeedShowOperatorColumn, isMobile } from '../../../util/common'


export default {
  components: { ShowAgvTaskPhaseList, ShowTaskPhaseList, Toolbar, SearchForm, ToolbarExport },//注册子组件 
  data () {
    return {
      //导出组件相关 
      export_exportFileName: 'AGV任务信息',//要导出的文件名 
      export_currentPageData: [],//当前页面的列表数据 
      export_columnHeader: { 'TaskNo': 'AGV任务号', 'LesTaskNo': 'LES任务号', 'TaskType': '任务类型', 'TaskTypeName': '任务类型名称', 'CallOrderNo': '叫料单号', 'SourcePlace': '起始点', 'SourcePlaceName': '起始点名称', 'ToPlace': '目标点', 'ToPlaceName': '目标点名称', 'MaterialId': '物料表ID', 'MaterialCode': '物料号', 'MaterialName': '物料名称', 'CargoNum': '货物数量', 'TaskStatus': '任务状态', 'TaskCreateTime': '任务创建时间', 'TaskIssueTime': '下发时间', 'TaskFinishTime': '完成时间', 'CreateBy': '创建者', 'CreateTime': '创建时间', 'ModifyBy': '修改者', 'ModifyTime': '修改时间', 'IsDeleted': '是否逻辑删除' },//当前页面列表的表头汉字和属性数组，导出用  
      //搜索框相关 
      c_commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准" 
      c_searchValControlStyle: {//设置通用搜索框的长度等样式  
        width: '300px',
      },
      c_searchFormInputPlaceholder: '请输入物料号/物料名称/车辆/AGV任务号/LES任务号/叫料单号/起始点编号/目标点编号/起始点名称/目标点名称/工件号/工件名称/操作说明/创建者/修改者',//要给子搜索组件传递的值 
      c_searchFormInputAttrs: ['MaterialCode', 'MaterialName', 'AgvNameDesc', 'TaskNo', 'LesTaskNo', 'CallOrderNo', 'SourceAgvStation', 'ToPlaceAgvStation', 'SourcePlaceName', 'ToPlaceName', 'MaterialCode', 'MaterialName', 'OperationRemark', 'CreateBy', 'ModifyBy'],//要给子搜索组件传递的属性名 
      h_controlStyle: {//设置高级搜索控件的长度等样式  
        width: '350px',
      },
      h_labelWidth: "100px",//显示Label的宽度 
      h_drawerSize: "600px",//drawner宽度设置 
      h_formOptions: [
        {
          label: 'AGV任务号',
          prop: 'TaskNo',
          element: 'el-input',
        },
        {
          label: 'LES任务号',
          prop: 'LesTaskNo',
          element: 'el-input',
        },
        {
          label: '任务类型',
          prop: 'TaskType',
          element: 'el-select',
          options: []
        },
        {
          label: '任务状态',
          prop: 'TaskStatus',
          element: 'el-select',
          options: []
        },
        {
          label: '物料号',
          prop: 'MaterialCode',
          element: 'el-input',
        },
        {
          label: '物料名称',
          prop: 'MaterialName',
          element: 'el-input',
        },
        {
          label: '叫料单号',
          prop: 'CallOrderNo',
          element: 'el-input',
        },
        {
          label: '起始点编号',
          prop: 'SourceAgvStation',
          element: 'el-input',
        },
        {
          label: '起始点名称',
          prop: 'SourcePlaceName',
          element: 'el-input',
        },
        {
          label: '目标点编号',
          prop: 'ToPlaceAgvStation',
          element: 'el-input',
        },
        {
          label: '目标点名称',
          prop: 'ToPlaceName',
          element: 'el-input',
        },
        {
          label: '工件号',
          prop: 'MaterialCode',
          element: 'el-input',
        },
        {
          label: '工件名称',
          prop: 'MaterialName',
          element: 'el-input',
        },
        {
          label: '任务创建时间',
          prop: 'TaskCreateTime',
          type: 'datetimerange',
          element: 'el-date-picker',
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
        },
      ],

      isShowOperatorColumn: true,// 是否显示【操作】列,true显示 false不显示 
      tableHeight: window.innerHeight - 180, // 控制表格的高度 

      isAdd: false, // 是否是新增 
      dialogTitle: '',

      filters: {
        name: ''
      },
      AgvTasks: [],
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

      },
    }
  },
  methods: {
    //显示 LES任务阶段列表
    showDetailForTaskPhaseList (selRow) {
      this.$refs.ShowTaskPhaseListMethod.showDetailForTaskPhaseList(selRow.LesTaskNo)
    },
    //显示 AGV任务阶段列表
    showDetailForAgvTaskPhaseList (selRow) {
      this.$refs.ShowAgvTaskPhaseListMethod.showDetailForAgvTaskPhaseList(selRow.Id)
    },
    getEnumberListForTaskStatus () {
      let para = {
        category: 'TaskStatusEnum'
      }
      getEnumberList(para).then((res) => {
        //给搜索框中的select控件赋值
        let propValue = "TaskStatus";
        this.h_formOptions = setformOptionsNewValue(this.h_formOptions, propValue, res.data.response);
        this.$refs.ChildSearchForm.setNewFormOptions(this.h_formOptions)
      });
    },
    getEnumberListForEAGVTaskType () {
      let para = {
        category: 'AGVTaskType'
      }
      getEnumberList(para).then((res) => {
        //给搜索框中的select控件赋值
        let propValue = "TaskType";
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
      this.getAgvTasks();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getAgvTasks();
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
    //获取列表 
    getAgvTasks (formValue) {
      //this.total = 0 
      //this.AgvTasks = null 
      let para = this.getPostParam('1');
      this.listLoading = true;
      getAgvTaskListPage(para).then((res) => {
        if (res.data.success) {
          this.total = res.data.response.dataCount;
          this.AgvTasks = res.data.response.data;
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
      getAgvTaskListPage(para).then((res) => {
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
        removeAgvTask(para).then((res) => {

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

          this.getAgvTasks();
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
      this.addForm = {
      };
      debugger
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
    // 更新数据 
    SaveData (formName) {
      this.submitDisabled = true

      this.$refs.addForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认提交吗？', '提示', {}).then(() => {
            this.addLoading = true;
            let para = Object.assign({}, this.addForm);
            if (this.isAdd) {//新增 
              addAgvTask(para).then((res) => {
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
                  this.getAgvTasks();
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
              editAgvTask(para).then((res) => {
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
                  this.getAgvTasks();
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
      this.export_currentPageData = this.AgvTasks
      this.$refs.cmToolbarExport.showPrintPage()
    },
  },
  mounted () {
    if (isMobile()) {
      this.h_drawerSize = "99%"//drawner宽度设置 
    } else {
      this.h_drawerSize = "600px"//drawner宽度设置 
    }
    this.getEnumberListForEAGVTaskType();
    this.getEnumberListForTaskStatus();
    this.getAgvTasks();

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
