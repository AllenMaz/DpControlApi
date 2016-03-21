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
    public class LogsController:BaseAPIController
    {
        [FromServices]
        public ILogRepository _logRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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
        /// Search data by LogId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [EnableQuery(true,typeof(LogSearchModel))]
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

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<LogSearchModel>> GetAllAsync([FromUri] Query query)
        {

            var result = await _logRepository.GetAllAsync(query); ;

            return result;
        }


        /// <summary>
        /// Delete data by LogId
        /// </summary>
        /// <param name="logId"></param>
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{logId}")]
        public async Task<IActionResult> DeleteByLogIdAsync(int logId)
        {
            await _logRepository.RemoveByIdAsync(logId);
            return Ok();
        }
        
    }
}
