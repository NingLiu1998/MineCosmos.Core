using System.Threading.Tasks;
using MineCosmos.Core.IServices.BASE;
using MineCosmos.Core.Model.Models;

namespace MineCosmos.Core.IServices
{
    public partial interface IPasswordLibServices :IBaseServices<PasswordLib>
    {
        Task<bool> TestTranPropagation2();
        Task<bool> TestTranPropagationNoTranError();
        Task<bool> TestTranPropagationTran2();
    }
}
