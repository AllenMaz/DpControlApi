﻿using DpControl.Domain.IRepository;
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
    public class AlarmsController:BaseAPIController
    {
        [FromServices]
        public IAlarmRepository _alarmRepository { get; set; }
        
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
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
        /// Search data by AlarmId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
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

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<AlarmSearchModel>> GetAllAsync()
        {

            var result = await _alarmRepository.GetAllAsync(); ;

            return result;
        }


        /// <summary>
        /// Delete data by AlarmId
        /// </summary>
        /// <param name="alarmId"></param>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpDelete("{alarmId}")]
        public async Task<IActionResult> DeleteByAlarmIdAsync(int alarmId)
        {
            await _alarmRepository.RemoveByIdAsync(alarmId);
            return Ok();
        }
    }
}
