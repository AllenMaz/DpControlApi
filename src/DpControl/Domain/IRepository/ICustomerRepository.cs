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
        void Update(CustomerUpdateModel mcustomer);
        Task UpdateAsync(CustomerUpdateModel mcustomer);
        void RemoveById(int id);
        Task RemoveByIdAsync(int id);
    }
}
