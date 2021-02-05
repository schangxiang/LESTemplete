using System;
using SqlSugar;


namespace LES.Core.Model.Models
{
    /// <summary>
    /// RGV异常信息
    /// </summary>
    public class Les_RgvExcepiton : RootEntityTkey<int>
    {

        /// <summary>
        /// 任务号
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "任务号")]
        public string TaskNo { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        [SugarColumn(ColumnDescription = "任务类型")]
        public int TaskType { get; set; }


        /// <summary>
        /// 叫料单号
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "叫料单号")]
        public string CallOrderNo { get; set; }

        /// <summary>
        /// 起始点
        /// </summary>
        [SugarColumn(ColumnDescription = "起始点")]
        public int SourcePlaceNo { get; set; }

        /// <summary>
        /// 起始点名称
        /// </summary>
        [SugarColumn(ColumnDescription = "起始点名称")]
        public int SourcePlaceName { get; set; }

        /// <summary>
        /// 目标点
        /// </summary>
        [SugarColumn(ColumnDescription = "目标点")]
        public int ToPlaceNo { get; set; }

        /// <summary>
        /// 目标点名称
        /// </summary>
        [SugarColumn(ColumnDescription = "目标点名称")]
        public int ToPlaceName { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [SugarColumn(ColumnDescription = "任务状态")]
        public int TaskStatus { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "任务创建时间")]
        public DateTime TaskCreateTime { get; set; }

        /// <summary>
        /// 下发时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "下发时间")]
        public DateTime? TaskIssueTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "完成时间")]
        public DateTime? TaskFinishTime { get; set; }

        /// <summary>
        /// 处理措施
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "处理措施")]
        public int? DoType { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 500, ColumnDescription = "异常信息")]
        public string ExcepitonMsg { get; set; }




        #region 公共字段

        /// <summary>
        /// 操作说明
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 500, ColumnDescription = "操作说明")]
        public string OperationRemark { get; set; }

        /// <summary>
        /// 创建ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建者ID")]
        public int? CreateId { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true, ColumnDescription = "创建者")]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改者ID")]
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true, ColumnDescription = "修改者")]
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? ModifyTime { get; set; } = DateTime.Now;

        #endregion
    }
}
