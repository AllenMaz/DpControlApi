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
    public class DevicesController:BaseAPIController
    {
        [FromServices]
        public IDeviceRepository _deviceRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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
        /// Search data by DeviceId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [EnableQuery(typeof(DeviceSearchModel))]
        [HttpGet("{deviceId}", Name = "GetByDeviceIdAsync")]
        public async Task<IActionResult> GetBySceneIdAsync(int deviceId)
        {
            var deviceLocation = await _deviceRepository.FindByIdAsync(deviceId);
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
        public async Task<IEnumerable<DeviceSearchModel>> GetAllAsync([FromUri] Query query)
        {

            var result = await _deviceRepository.GetAllAsync(query); ;

            return result;
        }

        /// <summary>
        /// Edit data by deviceId
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteByDeviceIdAsync(int deviceId)
        {
            await _deviceRepository.RemoveByIdAsync(deviceId);
            return Ok();
        }
    }
}
