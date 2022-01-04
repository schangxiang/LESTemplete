import validate from './validate.js'

/*
 * 根据物料判断是否需要使用默认的配料任务号
*/
export function validateIsUseDefaultBurdenWorkNo (name) {
  //如果选择的是 风机工序下的某些物料，
  if (name == 'GX_FH_涡流风机WFD-900'
    || name == 'GX_FH_涡流风机WFD-1250'
    || name == 'GX_FH_涡流风机WFD-900+风机支架-A款-Z 9000 0075'
    || name == 'GX_FH_涡流风机WFD-900+风机支架-B款-Z 9000 0080'
  ) {
    return true
  } else {
    return false
  }
}


/*
 * 整体字符串，根据工序去验证输入的配料任务号是否符合
*/
export function validateNewBurdenWorkNoForWhole (materialCodeItemId, allowProcedureName, newBurdenWorkNo, MaterialCodeItemCode, MaterialCodeItemName) {
  var all = false
  //判断是否按照逗号分隔过了
  if (newBurdenWorkNo == '') {
    return "请输入配料任务号"
  }
  let arr = newBurdenWorkNo.split(',')
  let msg = ''
  //验证是否重复的任务号
  msg = ValidateArryRepetition(arr)
  if (msg != '') return msg
  arr = newBurdenWorkNo.split(',') //重新编排
  for (let index = 0; index < arr.length; index++) {
    const element = arr[index]
    debugger
    msg = validateNewBurdenWorkNo(materialCodeItemId, allowProcedureName, element, MaterialCodeItemCode, MaterialCodeItemName)
    if (msg !== '') {
      return arr.length > 1 ? ('第' + (index + 1) + '个任务号:' + msg) : (msg)
    }

  }
  return msg
}

/**
 * 验证数组中元素是否重复
 * @param {} ary 
 * @returns 
 */
function ValidateArryRepetition (ary) {
  var nary = ary.sort();
  for (var i = 0; i < nary.length - 1; i++) {
    if (nary[i] == nary[i + 1]) {
      return ("有重复的任务号：" + nary[i]);
    }
  }
  return ''
}


/*
 * 根据工序去验证输入的配料任务号是否符合
*/
export function validateNewBurdenWorkNo (materialCodeItemId, allowProcedureName, newBurdenWorkNo, MaterialCodeItemCode, MaterialCodeItemName) {
  var all = false
  //首先判断物料是否被选中了
  if (materialCodeItemId == '') {
    return "请先选择物料！"
  }
  if (newBurdenWorkNo == '') {
    return "请输入配料任务号"
  } else {
    var str = ""
    for (var i = 0; i < newBurdenWorkNo.length; i++) {
      str = newBurdenWorkNo.charCodeAt(i)
      if (str > 65280 && str < 65375 || str == 12288) {
        all = true
      }
    }
    if (all == true) {
      return "请使用半角字符输入"
    } else {
      //根据物料类型去验证输入的配料任务号是否符合
      var msg = validateNewBurdenWorkNoIsRight(allowProcedureName, newBurdenWorkNo, MaterialCodeItemCode, MaterialCodeItemName)
      return msg
    }
  }
}


