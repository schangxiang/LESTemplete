using System.Collections.Generic;

namespace LES.Core.Model.ParamModels
{
    public class PageParam
    {
        /// <summary>
        /// 第几页
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页显示多少条
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 关键字模糊查询
        /// </summary>
        public string searchVal { get; set; }

        /// <summary>
        /// 通用查询配置 (1:模糊查询 , 2:精准查询)
        /// </summary>
        public int searchVal_FilterMode { get; set; }

        /// <summary>
        /// 子搜索组件传递的通用查询配置的属性名
        /// </summary>
        public List<string> searchFormInputAttrs { get; set; }
    }
}
