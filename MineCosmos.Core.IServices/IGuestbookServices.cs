using MineCosmos.Core.IServices.BASE;
using MineCosmos.Core.Model;
using MineCosmos.Core.Model.Models;
using System.Threading.Tasks;

namespace MineCosmos.Core.IServices
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        Task<MessageModel<string>> TestTranInRepository();
        Task<bool> TestTranInRepositoryAOP();

        Task<bool> TestTranPropagation();

        Task<bool> TestTranPropagationNoTran();

        Task<bool> TestTranPropagationTran();
    }
}