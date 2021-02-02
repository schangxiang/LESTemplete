using LES.Core.IServices;
using LES.Core.Model;
using LES.Core.Model.Models;
using LES.Core.Model.ParamModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using LES.Core.Common.HttpContextUser;
using System.Collections.Generic;
using LES.Core.Model.ViewModels.BasicData;
using LES.Core.Model.ViewModels;
using LES.Core.Extensions.Others;

namespace LES.Core.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class Base_CodeItemsController : ControllerBase
    {
        /// <summary>
        /// 服务器接口，因为是模板生成，所以首字母是大写的，自己可以重构下
        /// </summary>
        private readonly IBase_CodeItemsServices _base_CodeItemsServices;
        private readonly IBase_CodeSetsServices _base_CodeSetsServices;
        private readonly IV_CodeItemsServices _v_CodeItemsServices;
        private readonly IUser _user;

        public Base_CodeItemsController(IBase_CodeItemsServices Base_CodeItemsServices, IBase_CodeSetsServices Base_CodeSetsServices, IUser user
            , IV_CodeItemsServices v_CodeItemsServices)
        {
            _base_CodeItemsServices = Base_CodeItemsServices;
            _base_CodeSetsServices = Base_CodeSetsServices;
            _user = user;
            _v_CodeItemsServices = v_CodeItemsServices;
        }

        [HttpPost]
        public async Task<MessageModel<PageModel<V_CodeItems>>> Get([FromBody] Base_CodeItemsParam param)
        {
            if (string.IsNullOrEmpty(param.searchVal) || string.IsNullOrWhiteSpace(param.searchVal))
            {
                param.searchVal = string.Empty;
            }
            //这里写筛选条件
            var whereConditions = WhereConditionsExtensions.GetWhereConditions<V_CodeItems, Base_CodeItemsParam>(param);
            if (!whereConditions.IsSuccess)
            {
                return new MessageModel<PageModel<V_CodeItems>>()
                {
                    msg = whereConditions.ErrMsg,
                    success = false,
                    response = null
                };
            }
            var dataList = await _v_CodeItemsServices.QueryPage(whereConditions.data, param.page, param.pageSize);
            if (dataList != null && dataList.data != null)
            {
                foreach (var item in dataList.data)
                {
                    // var codeSets = await _base_CodeSetsServices.Query(x => x.code == item.setCode);
                    // item.setCodeName = codeSets.FirstOrDefault() == null ? "" : codeSets.FirstOrDefault().name;
                }
            }

            return new MessageModel<PageModel<V_CodeItems>>()
            {
                msg = "获取成功",
                success = true,
                response = dataList
            };

        }

        [HttpGet]
        public async Task<MessageModel<Base_CodeItems>> Get(int id = 0)
        {
            return new MessageModel<Base_CodeItems>()
            {
                msg = "获取成功",
                success = true,
                response = await _base_CodeItemsServices.QueryById(id)
            };
        }


        /// <summary>
        /// 通过代码集获取代码项集合
        /// </summary>
        /// <param name="setCode">代码集编码</param>
        /// <returns>代码项集合</returns>
        [HttpGet]
        public async Task<MessageModel<List<SelectViewModel>>> GetCodeItemsBySetCode(string setCode)
        {
            List<SelectViewModel> reusltDataList = new List<SelectViewModel>();
            var queryData = await _v_CodeItemsServices.Query(x => x.setCodeCode == setCode);
            foreach (var item in queryData)
            {
                reusltDataList.Add(new SelectViewModel()
                {
                    value = item.Id,
                    label = item.name
                });
            }
            return new MessageModel<List<SelectViewModel>>()
            {
                msg = "获取成功",
                success = true,
                response = reusltDataList
            };
        }

        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Base_CodeItems request)
        {
            var data = new MessageModel<string>();

            //判断是否已经存在
            if (await _base_CodeItemsServices.IsExistCodeItemBySetCode(request.setCode, request.code))
            {
                data.msg = "该码表集已经存在，不允许重复添加！";
                return data;
            }

            request.creator = request.lastModifier = _user.Name;
            request.createTime = request.lastModifyTime = DateTime.Now;
            var id = await _base_CodeItemsServices.Add(request);
            data.success = id > 0;

            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] Base_CodeItems request)
        {
            var data = new MessageModel<string>();
            if (request.Id > 0)
            {
                //判断是否已经存在
                if (await _base_CodeItemsServices.IsExistCodeItemBySetCodeExcludeId(request.setCode, request.code, request.Id))
                {
                    data.msg = "该码表集已经存在，不允许更新为该码表集！";
                    return data;
                }


                request.lastModifier = _user.Name;
                request.lastModifyTime = DateTime.Now;
                data.success = await _base_CodeItemsServices.Update(request);
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
                var detail = await _base_CodeItemsServices.QueryById(id);

                detail.lastModifier = _user.Name;
                detail.lastModifyTime = DateTime.Now;
                detail.delFlag = true;

                if (detail != null)
                {
                    data.success = await _base_CodeItemsServices.Update(detail);
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