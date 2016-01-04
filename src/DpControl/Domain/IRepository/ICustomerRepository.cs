using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Entities;

namespace DpControl.Domain.IRepository
{
    public interface ICustomerRepository
    {
        void Add( Customer item);
        IEnumerable<Customer> GetAll();
        Task<Customer> Find(string customerNo);
        Task Remove(string key);
        void Update(Customer item);
    }
}
