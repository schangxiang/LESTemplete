namespace LES.Core
{
    /// <summary>
    /// 系统通用的常量
    /// </summary>
    public static class SystemCommonVar
    {
        /// <summary>
        /// 通用的查询字符串
        /// </summary>
        public const string searchVal = "searchVal";

        /// <summary>
        /// 高级查询的后缀名
        /// </summary>
        public const string highSearchModeSuffix = "_FilterMode";


        /// <summary>
        /// 通用的查询字符串的配置模式
        /// </summary>
        public const string searchVal_FilterMode = searchVal + highSearchModeSuffix;


        /// <summary>
        /// 子搜索组件传递的属性名
        /// </summary>
        public const string searchFormInputAttrs = "searchFormInputAttrs";

        /// <summary>
        /// 通用的查询参数类的父类中的字段
        /// </summary>
        public const string commnParamFatherPros = "page|pageSize|" + searchVal + "|" + searchVal_FilterMode + "|" + searchFormInputAttrs;
    }

    /// <summary>
    /// DbFirst生成文件的路径
    /// </summary>
    public static class DbFirstFilePath
    {
        public const string FilePath = @"C:\my-file";
    }

    /// <summary>
    /// 权限变量配置
    /// </summary>
    public static class Permissions
    {
        public const string Name = "Permission";

        /// <summary>
        /// 当前项目是否启用IDS4权限方案
        /// true：表示启动IDS4
        /// false：表示使用JWT
        public static bool IsUseIds4 = false;
    }

    /// <summary>
    /// 路由变量前缀配置
    /// </summary>
    public static class RoutePrefix
    {
        /// <summary>
        /// 前缀名
        /// 如果不需要，尽量留空，不要修改
        /// 除非一定要在所有的 api 前统一加上特定前缀
        /// </summary>
        public const string Name = "";
    }

    /// <summary>
    /// RedisMqKey
    /// </summary>
    public static class RedisMqKey
    {
        public const string Loging = "Loging";
    }
}
