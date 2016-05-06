using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility.Filters;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers.APIControllers
{
    public class RolesController : BaseAPIController
    {
        [FromServices]
        public IRoleRepository _roleInfoRepository { get; set; }

        /// <summary>
        /// Search data by RoleId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
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

        #region Relations
        /// <summary>
        /// Get Users Relation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{roleId}/Users")]
        public async Task<IEnumerable<UserSubSearchModel>> GetUsersByRoleIdAsync(string roleId)
        {
            var users = await _roleInfoRepository.GetUsersByUserId(roleId);
            return users;
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<RoleSearchModel>> GetAllAsync()
        {

            var result = await _roleInfoRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] RoleAddModel mRoleAddModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            string roleId = await _roleInfoRepository.AddAsync(mRoleAddModel);
            return CreatedAtRoute("GetByRoleIdAsync", new { controller = "Roles", roleId = roleId }, mRoleAddModel);
        }

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
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteByRoleIdAsync(string roleId)
        {
            await _roleInfoRepository.RemoveByIdAsync(roleId);
            return Ok();
        }

        /// <summary>
        /// Create RelationShips:Users
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
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
        /// Remove RelationShips:Roles、Locations、Groups
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [HttpDelete("{userId}/{navigationProperty}")]
        public async Task<IActionResult> RemoveRelationsAsync(string userId, string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if (navigationPropertyIds == null || navigationPropertyIds.Count == 0)
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();

            await _roleInfoRepository.RemoveRelationsAsync(userId, navigationProperty, uniqueNavigationPropertyIds);
            return Ok();
        }

    }

}
