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
                    @onSearch="_getCodeItems" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="CodeItems"
              highlight-current-row
              size="small"
              :height="tableHeight"
              @current-change="selectCurrentRow"
              v-loading="listLoading"
              @selection-change="selsChange">
      <el-table-column type="index"
                       :index="indexMethod"
                       label="#"
                       align="center"
                       width="50">
      </el-table-column>
      <el-table-column prop="Id"
                       label="ID"
                       width="50"
                       align="center">
      </el-table-column>
      <el-table-column prop="setCodeCode"
                       label="代码集编码"
                       width="110"
                       align="center">
      </el-table-column>
      <el-table-column prop="setCodeName"
                       label="代码集名称"
                       align="center">
      </el-table-column>
      <el-table-column prop="code"
                       label="代码编码"
                       width="110"
                       align="center">
      </el-table-column>
      <el-table-column prop="name"
                       label="代码名称"
                       width="110"
                       align="center">
      </el-table-column>
      <el-table-column prop="note"
                       label="说明"
                       width="110"
                       align="center">
      </el-table-column>

      <el-table-column prop="delFlag"
                       label="状态"
                       align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.delFlag  ? 'danger' : 'success'"
                  disable-transitions>{{scope.row.delFlag ? "禁用":"启用"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="creator"
                       label="创建人"
                       align="center">
      </el-table-column>
      <el-table-column prop="createTime"
                       label="创建时间"
                       width="160"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
      <el-table-column prop="lastModifier"
                       label="修改人"
                       align="center">
      </el-table-column>
      <el-table-column prop="lastModifyTime"
                       label="修改时间"
                       width="160"
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
               v-model="addFormVisible"
               :close-on-click-modal="false">
      <el-form :model="addForm"
               size="small"
               label-width="100px"
               ref="addForm">
        <el-form-item label="代码集编码"
                      prop="setCode"
                      :rules="[{ required: true, message: '代码集编码不能为空'}]">
          <el-select v-model="addForm.setCode"
                     placeholder="请选择代码编码">
            <el-option v-for="item in allCodeSets"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="代码项编码"
                      :rules="[{ required: true, message: '代码项编码不能为空'}]"
                      prop="code">
          <el-input v-model="addForm.code"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="代码项名称"
                      :rules="[{ required: true, message: '代码项名称不能为空'}]"
                      prop="name">
          <el-input v-model="addForm.name"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="是否禁用"
                      prop="delFlag">
          <el-switch v-model="addForm.delFlag"></el-switch>
        </el-form-item>
        <el-form-item label="说明"
                      :rules="[{ required: true, message: '说明不能为空'}]"
                      prop="note">
          <el-input v-model="addForm.note"
                    auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="cancelSaveClick"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="SaveData"
                   :loading="addLoading"
                   icon="fa fa-send">提交</el-button>
        <el-button type="primary"
                   v-if="isAdd"
                   @click.native="continueAddClick"
                   icon="el-icon-plus"
                   :loading="addLoading">继续新增</el-button>
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
import { getCodeItemListPage, removeCodeItem, editCodeItem, addCodeItem, getAllCodeSets } from '../../api/api';
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
      allCodeSets: [],

      //导出组件相关
      exportFileName: '码表集信息',//要导出的文件名
      currentPageData: [],//当前页面的列表数据
      exportColumnHeader: { 'code': '代码编码', 'name': '代码名称', 'note': '说明' },//当前页面列表的表头汉字和属性数组，导出用 

      //搜索框相关
      commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准"
      searchValControlStyle: {//设置通用搜索框的长度等样式 
        width: '300px',
      },
      controlStyle: {//设置搜索控件的长度等样式
        width: '350px',
      },
      labelWidth: "90px",//显示Label的宽度
      drawerSize: "600px",//drawner宽度设置
      searchFormInputPlaceholder: '请输入代码编码/名称',//要给子搜索组件传递的值
      searchFormInputAttrs: ['code', 'name'],//要给子搜索组件传递的属性名
      formOptions: [
        {
          label: '代码集编码',
          prop: 'setCode',
          element: 'el-select',
          options: [] //先默认为空，再通过请求获取数据
        },
        {
          label: '代码编码',
          prop: 'code',
          element: 'el-input'
        },
        {
          label: '代码名称',
          prop: 'name',
          element: 'el-input'
        },
        {
          label: '说明',
          prop: 'note',
          element: 'el-input'
        }
      ],

      isShowOperatorColumn: true,// 是否显示【操作】列,true显示 false不显示
      tableHeight: window.innerHeight - 180, // 控制表格的高度

      isAdd: false, // 是否是新增
      dialogTitle: '',

      filters: {
        name: ''
      },
      CodeItems: [],
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
        id: 0,
        name: '',
        code: '',
        note: '',
        delFlag: false
      }

    }
  },
  methods: {
    //是否显示某个操作按钮
    isShowOperatorButton (flag) {
      return isShowOperatorButtonCommon(this.isShowOperatorColumn, flag, this.buttonList)
    },
    // 获取所有代码集
    getAllCodeSet: function () {
      getAllCodeSets().then(res => {
        if (res.data.success) {
          this.allCodeSets = res.data.response

          //给搜索框中的select控件赋值
          let propValue = "setCode";
          this.formOptions = setformOptionsNewValue(this.formOptions, propValue, this.allCodeSets);
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
      this.getCodeItems();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getCodeItems();
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
    _getCodeItems () {
      this.page = 1
      this.getCodeItems()
    },
    //获取列表
    getCodeItems (formValue) {
      //this.total = 0
      //this.CodeItems = null
      let para = this.getPostParam('1');
      this.listLoading = true;
      getCodeItemListPage(para).then((res) => {
        if (res.data.success) {
          this.total = res.data.response.dataCount;
          this.CodeItems = res.data.response.data;
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
      getCodeItemListPage(para).then((res) => {
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
        removeCodeItem(para).then((res) => {

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

          this.getCodeItems();
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
    //触发取消按钮
    cancelSaveClick () {
      this.$refs['addForm'].resetFields();
      this.addFormVisible = false;
      this.getCodeItems();
    },
    //触发继续新增按钮
    continueAddClick () {
      this.handleAdd()
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
              addCodeItem(para).then((res) => {

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
                  this.getCodeItems();
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
              editCodeItem(para).then((res) => {

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
      this.currentPageData = this.CodeItems
      this.$refs.cmToolbarExport.showPrintPage()
    },
  },
  mounted () {
    this.getAllCodeSet()
    this.getCodeItems()

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
