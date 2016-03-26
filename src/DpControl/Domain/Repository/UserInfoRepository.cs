using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using Microsoft.Data.Entity;

namespace DpControl.Domain.Repository
{
    public class UserInfoRepository : IUserInfoRepository
    {

        ShadingContext _context;

        #region construct
        public UserInfoRepository()
        {

        }
        public UserInfoRepository(ShadingContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<UserSubSearchModel> FindByIdAsync(string userId)
        {
            var result = _context.Users.Where(v => v.Id == userId);
            result = (IQueryable<ApplicationUser>)ExpandOperator.ExpandRelatedEntities<ApplicationUser>(result);

            var user = await result.FirstOrDefaultAsync();
            var userSearch = UserOperator.SetUserSearchModelCascade(user);
            return userSearch;
        }

        public async Task<IEnumerable<UserSearchModel>> GetAllAsync()
        {
            var queryData = from U in _context.Users
                            select U;

            var result = QueryOperate<ApplicationUser>.Execute(queryData);
            result = (IQueryable<ApplicationUser>)ExpandOperator.ExpandRelatedEntities<ApplicationUser>(result);

            //以下执行完后才会去数据库中查询
            var users = await result.ToListAsync();
            var usersSearch = UserOperator.SetUserSearchModelCascade(users);
            return usersSearch;
        }

        public async Task<IEnumerable<GroupSubSearchModel>> GetGroupsByUserId(string userId)
        {
            var queryData = _context.UserGroups
               .Where(u => u.UserId == userId)
               .Select(u => u.Group);

            var result = QueryOperate<Group>.Execute(queryData);
            var groups = await result.ToListAsync();
            var groupsSearch = GroupOperator.SetGroupSubSearchModel(groups);
            return groupsSearch;
        }

        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsByUserId(string userId)
        {
            var queryData = _context.UserLocations
               .Where(u => u.UserId == userId)
               .Select(u => u.Location);

            var result = QueryOperate<Location>.Execute(queryData);
            var locations = await result.ToListAsync();
            var locationsSearch = LocationOperator.SetLocationSubSearchModel(locations);
            return locationsSearch;
        }
    }
}
