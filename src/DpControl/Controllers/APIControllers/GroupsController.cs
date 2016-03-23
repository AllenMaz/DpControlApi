using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility.Authentication;
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
    public class GroupsController:BaseAPIController
    {
        [FromServices]
        public IGroupRepository _groupRepository { get; set; }
        

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] GroupAddModel mGroup)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }
            
            var groupId = await _groupRepository.AddAsync(mGroup);
            return CreatedAtRoute("GetByGroupIdAsync", new { controller = "Groups", groupId = groupId }, mGroup);
        }

        /// <summary>
        /// Search data by GroupId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(GroupSearchModel))]
        [HttpGet("{groupId}", Name = "GetByGroupIdAsync")]
        public async Task<IActionResult> GetByProjectIdAsync(int groupId)
        {
            var group = await _groupRepository.FindByIdAsync(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(group);
        }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<GroupSearchModel>> GetAllAsync()
        {

            var result = await _groupRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Edit data by GroupId
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="Group"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateAsync(int groupId, [FromBody] GroupUpdateModel mGroup)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var projectId = await _groupRepository.UpdateByIdAsync(groupId, mGroup);
            return CreatedAtRoute("GetByGroupIdAsync", new { controller = "Groups", groupId = groupId }, mGroup);

        }

        /// <summary>
        /// Delete data by GroupId
        /// </summary>
        /// <param name="groupId"></param>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteByGroupIdIdAsync(int groupId)
        {
            await _groupRepository.RemoveByIdAsync(groupId);
            return Ok();
        }
    }
}
