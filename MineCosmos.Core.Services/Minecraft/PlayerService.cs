using MineCosmos.Core.Common;
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
    /// MinecraftÍæ¼Ò
    /// </summary>	
    public class PlayerService : BaseServices<MinecraftPlayer>, IPlayerService
    {
        public PlayerService()
        {
                
        }
    }
}
