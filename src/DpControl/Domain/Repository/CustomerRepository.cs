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
using DpControl.Utility.Authentication;

namespace DpControl.Domain.Repository
{
    public class CustomerRepository : ICustomerRepository
    {

        private ShadingContext _context;
        private readonly IUserInfoRepository _userInfo;

        #region Constructors
        public CustomerRepository()
        {
        }

        public CustomerRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        public CustomerRepository(ShadingContext dbContext,IUserInfoRepository userInfo)
        {
            _context = dbContext;
            _userInfo = userInfo;
        }

        #region Add
        public int Add(CustomerAddModel customer)
        {
            //Check whether the CustomerNo already exist
            var checkData = _context.Customers.Where(c => c.CustomerNo == customer.CustomerNo).ToList().Count ;
            if (checkData >0)
                throw new ExpectException("The data which CustomerNo equal to '"+customer.CustomerNo +"' already exist in system");

            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            var model = new Customer
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };

            _context.Customers.Add(model);

           _context.SaveChanges();
            return model.CustomerId; 
        }
        
        public async Task<int> AddAsync(CustomerAddModel customer)
        {
            //Check whether the CustomerNo already exist
            var checkData = await _context.Customers.Where(c => c.CustomerNo == customer.CustomerNo).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which CustomerNo equal to '" + customer.CustomerNo + "' already exist in system");

            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            var model = new Customer
            {
                CustomerName = customer.CustomerName,
                CustomerNo = customer.CustomerNo,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };

            _context.Customers.Add(model);

            await _context.SaveChangesAsync();
            return model.CustomerId;

        }
        
        #endregion

        public CustomerSearchModel FindById(int customerId)
        {
            var customer = _context.Customers
                        .Where(c => c.CustomerId == customerId)
                        .Select(c => new CustomerSearchModel
                        {
                            CustomerId = c.CustomerId,
                            CustomerName = c.CustomerName,
                            CustomerNo = c.CustomerNo,
                            Creator = c.Creator ,
                            CreateDate = c.CreateDate.ToString(),
                            Modifier = c.Modifier,
                            ModifiedDate = c.ModifiedDate.ToString()
                        }).FirstOrDefault();

            return customer;
        }

        public async Task<CustomerSearchModel> FindByIdAsync(int customerId)
        {
            var customer =await _context.Customers
                        .Where(c => c.CustomerId == customerId)
                        .Select(c => new CustomerSearchModel
                        {
                            CustomerId = c.CustomerId,
                            CustomerName = c.CustomerName,
                            CustomerNo = c.CustomerNo,
                            Creator = c.Creator,
                            CreateDate = c.CreateDate.ToString(),
                            Modifier = c.Modifier,
                            ModifiedDate = c.ModifiedDate.ToString()
                        }).FirstOrDefaultAsync();

            return customer;
        }


        public IEnumerable<CustomerSearchModel> GetAll(Query query)
        {
            var queryData = from P in _context.Customers
                            select P;

            var result = QueryOperate<Customer>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var customers = result.Include(c => c.Projects).ToList();

            var customerSearch = customers.Select(c => new CustomerSearchModel
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerNo = c.CustomerNo,
                Creator = c.Creator,
                CreateDate = c.CreateDate.ToString(),
                Modifier = c.Modifier,
                ModifiedDate = c.ModifiedDate.ToString()
            });

            return customerSearch;
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
                Creator = c.Creator,
                CreateDate = c.CreateDate.ToString(),
                Modifier = c.Modifier,
                ModifiedDate = c.ModifiedDate.ToString()
            });

            return customerSearch;
        }

        

        public int UpdateById(int customerId,CustomerUpdateModel mcustomer)
        {
            
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to " + customerId);

            //Check Edit value which is unique in database ,already exist in system or not 
            var checkData =  _context.Customers.Where(c => c.CustomerNo == mcustomer.CustomerNo
                                                            && c.CustomerId != customerId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which CustomerNo equal to '" + customer.CustomerNo + "' already exist in system");

            //Get UserInfo
            var user =  _userInfo.GetUserInfo();

            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;
            customer.Modifier = user.UserName;
            customer.ModifiedDate = DateTime.Now;

            _context.SaveChanges();

            return customer.CustomerId;
        }

        public async Task<int> UpdateByIdAsync(int customerId,CustomerUpdateModel mcustomer)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to " + customerId);
            //Check Edit value which is unique in database ,already exist in system or not 
            var checkData = await _context.Customers.Where(c => c.CustomerNo == mcustomer.CustomerNo 
                                                            && c.CustomerId != customerId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which CustomerNo equal to '" + mcustomer.CustomerNo + "' already exist in system");

            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;
            customer.Modifier = user.UserName;
            customer.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return customer.CustomerId; 
        }

        public void RemoveById(int id)
        {
            _context.Database.ExecuteSqlCommand("delete from controlsystem.customers where customerId equal to " + id);

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
