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
    public class LogsController:BaseAPIController
    {
        [FromServices]
        public ILogRepository _logRepository { get; set; }
        
        /// <summary>
        /// Search data by LogId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(LogSearchModel))]
        [HttpGet("{logId}", Name = "GetByLogIdAsync")]
        public async Task<IActionResult> GetByLogIdAsync(int logId)
        {
            var log = await _logRepository.FindByIdAsync(logId);
            if (log == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(log);
        }

        #region Relations
        /// <summary>
        /// Get Location Relation
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(LocationSubSearchModel))]
        [HttpGet("{logId}/Location")]
        public async Task<IActionResult> GetLocationByLogIdAsync(int logId)
        {
            var location = await _logRepository.GetLocationByLogIdAsync(logId);
            if (location == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(location);
        }

        /// <summary>
        /// Get LogDescription Relation
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(LogDescriptionSubSearchModel))]
        [HttpGet("{logId}/LogDescription")]
        public async Task<IActionResult> GetLogDescriptionByLogIdAsync(int logId)
        {
            var logDescription = await _logRepository.GetLogDescriptionByLogIdAsync(logId);
            if (logDescription == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(logDescription);
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<LogSearchModel>> GetAllAsync()
        {

            var result = await _logRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] LogAddModel mLog)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var logId = await _logRepository.AddAsync(mLog);
            return CreatedAtRoute("GetByLogIdAsync", new { controller = "Logs", logId = logId }, mLog);
        }

        /// <summary>
        /// Delete data by LogId
        /// </summary>
        /// <param name="logId"></param>
        [Authorize(Roles = "Admin,Public")]
        [HttpDelete("{logId}")]
        public async Task<IActionResult> DeleteByLogIdAsync(int logId)
        {
            await _logRepository.RemoveByIdAsync(logId);
            return Ok();
        }
        
    }
}
