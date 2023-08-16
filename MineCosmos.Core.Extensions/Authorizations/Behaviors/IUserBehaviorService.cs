using System.Threading.Tasks;

namespace MineCosmos.Core.Extensions.Authorizations.Behaviors
{
    public interface IUserBehaviorService
    {

        Task<bool> CreateOrUpdateUserAccessByUid();

        Task<bool> RemoveAllUserAccessByUid();

        Task<bool> CheckUserIsNormal();

        Task<bool> CheckTokenIsNormal();
    }
}
