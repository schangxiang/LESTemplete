using LES.Core.Common; 
using System.Collections.Generic; 
 
namespace LES.Core.Model.ParamModels 
{ 
    /// <summary> 
    /// 查询RGV异常类 
    /// </summary> 
    public class Les_RgvExcepitonParam : PageParam 
    { 
                /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 任务号-查询过滤模式 
        /// </summary>
        public string TaskNo_FilterMode { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public int? TaskType { get; set; }

        /// <summary>
        /// 任务类型-查询过滤模式 
        /// </summary>
        public string TaskType_FilterMode { get; set; }

        /// <summary>
        /// 叫料单号
        /// </summary>
        public string CallOrderNo { get; set; }

        /// <summary>
        /// 叫料单号-查询过滤模式 
        /// </summary>
        public string CallOrderNo_FilterMode { get; set; }

        /// <summary>
        /// 起始点
        /// </summary>
        public int? SourcePlaceNo { get; set; }

        /// <summary>
        /// 起始点-查询过滤模式 
        /// </summary>
        public string SourcePlaceNo_FilterMode { get; set; }

        /// <summary>
        /// 起始点名称
        /// </summary>
        public int? SourcePlaceName { get; set; }

        /// <summary>
        /// 起始点名称-查询过滤模式 
        /// </summary>
        public string SourcePlaceName_FilterMode { get; set; }

        /// <summary>
        /// 目标点
        /// </summary>
        public int? ToPlaceNo { get; set; }

        /// <summary>
        /// 目标点-查询过滤模式 
        /// </summary>
        public string ToPlaceNo_FilterMode { get; set; }

        /// <summary>
        /// 目标点名称
        /// </summary>
        public int? ToPlaceName { get; set; }

        /// <summary>
        /// 目标点名称-查询过滤模式 
        /// </summary>
        public string ToPlaceName_FilterMode { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public int? TaskStatus { get; set; }

        /// <summary>
        /// 任务状态-查询过滤模式 
        /// </summary>
        public string TaskStatus_FilterMode { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> TaskCreateTime { get; set; }


        /// <summary>
        /// 下发时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> TaskIssueTime { get; set; }


        /// <summary>
        /// 完成时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> TaskFinishTime { get; set; }


        /// <summary>
        /// 处理措施
        /// </summary>
        public int? DoType { get; set; }

        /// <summary>
        /// 处理措施-查询过滤模式 
        /// </summary>
        public string DoType_FilterMode { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExcepitonMsg { get; set; }

        /// <summary>
        /// 异常信息-查询过滤模式 
        /// </summary>
        public string ExcepitonMsg_FilterMode { get; set; }

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
