<template>
  <section>
    <div style="margin-top:10px;">
      <span class="spanclass"
            style="margin-left:10px;"><span class="redclass"
              style="color: #fff">锁定 </span>
        &nbsp;&nbsp;&nbsp;&nbsp;<span class="disabledClass"
              style="color: #fff"> 禁用 </span>
        &nbsp;&nbsp;&nbsp;&nbsp;<span class="greenClass"
              style="color: #fff"> 有分拣料 </span>
        &nbsp;&nbsp;&nbsp;&nbsp;<span class="emptyClass"
              style="color: #fff"> 有余料 </span>
        &nbsp;&nbsp;&nbsp;&nbsp;<span class="defalutClass"
              style="color: #fff"> 空闲 </span> &nbsp;&nbsp;&nbsp;&nbsp;
        <el-tag type="success">数据{{timingValue}}秒刷新一次</el-tag>
      </span>
    </div>
    <div class="el-row"
         style="margin-bottom:10px;margin-top:10px">
      <div v-for="o in StockInfos"
           :key="o"
           class="text item">
        <div class="el-col-xs-12 "
             style="width:100%">
          <el-card style="margin-left: 3px;
  margin-right: 3px;margin-bottom:10px;">
            <div slot="header"
                 class="clearfix">
              <span>{{o.CardTitle}}</span>
            </div>
            <div class="el-col-xs-12 singleDiv"
                 :style="{width:singleDivWidth}"
                 v-for="item in o.bufferStockCollectByStationList">
              <div class="demo-color-box-group"
                   @click="singleDivClick(item)">
                <div :class="classForBox(item)">{{item.StationCode}}
                  <div class="value">{{item.BurdenWorkNo}}</div>
                  <div class="value">{{item.MaterialName}}</div>
                  <div class="value">{{item.PlaceTypeName}}</div>
                </div>
              </div>
            </div>
          </el-card>
        </div>
      </div>
    </div>
    <div style="margin-top:40px;"></div>

    <!--操作界面-->
    <el-dialog title="站点操作"
               :visible.sync="mainFormVisible"
               :close-on-click-modal="false">
      <el-form :model="mainForm"
               size="small"
               label-width="100px"
               ref="mainForm">
        <el-form-item label="站点编号"
                      prop="StationCode">
          <el-input v-model="mainForm.StationCode"
                    disabled>
          </el-input>
        </el-form-item>
        <el-form-item label="站点名称"
                      prop="StationName">
          <el-input v-model="mainForm.StationName"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label="配料任务号"
                      prop="BurdenWorkNo">
          <el-input v-model="mainForm.BurdenWorkNo"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label="物料"
                      prop="MaterialName">
          <el-input v-model="mainForm.MaterialName"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label="是否锁定"
                      prop="IsLock">
          <el-switch v-model="mainForm.IsLock"
                     disabled
                     :active-value="1"
                     :inactive-value="0"
                     active-text="是"
                     inactive-text="否">
          </el-switch>
        </el-form-item>
        <el-form-item label="是否有货"
                      prop="IsFull">
          <el-switch v-model="mainForm.IsFull"
                     disabled
                     :active-value="1"
                     :inactive-value="0"
                     active-text="是"
                     inactive-text="否">
          </el-switch>
        </el-form-item>
        <el-form-item label="是否禁用"
                      prop="Station_IsDeleted">
          <el-switch v-model="mainForm.Station_IsDeleted"
                     disabled
                     active-text="是"
                     inactive-text="否">
          </el-switch>
        </el-form-item>
        <el-form-item label="说明"
                      prop="Station_OperationRemark">
          <el-input v-model="mainForm.Station_OperationRemark"
                    disabled></el-input>
        </el-form-item>
        <el-form-item label="操作说明"
                      prop="OperationRemark"
                      :rules="[{ required: true, message: '操作说明不能为空'}]">
          <el-input v-model="mainForm.OperationRemark"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer"
           style="margin-top:-50px;"
           class="dialog-footer">
        <el-button type="danger"
                   class="footSty"
                   icon="el-icon-delete"
                   @click="doClick(1)">清空库存</el-button>
        <el-button type="warning"
                   class="footSty"
                   icon="el-icon-lock"
                   @click="doClick(2)">锁定</el-button>
        <el-button type="success"
                   class="footSty"
                   icon="el-icon-unlock"
                   @click="doClick(3)">解锁</el-button>
        <el-button type="info"
                   class="footSty"
                   icon="fa fa-power-off"
                   @click="doClick(4)">禁用</el-button>
        <el-button type="success"
                   class="footSty"
                   icon="fa fa-power-off"
                   @click="doClick(5)">启用</el-button>
        <el-button type="danger"
                   class="footSty"
                   icon="el-icon-delete"
                   @click="doClick(6)">一键清空余料</el-button>
      </div>
    </el-dialog>
  </section>
