
using SqlSugar;
using System;

//注意：要想DBFirst，命名空间必须是 LES.Core.Model.Models
namespace LES.Core.Model.Models
{
    /// <summary>
    /// 代码项表实体类
    /// </summary>
    public class Base_CodeItems : RootEntityTkey<int>
    {

        /// <summary>
        /// 代码项编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 代码项名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 代码编码ID
        /// </summary>
        public int setCode { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string note { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? delFlag { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? createTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string lastModifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? lastModifyTime { get; set; }

    }
}