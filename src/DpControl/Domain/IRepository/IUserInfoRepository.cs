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
        UserInfo GetLoginUserInfo();
        

    }

    /// <summary>
    /// use to operator database data
    /// </summary>
    public interface IUserInfoRepository
    {
        Task<IEnumerable<UserSearchModel>> GetAllAsync();
        Task<UserSubSearchModel> FindByIdAsync(string userId);
        Task<IEnumerable<GroupSubSearchModel>> GetGroupsByUserId(string userId);
        Task<IEnumerable<LocationSubSearchModel>> GetLocationsByUserId(string userId);
    }
}
