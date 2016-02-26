using System;
using System.Reflection;
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

        public int Add(CustomerAddModel customer)
        {
            var model = new Customer
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                CreateDate = DateTime.Now
            };
            _context.Customers.Add(model);

           _context.SaveChanges();
            return model.CustomerId; 
        }

        public async Task<int> AddAsync(CustomerAddModel customer)
        {
            var model = new Customer
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                CreateDate = DateTime.Now
            };

            _context.Customers.Add(model);

            await _context.SaveChangesAsync();
            return model.CustomerId;

        }
        public CustomerSearchModel FindByCustomerId(int customerId)
        {
            var customer = _context.Customers
                        .Where(c => c.CustomerId == customerId)
                        .Select(c => new CustomerSearchModel
                        {
                            CustomerId = c.CustomerId,
                            CustomerName = c.CustomerName,
                            CustomerNo = c.CustomerNo,
                            CreateDate = c.CreateDate
                        }).FirstOrDefault();

            return customer;
        }

        public async Task<CustomerSearchModel> FindByCustomerIdAsync(int customerId)
        {
            var customer =await _context.Customers
                        .Where(c => c.CustomerId == customerId)
                        .Select(c => new CustomerSearchModel
                        {
                            CustomerId = c.CustomerId,
                            CustomerName = c.CustomerName,
                            CustomerNo = c.CustomerNo,
                            CreateDate = c.CreateDate
                        }).FirstOrDefaultAsync();

            return customer;
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
                                CreateDate = c.CreateDate
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
                            CreateDate = c.CreateDate
                        });

            return await customer.ToListAsync<CustomerSearchModel>();
        }

        public IEnumerable<CustomerSearchModel> GetAll(Query query)
        {
            var customers = _context.Customers.Include(c=>c.Projects).Select(c => new CustomerSearchModel
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerNo = c.CustomerNo,
                CreateDate = c.CreateDate,
                Projects = c.Projects.Select(p => new ProjectSearchModel
                {
                    CustomerId = p.CustomerId,
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    ProjectNo = p.ProjectNo,
                    CreateDate = p.CreateDate
                }).ToList()
            })
                .OrderBy(c => c.CustomerNo)
                .ToList<CustomerSearchModel>();

            return customers;
        }

        public async Task<IEnumerable<CustomerSearchModel>> GetAllAsync(Query query)
        {
            
            var queryData = from P in _context.Customers
                        select P;

            var result = QueryOperate<Customer>.Execute(queryData, query);
            
            //以下执行完后才会去数据库中查询
            var customers = await result.Include(c => c.Projects).ToListAsync();

            var customerSearch = customers.Select(c => new CustomerSearchModel
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerNo = c.CustomerNo,
                CreateDate = c.CreateDate,
                Projects = c.Projects.Select(p => new ProjectSearchModel
                {
                    CustomerId = p.CustomerId,
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    ProjectNo = p.ProjectNo,
                    CreateDate = p.CreateDate
                }).ToList()
            });

            return customerSearch;
        }

        

        public int UpdateById(int customerId,CustomerUpdateModel mcustomer)
        {
            
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to " + customerId);

            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;

            _context.SaveChanges();

            return customer.CustomerId;
        }

        public async Task<int> UpdateByIdAsync(int customerId,CustomerUpdateModel mcustomer)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to " + customerId);

            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;

            await _context.SaveChangesAsync();
            return customer.CustomerId; 
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
