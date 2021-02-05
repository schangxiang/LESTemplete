 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// RGV异常服务接口实现 
	/// </summary>	 
    public class Les_RgvExcepitonServices : BaseServices<Les_RgvExcepiton>, ILes_RgvExcepitonServices 
    { 
        private readonly IBaseRepository<Les_RgvExcepiton> _dal; 
        public Les_RgvExcepitonServices(IBaseRepository<Les_RgvExcepiton> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
