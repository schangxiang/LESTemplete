/**
 * 搜索栏公共组件
 */
<template>
  <div class="search-form-box">
    <el-form :model="formData"
             :label-width="labelWidth"
             ref="formRef"
             size="small"
             :inline="true">

      <el-form-item>
        <el-input v-model="formData.searchVal"
                  size="small"
                  class="searchClass"
                  :placeholder="searchFormInputPlaceholder">
          <div slot="prepend">
            <div class="centerClass">
              <el-select v-model="formData.searchVal_FilterMode"
                         placeholder="请选择"
                         size="small"
                         style="width: 90px">
                <el-option v-for="item in commonSearchOptions"
                           :key="item.value"
                           :label="item.label"
                           :value="item.value">
                </el-option>
              </el-select>
            </div>
          </div>
        </el-input>

      </el-form-item>

      <el-form-item label="">
        <el-button v-if="btnItems.includes('search')"
                   type="primary"
                   class="btn-search"
                   icon="el-icon-search"
                   size="small"
                   @click="onSearch">查询</el-button>
        &nbsp;
        <el-link v-if="btnItems.includes('reset')"
                 :underline="false"
                 type="primary"
                 class="btn-reset"
                 @click="onReset">重置</el-link>
        &nbsp;
        <el-link type="primary"
                 @click="showHighFilter"
                 :underline="false">高级</el-link>
        <el-drawer style="top:140px;height:520px;"
                   :modal="false"
                   :withHeader="false"
                   :size="drawerSize"
                   :close-on-press-escape="true"
                   :show-close="false"
                   :visible.sync="drawer"
                   :direction="direction">
          <div class="popup-right">
            <div slot="header"
                 class="drawer_header">高级查询</div>
            <div class="drawer_main"
                 style="height:auto;margin:10px 5px 15px 20px;">
              <el-scrollbar :noresize="false"
                            :native="false"
                            wrap-class="scrollbar-wrap">
                <el-form-item v-for="(item, index) in formOptions"
                              :key="newKeys[index]"
                              :prop="item.prop"
                              class="searchFormCss"
                              style="height:auto;margin:10px 5px 15px 20px;"
                              :label="item.label ? (item.label + '：') : ''"
                              :rules="item.rules">
                  <formItem ref="SonFormItem"
                            v-model="formData[item.prop]"
                            :currentValMode="formData[item.prop+'_FilterMode']"
                            @currentValModeInput="valueMode => { formData[item.prop+'_FilterMode'] = valueMode }"
                            :controlStyle="controlStyle"
                            :itemOptions="item" />
                </el-form-item>
              </el-scrollbar>
            </div>
            <div slot="footer"
                 class="drawer_footer">
              <el-button class="left"
                         type="text"
                         size="small"
                         @click="drawer=false">收回</el-button>
              <el-button class="right"
                         icon="el-icon-refresh-left"
                         size="small"
                         @click="onReset">重置</el-button>
              <el-button class="right"
                         type="primary"
                         icon="el-icon-search"
                         size="small"
                         @click="onSearch">查询</el-button>
            </div>
          </div>
        </el-drawer>
      </el-form-item>

    </el-form>
  </div>
</template>

<script>
import formItem from './FormItem'
import { createUUID } from '../../util/tools'

