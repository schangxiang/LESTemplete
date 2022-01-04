<template>
  <div style="margin-top: 30px;">
    <el-row class="panel-group">
      <el-col class="card-panel-col"
              :style="{width:card_panel_col_width}">
        <div class="card-panel extoday"
             @click="goToAgvNoFinishedTaskPage()">
          <div class="card-panel-icon-wrapper icon-people">
            <svg-icon icon-class="all_count"
                      class-name="card-panel-icon" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text greenColor">总数（个）</div>
            <span data-v-6723c96e
                  class="card-acuser-num">{{welcomeInitData.AllRuningTaskCount>9 ? welcomeInitData.AllRuningTaskCount:'0'+welcomeInitData.AllRuningTaskCount}}</span>
          </div>
        </div>
      </el-col>
      <el-col class="card-panel-col"
              :style="{width:card_panel_col_width}">
        <div class="card-panel extoday"
             @click="goToAgvNoFinishedTaskPage()">
          <div class="card-panel-icon-wrapper icon-people">
            <svg-icon icon-class="documentation"
                      class-name="card-panel-icon" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text greenColor">待下发</div>
            <span data-v-6723c96e
                  class="card-acuser-num">{{welcomeInitData.AllNoStartTaskCount>9 ? welcomeInitData.AllNoStartTaskCount:'0'+welcomeInitData.AllNoStartTaskCount}}</span>
          </div>
        </div>
      </el-col>
      <el-col class="card-panel-col"
              :style="{width:card_panel_col_width}">
        <div class="card-panel extoday"
             @click="goToAgvNoFinishedTaskPage()">
          <div class="card-panel-icon-wrapper icon-people">
            <svg-icon icon-class="issued"
                      class-name="card-panel-icon" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text greenColor">已下发</div>
            <span data-v-6723c96e
                  class="card-acuser-num">{{welcomeInitData.AllIssueTaskCount>9 ? welcomeInitData.AllIssueTaskCount:'0'+welcomeInitData.AllIssueTaskCount}}</span>
          </div>
        </div>
      </el-col>
      <el-col class="card-panel-col"
              :style="{width:card_panel_col_width}">
        <div class="card-panel extoday"
             @click="goToAgvNoFinishedTaskPage()">
          <div class="card-panel-icon-wrapper icon-people">
            <svg-icon icon-class="daifenpei"
                      class-name="card-panel-icon" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text greenColor">待分配</div>
            <span data-v-6723c96e
                  class="card-acuser-num">{{welcomeInitData.AllWaitForDistributionTaskCount>9 ? welcomeInitData.AllWaitForDistributionTaskCount:'0'+welcomeInitData.AllWaitForDistributionTaskCount}}</span>
          </div>
        </div>
      </el-col>
      <el-col class="card-panel-col"
              :style="{width:card_panel_col_width}">
        <div class="card-panel extoday"
             @click="goToAgvNoFinishedTaskPage()">
          <div class="card-panel-icon-wrapper icon-people">
            <svg-icon icon-class="doing"
                      class-name="card-panel-icon" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text yellowColor">执行中</div>
            <span data-v-6723c96e
                  class="card-acuser-num yellowColor">{{welcomeInitData.BEING_PROCESSED_Count>9 ? welcomeInitData.BEING_PROCESSED_Count:'0'+welcomeInitData.BEING_PROCESSED_Count}}</span>
          </div>
        </div>
      </el-col>
      <el-col class="card-panel-col"
              :style="{width:card_panel_col_width}">
        <div class="card-panel extoday"
             @click="goToWarningPage()">
          <div class="card-panel-icon-wrapper icon-people">
            <svg-icon icon-class="alarm"
                      class-name="card-panel-icon" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text hightemp">
              设备报警</div>
            <span data-v-6723c96e
                  class="card-panel-num hightemp">{{welcomeInitData.AgvWarnCount >9 ? welcomeInitData.AgvWarnCount:'0'+welcomeInitData.AgvWarnCount}}</span>
          </div>
        </div>
      </el-col>
      <el-col class="card-panel-col"
              :style="{width:card_panel_col_width}">
        <div class="card-panel extoday"
             @click="goToWarningPage()">
          <el-carousel height="120px"
                       direction="vertical">
            <el-carousel-item v-for="item in welcomeInitData.AgvWarningList"
                              :key="item">
              <div class="warnDivStyle">报警车辆: {{item.AgvNameDesc}}</div>
              <div class="warnDivStyle">报警时间: {{item.Str_WarningTime}}</div>
              <div class="warnDivStyle">报警位置: {{item.WarningLocation}}</div>
              <div class="warnDivStyle">报警内容: {{item.WarningContent}}</div>
            </el-carousel-item>
          </el-carousel>
        </div>
      </el-col>
    </el-row>
    <el-card class="welcome-card note"
             style="width: calc(100%);margin-right: 10px;">
      <div slot="header"
           class="clearfix">
        <span>AGV车辆 &nbsp;&nbsp; <el-tag type="success">{{timingValue}}秒刷新一次</el-tag></span>
      </div>
      <el-col :span="24"
              class="echarts-item">
        <el-table :data="data_allVehicles"
                  highlight-current-row
                  border
                  v-loading="listLoading"
                  style="min-height:500px;width: 100%;font-size: 12px;margin-bottom: 10px;">
          <el-table-column prop="AgvNameDesc"
                           label="车号"
                           width="70px"
                           sortable></el-table-column>
          <el-table-column prop="StateName"
                           label="状态"
                           width="150px"
                           sortable></el-table-column>
          <el-table-column prop="TransportOrder"
                           label="当前任务"
                           width="150px"
                           sortable></el-table-column>
          <el-table-column prop="MaterialName"
                           label="物料"
                           width="150px"
                           sortable></el-table-column>
          <el-table-column prop="EnergyLevel"
                           label="电量"
                           width="150px"></el-table-column>
          <el-table-column prop="CurrentPosition"
                           label="当前位置"
                           width="150px"></el-table-column>
          <el-table-column prop="XCoordinate"
                           label="X坐标"
                           width></el-table-column>
          </el-table-column>
          <el-table-column prop="YCoordinate"
                           label="Y坐标"
                           width></el-table-column>
          </el-table-column>
        </el-table>
      </el-col>

    </el-card>
    <el-card class="welcome-card note"
             style="width: 98%; margin-top: 20px">
      <div slot="header"
           class="clearfix">
        <span>
          <el-select v-model="day_value"
                     size="small"
                     @change="changeDay"
                     placeholder="请选择">
            <el-option v-for="item in day_options"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
            </el-option>
          </el-select>&nbsp; &nbsp; <el-select v-model="ProductYieldByTimeRange_flag_value"
                     size="small"
                     @change="changeRange"
                     placeholder="请选择">
            <el-option v-for="item in ProductYieldByTimeRange_flag_options"
                       :key="item.value"
                       :label="item.label"
                       :value="item.value">
            </el-option>
          </el-select>&nbsp; &nbsp; AGV任务曲线图
        </span>
      </div>

      <el-col :span="24"
              class="echarts-item">
        <ve-line :data="lineChartData_WelcomeAccessApiDateView"
                 :extend="extend"
                 :settings="lineChartSettings_WelcomeAccessApiDateView"
                 :mark-point="lineChartMarkPoint"></ve-line>
      </el-col>
    </el-card>
    <el-card class="welcome-card note pieStyle"
             :style="{width: pie_width}">
      <div slot="header"
           class="clearfix">
        <span style="font-size: 16px;">立库分拣缓存区库存状况</span>
      </div>
      <el-col :span="24"
              class="echarts-item">
        <ve-pie :data="chartData"
                width="100%"
                height="350px"
                :settings="chartSettings"></ve-pie>
      </el-col>
    </el-card>
    <el-card class="welcome-card note pieStyle"
             :style="{width: pie_width}">
      <div slot="header"
           class="clearfix">
        <el-date-picker v-model="queryTimeForUseRatioPie"
                        type="daterange"
                        @change="_getAgvForUseRatioPie"
                        align="right"
                        unlink-panels
                        range-separator="至"
                        start-placeholder="开始日期"
                        end-placeholder="结束日期"
                        :picker-options="pickerOptions">
        </el-date-picker>
        <span style="font-size: 16px;">&nbsp;&nbsp;&nbsp;AGV利用率</span>
      </div>
      <el-col :span="24"
              class="echarts-item">
        <ve-pie :data="chartDataForUseRatioPie"
                width="100%"
                height="350px"
                :settings="chartSettings"></ve-pie>
      </el-col>
    </el-card>
  </div>
