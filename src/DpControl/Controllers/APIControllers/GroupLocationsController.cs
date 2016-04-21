using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
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
    public class GroupLocationsController:BaseAPIController
    {
        [FromServices]
        public IGroupLocationRepository _groupLocationRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] GroupLocationAddModel mGroupLocation)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var groupLocationId = await _groupLocationRepository.AddAsync(mGroupLocation);
            return CreatedAtRoute("GetByGroupLocationIdAsync", new { controller = "GroupLocations", groupLocationId = groupLocationId }, mGroupLocation);
        }

        /// <summary>
        /// Search data by GroupLocationId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(GroupLocationSearchModel))]
        [HttpGet("{groupLocationId}", Name = "GetByGroupLocationIdAsync")]
        public async Task<IActionResult> GetByGroupLocationIdAsync(int groupLocationId)
        {
            var groupLocation = await _groupLocationRepository.FindByIdAsync(groupLocationId);
            if (groupLocation == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(groupLocation);
        }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<GroupLocationSearchModel>> GetAllAsync()
        {

            var result = await _groupLocationRepository.GetAllAsync(); ;

            return result;
        }


        /// <summary>
        /// Delete data by GroupLocationId
        /// </summary>
        /// <param name="groupLocationId"></param>
        [Authorize(Roles = "Admin,Public")]
        [HttpDelete("{groupLocationId}")]
        public async Task<IActionResult> DeleteByGroupLocationIdAsync(int groupLocationId)
        {
            await _groupLocationRepository.RemoveByIdAsync(groupLocationId);
            return Ok();
        }
    }
}
