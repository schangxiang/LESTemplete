using LES.Core.IRepository.Base;
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;

namespace LES.Core.Services
{
    public partial class TasksQzServices : BaseServices<TasksQz>, ITasksQzServices
    {
        IBaseRepository<TasksQz> _dal;
        public TasksQzServices(IBaseRepository<TasksQz> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}
                    