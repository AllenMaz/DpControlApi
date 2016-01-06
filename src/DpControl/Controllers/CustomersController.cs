
using DpControl.Controllers;
using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Domain.Repository;
using DpControl.Domain.Utility;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpControl.Controllers
{
    
    public class CustomersController: BaseController
    {
        [FromServices]
        public ICustomerRepository _customerRepository { get; set; }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseData<IEnumerable<MCustomer>>> GetAll()
        {
            var customers = await _customerRepository.GetAll();
            var responseData = ResponseUtility.ConstructResponse<IEnumerable<MCustomer>>(customers);
            return responseData;
        }

        /// <summary>
        /// Search data by CustomerNo
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet("{customerNo}")]
        public async Task<IActionResult> GetByCustomerNo(string customerNo)
        {

            var customer = await _customerRepository.Find(customerNo);
            var responseData = ResponseUtility.ConstructResponse<MCustomer>(customer);

            return new ObjectResult(responseData);
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
        /// Edit data by CustomerNo
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut("{customerNo}")]
        public IActionResult Update(string customerNo, [FromBody] Customer customer)
        {

            return new NoContentResult();
        }

        /// <summary>
        /// Delete data by CustomerNo
        /// </summary>
        /// <param name="customerNo"></param>
        [HttpDelete("{customerNo}")]
        public async Task Delete(string customerNo)
        {
            await _customerRepository.Remove(customerNo);

        }
    }
    
}
