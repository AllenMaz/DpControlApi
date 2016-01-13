
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

namespace DpControl.APIControllers
{
    
    public class CustomersController: BaseAPIController
    {
        [FromServices]
        public ICustomerRepository _customerRepository { get; set; }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        [TypeFilter(typeof(StandardReturnType),Arguments = new object[] { Common.ActionReturnType_GetList})]
        public async Task<IEnumerable<MCustomer>> GetAll([FromUri] Query query)
        {
            var customers = await _customerRepository.GetAll();
            return customers;
        }
        

        /// <summary>
        /// Search data by CustomerNo
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet("{customerNo}",Name = "GetByCustomerNo")]
        [TypeFilter(typeof(StandardReturnType), Arguments = new object[] { Common.ActionReturnType_GetSingle })]
        public async Task<IActionResult> GetByCustomerNo(string customerNo)
        {

            var customer = await _customerRepository.Find(customerNo);
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
        [TypeFilter(typeof(StandardReturnType), Arguments = new object[] { Common.ActionReturnType_Post })]
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
        [TypeFilter(typeof(StandardReturnType), Arguments = new object[] { Common.ActionReturnType_Put })]
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
            await _customerRepository.Remove(customerId);

        }
    }
    
}
