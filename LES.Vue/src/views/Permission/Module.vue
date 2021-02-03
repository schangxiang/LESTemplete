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
                    @onSearch="getModules" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="users"
              size="small"
              :height="tableHeight"
              highlight-current-row
              @current-change="selectCurrentRow"
              v-loading="listLoading"
              @selection-change="selsChange"
              style="width: 100%;">
      <el-table-column type="index"
                       :index="indexMethod"
                       label="#"
                       align="center"
                       width="50">
      </el-table-column>
      <el-table-column prop="LinkUrl"
                       label="接口地址"
                       width=""
                       sortable>
      </el-table-column>
      <el-table-column prop="Name"
                       label="描述"
                       width="300"
                       sortable>
      </el-table-column>
      <el-table-column prop="CreateTime"
                       label="创建时间"
                       :formatter="formatCreateTime"
                       width=""
                       sortable>
      </el-table-column>
      <el-table-column prop="CreateBy"
                       label="创建者"
                       width=""
                       sortable>
      </el-table-column>
      <el-table-column prop="Enabled"
                       label="状态"
                       width=""
                       sortable>
        <template slot-scope="scope">
          <el-tag :type="scope.row.Enabled  ? 'success' : 'danger'"
                  disable-transitions>{{scope.row.Enabled ? "正常":"禁用"}}
          </el-tag>
        </template>
      </el-table-column>
      <!-- <el-table-column label="操作" width="150">
                <template scope="scope">
                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
                    <el-button type="danger" size="small" @click="handleDel(scope.$index, scope.row)">删除</el-button>
                </template>
            </el-table-column> -->
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
               size="small"
               :visible.sync="editFormVisible"
               v-model="editFormVisible"
               :close-on-click-modal="false">
      <el-form :model="editForm"
               label-width="80px"
               :rules="editFormRules"
               ref="editForm">
        <el-form-item label="接口地址"
                      prop="LinkUrl">
          <el-input v-model="editForm.LinkUrl"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="接口描述"
                      prop="Name">
          <el-input v-model="editForm.Name"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="Enabled">
          <el-select v-model="editForm.Enabled"
                     placeholder="请选择状态">
            <el-option v-for="item in statusList"
                       :key="item.value"
                       :label="item.LinkUrl"
                       :value="item.value"></el-option>

          </el-select>
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
               size="small"
               :visible.sync="addFormVisible"
               v-model="addFormVisible"
               :close-on-click-modal="false">
      <el-form :model="addForm"
               label-width="80px"
               :rules="addFormRules"
               ref="addForm">
        <el-form-item label="接口地址"
                      prop="LinkUrl">
          <el-input v-model="addForm.LinkUrl"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="接口描述"
                      prop="Name">
          <el-input v-model="addForm.Name"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="Enabled">
          <el-select v-model="addForm.Enabled"
                     placeholder="请选择状态">
            <el-option label="激活"
                       value="true"></el-option>
            <el-option label="禁用"
                       value="false"></el-option>
          </el-select>
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
  </section>
</template>

<script>
import util from '../../../util/date'
import { getModuleListPage, removeModule, editModule, addModule } from '../../api/api';
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";
import SearchForm from "../../components/SearchForm";
import ToolbarExport from "../../components/ToolbarExport";
import { formatDate } from '../../../util/tools'

