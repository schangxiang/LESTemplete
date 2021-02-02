using LES.Core.IRepository.Base;
using LES.Core.Model.IDS4DbModels;
using LES.Core.Services.BASE;

namespace LES.Core.IServices
{
    public class ApplicationUserServices : BaseServices<ApplicationUser>, IApplicationUserServices
    {

        IBaseRepository<ApplicationUser> _dal;
        public ApplicationUserServices(IBaseRepository<ApplicationUser> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}