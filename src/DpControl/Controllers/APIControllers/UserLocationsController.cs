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
    public class UserLocationsController:BaseAPIController
    {
        [FromServices]
        public IUserLocationRepository _userLocationRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserLocationAddModel mUserLocation)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var userLocationId = await _userLocationRepository.AddAsync(mUserLocation);
            return CreatedAtRoute("GetByUserLocationIdAsync", new { controller = "UserLocations", userLocationId = userLocationId }, mUserLocation);
        }

        /// <summary>
        /// Search data by UserLocationId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(UserLocationSearchModel))]
        [HttpGet("{userLocationId}", Name = "GetByUserLocationIdAsync")]
        public async Task<IActionResult> GetByUserLocationIdAsync(int userLocationId)
        {
            var userGroup = await _userLocationRepository.FindByIdAsync(userLocationId);
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
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<UserLocationSearchModel>> GetAllAsync()
        {

            var result = await _userLocationRepository.GetAllAsync(); ;

            return result;
        }


        /// <summary>
        /// Delete data by UserLocationId
        /// </summary>
        /// <param name="userLocationId"></param>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpDelete("{userLocationId}")]
        public async Task<IActionResult> DeleteByUserLocationIdAsync(int userLocationId)
        {
            await _userLocationRepository.RemoveByIdAsync(userLocationId);
            return Ok();
        }
    }
}
