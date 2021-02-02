using LES.Core.Common;
using System.Collections.Generic;

namespace LES.Core.Model.ParamModels
{
    public class RoleParam : PageParam
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }

        #region 单独处理日期

        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> CreateTime { get; set; }

        #endregion

        #region 查询过滤模式

        /// <summary>
        /// 角色名-查询过滤模式
        /// </summary>
        public string Name_FilterMode { get; set; }
        /// <summary>
        ///描述-查询过滤模式
        /// </summary>
        public string Description_FilterMode { get; set; }

        #endregion
    }
}
