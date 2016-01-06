using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Entities;
using DpControl.Domain.Repository;

namespace DpControl.Domain.IRepository
{
    public interface ICustomerRepository
    {
        Task Add( Customer item);
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> Find(string customerNo);
        Task Remove(string key);
        void Update(Customer item);
    }
}
