
using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using DpControl.Domain.IRepository;
using DpControl.Domain.Repository;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpControl.Controllers
{
    
    public class CustomersController: BaseV1Controller
    {
        [FromServices]
        public ICustomerRepository _customerRepository { get; set; }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Customer>> GetAll()
        {
            var customers = await _customerRepository.GetAll();
            
            return customers;
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
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(); 
            }

            await _customerRepository.Add(customer);
                
            return CreatedAtRoute("GetByCustomerNo", new { controller = "Customers", customerNo = customer.CustomerNo }, customer);
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
