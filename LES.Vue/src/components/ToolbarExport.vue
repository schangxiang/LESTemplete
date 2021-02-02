<template>
  <el-dialog title="提示"
             :visible.sync="sonPrintDialogVisible"
             width="30%"
             center>
    <span>请选择导出范围</span>
    <span slot="footer"
          class="dialog-footer">
      <el-button type="primary"
                 @click="exprotCurPage">仅 本 页 导 出</el-button>
      <el-button type="success"
                 @click="exprotAll">全 部 导 出</el-button>
      <el-button @click="sonPrintDialogVisible = false">取 消 导 出</el-button>

    </span>
  </el-dialog>
</template>
<script>
import XLSX, { WorkSheet } from "xlsx";
import { getNowFormatDateForExport } from '../../util/tools'
export default {
  name: "ToolbarExport",
  data () {
    return {
      sonPrintDialogVisible: false
    };
  },
  props: {
    exportFileName: {
      type: String,
      required: true,
    },
    //接受父组件传值
    filterVal: {
      type: Array,
      required: true,
    },
    tHeader: {
      type: Array,
      required: true,
    },
    currentPageData: {
      type: Array,
      required: true,
    }
  },
  methods: {
    showPrintPage () {
      this.sonPrintDialogVisible = true
    },
    // 仅导出本页
    exprotCurPage () {
      this.export2Excel(this.currentPageData)
    },
    // 导出全部
    exprotAll () {
      this.$parent.exportAllData()
    },
    /** 加载方法 */
    loadingToload (text) {
      const loading = this.$loading({
        lock: true,
        text: text,
        spinner: 'el-icon-loading',
        background: 'rgba(255, 255, 255, 0.8)',
        target: document.querySelector('#mainDiv')
      })
      this.curLoading = loading
    },
    // 导出Excel表格
    export2Excel (exportData) {
      var text = this.exportFileName + '导出中...'
      this.loadingToload(text)

      require.ensure([], () => {
        const { export_json_to_excel } = require('../../util/Export2Excel.js') // Export2Excel.js路径
        const list = exportData // 数据源
        const data = this.formatJson(this.filterVal, list) // 数据为json格式
        export_json_to_excel(this.tHeader, data, this.exportFileName + '(' + getNowFormatDateForExport() + ')') // 导出表格名字
        this.$notify({
          title: '导出成功',
          message: this.exportFileName + '导出成功',
          type: 'success'
        })
        this.sonPrintDialogVisible = false
        this.curLoading.close()
      })
    },
    formatJson (filterVal, jsonData) {
      return jsonData.map(v => filterVal.map(j => v[j]))
    }
  }
};
</script>