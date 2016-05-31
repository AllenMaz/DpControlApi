using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility;
using DpControl.Utility.Filters;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers.APIControllers
{
    [Authorize(Roles = Role.Admin)]
    public class RolesController : BaseAPIController
    {
        [FromServices]
        public IRoleRepository _roleInfoRepository { get; set; }

        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:All<br/>
        /// Description：根据ID获取Role信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery(typeof(RoleSearchModel))]
        [HttpGet("{roleId}", Name = "GetByRoleIdAsync")]
        public async Task<IActionResult> GetByRoleIdAsync(string roleId)
        {
            var role = await _roleInfoRepository.FindByIdAsync(roleId);
            if (role == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(role);
        }

        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:All<br/>
        /// Description：根据roleId以及当前登录用户信息获取该Role下的Users
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{roleId}/Users")]
        public async Task<IEnumerable<UserSubSearchModel>> GetUsersByRoleIdAsync(string roleId)
        {
            var users = await _roleInfoRepository.GetUsersByRoleId(roleId);
            return users;
        }


        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:All<br/>
        /// Description：获取所有角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<RoleSearchModel>> GetAllAsync()
        {
            var result = await _roleInfoRepository.GetAllAsync();

            return result;
        }

        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:All<br/>
        /// Description：根据RoleId为Role添加User
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("{roleId}/{navigationProperty}")]
        public async Task<IActionResult> CreateRelationsAsync(string roleId, string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if (navigationPropertyIds == null || navigationPropertyIds.Count == 0)
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();
            await _roleInfoRepository.CreateRelationsAsync(roleId, navigationProperty, uniqueNavigationPropertyIds);

            var returnUrl = Url.RouteUrl(new Microsoft.AspNet.Mvc.Routing.UrlRouteContext()
            {
                RouteName = "GetByRoleIdAsync",
                Values = new { controller = "Roles", roleId = roleId },
                Protocol = HttpContext.Request.Scheme,
                Host = HttpContext.Request.Host.Value,
            });
            return Created(returnUrl + "?expand=" + navigationProperty, null);

        }

        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:All<br/>
        /// Description：根据RoleId为Role移除User
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{roleId}/{navigationProperty}")]
        public async Task<IActionResult> RemoveRelationsAsync(string roleId, string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if (navigationPropertyIds == null || navigationPropertyIds.Count == 0)
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();

            await _roleInfoRepository.RemoveRelationsAsync(roleId, navigationProperty, uniqueNavigationPropertyIds);
            return Ok();
        }


        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> AddAsync([FromBody] RoleAddModel mRoleAddModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return HttpBadRequest(ModelStateError());
        //    }

        //    string roleId = await _roleInfoRepository.AddAsync(mRoleAddModel);
        //    return CreatedAtRoute("GetByRoleIdAsync", new { controller = "Roles", roleId = roleId }, mRoleAddModel);
        //}

        /// <summary>
        /// Edit data by UserId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateAsync(string id, [FromBody] RoleUpdateModel mRole)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return HttpBadRequest(ModelStateError());
        //    }

        //    var roleId = await _roleInfoRepository.UpdateByIdAsync(id, mRole);
        //    return CreatedAtRoute("GetByRoleIdAsync", new { controller = "Roles", roleId = roleId }, mRole);

        //}

        /// <summary>
        /// Delete data by UserId
        /// </summary>
        /// <param name="roleId"></param>
        //[HttpDelete("{roleId}")]
        //public async Task<IActionResult> DeleteByRoleIdAsync(string roleId)
        //{
        //    await _roleInfoRepository.RemoveByIdAsync(roleId);
        //    return Ok();
        //}


    }

}