</template>

<script>
import Vue from "vue";
import VCharts from "v-charts";
import planImg from "../../public/static/images/plan.png";
Vue.use(VCharts);

import applicationUserManager from "../Auth/applicationusermanager";
import { isMobile } from '../../util/common'
import { getOneDay } from '../../util/date'
import {
  getAgvForUseRatioPie,
  productYieldByTimeRange,
  getWelcomeData,
  getAllVehicles,
  getServerInfo,
  getWelcomeDataForPie,
  getAccessLogs,
  getIds4UsersGrow,
  getActiveUsers,
  getAchieveUsers_IS4
} from "../api/api";

export default {
  name: "Welcome",
  data () {
    return {
      queryTimeForUseRatioPie: [],
      pickerOptions: {
        shortcuts: [{
          text: '最近一周',
          onClick (picker) {
            const end = new Date();
            const start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
            picker.$emit('pick', [start, end]);
          }
        }, {
          text: '最近一个月',
          onClick (picker) {
            const end = new Date();
            const start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
            picker.$emit('pick', [start, end]);
          }
        }, {
          text: '最近三个月',
          onClick (picker) {
            const end = new Date();
            const start = new Date();
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
            picker.$emit('pick', [start, end]);
          }
        }]
      },
      pie_width: "calc(50%)",
      card_panel_col_width: '20%',
      warningObjList: [],//报警列表
      welcomeInitData: {
        AllNoStartTaskCount: 0,
        AllIssueTaskCount: 0,
        AllRuningTaskCount: 0,
        BEING_PROCESSED_Count: 0,
        AllWaitForDistributionTaskCount: 0,
        AgvWarnCount: 0,
        AgvWarningList: []
      },
      agvWarnCount: 0,//AGV报警个数
      timer_welcomeData: '',
      //定时相关
      timingValue: 5, // 定时频率
      isTimingData: true, // 是否定时获取
      timer: '', // 定时刷新
      data_allVehicles: [],
      active: 2,
      //数据
      chartSettings: { type: 'pie' },
      chartData: {
        columns: ['key', 'value'],
        rows: [
          { "key": "1", "value": "111" },
          { "key": "2", "value": "222" },
          { "key": "3", "value": "333" }
        ]
      },
      chartDataForUseRatioPie: {
        columns: ['key', 'value'],
        rows: [
          { "key": "1", "value": "111" },
          { "key": "2", "value": "222" },
          { "key": "3", "value": "333" }
        ]
      },


      planImgUrl: planImg,
      listLoading: false,

      serverInfo: {},
      extend: {
        series: {
          label: {
            normal: {
              show: true
            }
          }
        }
      },
      lineChartDataIDS4: {
        columns: [],
        rows: [],
        today: 0
      },
      lineChartSettings7Day: {
        metrics: ["count"],
        dimension: ["date"]
      },
      //曲线图
      day_options: [{
        value: '7',
        label: '近一周'
      }, {
        value: '14',
        label: '近两周'
      }, {
        value: '30',
        label: '近一月'
      }, {
        value: '60',
        label: '近两月'
      }, {
        value: '180',
        label: '近半年'
      },
      {
        value: '360',
        label: '近一年'
      }],
      day_value: '7', //默认一周

      ProductYieldByTimeRange_flag_value: '1',//默认查询 任务完成的
      ProductYieldByTimeRange_flag_options: [{
        value: '1',
        label: '任务完成的'
      }, {
        value: '2',
        label: '新建任务的'
      }],
      extend: {
        series: {
          label: {
            normal: {
              show: true,
            },
          },
        },
      },
      lineChartData_WelcomeAccessApiDateView: {
        columns: [],
        rows: []
      },
      lineChartSettings_WelcomeAccessApiDateView: {
        metrics: ["总量", "车辆1", "车辆2", "车辆3", "车辆4", "车辆5", "车辆6", "车辆7", "车辆8", "车辆9", "车辆10", "未分配"],
        dimension: ["日期"],
      },
      lineChartMarkPoint: {
        data: [
          {
            name: "最大值",
            type: "max",
          },
          {
            name: "最小值",
            type: "min",
          },
        ],
      },
    };
  },
  methods: {
    _getAgvForUseRatioPie (_selValue) {
      var aa = this.queryTimeForUseRatioPie
      var param = {
        startTime: this.queryTimeForUseRatioPie[0],
        endTime: this.queryTimeForUseRatioPie[1]
      }
      debugger
      getAgvForUseRatioPie(param).then((res) => {
        debugger
        this.chartDataForUseRatioPie.rows = res.data.response;
      });
    },
    changeDay (value) {
      //this.day_value = value;
      this.productYieldByTimeRange();
    },
    changeRange (value) {
      //this.ProductYieldByTimeRange_flag_value = value;
      this.productYieldByTimeRange();
    },
    productYieldByTimeRange (value) {
      productYieldByTimeRange({ day: this.day_value, flag: this.ProductYieldByTimeRange_flag_value }).then((res) => {
        this.lineChartData_WelcomeAccessApiDateView = res.data.response;
      });
    },
    getWelcomeDataForPie () {
      getWelcomeDataForPie({}).then((res) => {
        this.chartData.rows = res.data.response;
      });
    },
    //获取全部车辆列表
    getWelcomeData () {
      getWelcomeData({}).then((res) => {
        this.welcomeInitData = res.data.response;
      });
    },
    //获取全部车辆列表
    getAllVehicles () {
      getAllVehicles({}).then((res) => {
        this.data_allVehicles = res.data.response;
      });
    },
    // 定时刷新
    createdTimer () {
      this.timer = setInterval(this.getAllVehicles, this.timingValue * 1000)
      this.timer_welcomeData = setInterval(this.getWelcomeData, this.timingValue * 1000)
    },
    getBck (index) {
      return `background: rgb(${43 + index * 20}, ${148 +
        index * 10}, 255) none repeat scroll 0% 0%;`;
    },
    toLogs () {
      this.$router.replace({
        path: "/Logs/Index"
      });
    },
    //前往处理报警
    goToWarningPage () {
      this.$router.replace({
        path: "/AGV/AgvWarning",
        query: {

        }
      })
    },
    //前往在执行AGV任务队列
    goToAgvNoFinishedTaskPage () {
      this.$router.replace({
        path: "/AGV/AgvNoFinishedTask",
        query: {

        }
      })
    },
  },
  beforeDestroy () {

    if (this.timer) { // 如果定时器还在运行 或者直接关闭，不用判断
      clearInterval(this.timer)
    }
    if (this.timer_welcomeData) { // 如果定时器还在运行 或者直接关闭，不用判断
      clearInterval(this.timer_welcomeData)
    }
  },
  computed: {
    // 默认时间
    timeDefault () {
      let date = new Date()
      // 通过时间戳计算
      let defalutStartTime = date.getTime() - 7 * 24 * 3600 * 1000 // 转化为时间戳
      let defalutEndTime = date.getTime()
      let startDateNs = new Date(defalutStartTime)
      let endDateNs = new Date(defalutEndTime)
      // 月，日 不够10补0
      defalutStartTime = startDateNs.getFullYear() + '-' + ((startDateNs.getMonth() + 1) >= 10 ? (startDateNs.getMonth() + 1) : '0' + (startDateNs.getMonth() + 1)) + '-' + (startDateNs.getDate() >= 10 ? startDateNs.getDate() : '0' + startDateNs.getDate())
      defalutEndTime = endDateNs.getFullYear() + '-' + ((endDateNs.getMonth() + 1) >= 10 ? (endDateNs.getMonth() + 1) : '0' + (endDateNs.getMonth() + 1)) + '-' + (endDateNs.getDate() >= 10 ? endDateNs.getDate() : '0' + endDateNs.getDate())
      return [defalutStartTime, defalutEndTime]
    }
  },
  created () {
    this.queryTimeForUseRatioPie = this.timeDefault
  },
  mounted () {
    if (isMobile()) {
      this.card_panel_col_width = "100%"
      this.pie_width = "100%"
    } else {
      this.card_panel_col_width = "20%"
      this.pie_width = "48%"
    }
    this.getWelcomeDataForPie()
    this.productYieldByTimeRange();
    this.getWelcomeData()
    this.getAllVehicles()
    this.createdTimer() //定时


    this._getAgvForUseRatioPie()
    var curTime = new Date();
    if (window.localStorage.TokenExpire) {
      var expiretime = new Date(Date.parse(window.localStorage.TokenExpire));
      if (curTime >= expiretime) {
        if (global.IS_IDS4) {
          applicationUserManager.login();
        } else {
          this.$router.push("/login");
        }
      }
    } else {
      if (global.IS_IDS4) {
        applicationUserManager.login();
      } else {
        this.$router.push("/login");
      }
    }

    /*
        getAchieveUsers_IS4({}).then(res => {
          this.lineChartDataIDS4 = res.data.response;
        });
       */

    getServerInfo({}).then(res => {
      this.serverInfo = res.data.response;
    });

    // getActiveUsers({}).then(res => {
    //   this.welcomeInitData = res.data.response;
    // });
  }
};
</script>

