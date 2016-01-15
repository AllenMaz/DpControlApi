
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

namespace DpControl.APIControllers
{

    public class CustomersController : BaseAPIController
    {
        [FromServices]
        public ICustomerRepository _customerRepository { get; set; }

        [FromServices]
        public IMemoryCache _memoryCache { get; set; }

        [FromServices]
        public IDistributedCache _sqlServerCache { get; set; }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        [FormatReturnType]
        public async Task<IEnumerable<MCustomer>> GetAll([FromUri] Query query)
        {
            string cacheKey = "CustomerGetAllCache";
            IEnumerable<MCustomer> result;

            byte[] cacheResult = await _sqlServerCache.GetAsync(cacheKey);
            if (cacheResult == null)
            {
                //如果没有缓存，则从数据库查询数据，并缓存数据
                result = await _customerRepository.GetAll();

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
                result = JsonHandler.UnJson<IEnumerable<MCustomer>>(cacheResultStr);
                
            }
           
            

            return result;
        }


        /// <summary>
        /// Search data by CustomerNo
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet("{customerNo}",Name = "GetByCustomerNo")]
        public async Task<IActionResult> GetByCustomerNo(string customerNo)
        {

            var customer = await _customerRepository.FindByCustomerNo(customerNo);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(customer);
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MCustomer mCustomer)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(); 
            }

            await _customerRepository.Add(mCustomer);
            return CreatedAtRoute("GetByCustomerNo", new { controller = "Customers", customerNo = mCustomer.CustomerNo }, mCustomer);
        }

        /// <summary>
        /// Edit data by CustomerId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MCustomer mCustomer)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest();
            }
            mCustomer.CustomerId = id;
            await _customerRepository.UpdateById(mCustomer);
            return CreatedAtRoute("GetByCustomerNo", new { controller = "Customers", customerNo = mCustomer.CustomerNo }, mCustomer);

        }

        /// <summary>
        /// Delete data by CustomerNo
        /// </summary>
        /// <param name="customerId"></param>
        [HttpDelete("{customerId}")]
        public async Task DeleteByCustomerId(int customerId)
        {
            await _customerRepository.RemoveById(customerId);

        }
    }
    
}
