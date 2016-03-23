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
    public class AlarmMessagesController:BaseAPIController
    {
        [FromServices]
        public IAlarmMessageRepository _alarmMessageRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AlarmMessageAddModel mAlarmMessage)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var alarmMessageId = await _alarmMessageRepository.AddAsync(mAlarmMessage);
            return CreatedAtRoute("GetByAlarmMessageIdAsync", new { controller = "AlarmMessages", alarmMessageId = alarmMessageId }, mAlarmMessage);
        }

        /// <summary>
        /// Search data by AlarmMessageId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [EnableQuery(typeof(AlarmMessageSearchModel))]
        [HttpGet("{alarmMessageId}", Name = "GetByAlarmMessageIdAsync")]
        public async Task<IActionResult> GetByAlarmMessageIdAsync(int alarmMessageId)
        {
            var alarmMessage = await _alarmMessageRepository.FindByIdAsync(alarmMessageId);
            if (alarmMessage == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(alarmMessage);
        }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<AlarmMessageSearchModel>> GetAllAsync()
        {

            var result = await _alarmMessageRepository.GetAllAsync();

            return result;
        }


        /// <summary>
        /// Delete data by AlarmMessageId
        /// </summary>
        /// <param name="alarmMessageId"></param>
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{alarmMessageId}")]
        public async Task<IActionResult> DeleteByAlarmMessageIdAsync(int alarmMessageId)
        {
            await _alarmMessageRepository.RemoveByIdAsync(alarmMessageId);
            return Ok();
        }

        /// <summary>
        /// Edit data by AlarmMessageId
        /// </summary>
        /// <param name="AlarmMessageId"></param>
        /// <param name="AlarmMessage"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AlarmMessageUpdateModel mAlarmMessage)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var alarmMessageId = await _alarmMessageRepository.UpdateByIdAsync(id, mAlarmMessage);
            return CreatedAtRoute("GetByAlarmMessageIdAsync", new { controller = "AlarmMessages", alarmMessageId = alarmMessageId }, mAlarmMessage);


        }
    }
}
