using BCVP.IServices.BASE;
using BCVP.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCVP.IServices
{
    public interface ITopicDetailServices : IBaseServices<TopicDetail>
    {
        Task<List<TopicDetail>> GetTopicDetails();
    }
}
