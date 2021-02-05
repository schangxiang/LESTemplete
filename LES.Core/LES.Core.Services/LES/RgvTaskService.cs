 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// RGV任务服务接口实现 
	/// </summary>	 
    public class Les_RgvTaskServices : BaseServices<Les_RgvTask>, ILes_RgvTaskServices 
    { 
        private readonly IBaseRepository<Les_RgvTask> _dal; 
        public Les_RgvTaskServices(IBaseRepository<Les_RgvTask> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
