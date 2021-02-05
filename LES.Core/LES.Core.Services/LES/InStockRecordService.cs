 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// 入库记录服务接口实现 
	/// </summary>	 
    public class Les_InStockRecordServices : BaseServices<Les_InStockRecord>, ILes_InStockRecordServices 
    { 
        private readonly IBaseRepository<Les_InStockRecord> _dal; 
        public Les_InStockRecordServices(IBaseRepository<Les_InStockRecord> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
