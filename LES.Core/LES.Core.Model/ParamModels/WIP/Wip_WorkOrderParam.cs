using LES.Core.Common; 
using System.Collections.Generic; 
 
namespace LES.Core.Model.ParamModels 
{ 
    /// <summary> 
    /// 查询工单管理类 
    /// </summary> 
    public class Wip_WorkOrderParam : PageParam 
    { 
                /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工单号-查询过滤模式 
        /// </summary>
        public string WorkOrderNo_FilterMode { get; set; }

        /// <summary>
        /// 计划单号
        /// </summary>
        public string PlanNo { get; set; }

        /// <summary>
        /// 计划单号-查询过滤模式 
        /// </summary>
        public string PlanNo_FilterMode { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// SKU-查询过滤模式 
        /// </summary>
        public string SKU_FilterMode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// 数量-查询过滤模式 
        /// </summary>
        public string Quantity_FilterMode { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> PlanTime { get; set; }


        /// <summary>
        /// 生产序号
        /// </summary>
        public string ProductionIndex { get; set; }

        /// <summary>
        /// 生产序号-查询过滤模式 
        /// </summary>
        public string ProductionIndex_FilterMode { get; set; }

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
        /// 修改者ID
        /// </summary>
        public int? ModifyId { get; set; }

        /// <summary>
        /// 修改者ID-查询过滤模式 
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
