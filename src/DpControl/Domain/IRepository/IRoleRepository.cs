using DpControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.IRepository
{
    public interface IRoleRepository: IRelationsRepository<string>
    {
        Task<IEnumerable<RoleSearchModel>> GetAllAsync();
        Task<RoleSubSearchModel> FindByIdAsync(string roleId);
        Task<string> AddAsync(RoleAddModel role);
        Task<string> UpdateByIdAsync(string roleId, RoleUpdateModel role);
        Task RemoveByIdAsync(string roleId);
        Task<IEnumerable<UserSubSearchModel>> GetUsersByUserId(string roleId);

    }
}
