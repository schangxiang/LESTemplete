<template>
  <!--详细界面-->
  <el-dialog title="任务阶段列表"
             :visible.sync="detailShowForTaskPhase"
             width="70%"
             v-model="addFormVisible"
             :close-on-click-modal="false">
    <!--列表-->
    <el-table :data="taskPhaseList"
              :max-height="tableHeight"
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
      <el-table-column prop="LesTaskNo"
                       label="LES任务号"
                       width="160"
                       align="center">
      </el-table-column>
      <el-table-column prop="LesTaskStatusName"
                       label="任务状态"
                       width="160"
                       align="center">
      </el-table-column>
      <el-table-column prop="TaskTypeName"
                       label="任务类型"
                       align="center">
      </el-table-column>
      <el-table-column prop="IntendedVehicle"
                       label="分配车辆"
                       align="center">
      </el-table-column>
      <el-table-column prop="ProcessingVehicle"
                       label="执行车辆"
                       align="center">
      </el-table-column>
      <el-table-column prop="AgvState"
                       label="AGV状态"
                       align="center">
      </el-table-column>
      <el-table-column prop="AgvStateName"
                       label="AGV状态说明"
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
    </el-table>
    <div slot="footer"
         class="dialog-footer">
      <el-button @click.native="detailShowForTaskPhase = false"
                 icon="fa fa-power-off">关闭</el-button>
    </div>
  </el-dialog>
</template> 
 
<script> 
import util from '../../../../util/date'
import { getTaskPhaseListByLesTaskNo } from '../../../api/api';
import { formatDate, setformOptionsNewValue } from '../../../../util/tools'


export default {
  name: 'ShowTaskPhaseList',
  data () {
    return {
      taskPhaseList: [],
      detailShowForTaskPhase: false
    }
  },
  props: {
  },
  methods: {
    // 时间格式化 
    formatterDateTime (row, column, cellValue, index) {
      if (cellValue === null || cellValue === '') {
        return ''
      }
      var NewDtime = new Date(Date.parse(cellValue))
      return formatDate(NewDtime, 'yyyy-MM-dd hh:mm:ss')
    },
    showDetailForTaskPhaseList (_lesTaskNo) {
      this.detailShowForTaskPhase = true
      //获取明细信息
      let para = {
        lesTaskNo: _lesTaskNo
      };
      getTaskPhaseListByLesTaskNo(para).then((res) => {
        if (res.data.success) {
          this.taskPhaseList = res.data.response;
        } else {
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }
      });
      return false;//返回false，就是不让a标签点击了，跳转到新页面
    }
  },
  mounted () {

  }
}

</script> 
 
<style lang="scss" scoped>
</style> 
