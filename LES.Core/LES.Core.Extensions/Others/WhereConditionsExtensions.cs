using LES.Core.Common.Helper;
using LES.Core.Model.CommonModel;
using LES.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace LES.Core.Extensions.Others
{
    /// <summary>
    /// 生成Where条件扩展
    /// </summary>
    public class WhereConditionsExtensions
    {
        public static FunReturnResultModel<Expression<Func<T, bool>>> GetWhereConditions<T, TParam>(TParam param)
        {
            FunReturnResultModel<Expression<Func<T, bool>>> result = new FunReturnResultModel<Expression<Func<T, bool>>>();
            try
            {
                Expression<Func<T, bool>> conditions = PredicateExtensions.True<T>();

                var errMsg = "";

                #region 1、通用查询searchVal

                //判断通用查询searchVal
                var pro_value_searchVal = ClassHelper.GetPropertyValue<TParam>(param, SystemCommonVar.searchVal, out errMsg);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    result.IsSuccess = false;
                    result.ErrMsg = errMsg;
                    return result;
                }
                if (!string.IsNullOrEmpty(pro_value_searchVal))
                {//表示 通用查询searchVal不为空，需要查询
                    var pro_value_searchValMode = ClassHelper.GetPropertyValue<TParam>(param, SystemCommonVar.searchVal_FilterMode, out errMsg);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        result.IsSuccess = false;
                        result.ErrMsg = errMsg;
                        return result;
                    }

                    var pro_value_searchFormInputAttrs = ClassHelper.GetPropertyValueForList<TParam>(param, SystemCommonVar.searchFormInputAttrs, out errMsg);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        result.IsSuccess = false;
                        result.ErrMsg = errMsg;
                        return result;
                    }
                    if (pro_value_searchFormInputAttrs != null && pro_value_searchFormInputAttrs.Count > 0)
                    {
                        var i_pro_value_searchValMode = Convert.ToInt32(pro_value_searchValMode);
                        if (i_pro_value_searchValMode == Convert.ToInt32(SearchFilterModeEnum.模糊查询))
                        {//模糊查询
                            var new_conditions = PredicateExtensions.GetConditionExpressionForMoreField<T>(pro_value_searchFormInputAttrs.ToArray(), pro_value_searchVal);
                            conditions = conditions.And(new_conditions);
                        }
                        else if (i_pro_value_searchValMode == Convert.ToInt32(SearchFilterModeEnum.精准查询))
                        {//精准查询
                            var new_conditions = PredicateExtensions.GetConditionExpressionForMoreFieldByAnd<T>(pro_value_searchFormInputAttrs.ToArray(), pro_value_searchVal);
                            conditions = conditions.And(new_conditions);
                            /*
                          pro_value_searchFormInputAttrs.ForEach(x =>
                          {
                            if (!string.IsNullOrEmpty(x))
                            {
                                var myParam = Expression.Parameter(typeof(T));
                                var condition =
Expression.Lambda<Func<T, bool>>(
    Expression.Equal(
        Expression.Property(myParam, x),
        Expression.Constant(pro_value_searchVal, typeof(string))
    ),
    myParam
); // for LINQ to SQl/Entities skip Compile() call
                                conditions = conditions.And(condition);
                                
                        }
                           
                        });
                             //*/
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.ErrMsg = "通用查询配置不正确";
                            return result;
                        }
                    }
                }

                #endregion

                #region 2、高级查询 

                var high_pros = FilterHigh_pros<TParam>(param);
                List<HighSearchForDateTimeRangeModel> hsmForDatetimeList = new List<HighSearchForDateTimeRangeModel>();
                List<HighSearchModel> hsmList = GetHighSearchModelList<TParam>(param, high_pros, ref hsmForDatetimeList);
                if (hsmList.Count > 0)
                {
                    var high_conditions = PredicateExtensions.GetConditionExpressionForHighFieldByAnd<T>(hsmList);
                    conditions = conditions.And(high_conditions);
                }
                if (hsmForDatetimeList.Count > 0)
                {
                    var high_conditions = PredicateExtensions.GetConditionExpressionForHighFieldByAnd<T>(hsmForDatetimeList);
                    conditions = conditions.And(high_conditions);
                }
                #endregion


                result.data = conditions;
                result.IsSuccess = true;
                return result;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 过滤掉Mode后缀的属性 和一些特殊属性的，如DateTimeRange
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="param"></param>
        /// <param name="high_pros"></param>
        /// <returns></returns>
        private static List<HighSearchModel> GetHighSearchModelList<TParam>(TParam param, List<PropertyInfo> high_pros, ref List<HighSearchForDateTimeRangeModel> hsmForDatetimeList)
        {
            List<HighSearchModel> hsmList = new List<HighSearchModel>();
            string errMsg = "";
            SearchFilterModeEnum _svmEnum = default(SearchFilterModeEnum);
            var hsmValue = "";
            var pro_value = "";
            foreach (var pro in high_pros)
            {
                if (!pro.Name.Contains(SystemCommonVar.highSearchModeSuffix))
                {//不是Mode后缀的字段
                 //判断是不是有特性特性的值
                    var Attributes = ClassHelper.GetDIYAttributesByPro(pro);
                    if (Attributes.Length > 0)
                    {
                        object v = ClassHelper.GetPropertyValue<TParam>(param, pro);
                        if (v == null)
                        {
                            continue;
                        }
                        List<string> arr_value = v as List<string>;
                        if (arr_value.Count < 2)
                        {
                            continue;
                        }
                        //string[] arr_value = (string[])v;
                        //string[] arr_value = Convert.ToString(v).Split(',');
                        hsmForDatetimeList.Add(new HighSearchForDateTimeRangeModel()
                        {
                            fieldName = pro.Name,
                            start_fieldValue = arr_value[0],
                            end_fieldValue = arr_value[1]
                        });
                        continue;
                    }

                    pro_value = ClassHelper.GetPropertyValueByObject<TParam>(param, pro);
                    if (string.IsNullOrEmpty(pro_value))
                    {
                        continue;
                    }


                    //查找是相应Mode后缀的字段的值
                    hsmValue = ClassHelper.GetPropertyValue<TParam>(param, pro.Name + SystemCommonVar.highSearchModeSuffix, out errMsg);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        throw new Exception(errMsg);
                    }
                    if (hsmValue == "0" || string.IsNullOrEmpty(hsmValue))
                    {
                        continue;
                    }
                    _svmEnum = (SearchFilterModeEnum)Enum.Parse(typeof(SearchFilterModeEnum), hsmValue);
                    if (_svmEnum == SearchFilterModeEnum.空值)
                    {
                        continue;
                    }
                    hsmList.Add(new HighSearchModel()
                    {
                        fieldName = pro.Name,
                        fieldValue = pro_value,
                        filterMode = _svmEnum
                    });
                }
            }
            return hsmList;
        }


        /// <summary>
        /// 过滤一些不需要的字段,如Page、PageSize等
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        private static List<PropertyInfo> FilterHigh_pros<TParam>(TParam param)
        {
            var all_high_pros = ClassHelper.GetPropertyInfoList<TParam>(param);
            var high_pros = new List<PropertyInfo>();
            //排除不用的
            for (int i = 0; i < all_high_pros.Length; i++)
            {
                if (!SystemCommonVar.commnParamFatherPros.Contains(all_high_pros[i].Name))
                {
                    high_pros.Add(all_high_pros[i]);
                }
            }
            return high_pros;
        }
    }
}
