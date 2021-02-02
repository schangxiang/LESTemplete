using LES.Core.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace LES.Core.Extensions.Others
{
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }

    public static class PredicateExtensions
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)  
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first  
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression   
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }


        /// <summary>
        /// 拼接成 c.Name.contains("1111")||c.Name.Contains("2222")||c.Name.Contains("3333")) 形式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetConditionExpression<T>(string[] options, string fieldName)
        {
            ParameterExpression left = Expression.Parameter(typeof(T), "c");//c=>
            Expression expression = Expression.Constant(false);
            foreach (var optionName in options)
            {
                Expression right = Expression.Call
                       (
                          Expression.Property(left, typeof(T).GetProperty(fieldName)),  //c.DataSourceName
                          typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),// 反射使用.Contains()方法                         
                          Expression.Constant(optionName)           // .Contains(optionName)
                       );
                expression = Expression.Or(right, expression);//c.DataSourceName.contain("") || c.DataSourceName.contain("") 
            }
            Expression<Func<T, bool>> finalExpression
                = Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[] { left });
            return finalExpression;
        }

        /// <summary>
        /// 拼接成 c.Name.contains("1111")||c.Code.Contains("1111")||c.Address.Contains("1111")) 形式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetConditionExpressionForMoreField<T>(string[] fieldNames, string fieldValue)
        {
            ParameterExpression left = Expression.Parameter(typeof(T), "c");//c=>
            Expression expression = Expression.Constant(false);
            foreach (var fieldName in fieldNames)
            {
                Expression right = Expression.Call
                       (
                          Expression.Property(left, typeof(T).GetProperty(fieldName)),  //c.DataSourceName
                          typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),// 反射使用.Contains()方法                         
                          Expression.Constant(fieldValue)           // .Contains(fieldValue)
                       );
                expression = Expression.Or(right, expression);//c.AAA.contain("") || c.BBB.contain("") 
            }
            Expression<Func<T, bool>> finalExpression
                = Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[] { left });
            return finalExpression;
        }

        /// <summary>
        /// 拼接成 c.Name.equals("1111")||c.Code.equals("1111")||c.Address.equals("1111")) 形式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetConditionExpressionForMoreFieldByAnd<T>(string[] fieldNames, string fieldValue)
        {
            ParameterExpression left = Expression.Parameter(typeof(T), "c");//c=>
            Expression expression = Expression.Constant(false);
            foreach (var fieldName in fieldNames)
            {
                Expression right = Expression.Call
                       (
                          Expression.Property(left, typeof(T).GetProperty(fieldName)),  //c.DataSourceName
                          typeof(string).GetMethod("Equals", new Type[] { typeof(string) }),// 反射使用.Contains()方法                         
                          Expression.Constant(fieldValue)           // .Contains(fieldValue)
                       );
                expression = Expression.Or(right, expression);//c.AAA.contain("") || c.BBB.contain("") 
            }
            Expression<Func<T, bool>> finalExpression
                = Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[] { left });
            return finalExpression;
        }


        public static Expression<Func<T, bool>> GetConditionExpressionForHighFieldByAnd<T>(List<HighSearchModel> hsmList)
        {
            var whereHelper = new WhereHelper<T>();
            foreach (var field in hsmList)
            {
                switch (field.filterMode)
                {
                    case Model.Enums.SearchFilterModeEnum.不等于:
                        whereHelper.NotEqual(field.fieldName, field.fieldValue);
                        break;
                    case Model.Enums.SearchFilterModeEnum.大于:
                        whereHelper.GreaterThan(field.fieldName, field.fieldValue);
                        break;
                    case Model.Enums.SearchFilterModeEnum.大于等于:
                        whereHelper.GreaterThanOrEqual(field.fieldName, field.fieldValue);
                        break;
                    case Model.Enums.SearchFilterModeEnum.小于:
                        whereHelper.LessThan(field.fieldName, field.fieldValue);
                        break;
                    case Model.Enums.SearchFilterModeEnum.小于等于:
                        whereHelper.LessThanOrEqual(field.fieldName, field.fieldValue);
                        break;
                    case Model.Enums.SearchFilterModeEnum.模糊查询:
                        whereHelper.Contains(field.fieldName, field.fieldValue);
                        break;
                    case Model.Enums.SearchFilterModeEnum.精准查询:
                        whereHelper.Equal(field.fieldName, field.fieldValue);
                        break;
                }
            }

            Expression<Func<T, bool>> finalExpression
                = Expression.Lambda<Func<T, bool>>(whereHelper.filter, new ParameterExpression[] { whereHelper.param });
            return finalExpression;
        }

        public static Expression<Func<T, bool>> GetConditionExpressionForHighFieldByAnd<T>(List<HighSearchForDateTimeRangeModel> hsmForDatetimeList)
        {
            var whereHelper = new WhereHelper<T>();
            foreach (var field in hsmForDatetimeList)
            {
                whereHelper.GreaterThanOrEqual(field.fieldName, field.start_fieldValue);
                whereHelper.LessThanOrEqual(field.fieldName, field.end_fieldValue);
            }
            Expression<Func<T, bool>> finalExpression
                = Expression.Lambda<Func<T, bool>>(whereHelper.filter, new ParameterExpression[] { whereHelper.param });
            return finalExpression;
        }
    }
}
