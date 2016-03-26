using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ICustomerRepository:IBaseRepository<CustomerAddModel,CustomerUpdateModel,CustomerSearchModel>
    {
        #region Entity Relations

        Task<IEnumerable<ProjectSubSearchModel>> GetProjectsByCustomerIdAsync(int customerId);

        #endregion
    }
}
