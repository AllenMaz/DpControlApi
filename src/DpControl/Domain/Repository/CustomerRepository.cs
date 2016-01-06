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
using DpControl.Domain.Utility;
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
            var customers = await _dbContext.Customers.ToListAsync<Customer>();

            List<MCustomer> mCustomers = EntityModelUtility.ConverEntityToModel<MCustomer, Customer>(customers);
            mCustomers = mCustomers.OrderBy(v=>v.CustomerNo).ToList();

            return mCustomers;
        }
        public async Task<MCustomer> Find(string customerNo)
        {
            var customer= await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerNo == customerNo);
            if (customer == null)
                throw new KeyNotFoundException();

            MCustomer mCustomer = EntityModelUtility.ConverEntityToModel<MCustomer, Customer>(customer);
            return mCustomer;


        }

        public async Task Add(MCustomer mCustomer)
        {

            try
            {
                if (mCustomer == null)
                {
                    throw new ArgumentNullException();
                }
                //var bb = _dbContext.ChangeTracker.Entries();

                if (mCustomer == null)
                {
                    throw new ArgumentNullException();
                }
                Customer customer = EntityModelUtility.ConverModelToEntity<MCustomer, Customer>(mCustomer);
                customer.ModifiedDate = DateTime.Now;
                _dbContext.Customers.Add(customer);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new ProcedureException("数据新增失败。错误："+e.Message);
            }
            

        }

        public async Task UpdateById(MCustomer mCustomer)
        {
            Customer customer = EntityModelUtility.ConverModelToEntity<MCustomer, Customer>(mCustomer);

            var entity = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
            if (entity == null)
                throw new KeyNotFoundException();

            entity.CustomerName = customer.CustomerName;
            entity.CustomerNo = customer.CustomerNo;
            entity.ProjectName = customer.ProjectName;
            entity.ProjectNo = customer.ProjectNo;
            entity.ModifiedDate = DateTime.Now;
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
            else{
                return;
            }
            
        }
           
        public async Task<IEnumerable<String>> GetCustomerName()
        {
           
           return await _dbContext.Customers.Select(c => c.CustomerName).ToListAsync<String>();
            
            
        }
    }
    
}
