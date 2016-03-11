using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility.Authorization;
using DpControl.Utility.Filters;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DpControl.Controllers.APIControllers
{
    public class UserGroupsController:BaseAPIController
    {
        [FromServices]
        public IUserGroupRepository _userGroupRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserGroupAddModel mUserGroup)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var userGroupId = await _userGroupRepository.AddAsync(mUserGroup);
            return CreatedAtRoute("GetByUserGroupIdAsync", new { controller = "UserGroups", userGroupId = userGroupId }, mUserGroup);
        }

        /// <summary>
        /// Search data by UserGroupId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet("{userGroupId}", Name = "GetByUserGroupIdAsync")]
        public async Task<IActionResult> GetByUserGroupIdAsync(int userGroupId)
        {
            var userGroup = await _userGroupRepository.FindByIdAsync(userGroupId);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(userGroup);
        }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        [FormatReturnType]
        public async Task<IEnumerable<UserGroupSearchModel>> GetAllAsync([FromUri] Query query)
        {

            var result = await _userGroupRepository.GetAllAsync(query); ;

            return result;
        }


        /// <summary>
        /// Delete data by UserGroupId
        /// </summary>
        /// <param name="userGroupId"></param>
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{userGroupId}")]
        public async Task<IActionResult> DeleteByUserGroupIdAsync(int userGroupId)
        {
            await _userGroupRepository.RemoveByIdAsync(userGroupId);
            return Ok();
        }
    }
}
