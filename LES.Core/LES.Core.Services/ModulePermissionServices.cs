using LES.Core.Services.BASE;
using LES.Core.Model.Models;
using LES.Core.IRepository;
using LES.Core.IServices;
using LES.Core.IRepository.Base;

namespace LES.Core.Services
{	
	/// <summary>
	/// ModulePermissionServices
	/// </summary>	
	public class ModulePermissionServices : BaseServices<ModulePermission>, IModulePermissionServices
    {

        IBaseRepository<ModulePermission> _dal;
        public ModulePermissionServices(IBaseRepository<ModulePermission> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
