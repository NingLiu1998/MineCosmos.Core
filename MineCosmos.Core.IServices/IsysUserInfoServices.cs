    

using MineCosmos.Core.IServices.BASE;
using MineCosmos.Core.Model.Models;
using System.Threading.Tasks;

namespace MineCosmos.Core.IServices
{	
	/// <summary>
	/// sysUserInfoServices
	/// </summary>	
    public interface ISysUserInfoServices :IBaseServices<SysUserInfo>
	{
        Task<SysUserInfo> SaveUserInfo(string loginName, string loginPwd);
        Task<string> GetUserRoleNameStr(string loginName, string loginPwd);
    }
}
