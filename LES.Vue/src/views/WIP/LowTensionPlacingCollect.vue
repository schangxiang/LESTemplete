<template>
  <section id='con_lf_top_div'
           :class="{'full-screen':fullscreen}">
    <el-col :span="24"
            style="padding-top:5px;">
      <el-button @click="screen"
                 type="info"
                 size="mini"
                 round
                 v-if="!fullscreen"
                 plain><i class="iconfont icon-touyingyi"></i>参观模式</el-button>
    </el-col>
    <el-col :span="8"
            class="colCard">
      <el-card class="box-card"
               :style="!fullscreen ? {background : '#fff'} : {background : '#103763'}">
        <div slot="header"
             class="clearfix">
          <span>
            <h2 class="title"
                :style="!fullscreen ? {color : '#333'} : {color : '#fff'}">上线工位</h2>
          </span>
        </div>
        <div>
          <el-form :model="addForm"
                   :style="!fullscreen ? {color : '#333'} : {color : '#fff'}"
                   size="small"
                   label-width="80px"
                   ref="addForm">
            <el-form-item label="工件号:"
                          prop="WorkOrderNo">
              <el-label>022113661</el-label>
            </el-form-item>
            <el-form-item label="批次号:"
                          prop="WorkOrderNo">
              <el-label>022113661</el-label>
            </el-form-item>
            <el-form-item label="批次号:"
                          prop="WorkOrderNo">
              <el-label>022113661</el-label>
            </el-form-item>
          </el-form>
        </div>
      </el-card>
    </el-col>
    <el-col :span="8"
            class="colCard">
      <el-card class="box-card">
        <div slot="header"
             class="clearfix">
          <span>上线工位</span>
        </div>
        <div>
          <el-form :model="addForm"
                   size="small"
                   label-width="80px"
                   ref="addForm">
            <el-form-item label="工件号:"
                          prop="WorkOrderNo">
              <el-label>022113661</el-label>
            </el-form-item>
            <el-form-item label="批次号:"
                          prop="WorkOrderNo">
              <el-label>022113661</el-label>
            </el-form-item>
            <el-form-item label="批次号:"
                          prop="WorkOrderNo">
              <el-label>022113661</el-label>
            </el-form-item>
          </el-form>
        </div>
      </el-card>
    </el-col>
    <el-col :span="8"
            class="colCard">
      <el-card class="box-card">
        <div slot="header"
             class="clearfix">
          <span>上线工位</span>
        </div>
        <div>
          <el-form :model="addForm"
                   size="small"
                   label-width="80px"
                   ref="addForm">
            <el-form-item label="工件号:"
                          prop="WorkOrderNo">
              <el-label>022113661</el-label>
            </el-form-item>
            <el-form-item label="批次号:"
                          prop="WorkOrderNo">
              <el-label>022113661</el-label>
            </el-form-item>
            <el-form-item label="批次号:"
                          prop="WorkOrderNo">
              <el-label>022113661</el-label>
            </el-form-item>
          </el-form>
        </div>
      </el-card>
    </el-col>
  </section>
</template> 
 
<script> 
import { checkFull, setFullScreen } from '../../../util/tools'

export default {
  components: {},//注册子组件 
  data () {
    return {
      fullscreen: false,
      addForm: {}
    }
  },
  methods: {
    closeScreenFull () {
      this.fullscreen = false
    },
    //设置全屏
    screen () {
      setFullScreen(true, 'con_lf_top_div', this.fullscreen);
      this.fullscreen = !this.fullscreen;
    }
  },
  created () {
    let that = this
    window.onresize = function () {
      if (!checkFull()) {
        // 退出全屏后要执行的动作
        console.log("退出全屏")
        that.closeScreenFull();
      }
    }
  },
  mounted () {

  }
}

</script> 
 
<style lang="scss" scoped>
.colCard {
  padding-left: 10px;
  padding-right: 10px;
  padding-top: 10px;
  padding-bottom: 10px;
}
.full-screen {
  position: fixed;
  top: 0;
  right: 0;
  left: 0;
  bottom: 0;
  background: #103763;
  .wrap-out-box {
    top: 0;
    background: #103763;
  }
}
</style> 
