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
        Task<IEnumerable<MCustomer>> FindByCustomerNo(string customerNo);
        Task RemoveById(int Id);
        Task Update(MCustomer mcustomer);
        Task<IEnumerable<String>> GetCustomerName();

        //Task<IEnumerable<MCustomer>> FindRangeByOrder(Query query);
    }
}