export default {
  components: { Toolbar, SearchForm, ToolbarExport },//注册子组件 
  data () {
    return {

      //导出组件相关 
      exportFileName: '角色管理信息',//要导出的文件名 
      currentPageData: [],//当前页面的列表数据 
      tHeader: ['接口地址', '描述'],//当前页面列表的表头汉字数组，导出用 
      filterVal: ['LinkUrl', 'Name'],//当前页面列表的表头属性数组，导出用 

      //搜索框相关 
      ChildSearchForm: {},
      commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准"
      searchValControlStyle: {//设置通用搜索框的长度等样式 
        width: '300px',
      },
      controlStyle: {//设置搜索控件的长度等样式 
        width: '200px',
      },
      labelWidth: "90px",//显示Label的宽度 
      drawerSize: "450px",//drawner宽度设置 
      searchFormInputPlaceholder: '请输入接口地址/描述',//要给子搜索组件传递的值
      searchFormInputAttrs: ['LinkUrl', 'Name'],//要给子搜索组件传递的属性名
      formOptions: [
        {
          label: '接口地址',
          prop: 'LinkUrl',
          element: 'el-input',
        },
        {
          label: '描述',
          prop: 'Name',
          element: 'el-input',
        },
        {
          label: '创建时间',
          prop: 'CreateTime',
          type: 'datetimerange',
          element: 'el-date-picker',
        }
      ],


      tableHeight: window.innerHeight - 180, // 控制表格的高度

      buttonList: [],
      currentRow: null,
      filters: {
        LinkUrl: ''
      },
      users: [],
      statusList: [{ LinkUrl: '激活', value: true }, { LinkUrl: '禁用', value: false }],
      total: 0,
      page: 1,
      pageSize: 10,
      listLoading: false,
      sels: [],//列表选中列

      addDialogFormVisible: false,
      editFormVisible: false,//编辑界面是否显示
      editLoading: false,
      editFormRules: {

        LinkUrl: [
          { required: true, message: '请输入接口地址', trigger: 'blur' }
        ],

      },
      //编辑界面数据
      editForm: {
        Id: 0,
        CreateBy: '',
        LinkUrl: '',
        Name: '',
        Enabled: false,
      },

      addFormVisible: false,//新增界面是否显示
      addLoading: false,
      addFormRules: {

        LinkUrl: [
          { required: true, message: '请输入接口地址', trigger: 'blur' }
        ],

      },
      //新增界面数据
      addForm: {
        CreateBy: '',
        CreateId: '',
        LinkUrl: '',
        Name: '',
        Enabled: '',
      }

    }
  },
  methods: {
    // 设置索引 
    indexMethod (index) {
      return (this.page - 1) * this.pageSize + 1 + index
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
      this.getModules();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getModules();
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
    //性别显示转换
    formatEnabled: function (row, column) {
      return row.Enabled ? '正常' : '未知';
    },
    formatCreateTime: function (row, column) {
      return (!row.CreateTime || row.CreateTime == '') ? '' : util.formatDate.format(new Date(row.CreateTime), 'yyyy-MM-dd');
    },
    handleCurrentChange (val) {
      this.page = val;
      this.getModules();
    },
    //获取用户列表
    getModules () {
      let para = this.getPostParam('1');
      this.listLoading = true;

      //NProgress.start();
      getModuleListPage(para).then((res) => {
        this.total = res.data.response.dataCount;
        this.users = res.data.response.data;
        this.listLoading = false;
        //NProgress.done();
      });
    },
    //获取全部列表 
    exportAllData () {
      let para = this.getPostParam('2');
      this.listLoading = true;
      getModuleListPage(para).then((res) => {
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
        //NProgress.start();
        let para = { id: row.Id };
        removeModule(para).then((res) => {

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

          this.getModules();
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
      this.editFormVisible = true;
      this.editForm = Object.assign({}, row);
    },
    //显示新增界面
    handleAdd () {
      this.addFormVisible = true;
      this.addForm = {
        CreateBy: '',
        LinkUrl: '',
        Name: '',
        Enabled: 'true',
      };
    },
    //编辑
    editSubmit: function () {
      this.$refs.editForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认提交吗？', '提示', {}).then(() => {
            this.editLoading = true;
            //NProgress.start();
            let para = Object.assign({}, this.editForm);

            para.ModifyTime = util.formatDate.format(new Date(), 'yyyy-MM-dd');

            editModule(para).then((res) => {

              if (util.isEmt.format(res)) {
                this.editLoading = false;
                return;
              }
              if (res.data.success) {
                this.editLoading = false;
                //NProgress.done();
                this.$message({
                  message: res.data.msg,
                  type: 'success'
                });
                this.$refs['editForm'].resetFields();
                this.editFormVisible = false;
                this.getModules();
              } else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
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
      this.$refs.addForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认提交吗？', '提示', {}).then(() => {
            this.addLoading = true;
            //NProgress.start();
            let para = Object.assign({}, this.addForm);

            para.CreateTime = util.formatDate.format(new Date(), 'yyyy-MM-dd');
            para.ModifyTime = para.CreateTime;
            para.IsDeleted = false;

            var user = JSON.parse(window.localStorage.user);

            if (user && user.uID > 0) {
              para.CreateId = user.uID;
              para.CreateBy = user.uRealName;
            } else {
              this.$message({
                message: '用户信息为空，先登录',
                type: 'error'
              });
              _this.$router.replace(_this.$route.query.redirect ? _this.$route.query.redirect : "/");
            }


            addModule(para).then((res) => {

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
                this.getModules();
              }
              else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
                });

              }

            });

          });
        }
      });
    },
    selsChange: function (sels) {
      this.sels = sels;
    }
  },
  mounted () {
    this.getModules();

    let routers = window.localStorage.router
      ? JSON.parse(window.localStorage.router)
      : [];
    this.buttonList = getButtonList(this.$route.path, routers);
  }
}

</script>

<style scoped>
</style>
