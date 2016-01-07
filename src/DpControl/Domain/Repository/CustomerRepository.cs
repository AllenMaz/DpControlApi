using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using DpControl.Controllers.ExceptionHandler;
using DpControl.Domain.Models;
//using Microsoft.Extensions.DependencyInjection;


namespace DpControl.Domain.Repository
{
    public class CustomerRepository : ICustomerRepository
    {

        private ShadingContext _dbContext;

        #region 构造函数
        public CustomerRepository()
        {
        }

        public CustomerRepository(ShadingContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        public async Task<IEnumerable<MCustomer>> GetAll()
        {
            
            //using (var context = new ShadingContext())
            //{
            //    var AAA = await context.Customers.ToListAsync<Customer>();
            //}
                var customers = await _dbContext.Customers.Select(c => new MCustomer
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerNo = c.CustomerNo,
                ProjectName = c.ProjectName,
                ProjectNo = c.ProjectNo
            })
            .OrderBy(c => c.CustomerNo)
            .ToListAsync<MCustomer>();


            return customers;
        }
        public async Task<MCustomer> Find(string customerNo)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerNo == customerNo);
            if (customer == null)
                throw new KeyNotFoundException();
            return new MCustomer
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                ProjectName = customer.ProjectName,
                ProjectNo = customer.ProjectNo
            };
            
        }

        public async Task Add(MCustomer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException();
            }
            _dbContext.Customers.Add(new Customer
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                ProjectName = customer.ProjectName,
                ProjectNo = customer.ProjectNo,
                ModifiedDate = DateTime.Now
            });
            await _dbContext.SaveChangesAsync();
            



        }

        public async Task UpdateById(MCustomer mcustomer)
        {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == mcustomer.CustomerId);
                if (customer == null)
                    throw new KeyNotFoundException();
                customer.CustomerName = mcustomer.CustomerName;
                customer.CustomerNo = mcustomer.CustomerNo;
                customer.ProjectName = mcustomer.ProjectName;
                customer.ProjectNo = mcustomer.ProjectNo;
                customer.ModifiedDate = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            
        }

        public async Task Remove(string customerNo)
        {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerNo == customerNo);
                if (customer != null)
                {
                    _dbContext.Customers.Remove(customer);
                    await _dbContext.SaveChangesAsync();
                }
                else {
                    return;
                }
            
        }

        public async Task<IEnumerable<String>> GetCustomerName()
        {
            using (var context = new ShadingContext())
            {
                return await context.Customers.Select(c => c.CustomerName).ToListAsync<String>();
            }

        }
    }
    
}
