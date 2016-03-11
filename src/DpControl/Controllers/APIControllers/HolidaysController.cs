using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility.Authentication;
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
    public class HolidaysController:BaseAPIController
    {
        [FromServices]
        public IHolidayRepository _holidayRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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
        /// Search data by holidayId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        [FormatReturnType]
        public async Task<IEnumerable<HolidaySearchModel>> GetAllAsync([FromUri] Query query)
        {

            var result = await _holidayRepository.GetAllAsync(query); ;

            return result;
        }

        /// <summary>
        /// Edit data by holidayId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{holidayId}")]
        public async Task<IActionResult> DeleteByHolidayIdAsync(int holidayId)
        {
            await _holidayRepository.RemoveByIdAsync(holidayId);
            return Ok();
        }
    }
}
