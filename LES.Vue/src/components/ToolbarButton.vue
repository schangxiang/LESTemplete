<template>
  <div style="padding-bottom: 0px;float: left;">
    <el-form :inline="true"
             style="margin-bottom: 0px;"
             @submit.native.prevent>
      <el-form-item v-if="buttonList!=null&&buttonList.length>0"
                    style="margin-bottom:0px;">
        <el-button-group>
          <!-- 这个就是当前页面内，所有的btn列表 -->
          <el-form-item v-for="item in buttonList"
                        v-bind:key="item.id"
                        style="margin-bottom:0px;">
            <!--减少和顶部按钮的间距,增加样式 [EditBy shaocx,2021-02-02] -->
            <!-- 这里触发点击事件 -->
            <el-button :type="setType(item)"
                       :icon="setIcon(item)"
                       size="small"
                       v-if="!item.IsHide"
                       @click="callFunc(item)">{{item.name}}</el-button>
          </el-form-item>
        </el-button-group>
      </el-form-item>
    </el-form>
  </div>
</template>
<script>
export default {
  name: "ToolbarButton",
  data () {
    return {

    };
  },
  props: ["buttonList"], //接受父组件传值
  methods: {
    setIcon (item) {
      if (item.Func) {
        if (item.Func.toLowerCase().indexOf('handledel'.toLowerCase()) != -1) {
          return 'el-icon-delete'
        } else if (item.Func.toLowerCase().indexOf('handleedit'.toLowerCase()) != -1) {
          return 'el-icon-edit'
        } else if (item.Func.toLowerCase().indexOf('handleadd'.toLowerCase()) != -1) {
          return 'el-icon-plus'
        } else if (item.Func.toLowerCase().indexOf('get') != -1) {
          return 'el-icon-search'
        } else if (item.Func.toLowerCase().indexOf('export') != -1) {
          return 'el-icon-download'
        } else if (item.Func.toLowerCase().indexOf('import') != -1) {
          return 'el-icon-upload2'
        } else if (item.Func.toLowerCase().indexOf('handleview') != -1) {
          return 'el-icon-view'
        }
        return 'fa ' + item.iconCls
      }
    },
    setType (item) {
      if (item.ButtonType != undefined && item.ButtonType != '') {
        return item.ButtonType
      } else {
        if (item.Func != undefined && item.Func != '') {
          if ((item.Func.toLowerCase().indexOf('handledel') != -1 || item.Func.toLowerCase().indexOf('delete') != -1 || item.Func.toLowerCase().indexOf('stop') != -1)) {
            return 'danger'
          } else if (item.Func.toLowerCase().indexOf('export') != -1) {
            return 'success'
          }
        }
      }
      return 'primary'
    },
    callFunc (item) {
      this.$emit("callFunction", item); //将值传给父组件
    },
    showHighFilter () {
      this.$emit("showHighFilter", null);
    }
  }
};
</script>