using LES.Core.Common;
using System.Collections.Generic;

namespace LES.Core.Model.ParamModels
{
    /// <summary> 
    /// 查询模块管理类 
    /// </summary> 
    public class ModulesParam : PageParam
    {
        /// <summary>
        /// 
        /// </summary>
        public string IsDeleted { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string IsDeleted_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string Name_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string LinkUrl_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string Area_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string Controller_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string Action_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string Icon_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string Code_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int OrderSort { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string OrderSort_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string Description_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IsMenu { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string IsMenu_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string Enabled_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CreateId { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string CreateId_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string CreateBy_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> CreateTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int ModifyId { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string ModifyId_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModifyBy { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string ModifyBy_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> ModifyTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string ParentId_FilterMode { get; set; }


    }
}
