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
                    @onSearch="getRoles" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="Roles"
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
      <el-table-column prop="Name"
                       label="角色名"
                       align="center">
      </el-table-column>
      <el-table-column prop="Description"
                       label="说明"
                       align="center">
      </el-table-column>
      <el-table-column prop="Enabled"
                       label="状态"
                       width="200"
                       sortable>
        <template slot-scope="scope">
          <el-tag :type="scope.row.Enabled  ? 'success' : 'danger'"
                  disable-transitions>{{scope.row.Enabled ? "正常":"禁用"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="CreateBy"
                       label="创建人"
                       align="center">
      </el-table-column>
      <el-table-column prop="CreateTime"
                       label="创建时间"
                       width="150"
                       align="center"
                       :formatter="formatterDateTime">
      </el-table-column>
      <el-table-column prop="ModifyBy"
                       label="修改人"
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
    <!--编辑界面-->
    <el-dialog title="编辑"
               :visible.sync="editFormVisible"
               v-model="editFormVisible"
               :close-on-click-modal="false">
      <el-form :model="editForm"
               size="small"
               label-width="80px"
               :rules="editFormRules"
               ref="editForm">
        <el-form-item label="角色名"
                      prop="Name">
          <el-input v-model="editForm.Name"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="Enabled">
          <el-select v-model="editForm.Enabled"
                     placeholder="请选择角色状态">
            <el-option v-for="item in statusList"
                       :key="item.value"
                       :label="item.name"
                       :value="item.value"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="说明"
                      prop="Description">
          <el-input v-model="editForm.Description"
                    auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="editFormVisible = false">取消</el-button>
        <el-button type="primary"
                   @click.native="editSubmit"
                   :loading="editLoading">提交</el-button>
      </div>
    </el-dialog>

    <!--新增界面-->
    <el-dialog title="新增"
               :visible.sync="addFormVisible"
               v-model="addFormVisible"
               :close-on-click-modal="false">
      <el-form :model="addForm"
               size="small"
               label-width="80px"
               :rules="addFormRules"
               ref="addForm">
        <el-form-item label="角色名"
                      prop="Name">
          <el-input v-model="addForm.Name"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="Enabled">
          <el-select v-model="addForm.Enabled"
                     placeholder="请选择角色状态">
            <el-option label="激活"
                       value="true"></el-option>
            <el-option label="禁用"
                       value="false"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="说明"
                      prop="Description">
          <el-input v-model="addForm.Description"
                    auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="addFormVisible = false">取消</el-button>
        <el-button type="primary"
                   @click.native="addSubmit"
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
import { getRoleListPage, removeRole, editRole, addRole } from '../../api/api';
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
      exportFileName: '角色管理信息',//要导出的文件名 
      currentPageData: [],//当前页面的列表数据 
      tHeader: ['角色名', '说明'],//当前页面列表的表头汉字数组，导出用 
      filterVal: ['Name', 'Description'],//当前页面列表的表头属性数组，导出用 

      //搜索框相关 
      ChildSearchForm: {},
      commonSearchOptionSet: "精准",//通用查询的默认配置,"模糊"或"精准"
      searchValControlStyle: {//设置通用搜索框的长度等样式 
        width: '300px',
      },
      controlStyle: {//设置搜索控件的长度等样式 
        width: '200px',
      },
      labelWidth: "90px",//显示Label的宽度 
      drawerSize: "450px",//drawner宽度设置 
      searchFormInputPlaceholder: '请输入角色名/说明',//要给子搜索组件传递的值
      searchFormInputAttrs: ['Name', 'Description'],//要给子搜索组件传递的属性名
      formOptions: [
        {
          label: '角色名',
          prop: 'Name',
          element: 'el-input',
        },
        {
          label: '说明',
          prop: 'Description',
          element: 'el-input',
        },
        {
          label: '创建时间',
          prop: 'CreateTime',
          type: 'datetimerange',
          element: 'el-date-picker',
        }
      ],


      isShowOperatorColumn: true,// 是否显示【操作】列,true显示 false不显示
      tableHeight: window.innerHeight - 180, // 控制表格的高度

      isAdd: false, // 是否是新增 
      dialogTitle: '',

      statusList: [
        { name: "激活", value: true },
        { name: "禁用", value: false }
      ],
      filters: {
        name: ''
      },
      Roles: [],
      roles: [],
      total: 0,
      buttonList: [],
      currentRow: null,
      page: 1,
      pageSize: 10,
      listLoading: false,
      sels: [],//列表选中列 

      addDialogFormVisible: false,
      editFormVisible: false, //编辑界面是否显示
      editLoading: false,
      editFormRules: {
        Name: [{ required: true, message: "请输入角色名", trigger: "blur" }],
        Enabled: [{ required: true, message: "请选择状态", trigger: "blur" }]
      },
      //编辑界面数据
      editForm: {
        Id: 0,
        CreateBy: "",
        Name: "",
        Enabled: false
      },

      addFormVisible: false, //新增界面是否显示
      addLoading: false,
      addFormRules: {
        Name: [{ required: true, message: "请输入角色名", trigger: "blur" }],
        Enabled: [{ required: true, message: "请选择状态", trigger: "blur" }]
      },
      //新增界面数据
      addForm: {
        CreateBy: "",
        CreateId: "",
        Name: "",
        Enabled: true
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
      this.getRoles();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getRoles();
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
    getRoles (formValue) {
      //this.total = 0 
      //this.Roles = null 
      let para = this.getPostParam('1');
      this.listLoading = true;
      getRoleListPage(para).then((res) => {
        if (res.data.success) {
          this.total = res.data.response.dataCount;
          this.Roles = res.data.response.data;
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
      getRoleListPage(para).then((res) => {
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
        removeRole(para).then((res) => {

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

          this.getRoles();
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

      this.editFormVisible = true;
      this.editForm = Object.assign({}, row);
    },
    //显示新增界面
    handleAdd () {
      this.addFormVisible = true;
      this.addForm = {
        CreateBy: "",
        Name: "",
        Enabled: ""
      };
    },
    //编辑
    editSubmit: function () {
      this.$refs.editForm.validate(valid => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            this.editLoading = true;
            //NProgress.start();
            let para = Object.assign({}, this.editForm);

            para.birth =
              !para.birth || para.birth == ""
                ? util.formatDate.format(new Date(), "yyyy-MM-dd")
                : util.formatDate.format(new Date(para.birth), "yyyy-MM-dd");

            editRole(para).then(res => {
              if (util.isEmt.format(res)) {
                this.editLoading = false;
                return;
              }

              if (res.data.success) {
                this.editLoading = false;
                //NProgress.done();
                this.$message({
                  message: res.data.msg,
                  type: "success"
                });
                this.$refs["editForm"].resetFields();
                this.editFormVisible = false;
                this.getRoles();
              } else {
                this.$message({
                  message: res.data.msg,
                  type: "error"
                });
              }
            });
          });
        }
      });
    },
    //新增
    addSubmit: function () {
      let _this = this;
      this.$refs.addForm.validate(valid => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            this.addLoading = true;
            //NProgress.start();
            let para = Object.assign({}, this.addForm);
            para.birth =
              !para.birth || para.birth == ""
                ? util.formatDate.format(new Date(), "yyyy-MM-dd")
                : util.formatDate.format(new Date(para.birth), "yyyy-MM-dd");

            var user = JSON.parse(window.localStorage.user);

            if (user && user.uID > 0) {
              para.CreateId = user.uID;
              para.CreateBy = user.uRealName;
            } else {
              this.$message({
                message: "用户信息为空，先登录",
                type: "error"
              });
              _this.$router.replace(
                _this.$route.query.redirect ? _this.$route.query.redirect : "/"
              );
            }

            addRole(para).then(res => {
              if (util.isEmt.format(res)) {
                this.addLoading = false;
                return;
              }
              if (res.data.success) {
                this.addLoading = false;
                //NProgress.done();
                this.$message({
                  message: res.data.msg,
                  type: "success"
                });
                this.$refs["addForm"].resetFields();
                this.addFormVisible = false;
                this.getRoles();
              } else {
                this.$message({
                  message: res.data.msg,
                  type: "error"
                });
              }
            });
          });
        }
      });
    },
    selsChange: function (sels) {
      this.sels = sels;
    },
    //导出 
    handleExport () {
      this.currentPageData = this.Roles
      this.$refs.cmToolbarExport.showPrintPage()
    },
  },
  mounted () {
    this.getRoles();

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
