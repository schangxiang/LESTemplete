using LES.Core.Common; 
using System.Collections.Generic; 
 
namespace LES.Core.Model.ParamModels 
{ 
    /// <summary> 
    /// 查询出库记录类 
    /// </summary> 
    public class Wip_OutStockRecordParam : PageParam 
    { 
                /// <summary>
        /// 叫料单号
        /// </summary>
        public string CallMaterialCode { get; set; }

        /// <summary>
        /// 叫料单号-查询过滤模式 
        /// </summary>
        public string CallMaterialCode_FilterMode { get; set; }

        /// <summary>
        /// 托盘号
        /// </summary>
        public string SalverCode { get; set; }

        /// <summary>
        /// 托盘号-查询过滤模式 
        /// </summary>
        public string SalverCode_FilterMode { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 工单号-查询过滤模式 
        /// </summary>
        public string WorkOrderCode_FilterMode { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// SKU-查询过滤模式 
        /// </summary>
        public string SKU_FilterMode { get; set; }

        /// <summary>
        /// 货物编号
        /// </summary>
        public string CargoCode { get; set; }

        /// <summary>
        /// 货物编号-查询过滤模式 
        /// </summary>
        public string CargoCode_FilterMode { get; set; }

        /// <summary>
        /// 货物名称
        /// </summary>
        public string CargoName { get; set; }

        /// <summary>
        /// 货物名称-查询过滤模式 
        /// </summary>
        public string CargoName_FilterMode { get; set; }

        /// <summary>
        /// 货物数量
        /// </summary>
        public int? CargoNum { get; set; }

        /// <summary>
        /// 货物数量-查询过滤模式 
        /// </summary>
        public string CargoNum_FilterMode { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> OutStockDate { get; set; }


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
