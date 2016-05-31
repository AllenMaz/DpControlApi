
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
using DpControl.Domain.Execptions;

namespace DpControl.APIControllers
{
    [Authorize]
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
        /// Get Customer by id
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery(typeof(CustomerSearchModel))]
        [HttpGet("{customerId}", Name = "GetByCustomerById")]
        public IActionResult GetByCustomerById(int customerId)
        {
            var customer = _customerRepository.FindById(customerId);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(customer);
        }

        #region Relations
        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:SuperLevel,CustomerLevel<br/>
        /// Description：根据CustomerId及当前登录用户信息获取该Customer下的所有Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("{customerId}/Projects")]
        [EnableQuery]
        public async Task<IEnumerable<ProjectSubSearchModel>> GetProjectsByCustomerIdAsync(int customerId)
        {
            
            var loginUser = _loginUser.GetLoginUserInfo();
            if (loginUser.isProjectLevel)
                throw new UnauthorizedException();

            var result = await _customerRepository.GetProjectsByCustomerIdAsync(customerId);
            return result;
        }
        #endregion

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:SuperLevel<br/>
        /// Description：根据当前登录用户获取所有Customer
        /// </summary>
        /// <returns></returns>
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
            var loginUser = _loginUser.GetLoginUserInfo();
            if (!loginUser.isSuperLevel)
                throw new UnauthorizedException();

            var result = await _customerRepository.GetAllAsync();

            return result;
        }

        #endregion


        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:SuperLevel<br/>
        /// Description：新增Customer
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CustomerAddModel mCustomer)
        {
            var user = _loginUser.GetLoginUserInfo();
            if (!user.isSuperLevel)
                throw new UnauthorizedException();

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            int customerId = await _customerRepository.AddAsync(mCustomer);
            return CreatedAtRoute("GetByCustomerById", new { controller = "Customers", customerId = customerId }, mCustomer);
        }

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:SuperLevel<br/>
        /// Description：修改Customer
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CustomerUpdateModel mCustomer)
        {
            var user = _loginUser.GetLoginUserInfo();
            if (!user.isSuperLevel)
                throw new UnauthorizedException();

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var customerId = await _customerRepository.UpdateByIdAsync(id,mCustomer);
            return CreatedAtRoute("GetByCustomerById", new { controller = "Customers", customerId = customerId }, mCustomer);

        }

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:SuperLevel<br/>
        /// Description：删除Customer
        /// </summary>
        /// <param name="customerId"></param>
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteByCustomerIdAsync(int customerId)
        {
            var user = _loginUser.GetLoginUserInfo();
            if (!user.isSuperLevel)
                throw new UnauthorizedException();

            await _customerRepository.RemoveByIdAsync(customerId);
            return Ok();
        }
        
    }
}
