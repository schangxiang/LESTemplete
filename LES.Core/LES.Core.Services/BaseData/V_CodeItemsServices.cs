
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;
using LES.Core.IRepository.Base;
using System.Threading.Tasks;
using LES.Core.Model.ViewModels.BasicData;

namespace LES.Core.Services
{
    public class V_CodeItemsServices : BaseServices<Model.ViewModels.BasicData.V_CodeItems>, IV_CodeItemsServices
    {
        private readonly IBaseRepository<V_CodeItems> _dal;
        public V_CodeItemsServices(IBaseRepository<V_CodeItems> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}