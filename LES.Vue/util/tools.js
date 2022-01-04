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

/**
 * 判断字符串是否是日期类型
 * @param {} data 
 * @returns 
 */
export function IsDateStr (data) {
  if (isNaN(data) && !isNaN(Date.parse(data))) {
    //console.log("data是日期格式！")
    return true
  }
  return false
}

// 导出文件使用的日期格式
export function getNowFormatDateForExport () {
  var date = new Date()
  var seperator1 = '-'
  var seperator2 = '_'
  var month = date.getMonth() + 1
  var strDate = date.getDate()
  if (month >= 1 && month <= 9) {
    month = '0' + month
  }
  if (strDate >= 0 && strDate <= 9) {
    strDate = '0' + strDate
  }
  var currentdate = // date.getFullYear() + seperator1 +
    month + seperator1 + strDate +
    ' ' + date.getHours() + seperator2 + date.getMinutes()
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

/**
 * 检查是否全屏
 */
export function checkFull () {
  //判断浏览器是否处于全屏状态 （需要考虑兼容问题）
  //火狐浏览器
  var isFull = document.mozFullScreen ||
    document.fullScreen ||
    //谷歌浏览器及Webkit内核浏览器
    document.webkitIsFullScreen ||
    document.webkitRequestFullScreen ||
    document.mozRequestFullScreen ||
    document.msFullscreenEnabled
  if (isFull === undefined) {
    isFull = false
  }
  return isFull;
}
/**
 * 设置全屏
 * @param {是否是部分全屏} isPartFull 
 * @param {部分全屏的ID} elementId 
 * @param {是否全屏的标记} fullscreenFlag 
 */
export function setFullScreen (isPartFull, elementId, fullscreenFlag) {
  let element;
  if (isPartFull) {
    element = document.getElementById(elementId);//设置后就是   id==elementId 的容器全屏
  } else {
    element = document.documentElement;//设置后就是我们平时的整个页面全屏效果
  }
  if (fullscreenFlag) {
    if (document.exitFullscreen) {
      document.exitFullscreen();
    } else if (document.webkitCancelFullScreen) {
      document.webkitCancelFullScreen();
    } else if (document.mozCancelFullScreen) {
      document.mozCancelFullScreen();
    } else if (document.msExitFullscreen) {
      document.msExitFullscreen();
    }
  } else {
    if (element.requestFullscreen) {
      element.requestFullscreen();
    } else if (element.webkitRequestFullScreen) {
      element.webkitRequestFullScreen();
    } else if (element.mozRequestFullScreen) {
      element.mozRequestFullScreen();
    } else if (element.msRequestFullscreen) {
      // IE11
      element.msRequestFullscreen();
    }
  }
}