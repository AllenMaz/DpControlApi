using DpControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DpControl.Domain.IRepository
{
    /// <summary>
    /// use to get login user info
    /// </summary>
    public interface ILoginUserRepository
    {
        LoginUserInfo GetLoginUserInfo();
        Task<LoginUserInfo> GetLoginUserInfoAsync();
        
    }

    /// <summary>
    /// use to operator database data
    /// </summary>
    public interface IUserRepository
    {
        Task<IEnumerable<UserSearchModel>> GetAllAsync();
        Task<UserSubSearchModel> FindByIdAsync(string userId);
        Task<IEnumerable<GroupSubSearchModel>> GetGroupsByUserId(string userId);
        Task<IEnumerable<LocationSubSearchModel>> GetLocationsByUserId(string userId);
        Task<IEnumerable<RoleSubSearchModel>> GetRolesByUserId(string userId);

        Task<string> AddAsync(UserAddModel user);
        Task<string> UpdateByIdAsync(string userId, UserUpdateModel user);
        Task RemoveByIdAsync(string itemId);
        Task CreateRelationsAsync(string userId, string navigationProperty, List<string> navigationPropertyIds);
        Task RemoveRelationsAsync(string userId, string navigationProperty, List<string> navigationPropertyIds);

    }
}