export default {
  components: { formItem },
  props: {
    //通用查询配置
    commonSearchOptionSet: {
      type: String,
      required: true,
    },
    controlStyle: {
      type: Object,
      required: true,
    },
    drawerSize: {
      type: String,
      required: true,
    },
    //接受父组件传过来的值
    labelWidth: { // 显示label的宽度
      type: String,
      required: true,
    },
    //子搜索组件传递的显示内容
    searchFormInputPlaceholder: {
      type: String,
      required: true,
    },
    //子搜索组件传递的属性名
    searchFormInputAttrs: {
      type: Array,
      required: true,
    },
    /**
     * 表单配置
     * 示例：
     * [{
     *   label: '用户名', // label文字
     *   prop: 'username', // 字段名
     *   element: 'el-input', // 指定elementui组件
     *   initValue: '阿黄', // 字段初始值
     *   placeholder: '请输入用户名', // elementui组件属性
     *   rules: [{ required: true, message: '必填项', trigger: 'blur' }], // elementui组件属性
     *   events: { // elementui组件方法
     *     input (val) {
     *       console.log(val)
     *     },
     *     ...... // 可添加任意elementui组件支持的方法
     *   }
     *   ...... // 可添加任意elementui组件支持的属性
     * }]
     */
    formOptions: {
      type: Array,
      required: true,
      default () {
        return []
      }
    },
    // 提交按钮项，多个用逗号分隔（search, export, reset）
    btnItems: {
      type: String,
      default () {
        return 'search,reset'
      }
    },
    buttonList: {}
  },

  data () {
    return {
      drawer: false,
      direction: 'rtl',
      //表单form对象
      formData: {
      },
      commonSearchOptions: [
        {
          value: '1',
          label: '模糊'
        },
        {
          value: '2',
          label: '精准'
        }
      ]
    }
  },

  computed: {
    newKeys () {
      return this.formOptions.map(v => {
        return createUUID()
      })
    }
  },

  created () {
    this.addInitValue()
  },

  methods: {
    // 是否显示高级搜索框
    showHighFilter () {
      this.drawer = !this.drawer
    },
    // 校验
    onValidate (callback) {
      this.$refs.formRef.validate(valid => {
        if (valid) {
          console.log('提交成功')
          console.log(this.formData)
          callback()
        }
      })
    },
    // 搜索
    onSearch () {
      this.onValidate(() => {
        this.$emit('onSearch', this.formData)
      })
    },
    // 导出
    onExport () {
      this.onValidate(() => {
        this.$emit('onExport', this.formData)
      })
    },
    // 重置
    onReset () {
      //重置 高级选线中的通用筛选框
      //因为SonFormItem有多项，因此是一个数组！！！
      //debugger
      if (this.$refs.SonFormItem != undefined) {
        this.$refs.SonFormItem.forEach(x => {
          //调用子组件的方法
          x.resetCommonFitler()
        })
      }

      this.$refs.formRef.resetFields()
      //一些公共的值赋初始值
      this.formData.searchVal = ''
      this.formData.searchVal_FilterMode = '1' //默认是 模糊查询
      if (this.commonSearchOptionSet == '精准') {
        this.formData.searchVal_FilterMode = '2'
      }
      this.formData.searchFormInputAttrs = this.searchFormInputAttrs
    },
    // 添加初始值
    addInitValue () {
      const obj = {}
      this.formOptions.forEach(v => {
        if (v.initValue !== undefined) {
          obj[v.prop] = v.initValue
        } else {
          obj[v.prop] = '' //注意：初始的时候，必须form对象中的每个属性都赋初始值，否则会导致在【重置】按钮后，输入值输入不上的问题，即绑定脱绑问题
        }
      })
      obj.searchVal_FilterMode = '1' //默认是 模糊查询
      if (this.commonSearchOptionSet == '精准') {
        obj.searchVal_FilterMode = '2'
      }
      obj.searchFormInputAttrs = this.searchFormInputAttrs
      obj.searchVal = ''
      //debugger
      this.formData = obj
    },
    getFormData () {
      //debugger
      return this.formData
    },
    setNewFormOptions (new_formOptions) {
      this.formOptions = new_formOptions
    }

  }


}
</script>

<style lang='less' scoped>
.el-select .el-input {
  width: 130px;
}
.input-with-select .el-input-group__prepend {
  background-color: #fff;
}
.searchClass {
  //border: 1px solid #c5c5c5;
  border-radius: 20px;
  background: #f4f4f4;
  margin-top: 1px;
}
.searchClass .el-input-group__prepend {
  border: none;
  background-color: transparent;
  padding: 0 10px 0 30px;
}
.searchClass .el-input-group__append {
  border: none;
  background-color: transparent;
}
.searchClass .el-input__inner {
  height: 36px;
  line-height: 36px;
  border: none;
  background-color: transparent;
}
.searchClass .el-icon-search {
  font-size: 16px;
}
.searchClass .centerClass {
  height: 100%;
  line-height: 100%;
  display: inline-block;
  vertical-align: middle;
  text-align: right;
}
.searchClass .line {
  width: 1px;
  height: 26px;
  background-color: #c5c5c5;
  margin-left: 14px;
}
.searchClass:hover {
  //border: 1px solid #d5e3e8;
  background: #fff;
}
.searchClass:hover .line {
  background-color: #d5e3e8;
}
.searchClass:hover .el-icon-search {
  color: #409eff;
  font-size: 16px;
}

.searchFormCss {
  font-weight: 700;
  color: #606266;
}
.search-form-box {
  float: right;
  display: flex;
  margin-bottom: 0px;
  margin-top: 5px;

  .btn-box {
    padding-top: 5px;
    display: flex;

    button {
      height: 28px;
    }
  }
  .el-form {
    /deep/ .el-form-item__label {
      padding-right: 0;
    }
    .el-form-item {
      margin-bottom: 0;

      &.is-error {
        margin-bottom: 22px;
      }
    }
    // el-input宽度
    /deep/ .form-item {
      > .el-input:not(.el-date-editor) {
        width: 120px;
      }
    }
    /deep/ .el-select {
      width: 120px;
    }
    /deep/ .el-cascader {
      width: 200px;
    }
  }
}
.popup-right {
  text-align: left;
  line-height: 1.2;
  z-index: 1000;
  .drawer_header {
    background-color: #f6f6f6;
    line-height: 1.2;
    padding: 10px;
    font-size: 14px;
  }
  .drawer_main {
    background-color: #fff;
    padding: 0;
    max-height: 495px;
    min-height: 400px;
    /deep/ .el-scrollbar {
      height: 395px;
    }
    /deep/ .scrollbar-wrap {
      max-height: 410px;
      overflow-x: hidden;
      .el-form {
        padding: 10px;
        .el-form-item {
          margin-bottom: 10px;
          .el-date-editor .el-range-separator {
            width: 25px;
          }
          /deep/ .operator .el-input__inner {
            padding-left: 5px;
            padding-right: 20px;
            text-align: center;
          }
        }
      }
    }
  }
  .drawer_footer {
    background-color: #f6f6f6;
    padding: 13px;
    overflow: hidden;
    .left {
      float: left;
    }
    .right {
      float: right;
    }
  }
}
</style>

