
using DpControl.Controllers;
using DpControl.Utility.Filters;
using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using DpControl.Utility.Authorization;
using Microsoft.AspNet.Authorization;
using DpControl.Utility.Authentication;
using Microsoft.AspNet.Cors;

namespace DpControl.APIControllers
{
    public class CustomersController : BaseAPIController
    {
        [FromServices]
        public ICustomerRepository _customerRepository { get; set; }
        
        private ILoginUserRepository _loginUser;

        public CustomersController(ILoginUserRepository loginUser)
        {
            _loginUser = loginUser;
        }

        #region GET
        /// <summary>
        /// Search data by CustomerId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Authorize]
        [EnableQuery(typeof(CustomerSearchModel))]
        [HttpGet("{customerId}", Name = "GetByCustomerIdAsync")]
        public async Task<IActionResult> GetByCustomerIdAsync(int customerId)
        {
            var customer = await _customerRepository.FindByIdAsync(customerId);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(customer);
        }

        #region Relations
        /// <summary>
        /// Get Related Projects
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{customerId}/Projects")]
        [EnableQuery]
        public async Task<IEnumerable<ProjectSubSearchModel>> GetProjectsByCustomerIdAsync(int customerId)
        {
            var result = await _customerRepository.GetProjectsByCustomerIdAsync(customerId);
            return result;
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<CustomerSearchModel>> GetAllAsync()
        {
            //string cacheKey = "CustomerGetAllCache";
            //IEnumerable<CustomerSearchModel> result;

            //byte[] cacheResult = await _sqlServerCache.GetAsync(cacheKey);
            //if (cacheResult == null)
            //{
            //    //如果没有缓存，则从数据库查询数据，并缓存数据
            //    result = await _customerRepository.GetAllAsync(query);

            //    string jsonResult = JsonHandler.ToJson(result);
            //    var value = Encoding.UTF8.GetBytes(jsonResult);
            //    await _sqlServerCache.SetAsync(
            //        cacheKey,
            //        value,
            //        new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(10))
            //        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            //}
            //else
            //{
            //    //如果有缓存，则直接返回缓存数据
            //    string cacheResultStr = Encoding.UTF8.GetString(cacheResult);
            //    result = JsonHandler.UnJson<IEnumerable<CustomerSearchModel>>(cacheResultStr);

            //}

            
            var user = _loginUser.GetLoginUserInfo();
            if (user.hasCustomerNo)
            {
                //filter by CustomerNo
                Filter customerNoFilter = new Filter();
                var filterPropertyValue = new Dictionary<string, string>();
                filterPropertyValue.Add("CustomerNo", user.CustomerNo);
                customerNoFilter.FilterPropertyValue = filterPropertyValue;
                customerNoFilter.CompareOperator = FilterOperators.Equal;
                customerNoFilter.LogicalOperator = FilterOperators.And;
                Query.AddFilterCondition(customerNoFilter);
            }

            var result = await _customerRepository.GetAllAsync();

            return result;
        }

        #endregion

        
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin )]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CustomerAddModel mCustomer)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            int customerId = await _customerRepository.AddAsync(mCustomer);
            return CreatedAtRoute("GetByCustomerIdAsync", new { controller = "Customers", customerId = customerId }, mCustomer);
        }

        /// <summary>
        /// Edit data by CustomerId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CustomerUpdateModel mCustomer)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var customerId = await _customerRepository.UpdateByIdAsync(id,mCustomer);
            return CreatedAtRoute("GetByCustomerIdAsync", new { controller = "Customers", customerId = customerId }, mCustomer);

        }

        /// <summary>
        /// Delete data by CustomerNo
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        /// <param name="customerId"></param>
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteByCustomerIdAsync(int customerId)
        {
            await _customerRepository.RemoveByIdAsync(customerId);
            return Ok();
        }
    }
}
