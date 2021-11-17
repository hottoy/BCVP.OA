using BCVP.IServices.BASE;
using BCVP.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCVP.IServices
{
    public interface ITopicServices : IBaseServices<Topic>
    {
        Task<List<Topic>> GetTopics();
    }
}
