using SqlSugar;
using System;

namespace LES.Core.Model
{
    /// <summary>
    /// 业务通用的父类
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    public class GeneralBusinessRoot<Tkey> : RootEntityTkey<Tkey> where Tkey : IEquatable<Tkey>
    {
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

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否逻辑删除")]
        public bool? IsDeleted { get; set; }

        #endregion

    }
}
