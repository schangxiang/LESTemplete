using LES.Core.Common;
using System.Collections.Generic;

namespace LES.Core.Model.ParamModels
{
    /// <summary> 
    /// 查询托盘管理类 
    /// </summary> 
    public class Base_SalverParam : PageParam
    {
        /// <summary>
        /// 托盘编号
        /// </summary>
        public string SalverCode { get; set; }

        /// <summary>
        /// 托盘编号-查询过滤模式 
        /// </summary>
        public string SalverCode_FilterMode { get; set; }

        /// <summary>
        /// 托盘类型
        /// </summary>
        public string SalverType { get; set; }

        /// <summary>
        /// 托盘类型-查询过滤模式 
        /// </summary>
        public string SalverType_FilterMode { get; set; }

        /// <summary>
        /// 所属产线
        /// </summary>
        public string AllowLine { get; set; }

        /// <summary>
        /// 所属产线-查询过滤模式 
        /// </summary>
        public string AllowLine_FilterMode { get; set; }

        /// <summary>
        /// 创建者ID
        /// </summary>
        public int? CreateId { get; set; }

        /// <summary>
        /// 创建者ID-查询过滤模式 
        /// </summary>
        public string CreateId_FilterMode { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建者-查询过滤模式 
        /// </summary>
        public string CreateBy_FilterMode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> CreateTime { get; set; }


        /// <summary>
        /// 修改ID
        /// </summary>
        public int? ModifyId { get; set; }

        /// <summary>
        /// 修改ID-查询过滤模式 
        /// </summary>
        public string ModifyId_FilterMode { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string ModifyBy { get; set; }

        /// <summary>
        /// 修改者-查询过滤模式 
        /// </summary>
        public string ModifyBy_FilterMode { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> ModifyTime { get; set; }



    }
}
