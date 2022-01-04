<template>
  <el-dialog title="数据导入"
             :visible.sync="dialogVisible"
             width="30%"
             :before-close="handleClose">
    <el-upload class="upload-demo"
               ref="upload"
               accept=".xls, .xlsx"
               :action="fullUrl"
               :on-preview="handlePreview"
               :on-remove="handleRemove"
               :on-success="fileUploadSuccess"
               :on-error="fileUploadFail"
               :on-change="fileChange"
               :file-list="fileList"
               :limit="1"
               :auto-upload="false"
               :headers="headers">
      <el-button slot="trigger"
                 size="small"
                 type="primary">选取文件</el-button>
      &nbsp; &nbsp; &nbsp;
      <el-button size="small"
                 @click="downloadTemplate">导入模板下载</el-button>
      <div slot="tip"
           class="el-upload__tip">请按照导入模板中的数据格式导入</div>
    </el-upload>
    <span slot="footer"
          class="dialog-footer">
      <el-button @click="dialogVisible = false">取 消</el-button>
      <!-- <el-button type="primary" @click="dialogVisible = false">确 定</el-button> -->
      <el-button style="margin-left: 10px;"
                 type="success"
                 @click="submitUpload">数据导入</el-button>
      <!-- <div slot="tip" class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div> -->
    </span>
  </el-dialog>
</template>
<script>
export default {
  name: "ToolbarImportExcel",
  data () {
    return {
      fullUrl: "../api/Excel/FormSingleFileUpload?categroy=" + this.importExcelCategroy,//上传文件的请求地址
      fileList: [], //文件列表
      dialogVisible: false,//Dialog显示状态
      headers: { "X-Token": '' }//设置上传的请求头部
    };
  },
  props: {
    //接受父组件传值
    //导入Excel服务的分类名称
    importExcelCategroy: {
      type: String,
      required: true
    },
    //Excel模板下载地址
    templeteFileDownName: {
      type: String,
      required: true
    }
  },
  methods: {
    //打开弹窗
    showPage () {
      this.dialogVisible = true
    },
    //关闭弹窗
    handleClose () {
      this.dialogVisible = false;
    },
    //上传到服务器
    submitUpload () {
      console.log(this.fileList);
      if (this.fileList.length <= 0) {
        this.$message.error("请先选择需要上传的文件！");
        return false;
      }
      this.$refs.upload.submit();
    },
    //文件上传服务端失败时的钩子
    fileUploadFail: function (err, file, fileList) {
      console.log("文件上传失败", file, fileList);
    },
    //文件上传服务端成功时的钩子
    fileUploadSuccess: function (response, file, fileList) {
      console.log("上传成功");
      console.log(response);
      //清空已上传的文件列表
      this.$refs.upload.clearFiles();
      if (response.success) {
        this.dialogVisible = false;
        this.$message({
          message: response.msg,
          type: "success"
        });
      } else {
        this.$message.error(response.msg);
      }
    },
    //文件状态改变时的钩子，添加文件、上传成功和上传失败时都会被调用
    fileChange (file, fileList) {
      //解决无法判断el-upload是否上传过文件问题
      this.fileList = fileList;
      console.log("选择文件上传成功后显示的内容》", file, fileList);
    },
    //文件列表移除文件时的钩子
    handleRemove (file, fileList) {
      this.fileList = [];
      // return this.$confirm(`确定移除 ${file.name}？`);
    },
    //点击文件列表中已上传的文件时的钩子
    handlePreview (file) {
      console.log(file);
    },
    //导入模板下载
    downloadTemplate () {
      window.location.href = '../api/Excel/DownExcelTemplete?filename=' + this.templeteFileDownName;
    }
  }
};
</script>