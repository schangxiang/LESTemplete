using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LES.Core.Extensions.Others
{
    public class WhereHelper<T>

    //where T : class

    {

        public ParameterExpression param;

        public BinaryExpression filter;

        private MemberExpression common_left;

        private UnaryExpression common_right;

        public WhereHelper()

        {

            param = Expression.Parameter(typeof(T), "c");//构建 c=> 结构

            //1==1
            Expression left = Expression.Constant(1);
            filter = Expression.Equal(left, left);//构建出 c=> 1=1

        }

        public Expression<Func<T, bool>> GetExpression()

        {

            return Expression.Lambda<Func<T, bool>>(filter, param);

        }

        private void CommonLeftRight(string propertyName, object value)
        {

            common_left = Expression.Property(param, typeof(T).GetProperty(propertyName));//构建构建c.{propertyName}的结构

            var member = Expression.Property(param, propertyName);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType); // 1

            if (!converter.CanConvertFrom(typeof(string))) // 2
                throw new NotSupportedException();

            var propertyValue = converter.ConvertFromInvariantString(value.ToString()); // 3
            var constant = Expression.Constant(propertyValue);
            common_right = Expression.Convert(constant, propertyType); // 4
            
             // common_right = Expression.Constant(value, value.GetType());//构建一个常量,值是  value
        }

        /// <summary>
        /// 等于 =
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void Equal(string propertyName, object value)
        {
            this.CommonLeftRight(propertyName, value);

            Expression result = Expression.Equal(common_left, common_right);

            filter = Expression.And(filter, result);
        }

        /// <summary>
        /// 不等于 <>
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void NotEqual(string propertyName, object value)
        {

            this.CommonLeftRight(propertyName, value);

            Expression result = Expression.NotEqual(common_left, common_right);

            filter = Expression.And(filter, result);

        }

        /// <summary>
        /// 大于 >
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void GreaterThan(string propertyName, object value)
        {

            this.CommonLeftRight(propertyName, value);

            Expression result = Expression.GreaterThan(common_left, common_right);

            filter = Expression.And(filter, result);

        }

        /// <summary>
        /// 大于等于 >=
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void GreaterThanOrEqual(string propertyName, object value)
        {

            this.CommonLeftRight(propertyName, value);

            Expression result = Expression.GreaterThanOrEqual(common_left, common_right);

            filter = Expression.And(filter, result);

        }

        /// <summary>
        /// 小于 <
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void LessThan(string propertyName, object value)
        {

            this.CommonLeftRight(propertyName, value);

            Expression result = Expression.LessThan(common_left, common_right);

            filter = Expression.And(filter, result);

        }

        /// <summary>
        /// 小于等于 <=
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void LessThanOrEqual(string propertyName, object value)
        {

            this.CommonLeftRight(propertyName, value);

            Expression result = Expression.LessThanOrEqual(common_left, common_right);

            filter = Expression.And(filter, result);

        }

        /// <summary>
        /// 包含 LIKE
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void Contains(string propertyName, string value)

        {

            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));

            Expression right = Expression.Constant(value, value.GetType());

            Expression result = Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), right);

            filter = Expression.And(filter, result);

        }



    }
}
