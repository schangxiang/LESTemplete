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
                    @onSearch="_getStockInfos" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="StockInfos"
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
                       align="center">
      </el-table-column>
      <el-table-column prop="StationName"
                       label="站点名称"
                       width="150"
                       align="center">
      </el-table-column>
      <el-table-column prop="BurdenWorkNo"
                       label="配料任务号"
                       align="center">
      </el-table-column>
      <el-table-column prop="MaterialCode"
                       label="物料编号"
                       align="center">
      </el-table-column>
      <el-table-column prop="MaterialName"
                       label="物料名称"
                       align="center">
      </el-table-column>
      <el-table-column prop="CargoTypeName"
                       label="物料类型"
                       align="center">
      </el-table-column>
      <el-table-column prop="AgvName"
                       label="AGV车辆"
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
          <el-tag :type="scope.row.IsLock  ? 'danger' : 'success'"
                  disable-transitions>{{scope.row.IsLock===1 ? "是":"否"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="IsFull"
                       label="是否有货"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.IsFull  ? 'danger' : 'success'"
                  disable-transitions>{{scope.row.IsFull===1 ? "是":"否"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="CargoNum"
                       label="数量"
                       align="center">
      </el-table-column>
      <el-table-column prop="CargoWeight"
                       label="重量"
                       align="center">
      </el-table-column>
      <el-table-column prop="Unit"
                       label="单位"
                       align="center">
      </el-table-column>
      <el-table-column prop="InStoreTime"
                       label="入库时间"
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
    <!-- 导出组件 -->
    <ToolbarExport ref="cmToolbarExport"
                   :exportFileName="export_exportFileName"
                   :currentPageData="export_currentPageData"
                   :exportColumnHeader="exportColumnHeader" />
  </section>
</template> 
 
<script> 
import util from '../../../util/date'
import { getCodeItemsBySetCodeForGX, getStockInfoCollectListPage, removeStockInfo, editStockInfo, addStockInfo } from '../../api/api';
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";
import SearchForm from "../../components/SearchForm";
import ToolbarExport from "../../components/ToolbarExport";
import { formatDate, setformOptionsNewValue } from '../../../util/tools'


export default {
  components: { Toolbar, SearchForm, ToolbarExport },//注册子组件 
  data () {
    return {
      //导出组件相关 
      export_exportFileName: '库存信息信息',//要导出的文件名 
      export_currentPageData: [],//当前页面的列表数据 
      exportColumnHeader: {},//当前页面列表的表头汉字和属性数组，导出用 
      //搜索框相关 
      commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准" 
      searchValControlStyle: {//设置通用搜索框的长度等样式 
        width: '300px',
      },
      controlStyle: {//设置搜索控件的长度等样式 
        width: '300px',
      },
      labelWidth: "90px",//显示Label的宽度 
      drawerSize: "550px",//drawner宽度设置 
      searchFormInputPlaceholder: '请输入站点编号/配料任务号/AGV车辆',//要给子搜索组件传递的值 
      searchFormInputAttrs: ['StationCode', 'BurdenWorkNo', 'AgvName'],//要给子搜索组件传递的属性名 
      formOptions: [
        {
          label: '配料任务号',
          prop: 'BurdenWorkNo',
          element: 'el-input',
        },
        {
          label: '所属工序',
          prop: 'AllowProcedure',
          element: 'el-select',
          options: [] //先默认为空，再通过请求获取数据
        },
        {
          label: 'AGV车辆',
          prop: 'AgvName',
          element: 'el-input',
        }
      ],


      tableHeight: window.innerHeight - 180, // 控制表格的高度

      isAdd: false, // 是否是新增 
      dialogTitle: '',

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
      addForm: {

      }

    }
  },
  methods: {
    // 查询工序列表，使用权限，通过代码集编码获取代码项集合
    _getCodeItemsBySetCodeForGX: function () {
      let para = {
      }
      getCodeItemsBySetCodeForGX(para).then(res => {
        if (res.data.success) {
          //给搜索框中的select控件赋值
          let propValue = "AllowProcedure";
          this.formOptions = setformOptionsNewValue(this.formOptions, propValue, res.data.response);
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
      getStockInfoCollectListPage(para).then((res) => {
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
      getStockInfoCollectListPage(para).then((res) => {
        var allData = res.data.response.data;
        this.$refs.cmToolbarExport.export2Excel(allData);
        this.listLoading = false;
      });
    },
    //删除 
    handleDel () {
      let row = this.currentRow;
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
    handleEdit () {
      let row = this.currentRow;
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
      this.export_currentPageData = this.StockInfos
      this.$refs.cmToolbarExport.showPrintPage()
    },
  },
  mounted () {
    this._getCodeItemsBySetCodeForGX()
    this.getStockInfos();

    let routers = window.localStorage.router
      ? JSON.parse(window.localStorage.router)
      : [];
    this.buttonList = getButtonList(this.$route.path, routers);

  }
}

</script> 
 
<style lang="scss" scoped>
</style> 
