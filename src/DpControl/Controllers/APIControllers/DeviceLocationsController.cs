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
    public class DeviceLocationsController:BaseAPIController
    {
        [FromServices]
        public IDeviceLocationRepository _deviceLocationRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] DeviceLocationAddModel mDeviceLocation)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var deviceLocationId = await _deviceLocationRepository.AddAsync(mDeviceLocation);
            return CreatedAtRoute("GetByDeviceLocationIdAsync", new { controller = "DeviceLocations", deviceLocationId = deviceLocationId }, mDeviceLocation);
        }

        /// <summary>
        /// Search data by DeviceLocationId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet("{deviceLocationId}", Name = "GetByDeviceLocationIdAsync")]
        public async Task<IActionResult> GetBySceneIdAsync(int deviceLocationId)
        {
            var deviceLocation = await _deviceLocationRepository.FindByIdAsync(deviceLocationId);
            if (deviceLocation == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(deviceLocation);
        }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        [FormatReturnType]
        public async Task<IEnumerable<DeviceLocationSearchModel>> GetAllAsync([FromUri] Query query)
        {

            var result = await _deviceLocationRepository.GetAllAsync(query); ;

            return result;
        }

        /// <summary>
        /// Edit data by deviceLocationId
        /// </summary>
        /// <param name="deviceLocationId"></param>
        /// <param name="DeviceLocation"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] DeviceLocationUpdateModel mDeviceLocation)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var deviceLocationId = await _deviceLocationRepository.UpdateByIdAsync(id, mDeviceLocation);
            return CreatedAtRoute("GetByDeviceLocationIdAsync", new { controller = "DeviceLocations", deviceLocationId = deviceLocationId }, mDeviceLocation);

        }

        /// <summary>
        /// Delete data by DeviceLocationId
        /// </summary>
        /// <param name="DeviceLocationId"></param>
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{deviceLocationId}")]
        public async Task<IActionResult> DeleteByProjectIdIdAsync(int deviceLocationId)
        {
            await _deviceLocationRepository.RemoveByIdAsync(deviceLocationId);
            return Ok();
        }
    }
}
