<template>
  <el-form ref="editForm"
           :model="editForm"
           label-width="80px"
           @submit.prevent="onSubmit"
           style="margin:20px;width:60%;min-width:600px;height:1000px;">
    <el-form-item label="我的昵称">
      <el-input v-model="editForm.uRealName"></el-input>
    </el-form-item>
    <el-form-item label="旧密码"
                  prop="uLoginPWD">
      <el-input v-model="editForm.uLoginPWD"
                show-password
                auto-complete="off"></el-input>
    </el-form-item>
    <el-form-item label="新密码"
                  prop="uLoginPWDNew">
      <el-input v-model="editForm.uLoginPWDNew"
                show-password
                auto-complete="off"></el-input>
    </el-form-item>
    <el-form-item label="确认密码"
                  prop="uLoginPWDConfirm">
      <el-input v-model="editForm.uLoginPWDConfirm"
                show-password
                auto-complete="off"></el-input>
    </el-form-item>

    <el-form-item label="头像">
      <el-upload class="avatar-uploader"
                 :action="uploadPicUrl"
                 :show-file-list="false"
                 :headers="token"
                 :data="ruleForm"
                 :on-success="handleAvatarSuccess"
                 :before-upload="beforeAvatarUpload">
        <img v-if="editForm.tdLogo"
             :src="editForm.tdLogo"
             class="avatar" />
        <i v-else
           class="el-icon-plus avatar-uploader-icon plus-sign"></i>
      </el-upload>
      <el-button type="danger"
                 @click="removeLogo">移除头像</el-button>
    </el-form-item>

    <el-form-item label="备注"
                  prop="uRemark">
      <el-input type="textarea"
                auto-complete="off"
                v-model="editForm.uRemark"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button @click="onSubmit"
                 type="primary">更新</el-button>
      <el-button @click="onCancel">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>

import { md5Password, updatePerson, uploadPicUrl, removePic } from '../../api/api';

