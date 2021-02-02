using LES.Core.IRepository.Base;
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;

namespace LES.Core.Services
{
    /// <summary>
    /// ModuleServices
    /// </summary>	
    public class ModuleServices : BaseServices<Modules>, IModuleServices
    {

        IBaseRepository<Modules> _dal;
        public ModuleServices(IBaseRepository<Modules> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
