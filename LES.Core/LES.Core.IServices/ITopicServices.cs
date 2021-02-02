using LES.Core.IServices.BASE;
using LES.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LES.Core.IServices
{
    public interface ITopicServices : IBaseServices<Topic>
    {
        Task<List<Topic>> GetTopics();
    }
}
