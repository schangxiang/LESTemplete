//是否存在某个按钮
function isExistButton (flag, buttonList) {
  let isShow = false;
  var flag_toLowerCase = flag.toLowerCase()
  buttonList.forEach(x => {
    if (x.Func != null && x.Func.toLowerCase().indexOf(flag_toLowerCase) != -1) {
      isShow = true;
    }
  });
  return isShow;
}
//是否显示某个操作按钮
export function isShowOperatorButtonCommon (isShowOperatorColumn, flag, buttonList) {
  if (!isShowOperatorColumn) return false;
  if (buttonList == null || buttonList.length == 0)
    return false;
  return isExistButton(flag, buttonList);
}
//是否需要显示操作列
export function isNeedShowOperatorColumn (buttonList) {
  if (buttonList == null || buttonList.length == 0)
    return false;
  let isShow = false;

  var flag = 'edit';
  isShow = isExistButton(flag, buttonList);
  if (isShow) return isShow;

  flag = 'del';
  isShow = isExistButton(flag, buttonList);

  return isShow;
}
/*
 验证是不是手机屏幕
 */
export function isMobile () {
  let flag = navigator.userAgent.match(/(phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone)/i)
  return flag;
}