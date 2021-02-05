using LES.Core.Common; 
using System.Collections.Generic; 
 
namespace LES.Core.Model.ParamModels 
{ 
    /// <summary> 
    /// 查询库存信息类 
    /// </summary> 
    public class Les_StockInfoParam : PageParam 
    { 
                /// <summary>
        /// 
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string SKU_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CargoCode { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string CargoCode_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CargoName { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string CargoName_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? CargoNum { get; set; }

        /// <summary>
        /// -查询过滤模式 
        /// </summary>
        public string CargoNum_FilterMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> ValidityDate { get; set; }


        /// <summary>
        /// 
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> InStockDate { get; set; }


        /// <summary>
        /// 操作说明
        /// </summary>
        public string OperationRemark { get; set; }

        /// <summary>
        /// 操作说明-查询过滤模式 
        /// </summary>
        public string OperationRemark_FilterMode { get; set; }

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
