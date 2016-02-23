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
            try
            {
                _context.Customers.Add(new Customer
                {
                    CustomerName = customer.CustomerName,
                    CustomerNo = customer.CustomerNo,
                    CreateDate = DateTime.Now,
                    Creator = "System"
                });

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new AddException(e.Message);
            }
        }

        public async Task AddAsync(CustomerAddModel customer)
        {
            try
            {
                _context.Customers.Add(new Customer
                {
                    CustomerName = customer.CustomerName,
                    CustomerNo = customer.CustomerNo,
                    CreateDate = DateTime.Now,
                    Creator = "System"
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new AddException(e.Message);
            }

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
                Creator = c.Creator,
                CreateDate = c.CreateDate,
                Modifier = c.Modifier,
                ModifiedDate = c.ModifiedDate
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
                Creator = c.Creator,
                CreateDate = c.CreateDate,
                Modifier = c.Modifier,
                ModifiedDate = c.ModifiedDate
            })
                .OrderBy(c => c.CustomerNo)
                .ToListAsync<CustomerSearchModel>();

            return customers;
        }

        public void Update(CustomerUpdateModel mcustomer)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == mcustomer.CustomerId);
            if (customer == null)
                throw new DeleteException("Could not find data which CustomerId equal to " + mcustomer.CustomerId);

            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;
            customer.Modifier = "System";
            customer.ModifiedDate = DateTime.Now;

            _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CustomerUpdateModel mcustomer)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == mcustomer.CustomerId);
            if (customer == null)
                throw new DeleteException("Could not find data which CustomerId equal to " + mcustomer.CustomerId);

            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;
            customer.Modifier = "System";
            customer.ModifiedDate = DateTime.Now;

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
                throw new DeleteException("Could not find data which CustomerId equal to "+id);

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }


        #endregion

        //#region

        

        //public async Task<IEnumerable<String>> GetProjectName()
        //{
        //    return await _context.Projects.Select(c => c.ProjectName).ToListAsync<String>();
        //}

        ////public async Task<IEnumerable<MCustomer>> FindRangeByOrder(Query query)
        ////{
        ////    var customers =  _context.Customers.Select(c => new MCustomer
        ////    {
        ////        CustomerId = c.CustomerId,
        ////        CustomerName = c.CustomerName,
        ////        CustomerNo = c.CustomerNo,
        ////        ProjectName = c.ProjectName,
        ////        ProjectNo = c.ProjectNo
        ////    });

        ////    if (query.orderby.OrderbyBehavior == "DESC")
        ////    {
        ////        for(int i = 0; i < query.orderby.OrderbyField.Length; i++)
        ////        {
        ////            if(typeof(MCustomer).GetProperty)
        ////            customers = customers.OrderBy();

        ////        }
        ////    }
        ////    else
        ////    {
        ////        customers.OrderBy()
        ////    }
        ////    .OrderBy(c => c.CustomerNo)
        ////    .ToListAsync<MCustomer>();
        ////    return customers;
        ////}

        ////        Array
        //#endregion
    }

}