/*
 * 根据工序去验证输入的配料任务号是否符合
*/
export function validateNewBurdenWorkNoIsRight (allowProcedureName, newBurdenWorkNo, MaterialCodeItemCode, MaterialCodeItemName) {
  if (allowProcedureName != undefined) {//按照工序进行校验
    //如果选择的是 装配、风机、侧出线工序
    if (allowProcedureName == 'GX_ZP'
      || allowProcedureName == 'GX_FJ'
      || allowProcedureName == 'GX_CCX'
    ) {
      /*
       * 规则：以“E”开头（仅对应装配与发货工序），长度9个字符
      */
      if (newBurdenWorkNo.substr(0, 1) != 'E') {
        return '装配、风机、侧出线工序的配料任务号，首字母必须是E开头'
      } else if (newBurdenWorkNo.length != 9) {
        return '装配、风机、侧出线工序的配料任务号，长度只能输入9位，现在是' + newBurdenWorkNo.length + '位'
      }
      return ''
    }
    //这里注释掉 【EditBy shaocx,2021-10-11】
    /*
    else if (allowProcedureName == 'GX_GYRX' || allowProcedureName == 'GX_DYRX') {
     if (!validate.isNumber(newBurdenWorkNo)) {
       return '高压绕线、低压绕线工序的配料任务号，只能输入纯数字'
     }
     if (newBurdenWorkNo.length != 5) {
       return '高压绕线、低压绕线工序的配料任务号，长度只能输入5位,现在是' + newBurdenWorkNo.length + '位'
     }
     return ''
    }
    //*/
  }

  if (MaterialCodeItemCode == undefined) {
    return ''
  }
  var c_msg = MaterialCodeItemName + '物料的配料任务号，'
  //根据物料码表编号判断规则
  if (MaterialCodeItemCode == 'GX_GYRX_GYCJY'
    || MaterialCodeItemCode == 'GX_DYRX_DYXQ' //低压浇注线圈
    || MaterialCodeItemCode == 'GX_GYRX_GYRXXQ') {//高压绕线线圈或 高压绝缘
    var arr = newBurdenWorkNo.split('-')
    if (arr.length != 4) {
      return c_msg + '必须用-分隔为四部分'
    }
    if (!validate.isNumber(arr[0])) {
      return c_msg + '第一部分只能输入纯数字'
    }
    if (arr[0].length != 5) {
      return c_msg + '第一部分长度只能输入5位,现在是' + arr[0].length + '位'
    }
    if (arr[1] != 'HV') {
      return c_msg + '第二部分必须是HV'
    }
    if (!validate.isNumber(arr[2])) {
      return c_msg + '第三部分只能输入纯数字'
    }
    if (!validate.isNumber(arr[3])) {
      return c_msg + '第四部分只能输入纯数字'
    }

  } else if (MaterialCodeItemCode == 'GX_GYRX_GYDT') {//高压导体
    var arr = newBurdenWorkNo.split('-')
    if (arr.length != 4 && arr.length != 2) {
      return c_msg + '必须用-分隔为二部分或者四部分'
    }
    if (!validate.isNumber(arr[0])) {
      return c_msg + '第一部分只能输入纯数字'
    }
    if (arr[0].length != 5) {
      return c_msg + '第一部分长度只能输入5位'
    }
    if (arr[1] != 'HV') {
      return c_msg + '第二部分必须是HV'
    }
    if (arr.length == 4) {
      if (!validate.isNumber(arr[2])) {
        return c_msg + '第三部分只能输入纯数字'
      }
      if (!validate.isNumber(arr[3])) {
        return c_msg + '第四部分只能输入纯数字'
      }
    }
  }
  else if (MaterialCodeItemCode == 'GX_DYRX_DYCJY'
    || MaterialCodeItemCode == 'GX_DYJYGX_DYJY'
    || MaterialCodeItemCode == 'GX_GDYXQZY_DYXQ'
    || MaterialCodeItemCode == 'GGWL_DYXQ') {//低压绕线线圈或 低压绝缘
    var arr = newBurdenWorkNo.split('-')
    if (arr.length != 4) {
      return c_msg + '必须用-分隔为四部分'
    }
    if (!validate.isNumber(arr[0])) {
      return c_msg + '第一部分只能输入纯数字'
    }
    if (arr[0].length != 5) {
      return c_msg + '第一部分长度只能输入5位,现在是' + arr[0].length + '位'
    }
    if (arr[1] != 'LV') {
      return c_msg + '第二部分必须是LV'
    }
    if (!validate.isNumber(arr[2])) {
      return c_msg + '第三部分只能输入纯数字'
    }
    if (!validate.isNumber(arr[3])) {
      return c_msg + '第四部分只能输入纯数字'
    }

  } else if (MaterialCodeItemCode == 'GX_DYRX_DYDT'
    || MaterialCodeItemCode == 'GX_DYJYGX_DYDTA' || MaterialCodeItemCode == 'GX_DYJYGX_DYDTB') {//低压导体
    var arr = newBurdenWorkNo.split('-')
    if (arr.length != 4 && arr.length != 2) {
      return c_msg + '必须用-分隔为二部分或者四部分'
    }
    if (!validate.isNumber(arr[0])) {
      return c_msg + '第一部分只能输入纯数字'
    }
    if (arr[0].length != 5) {
      return c_msg + '第一部分长度只能输入5位,现在是' + arr[0].length + '位'
    }
    if (arr[1] != 'LV') {
      return c_msg + '第二部分必须是LV'
    }
    if (arr.length == 4) {
      if (!validate.isNumber(arr[2])) {
        return c_msg + '第三部分只能输入纯数字'
      }
      if (!validate.isNumber(arr[3])) {
        return c_msg + '第四部分只能输入纯数字'
      }
    }
  }

  return '' //最后返回验证通过
}