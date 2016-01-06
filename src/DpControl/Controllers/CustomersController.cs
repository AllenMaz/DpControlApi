
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
    [Route("v1/[controller]")]
    public class CustomersController: BaseController
    {
        [FromServices]
        public ICustomerRepository _customerRepository { get; set; }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Customer>> GetAll()
        {
            var customers = await _customerRepository.GetAll();
            if (customers.Count() ==0)
            {
                //Response.StatusCode = 111;
                var data = Encoding.UTF8.GetBytes("没有数据");
                await Response.Body.WriteAsync(data, 0, data.Length);
            }

            return customers;
        }

        /// <summary>
        /// 根据CustomerNo获取数据
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
        /// 新增数据
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
    }
}
