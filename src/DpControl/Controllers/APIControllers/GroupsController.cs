using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility.Authentication;
using DpControl.Utility.Authorization;
using DpControl.Utility.Filters;
using Microsoft.AspNet.Authorization;
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
        /// Search data by GroupId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(GroupSearchModel))]
        [HttpGet("{groupId}", Name = "GetByGroupIdAsync")]
        public async Task<IActionResult> GetByGroupIdAsync(int groupId)
        {
            var group = await _groupRepository.FindByIdAsync(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(group);
        }

        #region Relations
        /// <summary>
        /// Get Project Relation
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(ProjectSubSearchModel))]
        [HttpGet("{groupId}/Project")]
        public async Task<IActionResult> GetProjectByGroupIdAsync(int groupId)
        {
            var project = await _groupRepository.GetProjectByGroupIdAsync(groupId);
            if (project == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(project);
        }

        /// <summary>
        /// Get Scene Relation
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(SceneSubSearchModel))]
        [HttpGet("{groupId}/Scene")]
        public async Task<IActionResult> GetSceneByGroupIdAsync(int groupId)
        {
            var scene = await _groupRepository.GetSceneByGroupIdAsync(groupId);
            if (scene == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(scene);
        }

        /// <summary>
        /// Get Locations Relation
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{groupId}/Locations")]
        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsByGroupIdAsync(int groupId)
        {
            var locations = await _groupRepository.GetLocationsByGroupIdAsync(groupId);
            return locations;
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<GroupSearchModel>> GetAllAsync()
        {

            var result = await _groupRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
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
        /// Edit data by GroupId
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="Group"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
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
        [Authorize(Roles = "Admin,Public")]
        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteByGroupIdIdAsync(int groupId)
        {
            await _groupRepository.RemoveByIdAsync(groupId);
            return Ok();
        }

        /// <summary>
        /// Create RelationShips:Users、Locations
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [HttpPost("{groupId}/{navigationProperty}")]
        public async Task<IActionResult> CreateRelationsAsync(int groupId, string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if (navigationPropertyIds == null || navigationPropertyIds.Count == 0)
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();
            await _groupRepository.CreateRelationsAsync(groupId, navigationProperty, uniqueNavigationPropertyIds);

            string returnUrl = CreateCustomUrl("GetByGroupIdAsync",
                new { controller = "Groups", groupId = groupId },
                "?expand=" + navigationProperty);

            return Created(returnUrl, null);

        }

        /// <summary>
        /// Remove RelationShips:Users、Locations
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [HttpDelete("{groupId}/{navigationProperty}")]
        public async Task<IActionResult> RemoveRelationsAsync(int groupId, string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if (navigationPropertyIds == null || navigationPropertyIds.Count == 0)
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();

            await _groupRepository.RemoveRelationsAsync(groupId, navigationProperty, uniqueNavigationPropertyIds);
            return Ok();
        }
    }
}
