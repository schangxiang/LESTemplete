 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// 出库记录服务接口实现 
	/// </summary>	 
    public class Wip_OutStockRecordServices : BaseServices<Wip_OutStockRecord>, IWip_OutStockRecordServices 
    { 
        private readonly IBaseRepository<Wip_OutStockRecord> _dal; 
        public Wip_OutStockRecordServices(IBaseRepository<Wip_OutStockRecord> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
