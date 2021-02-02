<template>
  <el-col :span="24"
          class="toolbar"
          style="padding-bottom: 0px;">
    <el-form :inline="true"
             @submit.native.prevent>
      <el-form-item v-if="false">
        <el-input v-model="searchVal"
                  placeholder="请输入内容"></el-input>
      </el-form-item>
      <el-form-item v-if="buttonList!=null&&buttonList.length>0">
        <el-button-group>
          <!-- 这个就是当前页面内，所有的btn列表 -->
          <el-form-item v-for="item in buttonList"
                        v-bind:key="item.id">
            <!-- 这里触发点击事件 -->
            <el-button :type="setType(item)"
                       :icon="setIcon(item)"
                       v-if="!item.IsHide"
                       @click="callFunc(item)">{{item.name}}</el-button>
          </el-form-item>
        </el-button-group>
      </el-form-item>
      <el-form style="float:right;">
        <el-form-item label="">
          <el-input placeholder="请输入内容"></el-input>
        </el-form-item>
        <el-form-item label="">
          <el-button type="primary"
                     icon="el-icon-search">查询</el-button>
          &nbsp;
          <el-link type="primary"
                   :underline="false">重置</el-link>
          &nbsp;
          <el-link type="primary"
                   @click="showHighFilter"
                   :underline="false">高级</el-link>
        </el-form-item>
      </el-form>
    </el-form>
  </el-col>
</template>
<script>
export default {
  name: "Toolbar",
  data () {
    return {
      searchVal: "" //双向绑定搜索内容
    };
  },
  props: ["buttonList"], //接受父组件传值
  methods: {
    setIcon (item) {
      if (item.Func) {
        if (item.Func.toLowerCase().indexOf('handleDel'.toLowerCase()) != -1) {
          return 'el-icon-delete'
        } else if (item.Func.toLowerCase().indexOf('handleEdit'.toLowerCase()) != -1) {
          return 'el-icon-edit'
        } else if (item.Func.toLowerCase().indexOf('handleAdd'.toLowerCase()) != -1) {
          return 'el-icon-plus'
        } else if (item.Func.toLowerCase().indexOf('get') != -1) {
          return 'el-icon-search'
        }
      }
      return ""
    },
    setType (item) {
      return item.Func && (item.Func.toLowerCase().indexOf('handledel') != -1 || item.Func.toLowerCase().indexOf('stop') != -1) ? 'danger' : 'primary'
    },
    callFunc (item) {
      item.search = this.searchVal;
      this.$emit("callFunction", item); //将值传给父组件
    },
    showHighFilter () {
      this.$emit("showHighFilter", null);
    }
  }
};
</script>