using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LES.Core.Model.Enums
{
    /// <summary>
    /// 通用查询配置模式
    /// </summary>
    public enum SearchFilterModeEnum
    {
        模糊查询 = 1,
        精准查询 = 2,
        空值 = 3,
        大于等于 = 4,
        小于等于 = 5,
        大于 = 6,
        小于 = 7,
        不等于 = 8
    }
}
