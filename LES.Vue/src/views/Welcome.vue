<template>
  <div style="margin-top: 30px; min-hight:1000px;">
    <el-row class="panel-group">
      <el-col class="card-panel-col"
              style="float: left;width: calc(96% - 430px);margin: 0;">
        <el-card class="welcome-card activeuser note">
          <div slot="header"
               class="clearfix">
            <span>欢迎访问LES系统</span>
          </div>

        </el-card>
      </el-col>

    </el-row>

  </div>
</template>

<script>
import Vue from "vue";
import VCharts from "v-charts";
Vue.use(VCharts);

import applicationUserManager from "../Auth/applicationusermanager";
import {
  getServerInfo,
  getAccessLogs,
  getIds4UsersGrow,
  getActiveUsers,
  getAchieveUsers_IS4
} from "../api/api";

export default {
  name: "Welcome",
  data () {
    return {
      listLoading: false,
      welcomeInitData: {},
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
      lineChartMarkPoint: {
        data: [
          {
            name: "最大值",
            type: "max"
          },
          {
            name: "最小值",
            type: "min"
          }
        ]
      }
    };
  },
  methods: {
    getBck (index) {
      return `background: rgb(${43 + index * 20}, ${148 +
        index * 10}, 255) none repeat scroll 0% 0%;`;
    },
    toLogs () {
      this.$router.replace({
        path: "/Logs/Index"
      });
    }
  },
  mounted () {
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

    getAchieveUsers_IS4({}).then(res => {
      this.lineChartDataIDS4 = res.data.response;
    });

    getServerInfo({}).then(res => {
      this.serverInfo = res.data.response;
    });

    getActiveUsers({}).then(res => {
      this.welcomeInitData = res.data.response;
    });
  }
};
</script>

<style scoped>
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
.panel-group {
  margin-top: 18px;
}
.card-panel-col {
  margin-bottom: 32px;
  width: 115px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
  margin-right: 2%;
  float: right;
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
  color: rgba(0, 0, 0, 0.45);
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

@media (max-width: 550px) {
  .card-panel-description {
    display: none;
  }

  .card-panel-icon-wrapper {
    float: none !important;
    width: 100%;
    height: 100%;
    margin: 0 !important;
  }
  .card-panel-icon-wrapper .svg-icon {
    display: block;
    margin: 14px auto !important;
    float: none !important;
  }
}
</style>