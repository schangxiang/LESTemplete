using LES.Core.IServices.BASE;
using LES.Core.Model.Models;
using System.Threading.Tasks;

namespace LES.Core.IServices
{
    /// <summary>
    /// IBase_CodeItemsServices
    /// </summary>	
    public interface IBase_CodeItemsServices : IBaseServices<Base_CodeItems>
    {
        /// <summary>
        /// 同一个码表集，是否已经存在了该码表项
        /// </summary>
        /// <param name="setCode">码表集编码</param>
        /// <param name="code">码表项编码</param>
        /// <returns>true:已存在，false：不存在</returns>
        Task<bool> IsExistCodeItemBySetCode(int setCode, string code);

        /// <summary>
        /// 同一个码表集，是否已经存在了该码表项(排除ID)
        /// </summary>
        /// <param name="setCode">码表集编码</param>
        /// <param name="code">码表项编码</param>
        /// <param name="id">码表项ID</param>
        /// <returns>true:已存在，false：不存在</returns>
        Task<bool> IsExistCodeItemBySetCodeExcludeId(int setCode, string code, int id);
    }
}