using LES.Core.IServices.BASE;
using LES.Core.Model.Models;
using System.Threading.Tasks;

namespace LES.Core.IServices
{	
	/// <summary>
	/// RoleServices
	/// </summary>	
    public interface IRoleServices :IBaseServices<Role>
	{
        Task<Role> SaveRole(string roleName);
        Task<string> GetRoleNameByRid(int rid);

    }
}
