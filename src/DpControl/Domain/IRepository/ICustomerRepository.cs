using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ICustomerRepository
    {
        
        void Add(CustomerAddModel customer);
        Task AddAsync(CustomerAddModel customer);
        IEnumerable<CustomerSearchModel> GetAll();
        Task<IEnumerable<CustomerSearchModel>> GetAllAsync();
        IEnumerable<CustomerSearchModel> FindByCustomerNo(string customerNo);
        Task<IEnumerable<CustomerSearchModel>> FindByCustomerNoAsync(string customerNo);
        void UpdateById(int customerId,CustomerUpdateModel mcustomer);
        Task UpdateByIdAsync(int customerId,CustomerUpdateModel mcustomer);
        void RemoveById(int customerId);
        Task RemoveByIdAsync(int customerId);
    }
}
