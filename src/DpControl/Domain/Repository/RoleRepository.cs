using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;
using Microsoft.AspNet.Identity.EntityFramework;
using DpControl.Domain.Entities;

namespace DpControl.Domain.Repository
{
    public class RoleRepository : IRoleRepository
    {
        ShadingContext _context;
        private ILoginUserRepository _loginUser;

        #region construct
        public RoleRepository()
        {

        }
        public RoleRepository(ShadingContext context)
        {
            _context = context;
        }
        public RoleRepository(ShadingContext context, ILoginUserRepository loginUser)
        {
            _context = context;
            _loginUser = loginUser;
        }
        #endregion

        public async Task<string> AddAsync(RoleAddModel role)
        {
            //Role must be unique
            var checkData = await _context.Roles.Where(u => u.Name == role.Name).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("Role:" + role.Name + "' already exist in system");

            var model = new IdentityRole
            {
                Name = role.Name,
                NormalizedName = role.Name.ToUpper()
            };

            _context.Roles.Add(model);
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task<RoleSubSearchModel> FindByIdAsync(string roleId)
        {
            var result = _context.Roles.Where(v => v.Id == roleId);
            result = (IQueryable<IdentityRole>)ExpandOperator.ExpandRelatedEntities<IdentityRole>(result);

            var role = await result.FirstOrDefaultAsync();
            var roleSearch = RoleOperator.SetRoleSearchModelCascade(role);
            return roleSearch;
        }

        public async Task<IEnumerable<RoleSearchModel>> GetAllAsync()
        {
            var queryData = from R in _context.Roles
                            select R;

            var result = QueryOperate<IdentityRole>.Execute(queryData);
            result = (IQueryable<IdentityRole>)ExpandOperator.ExpandRelatedEntities<IdentityRole>(result);

            //以下执行完后才会去数据库中查询
            var roles = await result.ToListAsync();
            var rolesSearch = RoleOperator.SetRoleSearchModelCascade(roles);
            return rolesSearch;
        }
        
        public async Task RemoveByIdAsync(string roleId)
        {
            var role = _context.Roles.FirstOrDefault(u => u.Id == roleId);
            if (role == null)
                throw new ExpectException("Could not find data which RoleId equal to " + roleId);

            _context.Roles.Remove(role);

            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateByIdAsync(string roleId, RoleUpdateModel role)
        {
            var roleData = _context.Roles.FirstOrDefault(u => u.Id == roleId);
            if (roleData == null)
                throw new ExpectException("Could not find data which RoleId equal to " + roleId);

            roleData.Name = role.Name;
            roleData.NormalizedName = role.Name.ToUpper();

            await _context.SaveChangesAsync();
            return roleData.Id;

        }

        public async Task<IEnumerable<UserSubSearchModel>> GetUsersByRoleId(string roleId)
        {
            var loginUser = _loginUser.GetLoginUserInfo();

            var userIds = await _context.UserRoles
               .Where(u => u.RoleId == roleId)
               .Select(u => u.UserId).ToListAsync();

            var queryData = _context.Users.Where(r => userIds.Contains(r.Id));
            #region filter by loginUser
            if(!string.IsNullOrEmpty(loginUser.CustomerNo))
            {
                queryData = queryData.Where(u=>u.CustomerNo == loginUser.CustomerNo);
            }
            if (!string.IsNullOrEmpty(loginUser.ProjectNo))
            {
                queryData = queryData.Where(u => u.ProjectNo == loginUser.ProjectNo);
            }
            #endregion

            var result = QueryOperate<ApplicationUser>.Execute(queryData);

            var users = await result.ToListAsync();
            var usersSearch = UserOperator.SetUserSubSearchModel(users);
            return usersSearch;
        }

        /// <summary>
        /// Create Relations
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        public async Task CreateRelationsAsync(string roleId, string navigationProperty, List<string> navigationPropertyIds)
        {
            var roleData = await _context.Roles.FirstOrDefaultAsync(u => u.Id == roleId);
            if (roleData == null)
                throw new ExpectException("Could not find data which RoleId equal to " + roleId);

            switch (navigationProperty)
            {
                case "Users":

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //check navigationProperty already exist in system
                        var user = _context.Users.FirstOrDefault(r => r.Id == navigationId);
                        if (user == null)
                            throw new ExpectException("User data which UserId equal to " + navigationId + " not exist in system");

                        var userrole = _context.UserRoles
                            .Where(ur => ur.RoleId == roleId && ur.UserId == navigationId).ToList();
                        if (userrole.Count > 0)
                            throw new ExpectException("Relation:" + navigationId + " already exist in system");
                        //add relations
                        var relation = new IdentityUserRole<string>() { UserId = navigationId, RoleId = roleId };
                        _context.UserRoles.Add(relation);
                    }


                    break;
                default:
                    throw new ExpectException("No relation:" + navigationProperty);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveRelationsAsync(string roleId, string navigationProperty, List<string> navigationPropertyIds)
        {
            var roleData = await _context.Roles.FirstOrDefaultAsync(u => u.Id == roleId);
            if (roleData == null)
                throw new ExpectException("Could not find data which RoleId equal to " + roleId);

            switch (navigationProperty)
            {
                case "Users":

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //is relation exist in system 
                        var userrole = _context.UserRoles
                            .Where(ur => ur.RoleId == roleId && ur.UserId == navigationId).FirstOrDefault();
                        if (userrole == null)
                            throw new ExpectException("Relation:" + navigationId + " already exist in system");
                        _context.UserRoles.Remove(userrole);
                    }


                    break;
                default:
                    throw new ExpectException("No relation:" + navigationProperty);
            }

            await _context.SaveChangesAsync();
        }
    }
}
