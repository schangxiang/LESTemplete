using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LES.Core.Model.ParamModels
{
    public class UserInfoParam : PageParam
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string uRealName { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string uLoginName { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleNames { get; set; }


        /// <summary>
        /// 昵称
        /// </summary>
        public string uRealName_FilterMode { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string uLoginName_FilterMode { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleNames_FilterMode { get; set; }
    }
}
