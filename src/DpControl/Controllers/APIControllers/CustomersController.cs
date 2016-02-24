
using DpControl.Controllers;
using DpControl.Utility.Filters;
using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Domain.Repository;
using DpControl.Models;
using DpControl.Utility;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNet.Authorization;
using DpControl.Utility.Authorization;

namespace DpControl.APIControllers
{
   
    public class CustomersController : BaseAPIController
    {
        [FromServices]
        public ICustomerRepository _customerRepository { get; set; }

        [FromServices]
        public IDistributedCache _sqlServerCache { get; set; }



        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CustomerAddModel mCustomer)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            await _customerRepository.AddAsync(mCustomer);
            return CreatedAtRoute("GetByCustomerNoAsync", new { controller = "Customers", customerNo = mCustomer.CustomerNo }, mCustomer);
        }

        /// <summary>
        /// Search data by CustomerNo
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet("{customerNo}", Name = "GetByCustomerNoAsync")]
        public async Task<IActionResult> GetByCustomerNoAsync(string customerNo)
        {
            if (string.IsNullOrEmpty(customerNo))
            {
               return HttpBadRequest(ResponseHandler.ReturnBadRequestError("CustomerNo can't be empty"));

            }

            var customer = await _customerRepository.FindByCustomerNoAsync(customerNo);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(customer);
        }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        [FormatReturnType]
        public async Task<IEnumerable<CustomerSearchModel>> GetAllAsync([FromUri] Query query)
        {
            string cacheKey = "CustomerGetAllCache";
            IEnumerable<CustomerSearchModel> result;

            byte[] cacheResult = await _sqlServerCache.GetAsync(cacheKey);
            if (cacheResult == null)
            {
                //如果没有缓存，则从数据库查询数据，并缓存数据
                result = await _customerRepository.GetAllAsync();

                string jsonResult = JsonHandler.ToJson(result);
                var value = Encoding.UTF8.GetBytes(jsonResult);
                await _sqlServerCache.SetAsync(
                    cacheKey,
                    value,
                    new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(10))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }
            else
            {
                //如果有缓存，则直接返回缓存数据
                string cacheResultStr = Encoding.UTF8.GetString(cacheResult);
                result = JsonHandler.UnJson<IEnumerable<CustomerSearchModel>>(cacheResultStr);

            }



            return result;
        }

        /// <summary>
        /// Edit data by CustomerId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CustomerUpdateModel mCustomer)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            await _customerRepository.UpdateByIdAsync(id,mCustomer);
            return CreatedAtRoute("GetByCustomerNoAsync", new { controller = "Customers", customerNo = mCustomer.CustomerNo }, mCustomer);

        }

        /// <summary>
        /// Delete data by CustomerNo
        /// </summary>
        /// <param name="customerId"></param>
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteByCustomerIdAsync(int customerId)
        {
            await _customerRepository.RemoveByIdAsync(customerId);
            return Ok();
        }
    }
}
