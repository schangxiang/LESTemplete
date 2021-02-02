using LES.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LES.Core.Model.CommonModel
{
    /// <summary>
    /// 高级查询实体
    /// </summary>
    public class HighSearchModel
    {
        public string fieldName { get; set; }

        public string fieldValue { get; set; }

        /// <summary>
        /// 筛选模式
        /// </summary>
        public SearchFilterModeEnum filterMode { get; set; }
    }

    /// <summary>
    /// 高级查询实体
    /// </summary>
    public class HighSearchForDateTimeRangeModel
    {
        public string fieldName { get; set; }

        public string start_fieldValue { get; set; }

        public string end_fieldValue { get; set; }

    }
}
