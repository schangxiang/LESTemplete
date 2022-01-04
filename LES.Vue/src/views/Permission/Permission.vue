<template>
  <section>
    <!--工具条-->
    <toolbar :buttonList="buttonList"
             @callFunction="callFunction"></toolbar>

    <!--列表-->
    <el-table :data="users"
              :height="tableHeight"
              highlight-current-row
              v-loading="listLoading"
              @selection-change="selsChange"
              @current-change="selectCurrentRow"
              row-key="Id"
              border
              lazy
              size="small"
              :load="load"
              :tree-props="{children: 'children', hasChildren: 'hasChildren'}"
              style="width: 100%;">
      <el-table-column type="index"
                       label="#"
                       width="80"></el-table-column>
      <el-table-column label="菜单/按钮"
                       width="200"
                       sortable>
        <template slot-scope="scope">
          <i class="fa"
             :class="scope.row.Icon"></i>

          {{scope.row.Name}}
        </template>
      </el-table-column>
      <!-- <el-table-column prop="PnameArr" label="父节点" width="" sortable>
      </el-table-column>-->
      <el-table-column prop="FllowSystemName"
                       label="所属系统"
                       width
                       sortable></el-table-column>
      <el-table-column prop="Code"
                       label="路由地址"
                       width
                       sortable></el-table-column>
      <el-table-column prop="MName"
                       label="API接口"
                       width
                       sortable></el-table-column>
      <el-table-column prop="OrderSort"
                       label="排序"
                       width="80"
                       sortable></el-table-column>
      <el-table-column prop="Icon"
                       label="图标"
                       width="150"
                       sortable></el-table-column>
      <el-table-column prop="IsButton"
                       label="是否按钮"
                       width="100"
                       sortable>
        <template slot-scope="scope">
          <el-tag :type="!scope.row.IsButton  ? 'success' : 'danger'"
                  disable-transitions>{{!scope.row.IsButton ? "否":"是"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="ButtonType"
                       label="按钮类型"
                       width="90"
                       sortable></el-table-column>
      <el-table-column prop="Func"
                       label="按钮事件"
                       width
                       sortable></el-table-column>
      <el-table-column prop="IsHide"
                       label="是否隐藏"
                       width="100"
                       sortable>
        <template slot-scope="scope">
          <el-tag :type="!scope.row.IsHide  ? 'success' : 'danger'"
                  disable-transitions>{{!scope.row.IsHide ? "否":"是"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="IskeepAlive"
                       label="keepAlive"
                       width="100"
                       sortable>
        <template slot-scope="scope">
          <el-tag :type="!scope.row.IskeepAlive  ? 'success' : 'danger'"
                  disable-transitions>{{!scope.row.IskeepAlive ? "否":"是"}}</el-tag>
        </template>
      </el-table-column>
      <!-- <el-table-column label="操作" width="150">
                <template scope="scope">
                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
                    <el-button type="danger" size="small" @click="handleDel(scope.$index, scope.row)">删除</el-button>
                </template>
      </el-table-column>-->
    </el-table>

    <!--工具条-->
    <!-- <el-col :span="24"
            class="toolbar">
      <el-pagination layout="prev, pager, next"
                     @current-change="handleCurrentChange"
                     :page-size="50"
                     :total="total"
                     style="float:right;"></el-pagination>
    </el-col> -->

    <!--编辑界面-->
    <el-dialog title="编辑"
               :visible.sync="editFormVisible"
               v-model="editFormVisible"
               :close-on-click-modal="false">
      <el-form :model="editForm"
               label-width="80px"
               :rules="editFormRules"
               ref="editForm">
        <el-form-item label="菜单名称"
                      prop="Name">
          <el-input v-model="editForm.Name"
                    auto-complete="off"></el-input>
        </el-form-item>

        <el-form-item label="路由地址"
                      prop="Code">
          <el-tooltip placement="top">
            <div slot="content">
              如果是目录，请填‘-’字符
              <br />如果是按钮，请输入空格即可
              <br />如果是外链，请带上协议，比如 https://www.baidu.com
              <br />注意：地址区分大小写！！！！
            </div>
            <el-input v-model="editForm.Code"
                      auto-complete="off"></el-input>
          </el-tooltip>
        </el-form-item>
        <el-form-item label="所属系统">
          <el-radio-group v-model="editForm.FllowSystemName"
                          @change="clkFllowSystemForEdit">
            <el-radio label="公共"></el-radio>
            <el-radio label="低压线圈控制系统"></el-radio>
            <el-radio label="物流LES系统"></el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="描述"
                      prop="Description">
          <el-input v-model="editForm.Description"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="Icon"
                      prop="Icon">
          <el-input v-model="editForm.Icon"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="Enabled">
          <el-select v-model="editForm.Enabled"
                     placeholder="请选择状态">
            <el-option v-for="item in statusList"
                       :key="item.value"
                       :label="item.Name"
                       :value="item.value"></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="排序"
                      prop="OrderSort">
          <el-input type="number"
                    v-model="editForm.OrderSort"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item prop="IsButton"
                      label="是否按钮"
                      width
                      sortable>
          <el-switch v-model="editForm.IsButton"></el-switch>
        </el-form-item>
        <el-form-item label="按钮事件"
                      prop="Func">
          <el-tooltip class="item"
                      effect="dark"
                      content="只写方法名即可，不用带括号，比如：handleEdit"
                      placement="top-start">
            <el-select v-model="editForm.Func"
                       filterable
                       allow-create
                       default-first-option
                       placeholder="请选择按钮事件或者输入">
              <el-option v-for="item in funcOptions"
                         :key="item.value"
                         :label="item.label"
                         :value="item.value">
                <span style="float: left">{{ item.label }}</span>
                <span style="float: right; color: #8492a6; font-size: 13px">{{ item.value }}</span>
              </el-option>
            </el-select>
          </el-tooltip>
        </el-form-item>
        <el-form-item label="按钮类型"
                      prop="ButtonType">
          <el-select v-model="editForm.ButtonType"
                     filterable
                     default-first-option
                     placeholder="请选择按钮类型">
            <el-option v-for="item in buttonTypeOptions"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
              <span style="float: left">{{ item.label }}</span>
              <span style="float: right; color: #8492a6; font-size: 13px">{{ item.value }}</span>
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item prop="IsHide"
                      label="隐藏菜单"
                      width
                      sortable>
          <el-switch v-model="editForm.IsHide"></el-switch>
        </el-form-item>
        <el-form-item prop="IskeepAlive"
                      label="keepAlive"
                      width
                      sortable>
          <el-switch v-model="editForm.IskeepAlive"></el-switch>
        </el-form-item>
        <el-form-item prop="PidArr"
                      label="父级菜单"
                      width
                      sortable>
          <el-cascader placeholder="请选择，支持搜索功能"
                       style="width: 400px"
                       v-model="editForm.PidArr"
                       :options="options"
                       filterable
                       change-on-select></el-cascader>
        </el-form-item>
        <el-form-item prop="Mid"
                      label="API接口"
                      width
                      sortable>
          <el-select style="width: 100%;"
                     v-model="editForm.Mid"
                     placeholder="请选择API">
            <el-option :key="0"
                       :value="0"
                       :label="'无需api'"></el-option>
            <el-option v-for="item in modules"
                       :key="item.Id"
                       :value="item.Id"
                       :label="item.LinkUrl">
              <span style="float: left">{{ item.LinkUrl }}</span>
              <span style="float: right; color: #8492a6; font-size: 13px">{{ item.Name }}</span>
            </el-option>
          </el-select>
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
               label-width="80px"
               :rules="addFormRules"
               ref="addForm">
        <el-form-item label="菜单名称"
                      prop="Name">
          <el-input v-model="addForm.Name"
                    auto-complete="off"></el-input>
        </el-form-item>

        <el-form-item label="菜单类型">
          <el-radio-group @change="clkType"
                          v-model="addForm.MenuType">
            <el-radio label="目录"></el-radio>
            <el-radio label="页面"></el-radio>
            <el-radio label="按钮"></el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="所属系统"
                      v-if="showFllowSystem">
          <el-radio-group @change="clkFllowSystem"
                          v-model="addForm.FllowSystemName">
            <el-radio label="公共"></el-radio>
            <el-radio label="低压线圈控制系统"></el-radio>
            <el-radio label="物流LES系统"></el-radio>
          </el-radio-group>
        </el-form-item>

        <el-form-item label="路由地址"
                      prop="Code">
          <el-tooltip placement="top">
            <div slot="content">
              如果是目录，请填‘-’字符
              <br />如果是按钮，请输入空格即可
              <br />如果是外链，请带上协议，比如 https://www.baidu.com
              <br />注意：地址区分大小写！！！！
            </div>
            <el-input v-model="addForm.Code"
                      auto-complete="off"></el-input>
          </el-tooltip>

        </el-form-item>
        <el-form-item label="描述"
                      prop="Description">
          <el-input v-model="addForm.Description"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="Icon"
                      prop="Icon">
          <el-input v-model="addForm.Icon"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="状态"
                      prop="Enabled">
          <el-select v-model="addForm.Enabled"
                     placeholder="请选择状态">
            <el-option v-for="item in statusList"
                       :key="item.value"
                       :label="item.Name"
                       :value="item.value"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="排序"
                      prop="OrderSort">
          <el-input v-model="addForm.OrderSort"
                    auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item prop="IsButton"
                      label="是否按钮"
                      width
                      sortable>
          <el-switch v-model="addForm.IsButton"></el-switch>
        </el-form-item>
        <el-form-item label="按钮事件"
                      prop="Func">
          <el-select v-model="addForm.Func"
                     filterable
                     allow-create
                     default-first-option
                     placeholder="请选择按钮事件或者输入">
            <el-option v-for="item in funcOptions"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
              <span style="float: left">{{ item.label }}</span>
              <span style="float: right; color: #8492a6; font-size: 13px">{{ item.value }}</span>
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="按钮类型"
                      prop="ButtonType">
          <el-select v-model="addForm.ButtonType"
                     filterable
                     default-first-option
                     placeholder="请选择按钮类型">
            <el-option v-for="item in buttonTypeOptions"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
              <span style="float: left">{{ item.label }}</span>
              <span style="float: right; color: #8492a6; font-size: 13px">{{ item.value }}</span>
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="">
          <el-row>
            <el-button type="primary"
                       size="mini">主要按钮</el-button>
            <el-button type="success"
                       size="mini">成功按钮</el-button>
            <el-button type="info"
                       size="mini">信息按钮</el-button>
            <el-button type="warning"
                       size="mini">警告按钮</el-button>
            <el-button type="danger"
                       size="mini">危险按钮</el-button>
          </el-row>
        </el-form-item>
        <el-form-item prop="IsHide"
                      label="是否隐藏"
                      width
                      sortable>
          <el-switch v-model="addForm.IsHide"></el-switch>
        </el-form-item>
        <el-form-item prop="IskeepAlive"
                      label="keepAlive"
                      width
                      sortable>
          <el-switch v-model="addForm.IskeepAlive"></el-switch>
        </el-form-item>
        <el-form-item prop="PidArr"
                      label="父级菜单"
                      width
                      sortable>
          <el-cascader style="width: 400px"
                       v-model="addForm.PidArr"
                       :options="options"
                       filterable
                       change-on-select></el-cascader>
        </el-form-item>

        <el-form-item prop="Mid"
                      label="API接口"
                      width
                      sortable>
          <el-select style="width: 100%;"
                     filterable
                     v-model="addForm.Mid"
                     placeholder="请选择API">
            <el-option :key="0"
                       :value="0"
                       :label="'无需api'"></el-option>
            <el-option v-for="item in modules"
                       :key="item.Id"
                       :value="item.Id"
                       :label="item.LinkUrl">
              <span style="float: left">{{ item.LinkUrl }}</span>
              <span style="float: right; color: #8492a6; font-size: 13px">{{ item.Name }}</span>
            </el-option>
          </el-select>
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
  </section>
</template>

<script>
import util from "../../../util/date";
import {
  getPermissionListPage,
  getPermissionTreeTable,
  removePermission,
  editPermission,
  addPermission,
  getPermissionTree,
  getModuleListPage,
  getAllModuleList
} from "../../api/api";
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/ToolbarButton";

export default {
  components: { Toolbar },
  data () {
    return {
      showFllowSystem: false,
      tableHeight: window.innerHeight - 140, // 控制表格的高度

      funcOptions: [
        {
          value: 'handleAdd',
          label: '新增'
        },
        {
          value: 'handleEdit',
          label: '编辑'
        },
        {
          value: 'handleDel',
          label: '删除'
        },
        {
          value: 'handleView',
          label: '查看'
        },
        {
          value: 'handleExport',
          label: '导出'
        },
        {
          value: 'handleImport',
          label: '导入'
        }
      ],
      buttonTypeOptions: [
        {
          value: 'primary',
          label: '主要按钮'
        },
        {
          value: 'success',
          label: '成功按钮'
        },
        {
          value: 'info',
          label: '信息按钮'
        },
        {
          value: 'warning',
          label: '警告按钮'
        },
        {
          value: 'danger',
          label: '危险按钮'
        }
      ],
      buttonList: [],
      currentRow: null,
      options: [],
      filters: {
        Name: ""
      },
      users: [],
      modules: [], //接口api列表
      statusList: [
        { Name: "激活", value: true },
        { Name: "禁用", value: false }
      ],
      total: 0,
      page: 1,
      listLoading: false,
      sels: [], //列表选中列

      addDialogFormVisible: false,
      editFormVisible: false, //编辑界面是否显示
      editLoading: false,
      editFormRules: {
        Name: [{ required: true, message: "请输入菜单名称", trigger: "blur" }],
        Code: [{ required: true, message: "请输入路由地址", trigger: "blur" }]
      },
      //编辑界面数据
      editForm: {
        Id: 0,
        Mid: 0,
        OrderSort: 0,
        PidArr: [],
        CreateBy: "",
        Name: "",
        Code: "",
        Description: "",
        Icon: "",
        Func: "",
        Enabled: true,
        IsButton: false,
        IsHide: false,
        IskeepAlive: false
      },

      addFormVisible: false, //新增界面是否显示
      addLoading: false,
      addFormRules: {
        Name: [{ required: true, message: "请输入菜单名称", trigger: "blur" }],
        Code: [{ required: true, message: "请输入路由地址", trigger: "blur" }]
      },
      //新增界面数据
      addForm: {
        CreateBy: "",
        CreateId: "",
        PidArr: [],
        Mid: 0,
        OrderSort: 0,
        Name: "",
        Code: "",
        Description: "",
        Icon: "",
        Func: "",
        Enabled: true,
        IsButton: false,
        IsHide: false,
        IskeepAlive: false
      }
    };
  },
  methods: {
    selectCurrentRow (val) {
      this.currentRow = val;
    },
    clkType () {
      this.addForm.IsButton = false;
      this.showFllowSystem = false;
      this.addForm.IskeepAlive = false;
      if (this.addForm.MenuType == "页面") {
        this.showFllowSystem = true;
        this.addForm.Code = "";
        this.addForm.IskeepAlive = true;//默认创建的页面IskeepAlive为true，表示切换tab页时该页不会自动刷新 【EditBy shaocx,2021-06-28】
      } else if (this.addForm.MenuType == "目录") {
        this.showFllowSystem = true;
        this.addForm.Code = "-";
      } else if (this.addForm.MenuType == "按钮") {
        this.addForm.Code = " ";
        this.addForm.IsButton = true;
      }
    },
    clkFllowSystem () {
      if (this.addForm.FllowSystemName == "公共") {
        this.addForm.FllowSystem = 1;
      } else if (this.addForm.FllowSystemName == "低压线圈控制系统") {
        this.addForm.FllowSystem = 2;
      } else if (this.addForm.FllowSystemName == "物流LES系统") {
        this.addForm.FllowSystem = 3;
      }
    },
    clkFllowSystemForEdit () {
      if (this.editForm.FllowSystemName == "公共") {
        this.editForm.FllowSystem = 1;
      } else if (this.editForm.FllowSystemName == "低压线圈控制系统") {
        this.editForm.FllowSystem = 2;
      } else if (this.editForm.FllowSystemName == "物流LES系统") {
        this.editForm.FllowSystem = 3;
      }
    },
    callFunction (item) {
      this.filters = {
        name: item.search
      };
      this[item.Func].apply(this, item);
    },
    //禁用状态显示转换
    formatEnabled: function (row, column) {
      return row.Enabled ? "正常" : "未知";
    },
    formatCreateTime: function (row, column) {
      return !row.CreateTime || row.CreateTime == ""
        ? ""
        : util.formatDate.format(new Date(row.CreateTime), "yyyy-MM-dd");
    },
    handleCurrentChange (val) {
      this.page = val;
      this.getPermissions();
    },
    load (tree, treeNode, resolve) {
      let para = {
        page: this.page,
        f: tree.Id,
        key: this.filters.Name
      };
      getPermissionTreeTable(para).then(res => {
        resolve(res.data.response);
      });
    },
    //获取用户列表
    getPermissions () {
      this.users = null
      let para = {
        page: this.page,
        key: this.filters.Name
      };
      this.listLoading = true;

      //NProgress.start();
      getPermissionTreeTable(para).then(res => {
        this.users = res.data.response;
        this.listLoading = false;
        //NProgress.done();
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
      this.$confirm("确认删除该菜单按钮 [" + row.Name + "] 吗?", "提示", {
        type: "warning"
      })
        .then(() => {
          this.listLoading = true;
          //NProgress.start();
          let para = { id: row.Id };
          removePermission(para).then(res => {
            if (util.isEmt.format(res)) {
              this.listLoading = false;
              return;
            }
            this.listLoading = false;
            //NProgress.done();
            if (res.data.success) {
              this.$message({
                message: "删除成功",
                type: "success"
              });
            } else {
              this.$message({
                message: res.data.msg,
                type: "error"
              });
            }

            this.getPermissions();
          });
        })
        .catch(() => { });
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
      let that = this;
      that.editLoading = true;

      this.editFormVisible = true;
      this.editForm = {};

      this.options = [];
      let para = { pid: row.Id };
      getPermissionTree(para).then(res => {
        this.options.push(res.data.response);
        that.editForm = Object.assign({}, row);
        that.editLoading = false;
      });
    },
    //显示新增界面
    handleAdd () {
      this.options = [];
      this.addFormVisible = true;
      this.addForm = {
        CreateBy: "",
        CreateId: "",
        PidArr: [],
        Name: "",
        Code: "",
        OrderSort: 0,
        Description: "",
        Enabled: true,
        Icon: "",
        IsButton: false,
        IsHide: false,
        IskeepAlive: false
      };
      let row = this.currentRow;
      if (row) {
        let selectPidArr = row.PidArr
        selectPidArr.push(row.Id)
        this.addForm.PidArr = selectPidArr
      }

      let para = { pid: 0 };
      getPermissionTree(para).then(res => {
        this.options.push(res.data.response);
      });
    },
    //编辑
    editSubmit: function () {
      this.$refs.editForm.validate(valid => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            //debugger
            this.editLoading = true;
            //NProgress.start();
            let para = Object.assign({}, this.editForm);

            para.ModifyTime = util.formatDate.format(new Date(), "yyyy-MM-dd");

            para.Pid = para.PidArr.pop();

            if (para.Id == para.Pid) {
              this.$message({
                message: "警告，父节点不能是自己！",
                type: "error"
              });

              this.editLoading = false;
              return;
            }

            editPermission(para).then(res => {
              debugger
              if (util.isEmt.format(res)) {
                this.editLoading = false;
                return;
              }
              this.editLoading = false;
              if (res.data.success) {
                this.editLoading = false;
                //NProgress.done();
                this.$message({
                  message: res.data.msg,
                  type: "success"
                });
                //this.$refs["editForm"].resetFields();
                this.editFormVisible = false;
                this.getPermissions();
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
          if (_this.addForm.MenuType == "页面" && _this.addForm.FllowSystemName == undefined) {
            this.$message({
              message: "请选择所属系统",
              type: "error"
            });
            return;
          }
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            this.addLoading = true;
            //NProgress.start();
            let para = Object.assign({}, this.addForm);

            para.CreateTime = util.formatDate.format(new Date(), "yyyy-MM-dd");
            para.ModifyTime = para.CreateTime;
            para.IsDeleted = false;

            para.Pid = para.PidArr.pop();

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

            addPermission(para).then(res => {
              if (util.isEmt.format(res)) {
                this.addLoading = false;
                return;
              }
              this.addLoading = false;
              if (res.data.success) {
                this.addLoading = false;
                //NProgress.done();
                this.$message({
                  message: res.data.msg,
                  type: "success"
                });
                this.$refs["addForm"].resetFields();
                this.addFormVisible = false;
                this.getPermissions();
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
    }
  },
  mounted () {
    this.getPermissions();

    let para = {
      page: 1,
      pageSize: 10000
    }
    //为什么这里getModuleListPage并没有在该菜单中配置权限，那为什么能访问呢？？奇怪。
    getModuleListPage(para).then(res => {
      this.modules = res.data.response.data;
    });

    let routers = window.localStorage.router
      ? JSON.parse(window.localStorage.router)
      : [];
    this.buttonList = getButtonList(this.$route.path, routers);
  }
};
</script>

<style scoped>
</style>
