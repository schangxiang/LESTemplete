using System;
using SqlSugar;

//注意：要想DBFirst，命名空间必须是 LES.Core.Model.Models
namespace LES.Core.Model.Models
{
    /// <summary>
    /// 出库记录
    /// </summary>
    public class Les_OutStockRecord : RootEntityTkey<int>
    {

        /// <summary>
        /// 叫料单号
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "叫料单号")]
        public string CallMaterialCode { get; set; }

        /// <summary>
        /// 托盘号
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "托盘号")]
        public string SalverCode { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "工单号")]
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "SKU")]
        public string SKU { get; set; }

        /// <summary>
        /// 货物编号
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "货物编号")]
        public string CargoCode { get; set; }

        /// <summary>
        /// 货物名称
        /// </summary>
        [SugarColumn(Length = 500, ColumnDescription = "货物名称")]
        public string CargoName { get; set; }

        /// <summary>
        /// 货物数量
        /// </summary>
        [SugarColumn(ColumnDescription = "货物数量")]
        public int CargoNum { get; set; }


        /// <summary>
        /// 出库时间
        /// </summary>
        [SugarColumn(ColumnDescription = "出库时间")]
        public DateTime OutStockDate { get; set; }


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
        [SugarColumn(IsNullable = true, ColumnDescription = "修改ID")]
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
