/**
 * 表单匹配项
 */
<template>

  <div class='form-item'>
    <el-select v-model="currentValMode2"
               v-if="isShowCommonFitler"
               placeholder="请选择"
               size="small"
               style="width: 90px;margin-right:10px;">
      <el-option v-for="item in commonFitlerOptions"
                 :key="item.value"
                 :label="item.label"
                 :value="item.value">
      </el-option>
    </el-select>
    <el-input v-if="isInput"
              v-model="currentVal"
              v-bind="bindProps"
              :style="{width:controlStyle.width}"
              size="small"
              v-on="bindEvents"></el-input>

    <el-input-number v-if="isInputNumber"
                     v-model="currentVal"
                     v-bind="bindProps"
                     :style="{width:controlStyle.width}"
                     size="small"
                     v-on="bindEvents"
                     :controls-position="itemOptions['controls-position'] || 'right'"></el-input-number>

    <el-select v-if="isSelect"
               v-model="currentVal"
               v-bind="bindProps"
               size="small"
               :style="{width:controlStyle.width}"
               v-on="bindEvents"
               clearable>
      <el-option v-for="item in itemOptions.options"
                 :key="item.value"
                 :label="item.label"
                 :value="item.value"></el-option>
    </el-select>

    <!-- datetimerange/daterange -->
    <el-date-picker v-if="isDatePickerDateRange"
                    v-model="currentVal"
                    v-bind="bindProps"
                    size="small"
                    :style="{width:controlStyle.width}"
                    v-on="bindEvents"
                    :type="itemOptions.type || 'datetimerange'"
                    clearable
                    :picker-options="pickerOptionsRange"
                    start-placeholder="开始日期"
                    range-separator="至"
                    end-placeholder="结束日期"
                    :default-time="['00:00:00', '23:59:59']"
                    value-format="yyyy-MM-dd HH:mm:ss"></el-date-picker>

    <!-- monthrange -->
    <el-date-picker v-if="isDatePickerMonthRange"
                    v-model="currentVal"
                    v-bind="bindProps"
                    v-on="bindEvents"
                    type="monthrange"
                    :style="{width:controlStyle.width}"
                    size="small"
                    clearable
                    :picker-options="pickerOptionsRangeMonth"
                    start-placeholder="开始日期"
                    range-separator="至"
                    end-placeholder="结束日期"
                    value-format="yyyy-MM"></el-date-picker>

    <!-- others -->
    <el-date-picker v-if="isDatePickerOthers"
                    v-model="currentVal"
                    v-bind="bindProps"
                    size="small"
                    :style="{width:controlStyle.width}"
                    v-on="bindEvents"
                    :type="itemOptions.type"
                    clearable
                    placeholder="请选择日期"></el-date-picker>

    <el-cascader v-if="isCascader"
                 v-model="currentVal"
                 size="small"
                 :style="{width:controlStyle.width}"
                 v-bind="bindProps"
                 v-on="bindEvents"
                 clearable></el-cascader>
  </div>

</template>

<script>
import { pickerOptionsRange, pickerOptionsRangeMonth } from "../../util/tools"

export default {
  inheritAttrs: false,

  props: {
    currentValMode: {
      type: String,
      required: true,
      default () {
        return ''
      }
    },
    controlStyle: {
      type: Object,
      required: true,
    },
    /*是否要隐藏 条件运算符 */
    hideCommonFilter: {
      type: Object,
      required: true,
    },
    value: {},
    valueMode: '',
    itemOptions: {
      type: Object,
      default () {
        return {}
      }
    }
  },

  data () {
    return {
      currentValMode2: this.currentValMode,
      pickerOptionsRange: pickerOptionsRange,
      pickerOptionsRangeMonth: pickerOptionsRangeMonth,
      commonFitler: '',
      commonFitlerOptions: this.getValues()
    }
  },
  watch: {
    //监控数据变量currentValMode2的变化
    currentValMode2 () {
      this.$emit('currentValModeInput', this.currentValMode2)
    }
  },
  computed: {
    // 双向绑定数据值
    currentVal: {
      get () {
        return this.value
      },
      set (val) {
        this.$emit('input', val) //通知父组件改变值
      }
    },
    // 绑定属性
    bindProps () {
      let obj = { ...this.itemOptions }
      // 移除冗余属性
      delete obj.label
      delete obj.prop
      delete obj.element
      delete obj.initValue
      delete obj.rules
      delete obj.events
      if (obj.element === 'el-select') {
        delete obj.options
      }
      return obj
    },
    // 绑定方法
    bindEvents () {
      return this.itemOptions.events || {}
    },
    // el-input
    isInput () {
      return this.itemOptions.element === 'el-input'
    },
    // 是否显示通用的过滤模式
    isShowCommonFitler () {
      if (this.itemOptions.element === 'el-date-picker') {
        return false;
      }
      if (this.hideCommonFilter == undefined || this.hideCommonFilter == "undefined") {
        return true;
      }
      return false;
    },
    // el-input-number
    isInputNumber () {
      return this.itemOptions.element === 'el-input-number'
    },
    // el-select
    isSelect () {
      return this.itemOptions.element === 'el-select'
    },
    // el-date-picker (type: datetimerange/daterange)
    isDatePickerDateRange () {
      const isDatePicker = this.itemOptions.element === 'el-date-picker'
      const isDateRange = !this.itemOptions.type ||
        this.itemOptions.type === 'datetimerange' ||
        this.itemOptions.type === 'daterange'
      return isDatePicker && isDateRange
    },
    // el-date-picker (type: monthrange)
    isDatePickerMonthRange () {
      const isDatePicker = this.itemOptions.element === 'el-date-picker'
      const isMonthRange = this.itemOptions.type === 'monthrange'
      return isDatePicker && isMonthRange
    },
    //  el-date-picker (type: other)
    isDatePickerOthers () {
      const isDatePicker = this.itemOptions.element === 'el-date-picker'
      return isDatePicker && !this.isDatePickerDateRange && !this.isDatePickerMonthRange
    },
    // el-cascader
    isCascader () {
      return this.itemOptions.element === 'el-cascader'
    }
  },

  created () { },

  methods: {
    //重置 通用筛选框
    resetCommonFitler () {
      this.currentValMode2 = ''
    },
    getValues () {
      if (this.itemOptions.element === 'el-input') {
        return [
          {
            value: '1',
            label: '模糊'
          },
          /*
          {
            value: '3',
            label: '空值'
          },
          //*/
          {
            value: '2',
            label: '等于'
          },
          {
            value: '8',
            label: '不等于'
          }
        ]
      }
      else if (
        this.itemOptions.element === 'el-select') {
        return [
          /*
          {
            value: '1',
            label: '模糊'
          },
          {
            value: '3',
            label: '空值'
          },
          //*/
          {
            value: '2',
            label: '等于'
          },
          {
            value: '8',
            label: '不等于'
          }
        ]
      }
      return [
        {
          value: '1',
          label: '模糊'
        },
        /*
        {
          value: '3',
          label: '空值'
        },
        //*/
        {
          value: '2',
          label: '等于'
        },
        {
          value: '4',
          label: '大于等于'
        },
        {
          value: '5',
          label: '小于等于'
        },
        {
          value: '6',
          label: '大于'
        },

        {
          value: '7',
          label: '小于'
        },
        {
          value: '8',
          label: '不等于'
        }
      ]

    }
  },

  components: {}
}
</script>

<style lang='less' scoped>
</style>

