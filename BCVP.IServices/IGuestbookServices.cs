using BCVP.IServices.BASE;
using BCVP.Model;
using BCVP.Model.Models;
using System.Threading.Tasks;

namespace BCVP.IServices
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        Task<MessageModel<string>> TestTranInRepository();
        Task<bool> TestTranInRepositoryAOP();
    }
}
