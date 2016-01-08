
using DpControl.Controllers;
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
        public async Task<ResponseMessage<IEnumerable<MCustomer>>> GetAll()
        {
            var customers = await _customerRepository.GetAll();
            var responseData = ResponseHandler.ConstructResponse<IEnumerable<MCustomer>>(customers);
            return responseData;
        }

        /// <summary>
        /// Search data by CustomerNo
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet("{customerNo}",Name = "GetByCustomerNo")]
        public async Task<IActionResult> GetByCustomerNo(string customerNo)
        {

            var customer = await _customerRepository.Find(customerNo);
            if (customer == null)
            {
                return HttpNotFound();
            }
            var responseData = ResponseHandler.ConstructResponse<MCustomer>(customer);

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
            var responseData = ResponseHandler.ConstructResponse<MCustomer>(mCustomer);
            return CreatedAtRoute("GetByCustomerNo", new { controller = "Customers", customerNo = mCustomer.CustomerNo }, responseData);
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
            var responseData = ResponseHandler.ConstructResponse<MCustomer>(mCustomer);
            return CreatedAtRoute("GetByCustomerNo", new { controller = "Customers", customerNo = mCustomer.CustomerNo }, responseData);

        }

        /// <summary>
        /// Delete data by CustomerNo
        /// </summary>
        /// <param name="customerId"></param>
        [HttpDelete("{customerId}")]
        public async Task DeleteByCustomerNo(int customerId)
        {
            await _customerRepository.Remove(customerId);

        }
    }
    
}
