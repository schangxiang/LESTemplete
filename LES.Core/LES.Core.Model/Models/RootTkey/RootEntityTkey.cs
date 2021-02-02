using SqlSugar;
using System;

namespace LES.Core.Model
{
    public class RootEntityTkey<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        /// ID
        /// 泛型主键Tkey
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, ColumnDescription = "主键")]
        public Tkey Id { get; set; }


    }
}