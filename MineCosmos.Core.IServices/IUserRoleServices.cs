using MineCosmos.Core.IServices.BASE;
using MineCosmos.Core.Model.Models;
using System.Threading.Tasks;

namespace MineCosmos.Core.IServices
{	
	/// <summary>
	/// UserRoleServices
	/// </summary>	
    public interface IUserRoleServices :IBaseServices<UserRole>
	{

        Task<UserRole> SaveUserRole(int uid, int rid);
        Task<int> GetRoleIdByUid(int uid);
    }
}

