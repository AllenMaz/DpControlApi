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
    public class LogDescriptionsController:BaseAPIController
    {
        [FromServices]
        public ILogDescriptionRepository _logDescriptionRepository { get; set; }

        
        /// <summary>
        /// Search data by LogDescriptionId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery(typeof(LogDescriptionSearchModel))]
        [HttpGet("{logDescriptionId}", Name = "GetByLogDescriptionIdAsync")]
        public async Task<IActionResult> GetByLogDescriptionIdAsync(int logDescriptionId)
        {
            var logDescription = await _logDescriptionRepository.FindByIdAsync(logDescriptionId);
            if (logDescription == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(logDescription);
        }

        #region Relations
        /// <summary>
        /// Get Logs Relation
        /// </summary>
        /// <param name="logDescriptionId"></param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{logDescriptionId}/Logs")]
        public async Task<IEnumerable<LogSubSearchModel>> GetLogsByLogDescriptionIdAsync(int logDescriptionId)
        {
            var logs = await _logDescriptionRepository.GetLogsByLogDescriptionIdAsync(logDescriptionId);
            return logs;
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[EnableQuery]
        //public async Task<IEnumerable<LogDescriptionSearchModel>> GetAllAsync()
        //{

        //    var result = await _logDescriptionRepository.GetAllAsync(); ;

        //    return result;
        //}

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] LogDescriptionAddModel mLogDescription)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var logDescriptionId = await _logDescriptionRepository.AddAsync(mLogDescription);
            return CreatedAtRoute("GetByLogDescriptionIdAsync", new { controller = "LogDescriptions", logDescriptionId = logDescriptionId }, mLogDescription);
        }

        /// <summary>
        /// Delete data by LogDescriptionId
        /// </summary>
        /// <param name="logDescriptionId"></param>
        [HttpDelete("{logDescriptionId}")]
        public async Task<IActionResult> DeleteByLogDescriptionIdAsync(int logDescriptionId)
        {
            await _logDescriptionRepository.RemoveByIdAsync(logDescriptionId);
            return Ok();
        }

        /// <summary>
        /// Edit data by LogDescriptionId
        /// </summary>
        /// <param name="LogDescriptionId"></param>
        /// <param name="LogDescription"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] LogDescriptionUpdateModel mLogDescription)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var logDescriptionId = await _logDescriptionRepository.UpdateByIdAsync(id, mLogDescription);
            return CreatedAtRoute("GetByLogDescriptionIdAsync", new { controller = "LogDescriptions", logDescriptionId = logDescriptionId }, mLogDescription);


        }
    }
}
