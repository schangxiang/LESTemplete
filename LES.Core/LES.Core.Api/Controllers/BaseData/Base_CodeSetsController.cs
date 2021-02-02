using LES.Core.Common.HttpContextUser;
using LES.Core.Extensions.Others;
using LES.Core.IServices;
using LES.Core.Model;
using LES.Core.Model.Models;
using LES.Core.Model.ParamModels;
using LES.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LES.Core.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class Base_CodeSetsController : ControllerBase
    {
        /// <summary>
        /// 服务器接口，因为是模板生成，所以首字母是大写的，自己可以重构下
        /// </summary>
        private readonly IBase_CodeSetsServices _base_CodeSetsServices;
        private readonly IUser _user;

        public Base_CodeSetsController(IBase_CodeSetsServices Base_CodeSetsServices, IUser user)
        {
            _base_CodeSetsServices = Base_CodeSetsServices;
            _user = user;
        }

        /// <summary>
        /// 获取所有的码表集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<SelectViewModel>>> GetAll()
        {
            List<SelectViewModel> resultData = new List<SelectViewModel>();
            Expression<Func<Base_CodeSets, bool>> whereExpression = a => (a.Id > 0 && (a.delFlag == null || a.delFlag == false));

            var dataList = await _base_CodeSetsServices.Query(whereExpression);
            foreach (var item in dataList)
            {
                resultData.Add(new SelectViewModel()
                {
                    value = item.Id,
                    label = item.name
                });
            }
            return new MessageModel<List<SelectViewModel>>()
            {
                msg = "获取成功",
                success = true,
                response = resultData
            };
        }

        [HttpPost]
        public async Task<MessageModel<PageModel<Base_CodeSets>>> Get([FromBody] Base_CodeSetsParam param)
        {
            if (string.IsNullOrEmpty(param.searchVal) || string.IsNullOrWhiteSpace(param.searchVal))
            {
                param.searchVal = string.Empty;
            }
            //这里写筛选条件
            var whereConditions = WhereConditionsExtensions.GetWhereConditions<Base_CodeSets, Base_CodeSetsParam>(param);
            if (!whereConditions.IsSuccess)
            {
                return new MessageModel<PageModel<Base_CodeSets>>()
                {
                    msg = whereConditions.ErrMsg,
                    success = false,
                    response = null
                };
            }

            return new MessageModel<PageModel<Base_CodeSets>>()
            {
                msg = "获取成功",
                success = true,
                response = await _base_CodeSetsServices.QueryPage(whereConditions.data, param.page, param.pageSize)
            };
        }

        [HttpGet("{id}")]
        public async Task<MessageModel<Base_CodeSets>> Get(int id = 0)
        {
            return new MessageModel<Base_CodeSets>()
            {
                msg = "获取成功",
                success = true,
                response = await _base_CodeSetsServices.QueryById(id)
            };
        }

        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Base_CodeSets request)
        {
            var data = new MessageModel<string>();

            request.createTime = request.lastModifyTime = DateTime.Now;
            request.creator = request.lastModifier = _user.Name;

            var id = await _base_CodeSetsServices.Add(request);
            data.success = id > 0;

            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] Base_CodeSets request)
        {
            var data = new MessageModel<string>();
            if (request.Id > 0)
            {
                request.lastModifyTime = DateTime.Now;
                request.lastModifier = _user.Name;
                data.success = await _base_CodeSetsServices.Update(request);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = request?.Id.ObjToString();
                }
            }

            return data;
        }

        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id = 0)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var detail = await _base_CodeSetsServices.QueryById(id);

                detail.lastModifyTime = DateTime.Now;
                detail.lastModifier = _user.Name;
                detail.delFlag = true;

                if (detail != null)
                {
                    data.success = await _base_CodeSetsServices.Update(detail);
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