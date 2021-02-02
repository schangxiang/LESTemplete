 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// 入库记录服务接口实现 
	/// </summary>	 
    public class Wip_InStockRecordServices : BaseServices<Wip_InStockRecord>, IWip_InStockRecordServices 
    { 
        private readonly IBaseRepository<Wip_InStockRecord> _dal; 
        public Wip_InStockRecordServices(IBaseRepository<Wip_InStockRecord> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
