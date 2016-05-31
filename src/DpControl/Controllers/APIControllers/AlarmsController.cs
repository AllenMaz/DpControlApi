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
    public class AlarmsController:BaseAPIController
    {
        [FromServices]
        public IAlarmRepository _alarmRepository { get; set; }
        
        /// <summary>
        /// Search data by AlarmId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery(typeof(AlarmSearchModel))]
        [HttpGet("{alarmId}", Name = "GetByAlarmIdAsync")]
        public async Task<IActionResult> GetByAlarmIdAsync(int alarmId)
        {
            var alarm = await _alarmRepository.FindByIdAsync(alarmId);
            if (alarm == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(alarm);
        }

        #region Relations
        /// <summary>
        /// Get Location Relation
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        [EnableQuery(typeof(LocationSubSearchModel))]
        [HttpGet("{alarmId}/Location")]
        public async Task<IActionResult> GetLocationByAlarmIdAsync(int alarmId)
        {
            var location = await _alarmRepository.GetLocationByAlarmIdAsync(alarmId);
            if (location == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(location);
        }

        /// <summary>
        /// Get AlarmMessage Relation
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        [EnableQuery(typeof(AlarmMessageSubSearchModel))]
        [HttpGet("{alarmId}/AlarmMessage")]
        public async Task<IActionResult> GetAlarmMessageByAlarmIdAsync(int alarmId)
        {
            var alarmMessage = await _alarmRepository.GetAlarmMessageByAlarmIdAsync(alarmId);
            if (alarmMessage == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(alarmMessage);
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[EnableQuery]
        //public async Task<IEnumerable<AlarmSearchModel>> GetAllAsync()
        //{

        //    var result = await _alarmRepository.GetAllAsync(); ;

        //    return result;
        //}

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AlarmAddModel mAlarm)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var alarmId = await _alarmRepository.AddAsync(mAlarm);
            
            return CreatedAtRoute("GetByAlarmIdAsync", new { controller = "Alarms", alarmId = alarmId }, mAlarm);
        }

        /// <summary>
        /// Delete data by AlarmId
        /// </summary>
        /// <param name="alarmId"></param>
        [HttpDelete("{alarmId}")]
        public async Task<IActionResult> DeleteByAlarmIdAsync(int alarmId)
        {
            await _alarmRepository.RemoveByIdAsync(alarmId);
            return Ok();
        }
    }
}
