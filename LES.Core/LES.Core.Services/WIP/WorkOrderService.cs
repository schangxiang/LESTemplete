 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// 工单管理服务接口实现 
	/// </summary>	 
    public class Wip_WorkOrderServices : BaseServices<Wip_WorkOrder>, IWip_WorkOrderServices 
    { 
        private readonly IBaseRepository<Wip_WorkOrder> _dal; 
        public Wip_WorkOrderServices(IBaseRepository<Wip_WorkOrder> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
