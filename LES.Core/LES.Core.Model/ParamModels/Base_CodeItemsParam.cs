using LES.Core.Common;
using System.Collections.Generic;

namespace LES.Core.Model.ParamModels
{
    /// <summary> 
    /// 查询码表项管理类 
    /// </summary> 
    public class Base_CodeItemsParam : PageParam
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string code_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string name_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? setCode { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string setCode_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string note { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string note_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string delFlag { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string delFlag_FilterMode { get; set; }


    }
}
