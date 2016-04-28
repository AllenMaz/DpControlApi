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
using System.Dynamic;

namespace DpControl.Domain.Repository
{
    public class CustomerRepository : ICustomerRepository
    {

        private ShadingContext _context;
        private readonly ILoginUserRepository _loginUser; 

        #region Constructors
        public CustomerRepository()
        {
        }

        public CustomerRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        public CustomerRepository(ShadingContext dbContext, ILoginUserRepository loginUser)
        {
            _context = dbContext;
            _loginUser = loginUser;
        }

        #region Add
        public int Add(CustomerAddModel customer)
        {
            //CustomerNo must be unique
            var checkData = _context.Customers.Where(c => c.CustomerNo == customer.CustomerNo).ToList().Count ;
            if (checkData >0)
                throw new ExpectException("The data which CustomerNo equal to '"+customer.CustomerNo +"' already exist in system");

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

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
            //CustomerNo must be unique
            var checkData = await _context.Customers.Where(c => c.CustomerNo == customer.CustomerNo).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which CustomerNo equal to '" + customer.CustomerNo + "' already exist in system");

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

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
            var result = _context.Customers.Where(c => c.CustomerId == customerId);
            result = (IQueryable<Customer>)ExpandOperator.ExpandRelatedEntities<Customer>(result);

            var customer = result.FirstOrDefault();
            var customerSearch = CustomerOperator.SetCustomerSearchModelCascade(customer);

            return customerSearch;
        }

        public async Task<CustomerSearchModel> FindByIdAsync(int customerId)
        {
            var result = _context.Customers.Where(c => c.CustomerId == customerId);
            result = (IQueryable<Customer>)ExpandOperator.ExpandRelatedEntities<Customer>(result);

            var customer = await result.FirstOrDefaultAsync();
            var customerSearch = CustomerOperator.SetCustomerSearchModelCascade(customer);
            
            return customerSearch;
        }

        /// <summary>
        /// Get All Customers
        /// If user has CustomerNo ,then filter by CustomerNo
        /// if user's CustomerNo is null ,then return all Customers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerSearchModel> GetAll()
        {
            var queryData = from C in _context.Customers
                            select C;

            var result = QueryOperate<Customer>.Execute(queryData);
            result = (IQueryable<Customer>)ExpandOperator.ExpandRelatedEntities<Customer>(result);

            //以下执行完后才会去数据库中查询
            var customers = result.ToList();
            var customerSearch = CustomerOperator.SetCustomerSearchModelCascade(customers);

            return customerSearch;
        }
        
        public async Task<IEnumerable<CustomerSearchModel>> GetAllAsync()
        {
            var queryData = from C in _context.Customers
                        select C;
            
            var result = QueryOperate<Customer>.Execute(queryData);
            result = (IQueryable<Customer>)ExpandOperator.ExpandRelatedEntities<Customer>(result);

            //以下执行完后才会去数据库中查询
            //N+1 Select 
            var customers = await result.ToListAsync();
            var customerSearch = CustomerOperator.SetCustomerSearchModelCascade(customers);

            return customerSearch;
        }

        public async Task<IEnumerable<ProjectSubSearchModel>> GetProjectsByCustomerIdAsync(int customerId)
        {
            var queryData = _context.Projects
                .Where(v => v.CustomerId == customerId);

            var result = QueryOperate<Project>.Execute(queryData);
            var projects = await result.ToListAsync();

            var projectSearch = ProjectOperator.SetProjectSubSearchModel(projects);
            return projectSearch;
        }

        public int UpdateById(int customerId,CustomerUpdateModel mcustomer)
        {
            
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to " + customerId);

            //Check CustomerNo must be unique 
            var checkData =  _context.Customers.Where(c => c.CustomerNo == mcustomer.CustomerNo
                                                            && c.CustomerId != customerId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which CustomerNo equal to '" + customer.CustomerNo + "' already exist in system");

            //Get UserInfo
            var user =  _loginUser.GetLoginUserInfo();

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
            //Check CustomerNo must be unique 
            var checkData = await _context.Customers.Where(c => c.CustomerNo == mcustomer.CustomerNo 
                                                            && c.CustomerId != customerId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which CustomerNo equal to '" + mcustomer.CustomerNo + "' already exist in system");

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            customer.CustomerName = mcustomer.CustomerName;
            customer.CustomerNo = mcustomer.CustomerNo;
            customer.Modifier = user.UserName;
            customer.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return customer.CustomerId; 
        }

        public void RemoveById(int id)
        {
            var customer = _context.Customers.Include(c => c.Projects).FirstOrDefault(v => v.CustomerId == id);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to " + id);

            _context.Customers.Remove(customer);

            //Cascade delete Projects
            _context.Projects.RemoveRange(customer.Projects);
            //Casecade delete other 


            #endregion

            _context.SaveChanges();

        }

        public async Task RemoveByIdAsync(int id)
        {
            var customer = await _context.Customers.Include(c=>c.Projects).FirstOrDefaultAsync(v => v.CustomerId == id);
            if (customer == null)
                throw new ExpectException("Could not find data which CustomerId equal to "+id);

            _context.Customers.Remove(customer);
            #region Cascade delete dependent entities
            //Cascade delete Projects
            _context.Projects.RemoveRange(customer.Projects);
            //Casecade delete other 


            #endregion

            await _context.SaveChangesAsync();
        }

        
    }

}
