using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;

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

        public void Add(CustomerAddModel customer)
        {
            
            _context.Customers.Add(new Customer
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                CreateDate = DateTime.Now
            });

            _context.SaveChanges();
            
        }

        public async Task AddAsync(CustomerAddModel customer)
        {
            
            _context.Customers.Add(new Customer
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                CreateDate = DateTime.Now
            });

            await _context.SaveChangesAsync();
           

        }

        public IEnumerable<CustomerSearchModel> FindByCustomerNo(string customerNo)
        {
            var customer = _context.Customers
                            .Where(c => c.CustomerNo == customerNo)
                            .Select(c => new CustomerSearchModel
                            {
                                CustomerId = c.CustomerId,
                                CustomerName = c.CustomerName,
                                CustomerNo = c.CustomerNo,

                            });

            return customer.ToList<CustomerSearchModel>();
        }

        public async Task<IEnumerable<CustomerSearchModel>> FindByCustomerNoAsync(string customerNo)
        {
            var customer = _context.Customers
                        .Where(c => c.CustomerNo == customerNo)
                        .Select(c => new CustomerSearchModel
                        {
                            CustomerId = c.CustomerId,
                            CustomerName = c.CustomerName,
                            CustomerNo = c.CustomerNo,
                                
                        });

            return await customer.ToListAsync<CustomerSearchModel>();
        }

        public IEnumerable<CustomerSearchModel> GetAll()
        {
            var customers = _context.Customers.Select(c => new CustomerSearchModel
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerNo = c.CustomerNo,
                CreateDate = c.CreateDate
            })
                .OrderBy(c => c.CustomerNo)
                .ToList<CustomerSearchModel>();

            return customers;
        }

        public async Task<IEnumerable<CustomerSearchModel>> GetAllAsync()
        {
            var customers = await _context.Customers.Select(c => new CustomerSearchModel
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerNo = c.CustomerNo,
                CreateDate = c.CreateDate
            })
                .OrderBy(c => c.CustomerNo)
                .ToListAsync<CustomerSearchModel>();

            return customers;
        }

        public void UpdateById(int customerId,CustomerUpdateModel mcustomer)
        {
            
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to " + customerId);

            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;

            _context.SaveChangesAsync();
        }

        public async Task UpdateByIdAsync(int customerId,CustomerUpdateModel mcustomer)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to " + customerId);

            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;

            await _context.SaveChangesAsync();
            
        }

        public void RemoveById(int id)
        {
            _context.Database.ExecuteSqlCommand("delete from controlsystem.customers where customerId=" + id);

        }

        public async Task RemoveByIdAsync(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(v => v.CustomerId == id);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to "+id);

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }


        #endregion

       
    }

}
