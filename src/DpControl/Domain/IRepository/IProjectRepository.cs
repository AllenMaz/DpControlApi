using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IProjectRepository
    {
        Task Add( MProject item);
        Task<IEnumerable<MProject>> GetAll();
        Task<IEnumerable<MProject>> FindByCustomerNo(string customerNo);
        Task RemoveById(int Id);
        Task Update(MProject mcustomer);
 //       Task<IEnumerable<String>> GetCustomerName();

        //Task<IEnumerable<MCustomer>> FindRangeByOrder(Query query);
    }
}
