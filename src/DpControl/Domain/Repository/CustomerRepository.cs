using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
//using Microsoft.Extensions.DependencyInjection;


namespace DpControl.Domain.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        
       ShadingContext  dbContext;
       public CustomerRepository(ShadingContext dbContext)
        {
            this.dbContext = dbContext; 
        }
        public CustomerRepository()
        {

        }
        public async Task<IEnumerable<Customer>> GetAll()
        {
            using (var context=new ShadingContext())
            {
                return await context.Customers.ToListAsync<Customer>();
            }
        }
        public async Task<Customer> Find(string customerNo)
        {
            using (var context = new ShadingContext())
            {
                var customer= await context.Customers.FirstOrDefaultAsync(c => c.CustomerNo == customerNo);
                if (customer == null)
                    throw new KeyNotFoundException();
                return customer;
            }
        }

        public async void Add(Customer customer)
        {
            using (var context=new ShadingContext())
            {
                if (customer == null)
                {
                    throw new ArgumentNullException();
                }
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }
        }

        public void Update(Customer customer)
        {

        }
        public async Task Remove(string customerNo)
        {
            using (var context = new ShadingContext())
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
    }
}
