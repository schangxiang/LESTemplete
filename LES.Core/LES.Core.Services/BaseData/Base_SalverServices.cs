
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;
using LES.Core.IRepository.Base;

namespace LES.Core.Services
{
    public class Base_SalverServices : BaseServices<Base_Salver>, IBase_SalverServices
    {
        private readonly IBaseRepository<Base_Salver> _dal;
        public Base_SalverServices(IBaseRepository<Base_Salver> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}