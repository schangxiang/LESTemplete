using System;
using System.Collections.Generic;
using System.Reflection;

namespace LES.Core.Common.Helper
{
    /// <summary>
    /// 类帮助
    /// </summary>
    public class ClassHelper
    {
        /// <summary>
        /// 获取属性对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="proName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertyInfoList<T>(T t)
        {
            var pros = typeof(T).GetProperties();
            return pros;
        }

        /// <summary>
        /// 获取属性对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="proName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(T t, string proName, out string errMsg)
        {
            errMsg = "";
            var pro = typeof(T).GetProperty(proName);
            if (pro == null)
            {
                errMsg = "属性名'" + proName + "'不存在类'" + typeof(T).Name + "'中";
                return null;
            }
            return pro;
        }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="proName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static string GetPropertyValue<T>(T t, string proName, out string errMsg)
        {
            var pro = GetPropertyInfo<T>(t, proName, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return string.Empty;
            }
            var pro_value = pro.GetValue(t, null);
            var str = pro_value == null ? string.Empty : Convert.ToString(pro_value);
            return str;
        }


        /// <summary>
        /// 通过属性对象获取属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static string GetPropertyValueByObject<T>(T t, PropertyInfo pro)
        {
            var pro_value = pro.GetValue(t, null);
            var str = pro_value == null ? string.Empty : Convert.ToString(pro_value);
            return str;
        }

        /// <summary>
        /// 通过属性对象获取属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(T t, PropertyInfo pro)
        {
            var pro_value = pro.GetValue(t, null);
            return pro_value;
        }

        /// <summary>
        /// 通过特性名获取特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static object[] GetDIYAttributesByPro(PropertyInfo pro)
        {
            object[] Attributes = pro.GetCustomAttributes(typeof(HighSearchRangeAttribute), false);
            return Attributes;
        }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="proName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<string> GetPropertyValueForList<T>(T t, string proName, out string errMsg)
        {
            var pro = GetPropertyInfo<T>(t, proName, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return null;
            }
            var pro_value = pro.GetValue(t, null);
            var list = pro_value == null ? null : (List<string>)pro_value;
            return list;
        }
    }
}