</template> 
 
<script> 
import util from '../../../util/date'
import { updateStationIsLock, updateStationIsDeleted, getStockForBufferStockVisual, deleteStockByStationId, deleteAllAllEmptySalverStation, getCodeItemsBySetCodeForGX, getStockInfoCollectListPage } from '../../api/api';
import { isMobile } from '../../../util/common'


export default {
  components: {},//注册子组件 
  data () {
    return {
      mainFormVisible: false,
      mainForm: {
        OperationRemark: ''
      },
      singleDivWidth: '10%',
      StockInfos: [],
      listLoading: false,
      //定时相关
      timingValue: 10, // 定时频率
      isTimingData: true, // 是否定时获取
      timer: '', // 定时刷新
    }
  },
  methods: {
    // 定时刷新
    createdTimer () {
      this.timer = setInterval(this.getStockInfos, this.timingValue * 1000)
    },
    doClick (flag) {
      switch (flag) {
        case 1://清空库存
          this.do_handleDeleteStockByStationId()
          break;
        case 2://锁定
          this.mainForm.IsLock = 1
          this.do_handleIsLock()
          break;
        case 3://解锁
          this.mainForm.IsLock = 0
          this.do_handleIsLock()
          break;
        case 4://禁用
          this.mainForm.IsDeleted = 1
          this.do_handleIsDeleted()
          break;
        case 5://启用
          this.mainForm.IsDeleted = 0
          this.do_handleIsDeleted()
          break;
        case 6://一键清空余料
          this.handleAllEmptySalverStation()
          break;
      }
    },
    //根据站点ID删除库存 
    do_handleDeleteStockByStationId () {
      this.$refs.mainForm.validate((valid) => {
        if (valid) {
          this.$confirm('确认要清空站点 [' + this.mainForm.StationName + '] 的库存吗?', '提示', {
            type: 'warning'
          }).then(() => {
            this.listLoading = true;
            let para = {
              stationId: this.mainForm.StationId,
              manualHandlerRemark: this.mainForm.OperationRemark,
            };
            deleteStockByStationId(para).then((res) => {

              if (util.isEmt.format(res)) {
                this.listLoading = false;
                return;
              }
              this.listLoading = false;
              //NProgress.done(); 
              if (res.data.success) {
                this.$message({
                  message: '清空库存成功',
                  type: 'success'
                });
                this.closeMainForm()

              } else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
                });
              }


            });
          }).catch(() => {

          });
        }
      });
    },
    //删除  删除立库缓存区所有的空托盘和余料信息
    handleAllEmptySalverStation () {
      this.$confirm('确认要删除立库缓存区所有的空托盘和余料信息吗?', '提示', {
        type: 'warning'
      }).then(() => {
        this.listLoading = true;
        let para = {
          storeAreaEnumValue: 'StoreBuffer',//立库缓存区
          remark: '清空 立库缓存区的空托盘和余料工位'
        };
        deleteAllAllEmptySalverStation(para).then((res) => {

          if (util.isEmt.format(res)) {
            this.listLoading = false;
            return;
          }
          this.listLoading = false;
          if (res.data.success) {
            this.$message({
              message: '删除立库缓存区所有的空托盘和余料信息成功',
              type: 'success'
            });
            this.closeMainForm()
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
    do_handleIsLock () {
      this.$refs.mainForm.validate((valid) => {
        if (valid) {
          var alertMsg = this.mainForm.IsLock ? '锁定' : '解锁'
          this.$confirm('确认要 ' + alertMsg + ' 站点 [' + this.mainForm.StationName + '] 吗?', '提示', {
            type: 'warning'
          }).then(() => {
            this.listLoading = true;
            let para = {
              Id: this.mainForm.StationId,
              IsLock: this.mainForm.IsLock ? 1 : 0,
              OperationRemark: this.mainForm.OperationRemark
            };
            updateStationIsLock(para).then((res) => {

              if (util.isEmt.format(res)) {
                this.listLoading = false;
                return;
              }
              this.listLoading = false;
              //NProgress.done(); 
              if (res.data.success) {
                this.$message({
                  message: '修改锁定状态成功',
                  type: 'success'
                });
                this.closeMainForm()
              } else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
                });
              }
              this.updateIsLockVisible = false
              this.mainForm.OperationRemark = ''
              this.getStations();
            });
          }).catch(() => {
          });
        }
      });
    },
    do_handleIsDeleted () {
      this.$refs.mainForm.validate((valid) => {
        if (valid) {
          var alertMsg = this.mainForm.IsDeleted ? '禁用' : '启用'
          this.$confirm('确认要 ' + alertMsg + ' 站点 [' + this.mainForm.StationName + '] 吗?', '提示', {
            type: 'warning'
          }).then(() => {
            this.listLoading = true;
            let para = {
              Id: this.mainForm.StationId,
              IsDeleted: this.mainForm.IsDeleted,
              OperationRemark: this.mainForm.OperationRemark
            };
            updateStationIsDeleted(para).then((res) => {

              if (util.isEmt.format(res)) {
                this.listLoading = false;
                return;
              }
              this.listLoading = false;
              //NProgress.done(); 
              if (res.data.success) {
                this.$message({
                  message: '修改禁用状态成功',
                  type: 'success'
                });
                this.closeMainForm()
              } else {
                this.$message({
                  message: res.data.msg,
                  type: 'error'
                });
              }
              this.updateIsDeletedVisible = false
              this.mainForm.OperationRemark = ''
              this.getStations();
            });
          }).catch(() => {

          });
        }
      });

    },
    classForBox (item) {
      if (item.Station_IsDeleted) {
        return 'demo-color-box demo-color-box-other disabledClass '
      }
      else if (item.IsLock === 1) {
        return 'demo-color-box demo-color-box-other redclass '
      }
      else if (item.IsFull === 1) {
        if (item.PlaceTypeName.indexOf('余料') > -1) {
          return 'demo-color-box demo-color-box-other emptyClass '
        } else {
          return 'demo-color-box demo-color-box-other greenClass '
        }
      }
      return 'demo-color-box demo-color-box-other defalutClass'
    },
    styleForBox (item) {
      if (item.IsFull === 1) {
        // return 'background:red;'
      }
      //return 'background: rgb(48, 49, 51);'
    },
    singleDivClick (row) {
      this.mainFormVisible = true
      //绑定数据
      this.mainForm = {}
      this.mainForm = row
      this.mainForm.OperationRemark = ''
      this.mainForm.IsDeleted = row.IsDeleted
      this.mainFormVisible = true
    },
    closeMainForm () {
      this.mainForm.OperationRemark = ''
      this.mainFormVisible = false
      this.getStockInfos();
    },
    //获取列表 
    getStockInfos (formValue) {
      this.listLoading = true;
      getStockForBufferStockVisual({}).then((res) => {
        if (res.data.success) {
          this.StockInfos = res.data.response;
        } else {
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }
        this.listLoading = false;
      });
    },
  },
  beforeDestroy () {

    if (this.timer) { // 如果定时器还在运行 或者直接关闭，不用判断
      clearInterval(this.timer)
    }
  },
  mounted () {
    if (isMobile()) {
      this.drawerSize = "99%"//drawner宽度设置 
      this.singleDivWidth = "50%"
    } else {
      this.drawerSize = "550px"//drawner宽度设置 
      this.singleDivWidth = "10%"
    }
    //this._getCodeItemsBySetCodeForGX()
    this.getStockInfos();
    this.createdTimer()
  }
}

</script> 
 
<style lang="scss" scoped>
.demo-color-box-group .demo-color-box:first-child {
  border-radius: 4px 4px 0 0;
}
.demo-color-box-group .demo-color-box {
  border-radius: 0;
  margin: 0;
}
.demo-color-box-other {
  height: 84px;
  margin: 10px 0 !important;
  border-radius: 4px 4px 4px 4px !important;
  padding: 15px 20px;
}
.demo-color-box {
  position: relative;
  border-radius: 4px;
  padding: 15px;
  margin: 5px 0;
  height: 114px;
  box-sizing: border-box;
  color: #fff;
  font-size: 12px;
}
.singleDiv {
  padding-left: 6px;
  padding-right: 6px;
}
.redclass {
  background-color: #ee1818;
}
.yellowclass {
  background-color: #eeee66;
}
.disabledClass {
  background-color: rgb(95, 93, 93);
}
.greenClass {
  background-color: rgba(62, 133, 48, 0.774);
}
.emptyClass {
  background-color: rgba(214, 143, 11, 0.774);
}
.defalutClass {
  background: rgb(104, 131, 184);
}
.footSty {
  margin-top: 10px;
}
/* 重写,使用深度作用选择器才能修改！！！ */
.el-card /deep/ .el-card__body {
  padding: 10px;
}
.el-card /deep/ .el-card__header {
  padding: 5px 10px;
}
</style> 
