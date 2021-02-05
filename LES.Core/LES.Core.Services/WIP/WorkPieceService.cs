 
using LES.Core.IServices; 
using LES.Core.Model.Models; 
using LES.Core.Services.BASE; 
using LES.Core.IRepository.Base; 
 
namespace LES.Core.Services 
{ 
    /// <summary> 
	/// 工件数据服务接口实现 
	/// </summary>	 
    public class Wip_WorkPieceServices : BaseServices<Wip_WorkPiece>, IWip_WorkPieceServices 
    { 
        private readonly IBaseRepository<Wip_WorkPiece> _dal; 
        public Wip_WorkPieceServices(IBaseRepository<Wip_WorkPiece> dal) 
        { 
            this._dal = dal; 
            base.BaseDal = dal; 
        } 
    } 
} 
