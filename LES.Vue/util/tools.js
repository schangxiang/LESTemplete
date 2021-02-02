/**
 * 创建唯一的字符串
 * @return {string} ojgdvbvaua40
 */
export function createUUID () {
  const timestamp = +new Date() + ''
  const randomNum = parseInt((1 + Math.random()) * 65536) + ''
  return (+(randomNum + timestamp)).toString(32)
}
/**
 * 格式化字符串
 * @param {*} date 
 * @param {*} fmt 
 */
export function formatDate (date, fmt) {
  if (/(y+)/.test(fmt)) {
    fmt = fmt.replace(
      RegExp.$1,
      (date.getFullYear() + '').substr(4 - RegExp.$1.length)
    )
  }
  let o = {
    'M+': date.getMonth() + 1,
    'd+': date.getDate(),
    'h+': date.getHours(),
    'm+': date.getMinutes(),
    's+': date.getSeconds()
  }
  for (let k in o) {
    if (new RegExp(`(${k})`).test(fmt)) {
      let str = o[k] + ''
      fmt = fmt.replace(
        RegExp.$1,
        RegExp.$1.length === 1 ? str : padLeftZero(str)
      )
    }
  }
  return fmt
}
function padLeftZero (str) {
  return ('00' + str).substr(str.length)
}

// elementui日期时间范围 快捷选项
export const pickerOptionsRange = {
  shortcuts: [
    {
      text: '今天',
      onClick (picker) {
        const end = new Date()
        const start = new Date(new Date().toDateString())
        start.setTime(start.getTime())
        picker.$emit('pick', [start, end])
      }
    }, {
      text: '最近一周',
      onClick (picker) {
        const end = new Date()
        const start = new Date()
        start.setTime(end.getTime() - 3600 * 1000 * 24 * 7)
        picker.$emit('pick', [start, end])
      }
    }, {
      text: '最近一个月',
      onClick (picker) {
        const end = new Date()
        const start = new Date()
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30)
        picker.$emit('pick', [start, end])
      }
    }, {
      text: '最近三个月',
      onClick (picker) {
        const end = new Date()
        const start = new Date()
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90)
        picker.$emit('pick', [start, end])
      }
    }
  ]
}

// elementui月份范围 快捷选项
export const pickerOptionsRangeMonth = {
  shortcuts: [
    {
      text: '今年至今',
      onClick (picker) {
        const end = new Date()
        const start = new Date(new Date().getFullYear(), 0)
        picker.$emit('pick', [start, end])
      }
    },
    {
      text: '最近半年',
      onClick (picker) {
        const end = new Date()
        const start = new Date()
        start.setMonth(start.getMonth() - 6)
        picker.$emit('pick', [start, end])
      }
    },
    {
      text: '最近一年',
      onClick (picker) {
        const end = new Date()
        const start = new Date()
        start.setMonth(start.getMonth() - 12)
        picker.$emit('pick', [start, end])
      }
    }
  ]
}

// 导出文件使用的日期格式
export function getNowFormatDateForExport () {
  var date = new Date()
  var seperator1 = '-'
  var seperator2 = ':'
  var month = date.getMonth() + 1
  var strDate = date.getDate()
  if (month >= 1 && month <= 9) {
    month = '0' + month
  }
  if (strDate >= 0 && strDate <= 9) {
    strDate = '0' + strDate
  }
  var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate +
    ' ' + date.getHours() + seperator2 + date.getMinutes() +
    seperator2 + date.getSeconds()
  return currentdate
}

/**
 * 给formOptions赋值新值，用于子组件的select框下拉绑定
 * @param {*} formOptions 
 * @param {*} propValue 
 * @param {*} newValue 
 */
export function setformOptionsNewValue (formOptions, propValue, newValue) {
  const target = formOptions.filter((item, index) => propValue === item.prop)[0];
  if (target) {
    target['options'] = newValue;
  }
  return formOptions
}