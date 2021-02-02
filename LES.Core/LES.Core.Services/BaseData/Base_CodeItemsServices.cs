
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;
using LES.Core.IRepository.Base;
using System.Threading.Tasks;

namespace LES.Core.Services
{
    public class Base_CodeItemsServices : BaseServices<Base_CodeItems>, IBase_CodeItemsServices
    {
        private readonly IBaseRepository<Base_CodeItems> _dal;
        public Base_CodeItemsServices(IBaseRepository<Base_CodeItems> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
        /// <summary>
        /// 同一个码表集，是否已经存在了该码表项
        /// </summary>
        /// <param name="setCode">码表集编码</param>
        ///  <param name="code">码表项编码</param>
        /// <returns>true:已存在，false：不存在</returns>
        public async Task<bool> IsExistCodeItemBySetCode(int setCode, string code)
        {
            var result = false;
            var queryData = await _dal.Query(x => x.setCode == setCode && x.code == code);
            if (queryData != null && queryData.Count > 0)
            {
                result = true;
            }
            return result;
        }

        public async Task<bool> IsExistCodeItemBySetCodeExcludeId(int setCode, string code, int id)
        {
            var result = false;
            var queryData = await _dal.Query(x => x.setCode == setCode && x.code == code && x.Id != id);
            if (queryData != null && queryData.Count > 0)
            {
                result = true;
            }
            return result;
        }
    }
}