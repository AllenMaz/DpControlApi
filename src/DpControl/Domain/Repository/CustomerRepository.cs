using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
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

        public async Task<IEnumerable<Customer>> GetAll()
        {
            var customers = await _dbContext.Customers.ToListAsync<Customer>();
            if (customers.Count() == 0)
            {
                return await context.Customers.Select(c => new MCustomer
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    CustomerNo = c.CustomerNo,
                    ProjectName = c.ProjectName,
                    ProjectNo = c.ProjectNo
                })
                .OrderBy(c => c.CustomerNo)
                .ToArrayAsync<MCustomer>();
            }
            return customers;

        }
        public async Task<MCustomer> Find(string customerNo)
        {
            using (var context = new ShadingContext())
            {
                var customer= await context.Customers.FirstOrDefaultAsync(c => c.CustomerNo == customerNo);
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
        }

        public async void Add(MCustomer customer)
        {
            try
            {
                if (customer == null)
                {
                    throw new ArgumentNullException();
                }
                context.Customers.Add(new Customer
                {
                    CustomerName = customer.CustomerName,
                    CustomerNo = customer.CustomerNo,
                    ProjectName = customer.ProjectName,
                    ProjectNo = customer.ProjectNo,
                    ModifiedDate = DateTime.Now
                }); 
                await context.SaveChangesAsync();
            }
            

        }

        public async void UpdateById(MCustomer mcustomer)
        {
            using (var context=new ShadingContext())
        {
                var customer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == mcustomer.CustomerId);
                if (customer == null)
                    throw new KeyNotFoundException();
                customer.CustomerName = mcustomer.CustomerName;
                customer.CustomerNo = mcustomer.CustomerNo;
                customer.ProjectName = mcustomer.ProjectName;
                customer.ProjectNo = mcustomer.ProjectNo;
                customer.ModifiedDate = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }

        public async Task Remove(string customerNo)
        {
            using (var context =(ShadingContext)(new EFContextFactory().GetContext()))
            {
                var customer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerNo == customerNo);
                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    await context.SaveChangesAsync();
                }
                else{
                    return;
                }
            }
        }

        public async Task<IEnumerable<String>> GetCustomerName()
        {
            using (var context=new ShadingContext())
            {
                return await context.Customers.Select(c => c.CustomerName).ToListAsync<String>();
            }
            
        }
    }
    
}
