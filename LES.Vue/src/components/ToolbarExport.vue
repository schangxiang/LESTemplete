<template>
  <el-dialog title="提示"
             :visible.sync="sonPrintDialogVisible"
             width="35%"
             center>
    <span>请选择导出范围！</span>
    <span slot="footer"
          class="dialog-footer">
      <el-button type="primary"
                 icon="el-icon-menu"
                 @click="exprotCurPage">仅 本 页 导 出</el-button>
      <el-button type="success"
                 icon="el-icon-s-grid"
                 @click="exprotAll">全 部 导 出</el-button>
      <el-button @click="sonPrintDialogVisible = false"
                 icon="fa fa-power-off">取 消 导 出</el-button>
    </span>
  </el-dialog>
</template>
<script>
import XLSX, { WorkSheet } from "xlsx";
import { getNowFormatDateForExport, IsDateStr, formatDate } from '../../util/tools'
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
    exportColumnHeader: {
      type: Array,
      required: true
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
        let list = exportData // 数据源
        list = this.convertDataList(list)
        var _filterVal = Object.keys(this.exportColumnHeader);
        var _tHeader = Object.values(this.exportColumnHeader);
        const data = this.formatJson(_filterVal, list) // 数据为json格式
        export_json_to_excel(_tHeader, data, this.exportFileName + '(' + getNowFormatDateForExport() + ')') // 导出表格名字
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
    },
    //转换LIST，将日期类型转换成字符串
    convertDataList (list) {
      for (let index = 0; index < list.length; index++) {
        for (let j = 0; j < Object.values(list[index]).length; j++) {
          const element2 = Object.values(list[index])[j]
          if (IsDateStr(element2)) {
            Object.defineProperty(list[index], Object.keys(list[index])[j], {
              value: formatDate(new Date(element2), 'yyyy-MM-dd hh:mm:ss')
            })
          }
        }
      }
      return list
    }
  }
};
</script>