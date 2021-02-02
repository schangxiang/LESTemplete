using LES.Core.IRepository.Base;
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;

namespace LES.Core.Services
{
    /// <summary>
    /// PermissionServices
    /// </summary>	
    public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {

        IBaseRepository<Permission> _dal;
        public PermissionServices(IBaseRepository<Permission> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
