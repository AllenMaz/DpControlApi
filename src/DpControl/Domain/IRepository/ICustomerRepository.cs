using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ICustomerRepository
    {
        
        int Add(CustomerAddModel customer);
        Task<int> AddAsync(CustomerAddModel customer);
        IEnumerable<CustomerSearchModel> GetAll(Query query);
        Task<IEnumerable<CustomerSearchModel>> GetAllAsync(Query query);
        CustomerSearchModel FindByCustomerId(int customerId);
        Task<CustomerSearchModel> FindByCustomerIdAsync(int customerId);
        IEnumerable<CustomerSearchModel> FindByCustomerNo(string customerNo);
        Task<IEnumerable<CustomerSearchModel>> FindByCustomerNoAsync(string customerNo);
        int UpdateById(int customerId,CustomerUpdateModel mcustomer);
        Task<int> UpdateByIdAsync(int customerId,CustomerUpdateModel mcustomer);
        void RemoveById(int customerId);
        Task RemoveByIdAsync(int customerId);
    }
}
