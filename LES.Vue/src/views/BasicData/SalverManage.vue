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
                    :searchFormInputPlaceholder="searchFormInputPlaceholder"
                    :searchFormInputAttrs="searchFormInputAttrs"
                    ref="ChildSearchForm"
                    @onSearch="getSalvers" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="Salvers"
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
      <el-table-column prop="SalverCode"
                       label="托盘编号"
                       align="center">
      </el-table-column>
      <el-table-column prop="SalverType"
                       label="托盘类型"
                       align="center">
      </el-table-column>
      <el-table-column prop="AllowLine"
                       label="所属产线"
                       align="center">
      </el-table-column>
      <el-table-column prop="CreateId"
                       label="创建者ID"
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
      <el-table-column prop="ModifyId"
                       label="修改ID"
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
      <el-table-column label="操作"
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
               v-model="addFormVisible"
               :close-on-click-modal="false">
      <el-form :model="addForm"
               size="small"
               label-width="80px"
               ref="addForm">
        <el-form-item label="托盘编号"
                      prop="SalverCode"
                      :rules="[{ required: true, message: '托盘编号不能为空'}]">
          <el-input v-model="addForm.SalverCode"></el-input>
        </el-form-item>
        <el-form-item label="托盘类型"
                      prop="SalverType"
                      :rules="[{ required: true, message: '托盘类型不能为空'}]">
          <el-input v-model="addForm.SalverType"></el-input>
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
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="addFormVisible = false">取消</el-button>
        <el-button type="primary"
                   @click.native="SaveData"
                   :loading="addLoading">提交</el-button>
      </div>
    </el-dialog>
    <!-- 导出组件 -->
    <ToolbarExport ref="cmToolbarExport"
                   :exportFileName="exportFileName"
                   :filterVal="filterVal"
                   :currentPageData="currentPageData"
                   :tHeader="tHeader" />
  </section>
</template> 
 
<script> 
import util from '../../../util/date'
import { getCodeItemsBySetCode, getSalverListPage, removeSalver, editSalver, addSalver } from '../../api/api';
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";
import SearchForm from "../../components/SearchForm";
import ToolbarExport from "../../components/ToolbarExport";
import { formatDate } from '../../../util/tools'
import { isShowOperatorButtonCommon, isNeedShowOperatorColumn } from '../../../util/common'


export default {
  components: { Toolbar, SearchForm, ToolbarExport },//注册子组件 
  data () {
    return {
      //导出组件相关 
      exportFileName: '$ChinaComment$信息',//要导出的文件名 
      currentPageData: [],//当前页面的列表数据 
      tHeader: ['托盘编号', '托盘类型', '所属产线', '创建者ID', '创建者', '修改ID', '修改者', '修改时间'],//当前页面列表的表头汉字数组，导出用 
      filterVal: ['SalverCode', 'SalverType', 'AllowLine', 'CreateId', 'CreateBy', 'ModifyId', 'ModifyBy', 'ModifyTime'],//当前页面列表的表头属性数组，导出用 

      //搜索框相关 
      commonSearchOptionSet: "精准",//通用查询的默认配置,"模糊"或"精准"
      controlStyle: {//设置搜索控件的长度等样式
        width: '300px',
      },
      labelWidth: "90px",//显示Label的宽度
      drawerSize: "450px",//drawner宽度设置 
      searchFormInputPlaceholder: '请输入托盘编号/托盘类型',//要给子搜索组件传递的值 
      searchFormInputAttrs: ['SalverCode', 'SalverType'],//要给子搜索组件传递的属性名
      formOptions: [
        {
          label: '托盘编号',
          prop: 'SalverCode',
          element: 'el-input',
        },
        {
          label: '托盘类型',
          prop: 'SalverType',
          element: 'el-input',
        },
        {
          label: '所属产线',
          prop: 'AllowLine',
          element: 'el-input',
        },
        {
          label: '创建者ID',
          prop: 'CreateId',
          element: 'el-input',
        },
        {
          label: '创建者',
          prop: 'CreateBy',
          element: 'el-input',
        },
        {
          label: '修改ID',
          prop: 'ModifyId',
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

      isAdd: false, // 是否是新增 
      dialogTitle: '',

      filters: {
        name: ''
      },
      Salvers: [],
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
    //是否显示某个操作按钮
    isShowOperatorButton (flag) {
      return isShowOperatorButtonCommon(this.isShowOperatorColumn, flag, this.buttonList)
    },
    //通过代码集获取代码项集合 
    getCodeItemsBySetCode () {
      let para = {
        setCode: '002' // 车间码表集
      }
      getCodeItemsBySetCode(para).then((res) => {
        this.workshopCodeItems = res.data.response;
        //给搜索框中的select控件赋值
        let propValue = "AllowLine";
        this.formOptions = setformOptionsNewValue(this.formOptions, propValue, this.workshopCodeItems);
        this.$refs.ChildSearchForm.setNewFormOptions(this.formOptions)
      });
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
      this.getSalvers();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getSalvers();
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
    //获取码表集列表 
    getSalvers (formValue) {
      //this.total = 0 
      //this.Salvers = null 
      let para = this.getPostParam('1');
      this.listLoading = true;
      getSalverListPage(para).then((res) => {
        this.total = res.data.response.dataCount;
        this.Salvers = res.data.response.data;
        this.listLoading = false;
      });
    },
    //获取全部列表 
    exportAllData () {
      let para = this.getPostParam('2');
      this.listLoading = true;
      getSalverListPage(para).then((res) => {
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
        //NProgress.start(); 
        let para = { id: row.Id };
        debugger
        removeSalver(para).then((res) => {

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

          this.getSalvers();
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
              addSalver(para).then((res) => {

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
                  this.getSalvers();
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
              editSalver(para).then((res) => {

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
                  this.getSalvers();
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
      this.currentPageData = this.Salvers
      this.$refs.cmToolbarExport.showPrintPage()
    },
  },
  mounted () {
    this.getCodeItemsBySetCode();
    this.getSalvers();

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
