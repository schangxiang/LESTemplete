 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// 出库记录服务接口实现 
	/// </summary>	 
    public class Les_OutStockRecordServices : BaseServices<Les_OutStockRecord>, ILes_OutStockRecordServices
    { 
        private readonly IBaseRepository<Les_OutStockRecord> _dal; 
        public Les_OutStockRecordServices(IBaseRepository<Les_OutStockRecord> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