<style scoped>
.el-carousel__item h3 {
  color: #475669;
  font-size: 14px;
  opacity: 0.75;
  line-height: 200px;
  margin: 0;
}

.el-carousel__item:nth-child(2n) {
  background-color: #99a9bf;
}

.el-carousel__item:nth-child(2n + 1) {
  background-color: #d3dce6;
}

.bg-blue-sub-item {
  max-width: 120px !important;
  height: 50px;
  float: left;
  color: #fff;
  font-size: 15px;
  line-height: 50px;
  padding: 0 10px;
}
.note .text {
  font-size: 14px;
}

.note .item {
  margin-bottom: 18px;
}
</style>

<style scoped>
#u124 {
  border-width: 0px;
  position: absolute;
  left: 54px;
  top: 173px;
  width: 83px;
  height: 80px;
  opacity: 0.85;
  color: #ffffff;
}
.ax_default {
  font-family: "Arial Normal", "Arial";
  font-weight: 400;
  font-style: normal;
  font-size: 13px;
  color: #333333;
  text-align: center;
  line-height: normal;
}
#u124_img {
  border-width: 0px;
  position: absolute;
  left: 0px;
  top: 0px;
  width: 83px;
  height: 80px;
}

.panel-group {
  margin-top: 18px;
}
.card-panel-col {
  margin-bottom: 32px;
  width: 20%;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
  margin-right: 2%;
  margin-left: 2%;
  float: left;
}

