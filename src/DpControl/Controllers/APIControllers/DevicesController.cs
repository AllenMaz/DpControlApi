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
    public class DevicesController:BaseAPIController
    {
        [FromServices]
        public IDeviceRepository _deviceRepository { get; set; }
        
        /// <summary>
        /// Search data by DeviceId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(DeviceSearchModel))]
        [HttpGet("{deviceId}", Name = "GetByDeviceIdAsync")]
        public async Task<IActionResult> GetByDeviceIdAsync(int deviceId)
        {
            var device = await _deviceRepository.FindByIdAsync(deviceId);
            if (device == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(device);
        }

        #region Relations
        /// <summary>
        /// Get Locations Relation
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{deviceId}/Locations")]
        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsBySceneIdAsync(int deviceId)
        {
            var locations = await _deviceRepository.GetLocationsByDeviceIdAsync(deviceId);
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
        public async Task<IEnumerable<DeviceSearchModel>> GetAllAsync()
        {

            var result = await _deviceRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] DeviceAddModel mDevice)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var deviceId = await _deviceRepository.AddAsync(mDevice);
            return CreatedAtRoute("GetByDeviceIdAsync", new { controller = "Devices", deviceId = deviceId }, mDevice);
        }

        /// <summary>
        /// Edit data by deviceId
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] DeviceUpdateModel mDevice)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var deviceId = await _deviceRepository.UpdateByIdAsync(id, mDevice);
            return CreatedAtRoute("GetByDeviceIdAsync", new { controller = "Devices", deviceId = deviceId }, mDevice);

        }

        /// <summary>
        /// Delete data by DeviceId
        /// </summary>
        /// <param name="DeviceId"></param>
        [Authorize(Roles = "Admin,Public")]
        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteByDeviceIdAsync(int deviceId)
        {
            await _deviceRepository.RemoveByIdAsync(deviceId);
            return Ok();
        }
    }
}
