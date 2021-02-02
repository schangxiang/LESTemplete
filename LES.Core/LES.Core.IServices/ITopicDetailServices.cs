using LES.Core.IServices.BASE;
using LES.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LES.Core.IServices
{
    public interface ITopicDetailServices : IBaseServices<TopicDetail>
    {
        Task<List<TopicDetail>> GetTopicDetails();
    }
}
