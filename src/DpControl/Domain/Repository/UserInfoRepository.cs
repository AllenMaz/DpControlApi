using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;
using Microsoft.AspNet.Identity;

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

        public async Task<string> AddAsync(UserAddModel user)
        {
            //UserName must be unique
            var checkData = await _context.Users.Where(u => u.UserName == user.UserName).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which UserName equal to '" + user.UserName + "' already exist in system");

            //if CustomerNo has value,
            if (!string.IsNullOrEmpty(user.CustomerNo))
            {
                var customer = _context.Customers.FirstOrDefault(c => c.CustomerNo == user.CustomerNo);
                if (customer == null)
                    throw new ExpectException("Could not find Customer data which CustomerNo equal to " + user.CustomerNo);
            }
            //if ProjectNo has value,
            if (!string.IsNullOrEmpty(user.ProjectNo))
            {
                var project = _context.Projects.FirstOrDefault(c => c.ProjectNo == user.ProjectNo);
                if (project == null)
                    throw new ExpectException("Could not find Project data which ProjectNo equal to " + user.ProjectNo);
            }

            // Hash password
            var passwordHash = new PasswordHasher().HashPassword(null, user.Password);

            var model = new ApplicationUser
            {
                UserName = user.UserName,
                PasswordHash = passwordHash,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CustomerNo = user.CustomerNo,
                ProjectNo = user.ProjectNo
            };

            _context.Users.Add(model);
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task RemoveByIdAsync(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new ExpectException("Could not find data which UserId equal to " + userId);

            _context.Users.Remove(user);
            
            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateByIdAsync(string userId, UserUpdateModel user)
        {
            var userData = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (userData == null)
                throw new ExpectException("Could not find data which UserId equal to " + userId);

            //if CustomerNo has value,
            if (!string.IsNullOrEmpty(user.CustomerNo))
            {
                var customer = _context.Customers.FirstOrDefault(c => c.CustomerNo == user.CustomerNo);
                if (customer == null)
                    throw new ExpectException("Could not find Customer data which CustomerNo equal to " + user.CustomerNo);
            }
            //if ProjectNo has value,
            if (!string.IsNullOrEmpty(user.ProjectNo))
            {
                var project = _context.Projects.FirstOrDefault(c => c.ProjectNo == user.ProjectNo);
                if (project == null)
                    throw new ExpectException("Could not find Project data which ProjectNo equal to " + user.ProjectNo);
            }


            userData.Email = user.Email;
            userData.PhoneNumber = user.PhoneNumber;
            userData.CustomerNo = user.CustomerNo;
            userData.ProjectNo = user.ProjectNo;
            
            await _context.SaveChangesAsync();
            return userData.Id;
        }
    }
}
