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

        #region construct
        public UserRepository()
        {

        }
        public UserRepository(ShadingContext context)
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
        public async Task CreateRelationsAsync(string userId,string navigationProperty,List<string> navigationPropertyIds)
        {
            var userData = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userData == null)
                throw new ExpectException("Could not find data which UserId equal to " + userId);

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

            #region UserLeveal
            switch (user.UserLevel)
            {
                case (int)UserLevel.SuperLevel:
                    //if SuperLevel ,set CustomerNo and ProjectNo to be empty 
                    user.CustomerNo = string.Empty;
                    user.ProjectNo = string.Empty;
                    break;
                case (int)UserLevel.CustomerLevel:
                    //if CustomerLevel, CustomerNo is required and Set ProjectNo to be empty
                    if (string.IsNullOrEmpty(user.CustomerNo))
                        throw new ExpectException("When CustomerLevel CustomerNo is required ");
                    if (!SystemExistCustomerNo(user.CustomerNo))
                        throw new ExpectException("Could not find Customer data which CustomerNo equal to " + user.CustomerNo);

                    user.ProjectNo = string.Empty;
                    break;
                case (int)UserLevel.ProjectLevel:
                    //if ProjectLevel,CustomerNo and ProjectNo is required
                    if (string.IsNullOrEmpty(user.CustomerNo))
                        throw new ExpectException("When ProjectLevel CustomerNo is required ");
                    if (string.IsNullOrEmpty(user.ProjectNo))
                        throw new ExpectException("When ProjectLevel ProjectNo is required ");

                    //CustomerNo and ProjectNo must existed in system
                    if (!SystemExistCustomerNo(user.CustomerNo))
                        throw new ExpectException("Could not find Customer data which CustomerNo equal to " + user.CustomerNo);
                    if (!SystemExistProjectNo(user.ProjectNo))
                        throw new ExpectException("Could not find Project data which ProjectNo equal to " + user.ProjectNo);

                    // Project must belong to Customer
                    if (!ProjectBelongToCustomer(user.CustomerNo, user.ProjectNo))
                        throw new ExpectException("The Project " + user.ProjectNo + " is not belong to Customer " + user.CustomerNo);

                    break;
                default: break;
            }
            #endregion
            // Hash password
            var passwordHash = new PasswordHasher().HashPassword(null, user.Password);

            var model = new ApplicationUser
            {
                UserName = user.UserName,
                NormalizedUserName = user.UserName.ToUpper(),
                PasswordHash = passwordHash,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserLevel = user.UserLevel,
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

            #region UserLeveal
            switch (user.UserLevel)
            {
                case (int)UserLevel.SuperLevel:
                    //if SuperLevel ,set CustomerNo and ProjectNo to be empty 
                    user.CustomerNo = string.Empty;
                    user.ProjectNo = string.Empty;
                    break;
                case (int)UserLevel.CustomerLevel:
                    //if CustomerLevel, CustomerNo is required and Set ProjectNo to be empty
                    if (string.IsNullOrEmpty(user.CustomerNo))
                        throw new ExpectException("When CustomerLevel CustomerNo is required ");
                    if (!SystemExistCustomerNo(user.CustomerNo))
                        throw new ExpectException("Could not find Customer data which CustomerNo equal to " + user.CustomerNo);

                    user.ProjectNo = string.Empty;
                    break;
                case (int)UserLevel.ProjectLevel:
                    //if ProjectLevel,CustomerNo and ProjectNo is required
                    if (string.IsNullOrEmpty(user.CustomerNo))
                        throw new ExpectException("When ProjectLevel CustomerNo is required ");
                    if (string.IsNullOrEmpty(user.ProjectNo))
                        throw new ExpectException("When ProjectLevel ProjectNo is required ");

                    //CustomerNo and ProjectNo must existed in system
                    if (!SystemExistCustomerNo(user.CustomerNo))
                        throw new ExpectException("Could not find Customer data which CustomerNo equal to " + user.CustomerNo);
                    if (!SystemExistProjectNo(user.ProjectNo))
                        throw new ExpectException("Could not find Project data which ProjectNo equal to " + user.ProjectNo);

                    // Project must belong to Customer
                    if (!ProjectBelongToCustomer(user.CustomerNo, user.ProjectNo))
                        throw new ExpectException("The Project " + user.ProjectNo + " is not belong to Customer " + user.CustomerNo);

                    break;
                default: break;
            }
            #endregion

            userData.Email = user.Email;
            userData.PhoneNumber = user.PhoneNumber;
            userData.CustomerNo = user.CustomerNo;
            userData.ProjectNo = user.ProjectNo;
            userData.UserLevel = user.UserLevel;

            await _context.SaveChangesAsync();
            return userData.Id;
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
