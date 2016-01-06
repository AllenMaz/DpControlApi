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
            return await _dbContext.Customers.ToListAsync<Customer>();

        }

        public async Task<Customer> Find(string customerNo)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerNo == customerNo);

            return customer;

        }


        public async Task<OperateMessage> Add(Customer customer)
        {
            OperateMessage operateMessage = new OperateMessage();
            try
            {
                if (customer == null)
                {
                    throw new ArgumentNullException();
                }

                _dbContext.Customers.Add(customer);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw new NotImplementedException("数据新增失败,错误："+e.Message);

            }
            return operateMessage;
            

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
                else {
                    return;
                }
            }
        }
    }

    public class OperateMessage
    {
        public OperateMessage()
        {
            Success = true;
        }
        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
