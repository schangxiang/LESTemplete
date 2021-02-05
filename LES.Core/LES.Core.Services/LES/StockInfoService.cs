 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// 库存信息服务接口实现 
	/// </summary>	 
    public class Les_StockInfoServices : BaseServices<Les_StockInfo>, ILes_StockInfoServices
    { 
        private readonly IBaseRepository<Les_StockInfo> _dal; 
        public Les_StockInfoServices(IBaseRepository<Les_StockInfo> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
