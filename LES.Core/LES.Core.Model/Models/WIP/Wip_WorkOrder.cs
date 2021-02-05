using SqlSugar;
using System;

namespace LES.Core.Model.Models
{
    /// <summary>
    /// 工单数据
    /// </summary>
    public class Wip_WorkOrder : RootEntityTkey<int>
    {
        /// <summary>
        /// 工单号
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "工单号")]
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 计划单号
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "计划单号")]
        public string PlanNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "SKU")]
        public string SKU { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [SugarColumn(ColumnDescription = "数量")]
        public int Quantity { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "计划时间")]
        public DateTime? PlanTime { get; set; }


        /// <summary>
        /// 生产序号
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "生产序号")]
        public string ProductionIndex { get; set; }

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
