namespace LES.Core.Model.ParamModels
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class UpdateSysUserInfoParam
    {
        public int uID { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string uRealName { get; set; }

        /// <summary>
        /// 是否需要修改密码
        /// </summary>
        public bool isUpdatePWD { get; set; } = false;

        /// <summary>
        /// 登录新密码
        /// </summary>
        public string uLoginPWDNew { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string uRemark { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string tdLogo { get; set; }

    }
}
