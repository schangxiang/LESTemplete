using LES.Core.Common.Helper;
using LES.Core.Common.HttpContextUser;
using LES.Core.Extensions.Others;
using LES.Core.IServices;
using LES.Core.Model;
using LES.Core.Model.Models;
using LES.Core.Model.ParamModels;
using LES.Core.Model.ViewModels.BasicData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LES.Core.Api.Controllers
{
    /// <summary> 
    /// 站点管理控制器 
    /// </summary> 
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class Base_StationController : ControllerBase
    {
        private readonly IBase_StationServices _Base_StationServices;
        private readonly IBase_CodeItemsServices _base_CodeItemsServices;
        private readonly IUser _user;
        private readonly ILogger<Base_StationController> _logger;

        public Base_StationController(IBase_StationServices Base_StationServices, 
            IUser user, IBase_CodeItemsServices base_CodeItemsServices, ILogger<Base_StationController> logger)
        {
            _Base_StationServices = Base_StationServices;
            _user = user;
            _base_CodeItemsServices = base_CodeItemsServices;
            _logger = logger;
        }

        /// <summary> 
        /// 分页获取站点管理列表 
        /// </summary> 
        /// <param name="param">筛选条件</param> 
        /// <returns>获取结果</returns> 
        [HttpPost]
        public async Task<MessageModel<PageModel<StationViewModel>>> Get([FromBody] Base_StationParam param)
        {
            MessageModel<PageModel<StationViewModel>> result = new MessageModel<PageModel<StationViewModel>>();

            if (string.IsNullOrEmpty(param.searchVal) || string.IsNullOrWhiteSpace(param.searchVal))
            {
                param.searchVal = string.Empty;
            }
            //这里写筛选条件 
            var whereConditions = WhereConditionsExtensions.GetWhereConditions<Base_Station, Base_StationParam>(param);
            if (!whereConditions.IsSuccess)
            {
                return new MessageModel<PageModel<StationViewModel>>()
                {
                    msg = whereConditions.ErrMsg,
                    success = false,
                    response = null
                };
            }
;
            var queryData = await _Base_StationServices.QueryPage(whereConditions.data, param.page, param.pageSize);
            var newQueryData = ListHelper.T1ToT2<PageModel<Base_Station>, PageModel<StationViewModel>>(queryData);
            foreach (var item in newQueryData.data)
            {
                var aa = await _base_CodeItemsServices.QueryById(item.AllowLine);
                item.AllowLineName = aa.name;
            }

            result.msg = "获取成功";
            result.response = newQueryData;
            return result;

        }

        /// <summary> 
        /// 获取单个站点管理 
        /// </summary> 
        /// <param name="id">主键</param> 
        /// <returns>获取结果</returns> 
        [HttpGet]
        public async Task<MessageModel<Base_Station>> Get(int id = 0)
        {
            return new MessageModel<Base_Station>()
            {
                msg = "获取成功",
                success = true,
                response = await _Base_StationServices.QueryById(id)
            };
        }

        /// <summary> 
        /// 新增站点管理 
        /// </summary> 
        /// <param name="request">要新增的站点管理对象</param> 
        /// <returns></returns> 
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Base_Station request)
        {
            var data = new MessageModel<string>();

            request.CreateTime = request.ModifyTime = DateTime.Now;
            request.CreateBy = request.ModifyBy = _user.Name;
            request.CreateId = request.ModifyId = _user.ID;
            request.OperationRemark = "添加";
            var id = await _Base_StationServices.Add(request);
            data.success = id > 0;

            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        /// <summary> 
        /// 更新站点管理 
        /// </summary> 
        /// <param name="request">要更新的站点管理对象</param> 
        /// <returns>更新结果</returns> 
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] Base_Station request)
        {
            var data = new MessageModel<string>();
            if (request.Id > 0)
            {
                request.ModifyTime = DateTime.Now;
                request.ModifyBy = _user.Name;
                request.ModifyId = _user.ID;
                request.OperationRemark = "更新";
                data.success = await _Base_StationServices.Update(request);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = request?.Id.ObjToString();
                }
            }

            return data;
        }

        /// <summary> 
        /// 删除站点管理 
        /// </summary> 
        /// <param name="id">主键</param> 
        /// <returns>删除结果</returns> 
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id = 0)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var detail = await _Base_StationServices.QueryById(id);

                detail.ModifyTime = DateTime.Now;
                detail.ModifyBy = _user.Name;
                detail.ModifyId = _user.ID;
                detail.IsDeleted = true;
                detail.OperationRemark = "删除";

                if (detail != null)
                {
                    data.success = await _Base_StationServices.Update(detail);
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
