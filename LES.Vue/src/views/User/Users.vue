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
        <searchForm :formOptions="formOptions"
                    :commonSearchOptionSet="commonSearchOptionSet"
                    :drawerSize="drawerSize"
                    :labelWidth="labelWidth"
                    :controlStyle="controlStyle"
                    :searchValControlStyle="searchValControlStyle"
                    :searchFormInputPlaceholder="searchFormInputPlaceholder"
                    :searchFormInputAttrs="searchFormInputAttrs"
                    ref="ChildSearchForm"
                    @onSearch="getUsers" />
      </el-form>
    </el-col>

    <!--列表-->
    <el-table :data="users"
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
                       width="80">
      </el-table-column>
      <el-table-column prop="uRealName"
                       label="昵称"
                       width=""
                       sortable>
      </el-table-column>
      <el-table-column prop="uLoginName"
                       label="登录名"
                       width=""
                       sortable>
      </el-table-column>
      <el-table-column prop="RoleNames"
                       label="角色"
                       width=""
                       sortable>
        <template slot-scope="scope">
          <el-tag v-for="item in scope.row.RoleNames"
                  :key="item.Id">{{item}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="sex"
                       label="性别"
                       width=""
                       :formatter="formatSex"
                       sortable>
      </el-table-column>
      <el-table-column prop="birth"
                       label="生日"
                       :formatter="formatBirth"
                       width=""
                       sortable>
      </el-table-column>
      <el-table-column prop="uStatus"
                       label="状态"
                       width=""
                       sortable>
        <template slot-scope="scope">
          <el-tag :type="scope.row.uStatus == 0  ? 'success' : 'danger'"
                  disable-transitions>{{scope.row.uStatus == 0 ? "正常":"禁用"}}
          </el-tag>
        </template>
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
        <el-form-item label="昵称"
                      prop="uRealName">
          <el-input v-model="editForm.uRealName"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="登录名"
                      prop="uLoginName">
          <el-input v-model="editForm.uLoginName"
                    auto-complete="off"></el-input>
        </el-form-item>
        <!--<el-form-item label="密码" prop="uLoginPWD">-->
        <!--<el-input v-model="editForm.uLoginPWD" show-password  auto-complete="off"></el-input>-->
        <!--</el-form-item>-->

        <el-form-item label="角色"
                      prop="RIDs">
          <el-select multiple
                     v-model="editForm.RIDs"
                     placeholder="请选择角色">
            <el-option :key="0"
                       :label="'未选择角色'"
                       :value="0"></el-option>
            <el-option v-for="item in roles"
                       :key="item.Id"
                       :label="item.Name"
                       :value="item.Id"></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="性别">
          <el-radio-group v-model="editForm.sex">
            <el-radio class="radio"
                      :label="1">男</el-radio>
            <el-radio class="radio"
                      :label="0">女</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="年龄">
          <el-input-number v-model="editForm.age"
                           :min="0"
                           :max="200"></el-input-number>
        </el-form-item>
        <el-form-item label="生日">
          <el-date-picker type="date"
                          placeholder="选择日期"
                          v-model="editForm.birth"></el-date-picker>
        </el-form-item>
        <el-form-item label="地址">
          <el-input type="textarea"
                    v-model="editForm.addr"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="editFormVisible = false"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="editSubmit"
                   :loading="editLoading"
                   icon="fa fa-send">提交</el-button>
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
        <el-form-item label="昵称"
                      prop="uRealName">
          <el-input v-model="addForm.uRealName"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="登录名"
                      prop="uLoginName">
          <el-input v-model="addForm.uLoginName"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="密码"
                      prop="uLoginPWD">
          <el-input v-model="addForm.uLoginPWD"
                    show-password
                    auto-complete="off"></el-input>
        </el-form-item>
        <!--<el-form-item label="姓名" prop="name">-->
        <!--<el-input v-model="addForm.name" auto-complete="off"></el-input>-->
        <!--</el-form-item>-->
        <el-form-item label="性别">
          <el-radio-group v-model="addForm.sex">
            <el-radio class="radio"
                      :label="1">男</el-radio>
            <el-radio class="radio"
                      :label="0">女</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="年龄">
          <el-input-number v-model="addForm.age"
                           :min="0"
                           :max="200"></el-input-number>
        </el-form-item>
        <el-form-item label="生日">
          <el-date-picker type="date"
                          placeholder="选择日期"
                          v-model="addForm.birth"></el-date-picker>
        </el-form-item>
        <el-form-item label="地址">
          <el-input type="textarea"
                    v-model="addForm.addr"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           class="dialog-footer">
        <el-button @click.native="addFormVisible = false"
                   icon="fa fa-power-off">取消</el-button>
        <el-button type="primary"
                   @click.native="addSubmit"
                   :loading="addLoading"
                   icon="fa fa-send">提交</el-button>
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
import { getUserListPage, getAllRole, removeUser, batchRemoveUser, editUser, addUser } from '../../api/api';
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";
import searchForm from "../../components/SearchForm";
import ToolbarExport from "../../components/ToolbarExport";
import { isShowOperatorButtonCommon, isNeedShowOperatorColumn, isMobile } from '../../../util/common'


export default {
  components: { Toolbar, searchForm, ToolbarExport },//注册子组件
  data () {
    return {
      //导出组件相关
      exportFileName: '用户信息',//要导出的文件名
      currentPageData: [],//当前页面的列表数据
      exportColumnHeader: { 'uRealName': '昵称', 'uLoginName': '登录名', 'RoleNames': '角色' },//当前页面列表的表头汉字和属性数组，导出用 

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
      searchFormInputPlaceholder: '请输入昵称/登录名',//要给子搜索组件传递的值
      searchFormInputAttrs: ["uRealName", "uLoginName"],
      formOptions: [
        {
          label: '昵称',
          prop: 'uRealName',
          element: 'el-input'
        },
        {
          label: '登录名',
          prop: 'uLoginName',
          element: 'el-input'
        },
        {
          label: '角色名',
          prop: 'RoleNames',
          element: 'el-input'
        }
      ],

      isShowOperatorColumn: true,// 是否显示【操作】列,true显示 false不显示
      tableHeight: window.innerHeight - 180, // 控制表格的高度

      filters: {
        name: ''
      },
      users: [],
      roles: [],
      total: 0,
      buttonList: [],
      currentRow: null,
      page: 1,
      pageSize: 10,
      listLoading: false,
      sels: [],//列表选中列

      addDialogFormVisible: false,
      editFormVisible: false,//编辑界面是否显示
      editLoading: false,
      editFormRules: {
        uLoginName: [
          { required: true, message: '请输入登录名', trigger: 'blur' }
        ],
        uRealName: [
          { required: true, message: '请输入昵称', trigger: 'blur' }
        ],
        birth: [
          { required: true, message: '请填写生日', trigger: 'blur' }
        ]
      },
      //编辑界面数据
      editForm: {
        id: 0,
        uID: 0,
        RIDs: 0,
        uLoginName: '',
        uRealName: '',
        name: '',
        sex: -1,
        age: 0,
        birth: '',
        addr: ''
      },

      addFormVisible: false,//新增界面是否显示
      addLoading: false,
      addFormRules: {
        uLoginName: [
          { required: true, message: '请输入登录名', trigger: 'blur' }
        ],
        uRealName: [
          { required: true, message: '请输入昵称', trigger: 'blur' }
        ],
        uLoginPWD: [
          { required: true, message: '请输入密码', trigger: 'blur' }
        ],
        birth: [
          { required: true, message: '请填写生日', trigger: 'blur' }
        ]
      },
      //新增界面数据
      addForm: {
        name: '',
        uLoginName: '',
        uRealName: '',
        uLoginPWD: '',
        sex: -1,
        age: 0,
        birth: '',
        addr: ''
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
    selectCurrentRow (val) {
      this.currentRow = val;
    },
    callFunction (item) {
      this.filters = {
        name: item.search
      };
      this[item.Func].apply(this, item);
    },
    //性别显示转换
    formatSex: function (row, column) {
      return row.sex == 1 ? '男' : row.sex == 0 ? '女' : '未知';
    },
    formatBirth: function (row, column) {
      return (!row.birth || row.birth == '') ? '' : util.formatDate.format(new Date(row.birth), 'yyyy-MM-dd');
    },
    handleCurrentChange (val) {
      this.page = val;
      this.getUsers();
    },
    handleSizeChange (val) {
      this.pageSize = val;
      this.getUsers();
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
    //获取用户列表
    getUsers (formValue) {
      //this.total = 0
      //this.users = null
      let para = this.getPostParam('1');
      this.listLoading = true;
      getUserListPage(para).then((res) => {
        this.total = res.data.response.dataCount;
        this.users = res.data.response.data;
        this.listLoading = false;
      });
    },
    //获取全部用户列表
    exportAllData () {
      let para = this.getPostParam('2');
      this.listLoading = true;
      getUserListPage(para).then((res) => {
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
      this.$confirm('确认要删除用户 "' + row.uRealName + '" 吗？', '提示', {
        type: 'warning'
      }).then(() => {
        this.listLoading = true;
        //NProgress.start();
        let para = { id: row.uID };
        removeUser(para).then((res) => {

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

          this.getUsers();
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

      getAllRole().then((res) => {
        this.roles = res.data.response;
      });

    },
    //显示新增界面
    handleAdd () {
      this.addFormVisible = true;
      this.addForm = {
        uLoginName: '',
        uRealName: '',
        uLoginPWD: '',
        name: '',
        sex: -1,
        age: 0,
        birth: '',
        addr: ''
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

            para.birth = (!para.birth || para.birth == '') ? util.formatDate.format(new Date(), 'yyyy-MM-dd') : util.formatDate.format(new Date(para.birth), 'yyyy-MM-dd');

            editUser(para).then((res) => {

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
                this.getUsers();
              } else {
                this.editLoading = false;
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
      this.$refs.addForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认提交吗？', '提示', {}).then(() => {
            this.addLoading = true;
            //NProgress.start();
            let para = Object.assign({}, this.addForm);
            para.birth = (!para.birth || para.birth == '') ? util.formatDate.format(new Date(), 'yyyy-MM-dd') : util.formatDate.format(new Date(para.birth), 'yyyy-MM-dd');
            addUser(para).then((res) => {

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
                this.getUsers();
              }
              else {
                this.addLoading = false;
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
    },
    //批量删除
    batchRemove: function () {

      // return;

      var ids = this.sels.map(item => item.uID).toString();
      this.$confirm('确认要删除用户 "' + item.Name + '" 吗？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.listLoading = true;
        //NProgress.start();
        let para = { ids: ids };

        batchRemoveUser(para).then((res) => {
          this.listLoading = false;
          //NProgress.done();
          this.$message({
            message: '该功能未开放',
            type: 'warning'
          });
          console.log(res)
        });
      }).catch(() => {

      });
    },
    //导出
    handleExport () {
      this.currentPageData = this.users
      this.$refs.cmToolbarExport.showPrintPage()
    },
  },
  mounted () {
    this.getUsers();

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