export default {
  data () {
    return {
      uploadPicUrl: uploadPicUrl,
      editForm: {
        isUpdatePWD: false,//是否需要修改密码
        uRemark: "",
        id: 0,
        uID: 0,
        RID: 0,
        uLoginName: "",
        uRealName: "",
        name: "",
        sex: -1,
        age: 0,
        birth: "",
        desc: "",
        addr: "",
        tdLogo: "" //头像地址
      },
      token: {
        Authorization: "Bearer "
      },
      ruleForm: {
        max_ver: "",
        min_ver: "",
        enable: ""
      },
      beforeAvatarUpload (file) {
        const isJPG = file.type === "image/jpeg";
        const isLt1M = file.size / 1024 / 1024 < 1;

        // if (!isJPG) {
        //   this.$message.error('上传头像图片只能是 JPG 格式!')
        // }
        if (!isLt1M) {
          this.$message.error("上传头像图片大小不能超过 1MB!");
        }
        return isLt1M;
      }
    };
  },
  methods: {
    onCancel () {
      this.initUserInfo()
      this.editForm.uLoginPWD = ''
      this.editForm.uLoginPWDConfirm = ''
      this.editForm.uLoginPWDNew = ''
    },
    //删除图片
    removeLogo () {
      if (this.editForm.tdLogo == undefined) {
        this.$message({
          message: '没有要移除的头像',
          type: 'warning'
        });
        return;
      }
      let para = {
        path: this.editForm.tdLogo
      };
      removePic(para).then((res) => {
        if (res.data.success) {
          this.editForm.tdLogo = ''
          this.$message({
            message: '移除头像成功！',
            type: 'success'
          });
        }
        else {
          this.$message({
            message: res.data.msg,
            type: 'error'
          });
        }
      });

    },
    onSubmit () {
      let myEditForm = this.editForm
      if (!(myEditForm.uLoginPWDNew == undefined || myEditForm.uLoginPWDNew == '')
        && (myEditForm.uLoginPWDConfirm == undefined || myEditForm.uLoginPWDConfirm == '')) {
        //只有都为空的时候，才认为不修改密码
        if (myEditForm.uLoginPWDNew == undefined || myEditForm.uLoginPWDNew == '') {
          this.$message({
            message: "验证失败！新密码不能为空",
            type: "error"
          });
          return;
        }
        //验证新密码和确认密码是否一致
        if (myEditForm.uLoginPWDNew != myEditForm.uLoginPWDConfirm) {
          this.$message({
            message: "验证失败！新密码和确认密码不相同",
            type: "error"
          });
          return;
        }
        this.editForm.isUpdatePWD = true;
        let para = {
          password: this.editForm.uLoginPWD
        }
        md5Password(para).then((res) => {
          var user = JSON.parse(window.localStorage.user);
          if (res != user.uLoginPWD) {
            this.$message({
              message: "验证失败！旧密码不正确",
              type: "error"
            });
            return;
          }
          //密码验证成功后
          let para = Object.assign({}, this.editForm);
          updatePerson(para).then((res) => {
            if (res.data.success) {
              this.$message({
                message: res.data.msg + ',请重新登录',
                type: 'success'
              });
            }
            else {
              this.$message({
                message: res.data.msg,
                type: 'error'
              });
            }
          });
        });
      } else {
        //不修改密码
        this.editForm.isUpdatePWD = false;
        let para = Object.assign({}, this.editForm);
        updatePerson(para).then((res) => {
          if (res.data.success) {
            this.$message({
              message: res.data.msg + ',请重新登录',
              type: 'success'
            });
          }
          else {
            this.$message({
              message: res.data.msg,
              type: 'error'
            });
          }
        });
      }
    },
    handleAvatarSuccess (res, file) {
      this.editForm.tdLogo = "/" + res.response;
    },
    fileDownload () {
      let self = this
      let token = "132465"
      // 将token放到头中，设置下载进度、请求参数、返回类型
      axios({
        url: '/images/Down/Bmd?filename=' + self.rulesForm.data.ItemBmdPath,
        method: 'get',
        headers: { 'FileToken': token },
        onDownloadProgress: p => {
          // this.precent = Math.floor(100 * (p.loaded / p.total))
        },
        responseType: 'blob'
      }).then(data => {
        // 如果后端用encodeURI对文件名进行了编码，前端需用decodeURI进行解码（主要为处理特殊字符）
        let fileName = decodeURI(data.headers.filename)
        // 由于ie不支持download属性，故需要做兼容判断
        if (navigator.appVersion.toString().indexOf('.NET') > 0) {
          // ie独有的msSaveBlob属性，data.data为Blob文件流
          window.navigator.msSaveBlob(data.data, fileName)
        } else {
          // 以下流程即为文章开始的下载流程
          let url = window.URL.createObjectURL(data.data)
          let link = document.createElement('a')
          link.style.display = 'none'
          link.href = url
          link.download = fileName
          document.body.appendChild(link)
          link.click()
          window.URL.revokeObjectURL(link.href);
        }
      })
    },
    initUserInfo () {
      var user = JSON.parse(window.localStorage.user);
      //console.log(user)
      this.editForm.uRealName = user ? user.uRealName : "";
      this.editForm.uRemark = user ? user.uRemark : "";
      this.editForm.uID = user ? user.uID : "";
      this.editForm.tdLogo = user ? user.tdLogo : "";
    }
  },
  mounted () {
    let tokenStr = window.localStorage.Token;
    this.token = {
      Authorization: "Bearer " + tokenStr
    };

    this.initUserInfo()
  }
};
</script>

<style>
.avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}

.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}

.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 120px;
  height: 120px;
  text-align: center;
}
.plus-sign {
  line-height: 120px !important;
}
.avatar {
  width: 120px;
  height: 120px;
  display: block;
}

.markdown-body {
  height: 500px !important;
}
</style>