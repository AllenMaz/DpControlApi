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
        private readonly ILoginUserRepository _loginUser;

        #region Constructors
        public GroupRepository()
        {
        }

        public GroupRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        public GroupRepository(ShadingContext dbContext, ILoginUserRepository loginUser)
        {
            _context = dbContext;
            _loginUser = loginUser;
        }
        #endregion

        public int Add(GroupAddModel group)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == group.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + group.ProjectId);
            
            //If SceneId not null,check whether corresponding Scenes data existed
            if (group.SceneId != null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == group.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + group.SceneId);
            }

            //GroupName must be unique
            var checkData = _context.Groups.Where(g => g.GroupName == group.GroupName).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '" + group.GroupName + "' already exist in system.");

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

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

            //If SceneId not null,check whether corresponding Scenes data existed
            if (group.SceneId !=null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == group.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + group.SceneId);
            }

            //GroupName must be unique
            var checkData =await _context.Groups.Where(g => g.GroupName == group.GroupName).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '"+group.GroupName + "' already exist in system.");

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

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
            var result = _context.Groups.Where(v => v.GroupId == groupId);
            result = (IQueryable<Group>)ExpandOperator.ExpandRelatedEntities<Group>(result);

            var group = result.FirstOrDefault();
            var groupSearch = GroupOperator.SetGroupSearchModelCascade(group);

            return groupSearch;
        }

        public async Task<GroupSearchModel> FindByIdAsync(int groupId)
        {
            var result = _context.Groups.Where(v => v.GroupId == groupId);
            result = (IQueryable<Group>)ExpandOperator.ExpandRelatedEntities<Group>(result);

            var group = await result.FirstOrDefaultAsync();
            var groupSearch = GroupOperator.SetGroupSearchModelCascade(group);

            return groupSearch;
        }

        public IEnumerable<GroupSearchModel> GetAll()
        {
            var queryData = from G in _context.Groups
                            select G;

            #region filter by login user
            var loginUser = _loginUser.GetLoginUserInfo();
            if (loginUser.isCustomerLevel)
            {
                var customer = _context.Customers
                    .Include(c => c.Projects)
                    .Where(c => c.CustomerNo == loginUser.CustomerNo).FirstOrDefault();
                var projectIds = customer.Projects.Select(p => p.ProjectId);
                queryData = queryData.Where(s => projectIds.Contains((int)s.ProjectId));

            }
            else if (loginUser.isProjectLevel)
            {
                var project = _context.Projects.Where(p => p.ProjectNo == loginUser.ProjectNo).FirstOrDefault();
                queryData = queryData.Where(s => s.ProjectId == project.ProjectId);
            }
            #endregion

            var result = QueryOperate<Group>.Execute(queryData);
            result = (IQueryable<Group>)ExpandOperator.ExpandRelatedEntities<Group>(result);

            //以下执行完后才会去数据库中查询
            var groups =  result.ToList();
            var groupsSearch = GroupOperator.SetGroupSearchModelCascade(groups);

            return groupsSearch;
        }

        public async Task<IEnumerable<GroupSearchModel>> GetAllAsync()
        {
            var queryData = from G in _context.Groups
                            select G;

            #region filter by login user
            var loginUser = _loginUser.GetLoginUserInfo();
            if (loginUser.isCustomerLevel)
            {
                var customer = _context.Customers
                    .Include(c => c.Projects)
                    .Where(c => c.CustomerNo == loginUser.CustomerNo).FirstOrDefault();
                var projectIds = customer.Projects.Select(p => p.ProjectId);
                queryData = queryData.Where(s => projectIds.Contains((int)s.ProjectId));

            }
            else if (loginUser.isProjectLevel)
            {
                var project = _context.Projects.Where(p => p.ProjectNo == loginUser.ProjectNo).FirstOrDefault();
                queryData = queryData.Where(s => s.ProjectId == project.ProjectId);
            }
            #endregion

            var result = QueryOperate<Group>.Execute(queryData);
            result = (IQueryable<Group>)ExpandOperator.ExpandRelatedEntities<Group>(result);

            //以下执行完后才会去数据库中查询
            var groups = await result.ToListAsync();
            var groupsSearch = GroupOperator.SetGroupSearchModelCascade(groups);

            return groupsSearch;
        }

        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsByGroupIdAsync(int groupId)
        {
            var queryData = _context.GroupLocations
                .Where(gl=>gl.GroupId == groupId)
                .Select(gl => gl.Location);

            var result = QueryOperate<Location>.Execute(queryData);
            var locations = await result.ToListAsync();
            var locationsSearch = LocationOperator.SetLocationSubSearchModel(locations);
            return locationsSearch;
        }

        public async Task<ProjectSubSearchModel> GetProjectByGroupIdAsync(int groupId)
        {
            var group = await _context.Groups
                .Include(g => g.Project)
                .Where(g => g.GroupId == groupId).FirstOrDefaultAsync();
            var project = group == null ? null : group.Project;
            var projectSearch = ProjectOperator.SetProjectSubSearchModel(project);
            return projectSearch;
        }

        public async Task<SceneSubSearchModel> GetSceneByGroupIdAsync(int groupId)
        {
            var group = await _context.Groups
                .Include(g => g.Scene)
                .Where(g => g.GroupId == groupId).FirstOrDefaultAsync();
            var scene = group == null ? null : group.Scene;
            var sceneSearch = SceneOperator.SetSceneSubSearchModel(scene);
            return sceneSearch;
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
            //If SceneId not null,check whether corresponding Scenes data existed
            if (mgroup.SceneId != null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == mgroup.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + mgroup.SceneId);
            }
            //GroupName must be unique
            var checkData = _context.Groups.Where(g => g.GroupName == mgroup.GroupName
                                                        && g.GroupId != groupId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '" + mgroup.GroupName + "' already exist in system.");


            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

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
            //If SceneId not null,check whether corresponding Scenes data existed
            if (mgroup.SceneId != null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == mgroup.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + mgroup.SceneId);
            }
            //GroupName must be unique
            var checkData = await _context.Groups.Where(g => g.GroupName == mgroup.GroupName
                                                        && g.GroupId != groupId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '" + mgroup.GroupName + "' already exist in system.");


            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            group.GroupName = mgroup.GroupName;
            group.SceneId = mgroup.SceneId;
            group.Modifier = user.UserName;
            group.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return group.GroupId;
        }

        public async Task CreateRelationsAsync(int groupId, string navigationProperty, List<string> navigationPropertyIds)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(u => u.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);

            switch (navigationProperty)
            {
                case "Users":

                    foreach (string userId in navigationPropertyIds)
                    {
                        //is navigationProperty already exist in system
                        var user = _context.Users.FirstOrDefault(u=>u.Id == userId);
                        if (user == null)
                            throw new ExpectException("User data which UserId equal to " + userId + " not exist in system");
                        //is relationship already exist in system
                        var usergroup = _context.UserGroups
                            .Where(ul => ul.GroupId == groupId && ul.UserId == userId).ToList();
                        if (usergroup.Count > 0)
                            throw new ExpectException("Relation:" + userId + " already exist in system");
                        //add relations
                        var relation = new UserGroup() { UserId = userId, GroupId = groupId };
                        _context.UserGroups.Add(relation);
                    }


                    break;
                case "Locations":

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //conver navigationId to int
                        int locationId = Utilities.ConverRelationIdToInt(navigationId);
                        //is navigationProperty already exist in system
                        var location = _context.Locations.FirstOrDefault(r => r.LocationId == locationId);
                        if (location == null)
                            throw new ExpectException("Location data which LocationId equal to " + navigationId + " not exist in system");
                        //is relationship already exist in system
                        var grouplocation = _context.GroupLocations
                            .Where(ul => ul.LocationId == locationId && ul.GroupId == groupId).ToList();
                        if (grouplocation.Count > 0)
                            throw new ExpectException("Relation:" + navigationId + " already exist in system");
                        //add relations
                        var relation = new GroupLocation() { GroupId = groupId, LocationId = locationId };
                        _context.GroupLocations.Add(relation);
                    }

                    break;
                default:
                    throw new ExpectException("No relation:" + navigationProperty);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveRelationsAsync(int groupId, string navigationProperty, List<string> navigationPropertyIds)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(u => u.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);

            switch (navigationProperty)
            {
                case "Users":

                    foreach (string userId in navigationPropertyIds)
                    {
                       //is relationship already exist in system
                        var usergroup = _context.UserGroups
                            .Where(ul => ul.GroupId == groupId && ul.UserId == userId).FirstOrDefault();
                        if (usergroup == null)
                            throw new ExpectException("Relation:" + userId + " not exist in system");
                        //remove relations
                        _context.UserGroups.Remove(usergroup);
                    }


                    break;
                case "Locations":

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //conver navigationId to int
                        int locationId = Utilities.ConverRelationIdToInt(navigationId);//is relationship already exist in system
                        var grouplocation = _context.GroupLocations
                            .Where(ul => ul.LocationId == locationId && ul.GroupId == groupId).FirstOrDefault();
                        if (grouplocation == null)
                            throw new ExpectException("Relation:" + navigationId + " not exist in system");
                        //remove relations
                        _context.GroupLocations.Remove(grouplocation);
                    }

                    break;
                default:
                    throw new ExpectException("No relation:" + navigationProperty);
            }

            await _context.SaveChangesAsync();
        }
    }
}
