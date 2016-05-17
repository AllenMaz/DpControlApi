using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using DpControl.Domain.Repository;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Utility
{
    public static class DataSeeder
    {
        // TODO: Move this code when seed data is implemented in EF 7

        /// <summary>
        /// This is a workaround for missing seed data functionality in EF 7.0-rc1
        /// More info: https://github.com/aspnet/EntityFramework/issues/629
        /// </summary>
        /// <param name="app">
        /// An instance that provides the mechanisms to get instance of the database context.
        /// </param>
        public static void SeedData(this IApplicationBuilder app)
        {
            ShadingContext dbcontext = (ShadingContext)app.ApplicationServices.GetService(typeof(ShadingContext));

            string adminRoleId = string.Empty;
            #region init roles:Admin,Enginee,CustomerService,NormalUser
            #region Add Admin Role
            var adminRole = dbcontext.Roles.FirstOrDefault(r => r.Name == Role.Admin);
            if (adminRole == null)
            {
                var adminModel = new IdentityRole
                {
                    Name = Role.Admin,
                    NormalizedName = Role.Admin.ToUpper()
                };
                dbcontext.Add(adminModel);
                adminRoleId = adminModel.Id;
            }
            else
            {
                adminRoleId = adminRole.Id;
            }

            #endregion
            #region Add Engineer Role
            var engineerRole = dbcontext.Roles.FirstOrDefault(r => r.Name == Role.Engineer);
            if (engineerRole == null)
            {
                var engineerModel = new IdentityRole
                {
                    Name = Role.Engineer,
                    NormalizedName = Role.Engineer.ToUpper()
                };
                dbcontext.Add(engineerModel);
            }
            #endregion
            #region Add CustomerService Role
            var customerserviceRole = dbcontext.Roles.FirstOrDefault(r => r.Name == Role.CustomerService);
            if (customerserviceRole == null)
            {
                var customerserviceModel = new IdentityRole
                {
                    Name = Role.CustomerService,
                    NormalizedName = Role.CustomerService.ToUpper()
                };
                dbcontext.Add(customerserviceModel);
            }
            #endregion
            #region Add NormalUser Role
            var normaluserRole = dbcontext.Roles.FirstOrDefault(r => r.Name == Role.NormalUser);
            if (normaluserRole == null)
            {
                var normaluserModel = new IdentityRole
                {
                    Name = Role.NormalUser,
                    NormalizedName = Role.NormalUser.ToUpper()
                };
                dbcontext.Add(normaluserModel);
            }
            #endregion
            #endregion
            #region Init Admin User
            string userName = "Admin";
            string password = "admin123";
            var user = dbcontext.Users.FirstOrDefault(u => u.UserName == userName);
            if(user == null)
            {
                // Hash password
                var passwordHash = new PasswordHasher().HashPassword(null, password);

                var userModel = new ApplicationUser
                {
                    UserName = userName,
                    NormalizedUserName = userName.ToUpper(),
                    PasswordHash = passwordHash,
                    UserLevel = (int)UserLevel.SuperLevel
                };

                dbcontext.Add(userModel);
                //Add User to Admin
                var relation = new IdentityUserRole<string>() { UserId = userModel.Id, RoleId = adminRoleId };
                dbcontext.Add(relation);
            }
            #endregion


            dbcontext.SaveChanges();
        }
    }
}
