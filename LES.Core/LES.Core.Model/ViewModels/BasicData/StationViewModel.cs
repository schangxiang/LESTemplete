using System;

namespace LES.Core.Model.ViewModels.BasicData
{
    public class StationViewModel 
    {

        /// <summary>
        /// 站点编号
        /// </summary>
        public string StationCode { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 所属产线
        /// </summary>
        public int AllowLine { get; set; }

        public int AllowLineMode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public string RemarkMode { get; set; }

        /// <summary>
        /// 所属产线名称
        /// </summary>
        public string AllowLineName { get; set; }

        public string AllowLineNameMode { get; set; }

        /// <summary>
        /// 创建ID
        /// </summary>
        public int? CreateId { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改ID
        /// </summary>
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; } = DateTime.Now;
    }
}
