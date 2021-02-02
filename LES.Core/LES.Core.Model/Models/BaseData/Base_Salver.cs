using SqlSugar;
using System;

//注意：要想DBFirst，命名空间必须是 LES.Core.Model.Models
namespace LES.Core.Model.Models
{
    /// <summary>
    /// 托盘实体
    /// </summary>
    [SugarTable(tableName: "Base_Salver", tableDescription: "托盘管理表")]
    public class Base_Salver : RootEntityTkey<int>
    {
        /// <summary>
        /// 托盘编号
        /// </summary>
        [SugarColumn(ColumnDescription = "托盘编号")]
        public string SalverCode { get; set; }

        /// <summary>
        /// 托盘类型
        /// </summary>
        [SugarColumn(ColumnDescription = "托盘类型")]
        public string SalverType { get; set; }

        /// <summary>
        /// 所属产线
        /// </summary>
        [SugarColumn(ColumnDescription = "所属产线")]
        public string AllowLine { get; set; }

        #region 创建者等

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
