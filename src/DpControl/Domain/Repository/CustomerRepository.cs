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

        private ShadingContext _context;

        #region Constructors
        public CustomerRepository()
        {
        }

        public CustomerRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        #endregion

        public async Task<IEnumerable<MCustomer>> GetAll()
        {
            return await _context.Customers.Select(c => new MCustomer
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerNo = c.CustomerNo,
                ProjectName = c.ProjectName,
                ProjectNo = c.ProjectNo
            })
            .OrderBy(c => c.CustomerNo)
            .ToListAsync<MCustomer>();
        }
        public async Task<MCustomer> Find(string customerNo)
        {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerNo == customerNo);
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

        public async void Add(MCustomer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException();
            }
            _context.Customers.Add(new Customer
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                ProjectName = customer.ProjectName,
                ProjectNo = customer.ProjectNo,
                ModifiedDate = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }

        public async void UpdateById(MCustomer mcustomer)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == mcustomer.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException();
            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;
            customer.ProjectName = mcustomer.ProjectName;
            customer.ProjectNo = mcustomer.ProjectNo;
            customer.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task Remove(int Id)
        {
            var toDelete = new Customer { CustomerId = Id };
            _context.Customers.Attach(toDelete);
            _context.Customers.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<String>> GetCustomerName()
        {
            return await _context.Customers.Select(c => c.CustomerName).Distinct().ToListAsync<String>();
        }
    }
}
