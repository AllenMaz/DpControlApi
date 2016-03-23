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
    public class LocationsController:BaseAPIController
    {
        [FromServices]
        public ILocationRepository _locationRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] LocationAddModel mLocation)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var locationId = await _locationRepository.AddAsync(mLocation);
            return CreatedAtRoute("GetByLocationIdAsync", new { controller = "Locations", locationId = locationId }, mLocation);
        }

        /// <summary>
        /// Search data by LocationId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(LocationSearchModel))]
        [HttpGet("{locationId}", Name = "GetByLocationIdAsync")]
        public async Task<IActionResult> GetBySceneIdAsync(int locationId)
        {
            var location = await _locationRepository.FindByIdAsync(locationId);
            if (location == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(location);
        }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<LocationSearchModel>> GetAllAsync()
        {

            var result = await _locationRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Edit data by locationId
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] LocationUpdateModel mLocation)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var locationId = await _locationRepository.UpdateByIdAsync(id, mLocation);
            return CreatedAtRoute("GetByLocationIdAsync", new { controller = "Locations", locationId = locationId }, mLocation);

        }

        /// <summary>
        /// Delete data by LocationId
        /// </summary>
        /// <param name="LocationId"></param>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpDelete("{locationId}")]
        public async Task<IActionResult> DeleteByLocationIdAsync(int locationId)
        {
            await _locationRepository.RemoveByIdAsync(locationId);
            return Ok();
        }
    }
}
