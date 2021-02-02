
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;
using LES.Core.IRepository.Base;

namespace LES.Core.Services
{
    public class Base_StationServices : BaseServices<Base_Station>, IBase_StationServices
    {
        private readonly IBaseRepository<Base_Station> _dal;
        public Base_StationServices(IBaseRepository<Base_Station> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}