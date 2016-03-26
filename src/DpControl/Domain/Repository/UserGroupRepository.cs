using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using DpControl.Domain.Execptions;
using Microsoft.Data.Entity;
using DpControl.Domain.Entities;

namespace DpControl.Domain.Repository
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private ShadingContext _context;

        #region Constructors
        public UserGroupRepository()
        {
        }

        public UserGroupRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        #endregion

        public int Add(UserGroupAddModel mUserGroup)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == mUserGroup.GroupId);
            if (group == null)
                throw new ExpectException("Could not find Group data which GroupId equal to " + mUserGroup.GroupId);

            var user = _context.Users.FirstOrDefault(c => c.Id == mUserGroup.UserId);
            if (user == null)
                throw new ExpectException("Could not find User data which UserId equal to '" + mUserGroup.UserId+"'");

            //GroupId and UserId must be unique
            var checkData = _context.UserGroups
                .Where(c => c.GroupId == mUserGroup.GroupId
                                    && c.UserId == mUserGroup.UserId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("There is already exist data which GroupId equal to "
                    + mUserGroup.GroupId + " and UserId equal to '" + mUserGroup.UserId+"'");


            var model = new UserGroup
            {
                GroupId = mUserGroup.GroupId,
                UserId = mUserGroup.UserId
            };
            _context.UserGroups.Add(model);
            _context.SaveChanges();
            return model.UserGroupId;
        }

        public async Task<int> AddAsync(UserGroupAddModel mUserGroup)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == mUserGroup.GroupId);
            if (group == null)
                throw new ExpectException("Could not find Group data which GroupId equal to " + mUserGroup.GroupId);

            var user = _context.Users.FirstOrDefault(c => c.Id == mUserGroup.UserId);
            if (user == null)
                throw new ExpectException("Could not find User data which UserId equal to '" + mUserGroup.UserId+"'");

            //GroupId and UserId must be unique
            var checkData = await _context.UserGroups
                .Where(c => c.GroupId == mUserGroup.GroupId
                                    && c.UserId == mUserGroup.UserId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("There is already exist data which GroupId equal to "
                    + mUserGroup.GroupId + " and UserId equal to '" + mUserGroup.UserId+"'");


            var model = new UserGroup
            {
                GroupId = mUserGroup.GroupId,
                UserId = mUserGroup.UserId
            };
            _context.UserGroups.Add(model);
            await _context.SaveChangesAsync();
            return model.UserGroupId;
        }

        public UserGroupSearchModel FindById(int userGroupId)
        {
            var userGroup = _context.UserGroups
              .Where(v => v.UserGroupId == userGroupId)
              .Select(v => new UserGroupSearchModel()
              {
                  UserGroupId = v.UserGroupId,
                  GroupId = v.GroupId,
                  UserId = v.UserId
              }).FirstOrDefault();

            return userGroup;
        }

        public async Task<UserGroupSearchModel> FindByIdAsync(int userGroupId)
        {
            var userGroup = await _context.UserGroups
               .Where(v => v.UserGroupId == userGroupId)
               .Select(v => new UserGroupSearchModel()
               {
                   UserGroupId = v.UserGroupId,
                   GroupId = v.GroupId,
                   UserId = v.UserId
               }).FirstOrDefaultAsync();

            return userGroup;
        }

        public IEnumerable<UserGroupSearchModel> GetAll()
        {
            var queryData = from U in _context.UserGroups
                            select U;

            var result = QueryOperate<UserGroup>.Execute(queryData);

            //以下执行完后才会去数据库中查询
            var userGroups = result.ToList();

            var userGroupsSearch = userGroups.Select(v => new UserGroupSearchModel
            {
                UserGroupId = v.UserGroupId,
                GroupId = v.GroupId,
                UserId = v.UserId
            });

            return userGroupsSearch;
        }

        public async Task<IEnumerable<UserGroupSearchModel>> GetAllAsync()
        {
            var queryData = from U in _context.UserGroups
                            select U;

            var result = QueryOperate<UserGroup>.Execute(queryData);

            //以下执行完后才会去数据库中查询
            var userGroups = await result.ToListAsync();

            var userGroupsSearch = userGroups.Select(v => new UserGroupSearchModel
            {
                UserGroupId = v.UserGroupId,
                GroupId = v.GroupId,
                UserId = v.UserId
            });

            return userGroupsSearch;
        }

        public void RemoveById(int userGroupId)
        {
            var userGroup = _context.UserGroups.FirstOrDefault(c => c.UserGroupId == userGroupId);
            if (userGroup == null)
                throw new ExpectException("Could not find data which UserGroupId equal to " + userGroupId);

            _context.Remove(userGroup);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int userGroupId)
        {
            var userGroup = _context.UserGroups.FirstOrDefault(c => c.UserGroupId == userGroupId);
            if (userGroup == null)
                throw new ExpectException("Could not find data which UserGroupId equal to " + userGroupId);

            _context.Remove(userGroup);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int userGroupId, UserGroupUpdateModel mUserGroup)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateByIdAsync(int userGroupId, UserGroupUpdateModel mUserGroup)
        {
            throw new NotImplementedException();
        }
    }
}
