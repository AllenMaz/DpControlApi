using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;

namespace DpControl.Domain.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private ShadingContext _context;
        private readonly IUserInfoRepository _userInfo;

        #region Constructors
        public GroupRepository()
        {
        }

        public GroupRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        public GroupRepository(ShadingContext dbContext, IUserInfoRepository userInfo)
        {
            _context = dbContext;
            _userInfo = userInfo;
        }
        #endregion

        public int Add(GroupAddModel group)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == group.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + group.ProjectId);
            //Chech whether the Foreign key SceneId data exist
            if (group.SceneId != null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == group.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + group.SceneId);
            }

            //Check whether the GroupName already exist
            var checkData = _context.Groups.Where(g => g.GroupName == group.GroupName).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '" + group.GroupName + "' already exist in system.");

            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            var model = new Group
            {
                GroupName = group.GroupName,
                ProjectId = group.ProjectId,
                SceneId = group.SceneId,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Groups.Add(model);
            _context.SaveChanges();
            return model.GroupId;
        }

        public async Task<int> AddAsync(GroupAddModel group)
        {
            //Chech whether the Foreign key ProjectId data exist
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == group.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + group.ProjectId);

            //Chech whether the Foreign key SceneId data exist
            if (group.SceneId !=null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == group.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + group.SceneId);
            }
            

            //Check whether the GroupName already exist
            var checkData =await _context.Groups.Where(g => g.GroupName == group.GroupName).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '"+group.GroupName + "' already exist in system.");

            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            var model = new Group
            {
                GroupName = group.GroupName,
                ProjectId = group.ProjectId,
                SceneId = group.SceneId,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Groups.Add(model);
            await _context.SaveChangesAsync();
            return model.GroupId;
        }

        public GroupSearchModel FindById(int groupId)
        {
            var group = _context.Groups.Where(v => v.GroupId == groupId)
                .Select(v => new GroupSearchModel()
                {
                    GroupId = v.GroupId,
                    GroupName = v.GroupName,
                    ProjectId = v.ProjectId,
                    SceneId = v.SceneId,
                    Creator = v.Creator,
                    CreateDate = v.CreateDate.ToString(),
                    Modifier = v.Modifier,
                    ModifiedDate = v.ModifiedDate.ToString()
                }).FirstOrDefault();

            return group;
        }

        public async Task<GroupSearchModel> FindByIdAsync(int groupId)
        {
            var group = await _context.Groups.Where(v => v.GroupId == groupId)
                .Select(v => new GroupSearchModel()
                {
                    GroupId = v.GroupId,
                    GroupName = v.GroupName,
                    ProjectId = v.ProjectId,
                    SceneId = v.SceneId,
                    Creator = v.Creator,
                    CreateDate = v.CreateDate.ToString(),
                    Modifier = v.Modifier,
                    ModifiedDate = v.ModifiedDate.ToString()
                }).FirstOrDefaultAsync();

            return group;
        }

        public IEnumerable<GroupSearchModel> GetAll(Query query)
        {
            var queryData = from G in _context.Groups
                            select G;

            var result = QueryOperate<Group>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var groups =  result.ToList();

            var groupsSearch = groups.Select(c => new GroupSearchModel
            {
                GroupId = c.GroupId,
                GroupName = c.GroupName,
                ProjectId = c.ProjectId,
                SceneId = c.SceneId,
                Creator = c.Creator,
                CreateDate = c.CreateDate.ToString(),
                Modifier = c.Modifier,
                ModifiedDate = c.ModifiedDate.ToString()
            });

            return groupsSearch;
        }

        public async Task<IEnumerable<GroupSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from G in _context.Groups
                            select G;

            var result = QueryOperate<Group>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var groups = await result.ToListAsync();

            var groupsSearch = groups.Select(c => new GroupSearchModel
            {
                GroupId = c.GroupId,
                GroupName = c.GroupName,
                ProjectId = c.ProjectId,
                SceneId = c.SceneId,
                Creator = c.Creator,
                CreateDate = c.CreateDate.ToString(),
                Modifier = c.Modifier,
                ModifiedDate = c.ModifiedDate.ToString()
            });

            return groupsSearch;
        }

        public void RemoveById(int groupId)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);

            _context.Groups.Remove(group);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int groupId)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);

            _context.Groups.Remove(group);
           await _context.SaveChangesAsync();
        }

        public int UpdateById(int groupId, GroupUpdateModel mgroup)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);
            //Chech whether the Foreign key SceneId data exist
            if (mgroup.SceneId != null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == mgroup.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + mgroup.SceneId);
            }
            //Check Edit value which is unique in database ,already exist in system or not 
            var checkData = _context.Groups.Where(g => g.GroupName == mgroup.GroupName
                                                        && g.GroupId != groupId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '" + mgroup.GroupName + "' already exist in system.");


            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            group.GroupName = mgroup.GroupName;
            group.SceneId = mgroup.SceneId;
            group.Modifier = user.UserName;
            group.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
            return group.GroupId;
        }

        public async Task<int> UpdateByIdAsync(int groupId, GroupUpdateModel mgroup)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);
            //Chech whether the Foreign key SceneId data exist
            if (mgroup.SceneId != null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == mgroup.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + mgroup.SceneId);
            }
            //Check Edit value which is unique in database ,already exist in system or not 
            var checkData = await _context.Groups.Where(g => g.GroupName == mgroup.GroupName
                                                        && g.GroupId != groupId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '" + mgroup.GroupName + "' already exist in system.");


            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            group.GroupName = mgroup.GroupName;
            group.SceneId = mgroup.SceneId;
            group.Modifier = user.UserName;
            group.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return group.GroupId;
        }
        

    }
}
