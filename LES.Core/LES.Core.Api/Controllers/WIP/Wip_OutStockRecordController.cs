using LES.Core.Common.HttpContextUser; 
using LES.Core.Extensions.Others; 
using LES.Core.IServices; 
using LES.Core.Model; 
using LES.Core.Model.Models; 
using LES.Core.Model.ParamModels; 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic; 
using System.Threading.Tasks; 
using System; 
 
namespace LES.Core.Api.Controllers 
{ 
    /// <summary> 
    /// 出库记录控制器 
    /// </summary> 
    [Route("api/[controller]/[action]")] 
    [ApiController] 
    [Authorize(Permissions.Name)] 
    public class Wip_OutStockRecordController : ControllerBase 
    { 
        private readonly IWip_OutStockRecordServices _Wip_OutStockRecordServices; 
        private readonly IUser _user; 
 
        public Wip_OutStockRecordController(IWip_OutStockRecordServices Wip_OutStockRecordServices, IUser user) 
        { 
            _Wip_OutStockRecordServices = Wip_OutStockRecordServices; 
            _user = user; 
        } 
 
        /// <summary> 
        /// 分页获取出库记录列表 
        /// </summary> 
        /// <param name="param">筛选条件</param> 
        /// <returns>获取结果</returns> 
        [HttpPost] 
        public async Task<MessageModel<PageModel<Wip_OutStockRecord>>> Get([FromBody] Wip_OutStockRecordParam param) 
        { 
            if (string.IsNullOrEmpty(param.searchVal) || string.IsNullOrWhiteSpace(param.searchVal)) 
            { 
                param.searchVal = string.Empty; 
            } 
             
            var whereConditions = WhereConditionsExtensions.GetWhereConditions<Wip_OutStockRecord, Wip_OutStockRecordParam>(param); 
            if (!whereConditions.IsSuccess) { 
                return new MessageModel<PageModel<Wip_OutStockRecord>>() 
                { 
                    msg = whereConditions.ErrMsg, 
                    success = false, 
                    response = null 
                }; 
            } 
			var data = await _Wip_OutStockRecordServices.QueryPage(whereConditions.data, param.page, param.pageSize, " ModifyTime desc "); 
            return new MessageModel<PageModel<Wip_OutStockRecord>>() 
            { 
                msg = "获取成功", 
                success = true, 
                response = data 
            }; 
 
        } 
 
        /// <summary> 
        /// 获取单个出库记录 
        /// </summary> 
        /// <param name="id">主键</param> 
        /// <returns>获取结果</returns> 
        [HttpGet] 
        public async Task<MessageModel<Wip_OutStockRecord>> Get(int id = 0) 
        { 
            return new MessageModel<Wip_OutStockRecord>() 
            { 
                msg = "获取成功", 
                success = true, 
                response = await _Wip_OutStockRecordServices.QueryById(id) 
            }; 
        } 
 
        /// <summary> 
        /// 新增出库记录 
        /// </summary> 
        /// <param name="request">要新增的出库记录对象</param> 
        /// <returns></returns> 
        [HttpPost] 
        public async Task<MessageModel<string>> Post([FromBody] Wip_OutStockRecord request) 
        { 
            var data = new MessageModel<string>(); 
 
            request.CreateTime = request.ModifyTime = DateTime.Now; 
            request.CreateBy = request.ModifyBy = _user.Name; 
            request.CreateId = request.ModifyId = _user.ID; 
            request.OperationRemark = "添加"; 
            var id = await _Wip_OutStockRecordServices.Add(request); 
            data.success = id > 0; 
 
            if (data.success) 
            { 
                data.response = id.ObjToString(); 
                data.msg = "添加成功"; 
            } 
 
            return data; 
        } 
 
        /// <summary> 
        /// 更新出库记录 
        /// </summary> 
        /// <param name="request">要更新的出库记录对象</param> 
        /// <returns>更新结果</returns> 
        [HttpPut] 
        public async Task<MessageModel<string>> Put([FromBody] Wip_OutStockRecord request) 
        { 
            var data = new MessageModel<string>(); 
            if (request.Id > 0) 
            { 
                request.ModifyTime = DateTime.Now; 
                request.ModifyBy = _user.Name; 
                request.ModifyId = _user.ID; 
                request.OperationRemark = "更新"; 
                data.success = await _Wip_OutStockRecordServices.Update(request); 
                if (data.success) 
                { 
                    data.msg = "更新成功"; 
                    data.response = request?.Id.ObjToString(); 
                } 
            } 
 
            return data; 
        } 
 
        /// <summary> 
        /// 删除出库记录 
        /// </summary> 
        /// <param name="id">主键</param> 
        /// <returns>删除结果</returns> 
        [HttpDelete] 
        public async Task<MessageModel<string>> Delete(int id = 0) 
        { 
            var data = new MessageModel<string>(); 
            if (id > 0) 
            { 
                var detail = await _Wip_OutStockRecordServices.QueryById(id); 
                if (detail != null) 
                { 
                    data.success = await _Wip_OutStockRecordServices.DeleteById(detail.Id); 
                    if (data.success) 
                    { 
                        data.msg = "删除成功"; 
                        data.response = detail?.Id.ObjToString(); 
                    } 
                } 
            } 
 
            return data; 
        } 
 
    } 
} 