.card-panel {
  height: 108px;
  font-size: 12px;
  position: relative;
  overflow: hidden;
  color: #666;
  background: #fff;
  box-shadow: 4px 4px 40px rgba(0, 0, 0, 0.05);
  border-color: rgba(0, 0, 0, 0.05);
}

.card-panel .card-panel-icon-wrapper {
  color: #fff;
}
.card-panel .card-panel-icon-wrapper:hover {
  color: #36a3f7;
  background: #40c9c6;
}

.card-panel .card-panel .icon-people {
  background: #40c9c6;
}

.card-panel .card-panel .icon-message {
  background: #36a3f7;
}

.card-panel .card-panel .icon-money {
  background: #f4516c;
}

.card-panel .card-panel .icon-shopping {
  background: #34bfa3;
}

.card-panel .icon-people {
  color: #40c9c6;
}

.card-panel .icon-message {
  color: #36a3f7;
}

.card-panel .icon-money {
  color: #f4516c;
}

.card-panel .icon-shopping {
  color: #34bfa3;
}

.card-panel .card-panel-icon-wrapper {
  float: left;
  margin: 14px 0 0 14px;
  padding: 16px;
  transition: all 0.38s ease-out;
  border-radius: 6px;
}

.card-panel .card-panel-icon {
  float: left;
  font-size: 48px;
}

.card-panel .card-panel-description {
  float: left;
  font-weight: bold;
  margin-left: 30px;
  margin-top: 20px;
}
.card-panel .card-panel-description .card-panel-text {
  line-height: 18px;
  /* color: rgba(0, 0, 0, 0.45); */
  font-size: 16px;
  margin-bottom: 12px;
}

.card-panel .card-panel-description .card-panel-num {
  font-size: 36px;
  color: #f4516c;
}
.extoday {
  cursor: pointer;
}
.card-acuser-num {
  font-size: 36px;
  color: #40c9c6;
}
.greenColor {
  color: #40c9c6;
}
.hightemp {
  color: #f4516c;
}
.yellowColor {
  color: #f0b206ea;
}
.warnDivStyle {
  color: #f4516c;
  margin-left: 10px;
  margin-top: 10px;
  margin-bottom: 0px;
  font-size: 12px;
}
.pieStyle {
  margin-right: 10px;
  margin-top: 20px;
  margin-bottom: 20px;
}

@media (max-width: 550px) {
}
</style>