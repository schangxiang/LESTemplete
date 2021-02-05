using SqlSugar;
using System;

namespace LES.Core.Model.Models
{
    /// <summary>
    /// 工件数据
    /// </summary>
    public class Wip_WorkPiece : GeneralBusinessRoot<int>
    {
        /// <summary>
        /// 工件码
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "工件码")]
        public string WorkPieceNo { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "工单号")]
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工件名称
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "工件名称")]
        public string WorkPieceName { get; set; }

        /// <summary>
        /// 一次浇筑
        /// </summary>
        [SugarColumn(ColumnDescription = "一次浇筑")]
        public bool FirstPlacing { get; set; } = false;

        /// <summary>
        /// 二次浇筑
        /// </summary>
        [SugarColumn(ColumnDescription = "二次浇筑")]
        public bool SecondPlacing { get; set; } = false;

        /// <summary>
        /// 固化
        /// </summary>
        [SugarColumn(ColumnDescription = "固化")]
        public bool Solidify { get; set; } = false;


        /// <summary>
        /// 审核时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "审核时间")]
        public DateTime? AuditTime { get; set; }

    }
}
