﻿<template> 
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
                    @onSearch="getRgvExcepitons" /> 
      </el-form> 
    </el-col> 
 
    <!--列表--> 
    <el-table :data="RgvExcepitons" 
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
                <el-table-column prop="TaskNo" label="任务号" align="center" > 
          </el-table-column> 
          <el-table-column prop="TaskType" label="任务类型" align="center" > 
          </el-table-column> 
          <el-table-column prop="CallOrderNo" label="叫料单号" align="center" > 
          </el-table-column> 
          <el-table-column prop="SourcePlaceNo" label="起始点" align="center" > 
          </el-table-column> 
          <el-table-column prop="SourcePlaceName" label="起始点名称" align="center" > 
          </el-table-column> 
          <el-table-column prop="ToPlaceNo" label="目标点" align="center" > 
          </el-table-column> 
          <el-table-column prop="ToPlaceName" label="目标点名称" align="center" > 
          </el-table-column> 
          <el-table-column prop="TaskStatus" label="任务状态" align="center" > 
          </el-table-column> 
          <el-table-column prop="TaskCreateTime" label="任务创建时间" width="150" align="center" :formatter="formatterDateTime" > 
          </el-table-column> 
          <el-table-column prop="TaskIssueTime" label="下发时间" width="150" align="center" :formatter="formatterDateTime" > 
          </el-table-column> 
          <el-table-column prop="TaskFinishTime" label="完成时间" width="150" align="center" :formatter="formatterDateTime" > 
          </el-table-column> 
          <el-table-column prop="DoType" label="处理措施" align="center" > 
          </el-table-column> 
          <el-table-column prop="ExcepitonMsg" label="异常信息" align="center" > 
          </el-table-column> 
          <el-table-column prop="CreateBy" label="创建者" align="center" > 
          </el-table-column> 
          <el-table-column prop="CreateTime" label="创建时间" width="150" align="center" :formatter="formatterDateTime" > 
          </el-table-column> 
          <el-table-column prop="ModifyBy" label="修改者" align="center" > 
          </el-table-column> 
          <el-table-column prop="ModifyTime" label="修改时间" width="150" align="center" :formatter="formatterDateTime" > 
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
                        <el-form-item label="任务号" prop="TaskNo" :rules="[{ required: true, message: '任务号不能为空'}]"> 
                  <el-input v-model="addForm.TaskNo"></el-input> 
                </el-form-item> 
                <el-form-item label="任务类型" prop="TaskType" :rules="[{ required: true, message: '任务类型不能为空'}]"> 
                  <el-input v-model="addForm.TaskType"></el-input> 
                </el-form-item> 
                <el-form-item label="叫料单号" prop="CallOrderNo" :rules="[{ required: true, message: '叫料单号不能为空'}]"> 
                  <el-input v-model="addForm.CallOrderNo"></el-input> 
                </el-form-item> 
                <el-form-item label="起始点" prop="SourcePlaceNo" :rules="[{ required: true, message: '起始点不能为空'}]"> 
                  <el-input v-model="addForm.SourcePlaceNo"></el-input> 
                </el-form-item> 
                <el-form-item label="起始点名称" prop="SourcePlaceName" :rules="[{ required: true, message: '起始点名称不能为空'}]"> 
                  <el-input v-model="addForm.SourcePlaceName"></el-input> 
                </el-form-item> 
                <el-form-item label="目标点" prop="ToPlaceNo" :rules="[{ required: true, message: '目标点不能为空'}]"> 
                  <el-input v-model="addForm.ToPlaceNo"></el-input> 
                </el-form-item> 
                <el-form-item label="目标点名称" prop="ToPlaceName" :rules="[{ required: true, message: '目标点名称不能为空'}]"> 
                  <el-input v-model="addForm.ToPlaceName"></el-input> 
                </el-form-item> 
                <el-form-item label="任务状态" prop="TaskStatus" :rules="[{ required: true, message: '任务状态不能为空'}]"> 
                  <el-input v-model="addForm.TaskStatus"></el-input> 
                </el-form-item> 
                <el-form-item label="任务创建时间" prop="TaskCreateTime" :rules="[{ required: true, message: '任务创建时间不能为空'}]"> 
                  <el-input v-model="addForm.TaskCreateTime"></el-input> 
                </el-form-item> 
                <el-form-item label="下发时间" prop="TaskIssueTime "> 
                  <el-input v-model="addForm.TaskIssueTime"></el-input> 
                </el-form-item> 
                <el-form-item label="完成时间" prop="TaskFinishTime "> 
                  <el-input v-model="addForm.TaskFinishTime"></el-input> 
                </el-form-item> 
                <el-form-item label="处理措施" prop="DoType "> 
                  <el-input v-model="addForm.DoType"></el-input> 
                </el-form-item> 
                <el-form-item label="异常信息" prop="ExcepitonMsg "> 
                  <el-input v-model="addForm.ExcepitonMsg"></el-input> 
                </el-form-item> 
                <el-form-item label="操作说明" prop="OperationRemark "> 
                  <el-input v-model="addForm.OperationRemark"></el-input> 
                </el-form-item> 
                <el-form-item label="创建者ID" prop="CreateId "> 
                  <el-input v-model="addForm.CreateId"></el-input> 
                </el-form-item> 
                <el-form-item label="创建者" prop="CreateBy "> 
                  <el-input v-model="addForm.CreateBy"></el-input> 
                </el-form-item> 
                <el-form-item label="修改者ID" prop="ModifyId "> 
                  <el-input v-model="addForm.ModifyId"></el-input> 
                </el-form-item> 
                <el-form-item label="修改者" prop="ModifyBy "> 
                  <el-input v-model="addForm.ModifyBy"></el-input> 
                </el-form-item> 
                <el-form-item label="修改时间" prop="ModifyTime "> 
                  <el-input v-model="addForm.ModifyTime"></el-input> 
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
import { getRgvExcepitonListPage, removeRgvExcepiton, editRgvExcepiton, addRgvExcepiton } from '../../api/api'; 
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
      exportFileName: 'RGV异常信息',//要导出的文件名 
      currentPageData: [],//当前页面的列表数据 
      tHeader: ['任务号','任务类型','叫料单号','起始点','起始点名称','目标点','目标点名称','任务状态','任务创建时间','下发时间','完成时间','处理措施','异常信息','创建者','创建时间','修改者','修改时间'],//当前页面列表的表头汉字数组，导出用 
      filterVal: ['TaskNo','TaskType','CallOrderNo','SourcePlaceNo','SourcePlaceName','ToPlaceNo','ToPlaceName','TaskStatus','TaskCreateTime','TaskIssueTime','TaskFinishTime','DoType','ExcepitonMsg','CreateBy','CreateTime','ModifyBy','ModifyTime'],//当前页面列表的表头属性数组，导出用 
 
      //搜索框相关 
	  commonSearchOptionSet: "模糊",//通用查询的默认配置,"模糊"或"精准" 
	  searchValControlStyle: {//设置通用搜索框的长度等样式  
        width: '300px', 
      }, 
      controlStyle: {//设置高级搜索控件的长度等样式  
        width: '300px', 
      }, 
	  labelWidth: "90px",//显示Label的宽度 
      drawerSize: "550px",//drawner宽度设置 
      searchFormInputPlaceholder: '请输入任务号/叫料单号/异常信息/操作说明/创建者/修改者',//要给子搜索组件传递的值 
	  searchFormInputAttrs: ['TaskNo','CallOrderNo','ExcepitonMsg','OperationRemark','CreateBy','ModifyBy'],//要给子搜索组件传递的属性名 
      formOptions: [ 
                        { 
                   label: '任务号', 
                   prop: 'TaskNo', 
                   element: 'el-input', 
                }, 
                { 
                   label: '任务类型', 
                   prop: 'TaskType', 
                   element: 'el-input', 
                }, 
                { 
                   label: '叫料单号', 
                   prop: 'CallOrderNo', 
                   element: 'el-input', 
                }, 
                { 
                   label: '起始点', 
                   prop: 'SourcePlaceNo', 
                   element: 'el-input', 
                }, 
                { 
                   label: '起始点名称', 
                   prop: 'SourcePlaceName', 
                   element: 'el-input', 
                }, 
                { 
                   label: '目标点', 
                   prop: 'ToPlaceNo', 
                   element: 'el-input', 
                }, 
                { 
                   label: '目标点名称', 
                   prop: 'ToPlaceName', 
                   element: 'el-input', 
                }, 
                { 
                   label: '任务状态', 
                   prop: 'TaskStatus', 
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
                   label: '处理措施', 
                   prop: 'DoType', 
                   element: 'el-input', 
                }, 
                { 
                   label: '异常信息', 
                   prop: 'ExcepitonMsg', 
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
                },  
      ], 
 
	  isShowOperatorColumn: true,// 是否显示【操作】列,true显示 false不显示 
      tableHeight: window.innerHeight - 180, // 控制表格的高度 
 
      isAdd: false, // 是否是新增 
      dialogTitle: '', 
 
      filters: { 
        name: '' 
      }, 
      RgvExcepitons: [], 
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
      this.getRgvExcepitons(); 
    }, 
    handleSizeChange (val) { 
      this.pageSize = val; 
      this.getRgvExcepitons(); 
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
    //获取列表 
    getRgvExcepitons (formValue) { 
      //this.total = 0 
      //this.RgvExcepitons = null 
      let para = this.getPostParam('1'); 
      this.listLoading = true; 
      getRgvExcepitonListPage(para).then((res) => { 
	    if (res.data.success) { 
          this.total = res.data.response.dataCount; 
          this.RgvExcepitons = res.data.response.data; 
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
      getRgvExcepitonListPage(para).then((res) => { 
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
        removeRgvExcepiton(para).then((res) => { 
 
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
 
          this.getRgvExcepitons(); 
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
              addRgvExcepiton(para).then((res) => { 
 
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
                  this.getRgvExcepitons(); 
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
              editRgvExcepiton(para).then((res) => { 
 
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
                  this.getRgvExcepitons(); 
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
      this.currentPageData = this.RgvExcepitons 
      this.$refs.cmToolbarExport.showPrintPage() 
    }, 
  }, 
  mounted () { 
    this.getRgvExcepitons(); 
 
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
