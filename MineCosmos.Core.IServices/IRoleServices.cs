using MineCosmos.Core.IServices.BASE;
using MineCosmos.Core.Model.Models;
using System.Threading.Tasks;

namespace MineCosmos.Core.IServices
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
