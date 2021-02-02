using LES.Core.Extensions.Others;
using LES.Core.IServices;
using LES.Core.Model;
using LES.Core.Model.Models;
using LES.Core.Model.ParamModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LES.Core.Api.Controllers
{
    /// <summary>
    /// 托盘管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class Base_SalverController : ControllerBase
    {
        /// <summary>
        /// 服务器接口，因为是模板生成，所以首字母是大写的，自己可以重构下
        /// </summary>
        private readonly IBase_SalverServices _base_SalverServices;

        public Base_SalverController(IBase_SalverServices Base_SalverServices)
        {
            _base_SalverServices = Base_SalverServices;
        }

        [HttpPost]
        public async Task<MessageModel<PageModel<Base_Salver>>> Get([FromBody] Base_SalverParam param)
        {
            if (string.IsNullOrEmpty(param.searchVal) || string.IsNullOrWhiteSpace(param.searchVal))
            {
                param.searchVal = string.Empty;
            }
            var whereConditions = WhereConditionsExtensions.GetWhereConditions<Base_Salver, Base_SalverParam>(param);
            if (!whereConditions.IsSuccess)
            {
                return new MessageModel<PageModel<Base_Salver>>()
                {
                    msg = whereConditions.ErrMsg,
                    success = false,
                    response = null
                };
            }

            return new MessageModel<PageModel<Base_Salver>>()
            {
                msg = "获取成功",
                success = true,
                response = await _base_SalverServices.QueryPage(whereConditions.data, param.page, param.pageSize)
            };
        }

        [HttpGet("{id}")]
        public async Task<MessageModel<Base_Salver>> Get(int id = 0)
        {
            return new MessageModel<Base_Salver>()
            {
                msg = "获取成功",
                success = true,
                response = await _base_SalverServices.QueryById(id)
            };
        }

        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Base_Salver request)
        {
            var data = new MessageModel<string>();

            var id = await _base_SalverServices.Add(request);
            data.success = id > 0;

            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] Base_Salver request)
        {
            var data = new MessageModel<string>();
            if (request.Id > 0)
            {
                data.success = await _base_SalverServices.Update(request);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = request?.Id.ObjToString();
                }
            }

            return data;
        }

        [HttpDelete("{id}")]
        public async Task<MessageModel<string>> Delete(int id = 0)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var detail = await _base_SalverServices.QueryById(id);


                if (detail != null)
                {
                    data.success = await _base_SalverServices.Update(detail);
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