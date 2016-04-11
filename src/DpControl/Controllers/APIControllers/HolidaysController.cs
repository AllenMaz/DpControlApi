using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility.Authentication;
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
    public class HolidaysController:BaseAPIController
    {
        [FromServices]
        public IHolidayRepository _holidayRepository { get; set; }

        
        /// <summary>
        /// Search data by holidayId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(HolidaySearchModel))]
        [HttpGet("{holidayId}", Name = "GetByHolidayIdAsync")]
        public async Task<IActionResult> GetByHolidayIdAsync(int holidayId)
        {
            var holiday = await _holidayRepository.FindByIdAsync(holidayId);
            if (holiday == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(holiday);
        }

        #region Relations
        /// <summary>
        /// Get Project Relation
        /// </summary>
        /// <param name="holidayId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(ProjectSubSearchModel))]
        [HttpGet("{holidayId}/Project")]
        public async Task<IActionResult> GetProjectByHolidayIdAsync(int holidayId)
        {
            var project = await _holidayRepository.GetProjectByHolidayIdAsync(holidayId);
            if (project == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(project);
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<HolidaySearchModel>> GetAllAsync()
        {

            var result = await _holidayRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] HolidayAddModel mHoliday)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var holidayId = await _holidayRepository.AddAsync(mHoliday);
            return CreatedAtRoute("GetByHolidayIdAsync", new { controller = "Holidays", holidayId = holidayId }, mHoliday);
        }

        /// <summary>
        /// Edit data by holidayId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpPut("{holidayId}")]
        public async Task<IActionResult> UpdateAsync(int holidayId, [FromBody] HolidayUpdateModel mHoliday)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var id = await _holidayRepository.UpdateByIdAsync(holidayId, mHoliday);
            return CreatedAtRoute("GetByHolidayIdAsync", new { controller = "Holidays", holidayId = id }, mHoliday);

        }

        /// <summary>
        /// Delete data by HolidayId
        /// </summary>
        /// <param name="customerId"></param>
        [Authorize(Roles = "Admin,Public")]
        [HttpDelete("{holidayId}")]
        public async Task<IActionResult> DeleteByHolidayIdAsync(int holidayId)
        {
            await _holidayRepository.RemoveByIdAsync(holidayId);
            return Ok();
        }
    }
}
