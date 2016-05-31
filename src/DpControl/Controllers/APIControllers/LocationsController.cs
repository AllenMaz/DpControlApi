using DpControl.Domain.Entities;
using DpControl.Domain.Execptions;
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
    [Authorize]
    public class LocationsController:BaseAPIController
    {
        [FromServices]
        public ILocationRepository _locationRepository { get; set; }
        
        /// <summary>
        /// Search data by LocationId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
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

        #region Relations
        /// <summary>
        /// Get Project Relation
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [EnableQuery(typeof(ProjectSubSearchModel))]
        [HttpGet("{locationId}/Project")]
        public async Task<IActionResult> GetProjectBySceneIdAsync(int locationId)
        {
            var project = await _locationRepository.GetProjectByLocationIdAsync(locationId);
            if (project == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(project);
        }

        /// <summary>
        /// Get Device Relation
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [EnableQuery(typeof(DeviceSubSearchModel))]
        [HttpGet("{locationId}/Device")]
        public async Task<IActionResult> GetDeviceBySceneIdAsync(int locationId)
        {
            var device = await _locationRepository.GetDeviceByLocationIdAsync(locationId);
            if (device == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(device);
        }

        /// <summary>
        /// Get Groups Relation
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{locationId}/Groups")]
        public async Task<IEnumerable<GroupSubSearchModel>> GetGroupsBySceneIdAsync(int locationId)
        {
            var groups = await _locationRepository.GetGroupsByLocationIdAsync(locationId);
            return groups;
        }

        /// <summary>
        /// Get Logs Relation
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{locationId}/Logs")]
        public async Task<IEnumerable<LogSubSearchModel>> GetLogsBySceneIdAsync(int locationId)
        {
            var logs = await _locationRepository.GetLogsByLocationIdAsync(locationId);
            return logs;
        }

        /// <summary>
        /// Get Alarms Relation
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{locationId}/Alarms")]
        public async Task<IEnumerable<AlarmSubSearchModel>> GetAlarmsBySceneIdAsync(int locationId)
        {
            var alarms = await _locationRepository.GetAlarmsByLocationIdAsync(locationId);
            return alarms;
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<LocationSearchModel>> GetAllAsync()
        {

            var result = await _locationRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] LocationAddModel mLocation)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation), mLocation.Orientation))
            {
                string orientationUrl = CreateCustomUrl("GetOrientation", new { controller = "Utilities" });
                throw new ExpectException("Invalid Orientation. ref :" + orientationUrl);
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mLocation.DeviceType))
            {
                string deviceTypeUrl = CreateCustomUrl("GetDeviceType", new { controller = "Utilities" });
                throw new ExpectException("Invalid DeviceType. ref :" + deviceTypeUrl);
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mLocation.CommMode))
            {
                string commModeUrl = CreateCustomUrl("GetCommMode", new { controller = "Utilities" });
                throw new ExpectException("Invalid CommMode. ref :" + commModeUrl);
            }


            var locationId = await _locationRepository.AddAsync(mLocation);
            return CreatedAtRoute("GetByLocationIdAsync", new { controller = "Locations", locationId = locationId }, mLocation);
        }

        /// <summary>
        /// Edit data by locationId
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] LocationUpdateModel mLocation)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation), mLocation.Orientation))
            {
                string orientationUrl = CreateCustomUrl("GetOrientation", new { controller = "Utilities" });
                throw new ExpectException("Invalid Orientation. ref :" + orientationUrl);
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mLocation.DeviceType))
            {
                string deviceTypeUrl = CreateCustomUrl("GetDeviceType", new { controller = "Utilities" });
                throw new ExpectException("Invalid DeviceType. ref :" + deviceTypeUrl);
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mLocation.CommMode))
            {
                string commModeUrl = CreateCustomUrl("GetCommMode", new { controller = "Utilities" });
                throw new ExpectException("Invalid CommMode. ref :" + commModeUrl);
            }

            var locationId = await _locationRepository.UpdateByIdAsync(id, mLocation);
            return CreatedAtRoute("GetByLocationIdAsync", new { controller = "Locations", locationId = locationId }, mLocation);

        }

        /// <summary>
        /// Delete data by LocationId
        /// </summary>
        /// <param name="LocationId"></param>
        [HttpDelete("{locationId}")]
        public async Task<IActionResult> DeleteByLocationIdAsync(int locationId)
        {
            await _locationRepository.RemoveByIdAsync(locationId);
            return Ok();
        }

        /// <summary>
        /// Create RelationShips:Users、Groups
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [HttpPost("{locationId}/{navigationProperty}")]
        public async Task<IActionResult> CreateRelationsAsync(int locationId, string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if (navigationPropertyIds == null || navigationPropertyIds.Count == 0)
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();
            await _locationRepository.CreateRelationsAsync(locationId, navigationProperty, uniqueNavigationPropertyIds);

            string returnUrl = CreateCustomUrl("GetByLocationIdAsync",
                new { controller = "Locations", locationId = locationId },
                "?expand=" + navigationProperty);

            return Created(returnUrl, null);

        }

        /// <summary>
        /// Remove RelationShips:Users、Groups
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [HttpDelete("{locationId}/{navigationProperty}")]
        public async Task<IActionResult> RemoveRelationsAsync(int locationId, string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if (navigationPropertyIds == null || navigationPropertyIds.Count == 0)
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();

            await _locationRepository.RemoveRelationsAsync(locationId, navigationProperty, uniqueNavigationPropertyIds);
            return Ok();
        }
    }
}
