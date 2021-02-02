using LES.Core.Common;
using System.Collections.Generic;

namespace LES.Core.Model.ParamModels
{
    /// <summary> 
    /// 查询站点管理类 
    /// </summary> 
    public class Base_StationParam : PageParam
    {
        /// <summary>
        /// 站点编号
        /// </summary>
        public string StationCode { get; set; }

        /// <summary>
        /// 站点编号-查询过滤模式 
        /// </summary>
        public string StationCode_FilterMode { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 站点名称-查询过滤模式 
        /// </summary>
        public string StationName_FilterMode { get; set; }

        /// <summary>
        /// 所属产线
        /// </summary>
        public int? AllowLine { get; set; }

        /// <summary>
        /// 所属产线-查询过滤模式 
        /// </summary>
        public string AllowLine_FilterMode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备注-查询过滤模式 
        /// </summary>
        public string Remark_FilterMode { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public string IsDeleted { get; set; }

        /// <summary>
        /// 是否删除-查询过滤模式 
        /// </summary>
        public string IsDeleted_FilterMode { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string OperationRemark { get; set; }

        /// <summary>
        /// 操作说明-查询过滤模式 
        /// </summary>
        public string OperationRemark_FilterMode { get; set; }

        /// <summary>
        /// 创建者ID
        /// </summary>
        public int? CreateId { get; set; }

        /// <summary>
        /// 创建者ID-查询过滤模式 
        /// </summary>
        public string CreateId_FilterMode { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建者-查询过滤模式 
        /// </summary>
        public string CreateBy_FilterMode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> CreateTime { get; set; }


        /// <summary>
        /// 修改ID
        /// </summary>
        public int? ModifyId { get; set; }

        /// <summary>
        /// 修改ID-查询过滤模式 
        /// </summary>
        public string ModifyId_FilterMode { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string ModifyBy { get; set; }

        /// <summary>
        /// 修改者-查询过滤模式 
        /// </summary>
        public string ModifyBy_FilterMode { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        //表示是 高级查询范围查询特性
        [HighSearchRangeAttribute]
        public List<string> ModifyTime { get; set; }



    }
}
