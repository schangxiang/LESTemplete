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

namespace LES.Core.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class RoleController : ControllerBase
    {
        readonly IRoleServices _roleServices;
        readonly IUser _user;


        public RoleController(IRoleServices roleServices, IUser user)
        {
            _roleServices = roleServices;
            _user = user;
        }

        /// <summary>
        /// 分页获取全部角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<PageModel<Role>>> Get(RoleParam param)
        {
            if (string.IsNullOrEmpty(param.searchVal) || string.IsNullOrWhiteSpace(param.searchVal))
            {
                param.searchVal = string.Empty;
            }
            var whereConditions=WhereConditionsExtensions.GetWhereConditions<Role, RoleParam>(param);
            if (!whereConditions.IsSuccess) {
                return new MessageModel<PageModel<Role>>()
                {
                    msg = whereConditions.ErrMsg,
                    success = false,
                    response = null
                };
            }
            var data = await _roleServices.QueryPage(whereConditions.data, param.page, param.pageSize, " Id desc ");

            return new MessageModel<PageModel<Role>>()
            {
                msg = "获取成功",
                success = data.dataCount >= 0,
                response = data
            };
        }

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<Role>>> GetAll()
        {
            var data = await _roleServices.Query(a => a.IsDeleted != true);

            return new MessageModel<List<Role>>()
            {
                msg = "获取成功",
                success = true,
                response = data
            };
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "value";
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Role role)
        {
            var data = new MessageModel<string>();

            role.CreateId = _user.ID;
            role.CreateBy = _user.Name;

            var id = (await _roleServices.Add(role));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] Role role)
        {
            var data = new MessageModel<string>();
            if (role != null && role.Id > 0)
            {
                data.success = await _roleServices.Update(role);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = role?.Id.ObjToString();
                }
            }

            return data;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _roleServices.QueryById(id);
                userDetail.IsDeleted = true;
                data.success = await _roleServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = userDetail?.Id.ObjToString();
                }
            }

            return data;
        }
    }
}
