using MineCosmos.Core.Common;
using MineCosmos.Core.Common.Static;
using MineCosmos.Core.IRepository.Base;
using MineCosmos.Core.IServices;
using MineCosmos.Core.IServices.Minecraft;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Services.BASE;
using System.Linq;
using System.Threading.Tasks;

namespace MineCosmos.Core.Services
{
    /// <summary>
    /// Minecraft���
    /// </summary>	
    public class PlayerService : BaseServices<MinecraftPlayer>, IPlayerService
    {
        public PlayerService()
        {

        }

        /// <summary>
        /// ��ͨ��¼
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="uPwd"></param>
        /// <returns></returns>
        public async Task<MinecraftPlayer> LoginAsync(string uName, string uPwd)
        {
            if (!(await AnyAsync(a => a.Name == uName)))
                throw Oops.Bah($"��¼ʧ�ܣ������:{uName} ������");

            MinecraftPlayer player = await GetAsync(a => a.Name == uName && a.Pwd == uPwd);
            if (player is null) throw Oops.Bah("��½ʧ�ܣ��������");

            return player;
        }
    }

}
