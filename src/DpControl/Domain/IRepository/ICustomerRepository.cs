using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ICustomerRepository
    {
        Task Add( MCustomer item);
        Task<IEnumerable<MCustomer>> GetAll();
        Task<MCustomer> Find(string customerNo);
        Task Remove(int Id);
        Task UpdateById(MCustomer mcustomer);
        Task<IEnumerable<String>> GetCustomerName();
    }
}
