using LES.Core.Common; 
using System.Collections.Generic; 
 
namespace LES.Core.Model.ParamModels 
{ 
    /// <summary> 
    /// 查询工件数据类 
    /// </summary> 
    public class Wip_WorkPieceParam : PageParam 
    { 
                /// <summary>
        /// 工件码
        /// </summary>
        public string WorkPieceNo { get; set; }

        /// <summary>
        /// 工件码-查询过滤模式 
        /// </summary>
        public string WorkPieceNo_FilterMode { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工单号-查询过滤模式 
        /// </summary>
        public string WorkOrderNo_FilterMode { get; set; }

        /// <summary>
        /// 工件名称
        /// </summary>
        public string WorkPieceName { get; set; }

        /// <summary>
        /// 工件名称-查询过滤模式 
        /// </summary>
        public string WorkPieceName_FilterMode { get; set; }

        /// <summary>
        /// 一次浇筑
        /// </summary>
        public string FirstPlacing { get; set; }

        /// <summary>
        /// 一次浇筑-查询过滤模式 
        /// </summary>
        public string FirstPlacing_FilterMode { get; set; }

        /// <summary>
        /// 二次浇筑
        /// </summary>
        public string SecondPlacing { get; set; }

        /// <summary>
        /// 二次浇筑-查询过滤模式 
        /// </summary>
        public string SecondPlacing_FilterMode { get; set; }

        /// <summary>
        /// 固化
        /// </summary>
        public string Solidify { get; set; }

        /// <summary>
        /// 固化-查询过滤模式 
        /// </summary>
        public string Solidify_FilterMode { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> AuditTime { get; set; }


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
