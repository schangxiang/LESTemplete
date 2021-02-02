using LES.Core.Common;
using LES.Core.IRepository.Base;
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;
using System.Linq;
using System.Threading.Tasks;

namespace LES.Core.Services
{
    /// <summary>
    /// RoleServices
    /// </summary>	
    public class RoleServices : BaseServices<Role>, IRoleServices
    {

        IBaseRepository<Role> _dal;
        public RoleServices(IBaseRepository<Role> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="roleName"></param>
       /// <returns></returns>
        public async Task<Role> SaveRole(string roleName)
        {
            Role role = new Role(roleName);
            Role model = new Role();
            var userList = await base.Query(a => a.Name == role.Name && a.Enabled);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(role);
                model = await base.QueryById(id);
            }

            return model;

        }

        [Caching(AbsoluteExpiration = 30)]
        public async Task<string> GetRoleNameByRid(int rid)
        {
            return ((await base.QueryById(rid))?.Name);
        }
    }
}
