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
using Microsoft.AspNet.Identity.EntityFramework;

namespace DpControl.Domain.Repository
{
    public class UserRepository : IUserRepository
    {

        ShadingContext _context;
        private ILoginUserRepository _loginUser;

        #region construct
        public UserRepository()
        {

        }
        public UserRepository(ShadingContext context)
        {
            _context = context;
        }

        public UserRepository(ShadingContext context, ILoginUserRepository loginUser)
        {
            _context = context;
            _loginUser = loginUser;
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

        public UserSubSearchModel FindByName(string userName)
        {
            var result = _context.Users.Where(v => v.UserName == userName);
            result = (IQueryable<ApplicationUser>)ExpandOperator.ExpandRelatedEntities<ApplicationUser>(result);

            var user = result.FirstOrDefault();
            var userSearch = UserOperator.SetUserSearchModelCascade(user);
            return userSearch;
        }

        public async Task<IEnumerable<UserSearchModel>> GetAllAsync()
        {
            var queryData = from U in _context.Users
                            select U;

            #region extra filter condition by current login user
            var user = _loginUser.GetLoginUserInfo();
            
            if (!string.IsNullOrEmpty(user.CustomerNo))
                queryData = queryData.Where(u => u.CustomerNo == user.CustomerNo);
            if (!string.IsNullOrEmpty(user.ProjectNo))
                queryData = queryData.Where(u => u.ProjectNo == user.ProjectNo);

            #endregion

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

        public async Task<IEnumerable<RoleSubSearchModel>> GetRolesByUserId(string userId)
        {
            var roleIds = await _context.UserRoles
               .Where(u => u.UserId == userId)
               .Select(u => u.RoleId).ToListAsync();

            var queryData = _context.Roles .Where(r => roleIds.Contains(r.Id));
            var result = QueryOperate<IdentityRole>.Execute(queryData);

            var roles = await result.ToListAsync();
            var rolesSearch = RoleOperator.SetRoleSubSearchModel(roles);
            return rolesSearch;
        }

        /// <summary>
        /// Creation Relations 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        public async Task CreateRelationsAsync(string userId, string navigationProperty,List<string> navigationPropertyIds)
        {
            var userData = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userData == null)
                throw new ExpectException("Could not find data which UserId equal to " + userId);
            
            //获取当前登录用户信息
            var loginUser = _loginUser.GetLoginUserInfo();

            switch (navigationProperty)
            {
                case "Roles":

                    foreach(string navigationId in navigationPropertyIds)
                    {
                        //check navigationProperty already exist in system
                        var role = _context.Roles.FirstOrDefault(r => r.Id == navigationId);
                        if(role == null)
                            throw new ExpectException("Role data which RoleId equal to " + navigationId +" not exist in system");

                        var userrole = _context.UserRoles
                            .Where(ur=>ur.RoleId == navigationId && ur.UserId == userId).ToList();
                        if(userrole.Count >0)
                            throw new ExpectException("Relation:" + navigationId + " already exist in system");
                        //add relations
                        var relation = new IdentityUserRole<string>() { UserId = userId,RoleId = navigationId};
                        _context.UserRoles.Add(relation);
                    }
                 

                    break;
                case "Groups":
                    //只有ProjectLevel级别的管理员才有权限操作
                    if (!loginUser.isProjectLevel)
                        throw new UnauthorizedException();

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //conver navigationId to int
                        int navId = Utilities.ConverRelationIdToInt(navigationId);
                        //check navigationProperty already exist in system
                        var group = _context.Groups.FirstOrDefault(r => r.GroupId == navId);
                        if (group == null)
                            throw new ExpectException("Group data which GroupId equal to " + navigationId + " not exist in system");
                        //is relationship already exist in system
                        var usergroup = _context.UserGroups
                            .Where(ug =>ug.GroupId == navId && ug.UserId == userId).ToList();
                        if (usergroup.Count > 0)
                            throw new ExpectException("Relation:" + navigationId + " already exist in system");
                        //add relations
                        var relation = new UserGroup() { UserId = userId, GroupId = navId };
                        _context.UserGroups.Add(relation);
                    }

                    break;
                case "Locations":

                    //只有ProjectLevel级别的管理员才有权限操作
                    if (!loginUser.isProjectLevel)
                        throw new UnauthorizedException();

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //conver navigationId to int
                        int navId = Utilities.ConverRelationIdToInt(navigationId);
                        //is navigationProperty already exist in system
                        var location = _context.Locations.FirstOrDefault(r => r.LocationId == navId);
                        if (location == null)
                            throw new ExpectException("Location data which LocationId equal to " + navigationId + " not exist in system");
                        //is relationship already exist in system
                        var userlocation = _context.UserLocations
                            .Where(ul => ul.LocationId == navId && ul.UserId == userId).ToList();
                        if (userlocation.Count > 0)
                            throw new ExpectException("Relation:" + navigationId + " already exist in system");
                        //add relations
                        var relation = new UserLocation() { UserId = userId, LocationId = navId };
                        _context.UserLocations.Add(relation);
                    }

                    break;
                default:
                    throw new ExpectException("No relation:" + navigationProperty);
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove Relations
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        public async Task RemoveRelationsAsync(string userId, string navigationProperty, List<string> navigationPropertyIds)
        {
            var userData = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userData == null)
                throw new ExpectException("Could not find data which UserId equal to " + userId);
            
            switch (navigationProperty)
            {
                case "Roles":

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        // relation exist or not 
                        var userrole = _context.UserRoles
                            .Where(ur => ur.RoleId == navigationId && ur.UserId == userId).FirstOrDefault();
                        if (userrole == null)
                            throw new ExpectException("Relation:" + navigationId + " not exist in system");
                        //remove relations
                        _context.UserRoles.Remove(userrole);
                    }


                    break;
                case "Groups":

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //conver navigationId to int
                        int navId = Utilities.ConverRelationIdToInt(navigationId);
                       //is relationship already exist in system
                        var usergroup = _context.UserGroups
                            .Where(ug => ug.GroupId == navId && ug.UserId == userId).FirstOrDefault();
                        if (usergroup == null)
                            throw new ExpectException("Relation:" + navigationId + " not exist in system");
                       
                        _context.UserGroups.Remove(usergroup);
                    }

