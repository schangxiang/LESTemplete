using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//注意：要想DBFirst，命名空间必须是 LES.Core.Model.Models
namespace LES.Core.Model.Models
{
    /// <summary>
    /// 站点管理
    /// </summary>
    [SugarTable(tableName: "Base_Station", tableDescription: "站点管理表")]
    public class Base_Station : RootEntityTkey<int>
    {
        /// <summary>
        /// 站点编号
        /// </summary>
        [SugarColumn(ColumnDescription = "站点编号")]
        public string StationCode { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        [SugarColumn(ColumnDescription = "站点名称")]
        public string StationName { get; set; }

        /// <summary>
        /// 所属产线
        /// </summary>
        [SugarColumn(ColumnDescription = "所属产线")]
        public int AllowLine { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }



        #region 创建者等公共用的

        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否删除")]
        public bool IsDeleted { get; set; } = false;

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
