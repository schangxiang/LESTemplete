using System.Threading.Tasks;

namespace LES.Core.Extensions.Authorizations.Behaviors
{
    public interface IUserBehaviorService
    {

        Task<bool> CreateOrUpdateUserAccessByUid();

        Task<bool> RemoveAllUserAccessByUid();

        Task<bool> CheckUserIsNormal();

        Task<bool> CheckTokenIsNormal();
    }
}
