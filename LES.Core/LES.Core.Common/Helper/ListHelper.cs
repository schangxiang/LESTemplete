using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LES.Core.Common.Helper
{
    /// <summary>
    /// LIST帮助类 
    /// </summary>
    public class ListHelper
    {
        /// <summary>
        /// LIST对象互转
        /// </summary>
        /// <typeparam name="T1">原对象</typeparam>
        /// <typeparam name="T2">新对象</typeparam>
        /// <param name="param">原对象</param>
        /// <returns></returns>
        public static T2 T1ToT2<T1, T2>(T1 param)
        {
            string json = JsonConvert.SerializeObject(param);
            return JsonConvert.DeserializeObject<T2>(json);
        }

        /// <summary>
        /// LIST打乱顺序
        /// </summary>
        /// <param name="oldList">要处理的LIST</param>
        /// <returns>打乱顺序后的LIST</returns>
        public static List<T> RandomList<T>(List<T> oldList)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            List<T> newList = new List<T>();
            foreach (var item in oldList)
            {
                newList.Insert(r.Next(newList.Count), item);
            }
            return newList;
        }

        /// <summary>
        /// List分页处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="totalPage">总页数</param>
        /// <returns></returns>
        public static List<T> ListPage<T>(List<T> dataList, int page, int pageSize, out int totalPage)
        {
            totalPage = (dataList.Count + pageSize - 1) / pageSize;
            return dataList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// List分页处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList">原始集合</param>
        /// <param name="whereExpression"></param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="totalPage">总页数</param>
        /// <param name="dataCount">总条数</param>
        /// <returns>分页集合</returns>
        public static List<T> ListPage<T>(List<T> dataList, Expression<Func<T, bool>> whereExpression, int page, int pageSize, out int totalPage,out int  dataCount)
        {
            Func<T, bool> func = whereExpression.Compile();//编译成可执行的Linq
            dataList = dataList.Where(func).ToList();//通过过滤条件过滤成新的LIST

            dataCount = dataList.Count;
            totalPage = (dataList.Count + pageSize - 1) / pageSize;
            return dataList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
