 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// 库存信息服务接口实现 
	/// </summary>	 
    public class Wip_StockInfoServices : BaseServices<Wip_StockInfo>, IWip_StockInfoServices 
    { 
        private readonly IBaseRepository<Wip_StockInfo> _dal; 
        public Wip_StockInfoServices(IBaseRepository<Wip_StockInfo> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