                    break;
                case "Locations":

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //conver navigationId to int
                        int navId = Utilities.ConverRelationIdToInt(navigationId);
                       //is relationship already exist in system
                        var userlocation = _context.UserLocations
                            .Where(ul => ul.LocationId == navId && ul.UserId == userId).FirstOrDefault();
                        if (userlocation == null)
                            throw new ExpectException("Relation:" + navigationId + " not exist in system");
                        
                        _context.UserLocations.Remove(userlocation);
                    }

                    break;
                default:
                    throw new ExpectException("No relation:" + navigationProperty);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> AddAsync(UserAddModel user)
        {
            //UserName must be unique
            var checkData = await _context.Users.Where(u => u.UserName == user.UserName).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("User:" +user.UserName + "' already exist in system");

            CheckConditionByLoginUser(user.CustomerNo,user.ProjectNo);

            // Hash password
            var passwordHash = new PasswordHasher().HashPassword(null, user.Password);

            var model = new ApplicationUser
            {
                UserName = user.UserName,
                NormalizedUserName = user.UserName.ToUpper(),
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

            CheckConditionByLoginUser(user.CustomerNo, user.ProjectNo);

            userData.Email = user.Email;
            userData.PhoneNumber = user.PhoneNumber;
            userData.CustomerNo = user.CustomerNo;
            userData.ProjectNo = user.ProjectNo;

            await _context.SaveChangesAsync();
            return userData.Id;
        }

        /// <summary>
        /// Check condition by current login user
        /// </summary>
        private void CheckConditionByLoginUser(string customerNo,string projectNo)
        {
            #region CustomerNo and ProjectNo
            var loginUser = _loginUser.GetLoginUserInfo();

            if (loginUser.isSuperLevel)
            {
                #region if current login user's CustomerNo and ProjectNo are empty,then check user add logic
                //如果新增用户的ProjectNo不为空，则
                if (!string.IsNullOrEmpty(projectNo))
                {
                    if (string.IsNullOrEmpty(customerNo))
                        throw new ExpectException("CustomerNo is required");

                    //ProjectNo must existed in system
                    if (!SystemExistProjectNo(projectNo))
                        throw new ExpectException("Could not find Project data which ProjectNo equal to " + projectNo);

                    //judge if CustomerNo is exist in system
                    if (!SystemExistCustomerNo(customerNo))
                        throw new ExpectException("Could not find Customer data which CustomerNo equal to " + customerNo);

                    // Project must belong to Customer
                    if (!ProjectBelongToCustomer(customerNo, projectNo))
                        throw new ExpectException("The Project " + projectNo + " is not belong to Customer " + customerNo);

                }
                else
                {
                    //如果新增用户的CustomerNo不为空，则
                    if (!string.IsNullOrEmpty(customerNo))
                    {
                        //judge if CustomerNo is exist in system
                        if (!SystemExistCustomerNo(customerNo))
                            throw new ExpectException("Could not find Customer data which CustomerNo equal to " + customerNo);

                    }
                }
                #endregion
                
            }
            else if (loginUser.isCustomerLevel)
            {
                #region if current login user's only has CustomerNo ,then check user add logic
                //新增用户的CustomerNo必须与当前登录用户的CustomerNo一致
                if (loginUser.CustomerNo != customerNo)
                    throw new ExpectException("CustomerNo '" + customerNo + "' and current user's CustomerNo is not match.");

                //如果新增用户的ProjectNo不为空
                if (!string.IsNullOrEmpty(projectNo))
                {
                    //ProjectNo must existed in system
                    if (!SystemExistProjectNo(projectNo))
                        throw new ExpectException("Could not find Project data which ProjectNo equal to " + projectNo);

                    // Project must belong to Customer
                    if (!ProjectBelongToCustomer(customerNo, projectNo))
                        throw new ExpectException("The Project " + projectNo + " is not belong to Customer " + customerNo);

                }
                #endregion
            }
            else if (loginUser.isProjectLevel)
            {

                #region if current login user has both CustomerNo and ProjectNo
                //新增用户的CustomerNo必须与当前登录用户的CustomerNo一致
                if (loginUser.CustomerNo != customerNo)
                    throw new ExpectException("CustomerNo '" + customerNo + "' and current user's CustomerNo is not match.");

                //ProjectNo must be match current login user's ProjectNo
                if (loginUser.ProjectNo != projectNo)
                    throw new ExpectException("ProjectNo '" + projectNo + "' and current user's ProjectNo is not match.");
                #endregion
            }

            #endregion
        }
        

        /// <summary>
        /// Is CustomerNo existed in system
        /// </summary>
        /// <returns></returns>
        private bool SystemExistCustomerNo(string customerNo)
        {
            bool exist = true;
            if (!string.IsNullOrEmpty(customerNo))
            {
                var customer = _context.Customers.FirstOrDefault(c => c.CustomerNo == customerNo);
                if (customer == null) exist = false;
            }
            return exist;
        }

        /// <summary>
        /// Is ProjectNo existed in system
        /// </summary>
        /// <param name="projectNo"></param>
        /// <returns></returns>
        private bool SystemExistProjectNo(string projectNo)
        {
            bool exist = true;
            if (!string.IsNullOrEmpty(projectNo))
            {
                var project = _context.Projects.FirstOrDefault(p => p.ProjectNo == projectNo);
                if (project == null) exist = false;
            }
            return exist;
        }

        /// <summary>
        /// Is ProjectNo Belong to CustomerNo
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="projectNo"></param>
        /// <returns></returns>
        private bool ProjectBelongToCustomer(string customerNo,string projectNo)
        {
            bool projectBelongToCustomer = true;
            if (!string.IsNullOrEmpty(customerNo) && !string.IsNullOrEmpty(projectNo))
            {
                var customer = _context.Customers
                    .Include(c => c.Projects)
                    .FirstOrDefault(c=>c.CustomerNo == customerNo);
                if (customer ==null || !customer.Projects.Any(p=>p.ProjectNo == projectNo))
                {
                    projectBelongToCustomer = false;
                }
            }

            return projectBelongToCustomer;
        }

       
    }
}
