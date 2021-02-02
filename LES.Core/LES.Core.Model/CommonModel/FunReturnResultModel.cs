namespace LES.Core.Model.CommonModel
{
    /// <summary>
    /// 方法返回实体类
    /// </summary>
    public class FunReturnResultModel
    {
        public FunReturnResultModel()
        {
            this.IsSuccess = false;
        }

        /// <summary>
        ///  执行是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

    }


    /// <summary>
    /// 方法返回实体类
    /// </summary>
    public class FunReturnResultModel<T>
    {
        /// <summary>
        ///  执行是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T data { get; set; }

    }
}
