using DpControl.Domain.EFContext;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class RoleBaseModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class RoleAddModel: RoleBaseModel
    {

    }

    public class RoleUpdateModel : RoleBaseModel
    {

    }

    public class RoleSubSearchModel:RoleBaseModel
    {
        public string RoleId { get; set; }
    }

    public class RoleSearchModel : RoleSubSearchModel
    {
        public IEnumerable<UserSubSearchModel> Users { get; set; }
    }

    public static class RoleOperator
    {
        /// <summary>
        /// Cascade set RoleSearchModel Results
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static IEnumerable<RoleSearchModel> SetRoleSearchModelCascade(List<IdentityRole> roles)
        {
            var roleSearchModels = roles.Select(s => SetRoleSearchModelCascade(s));
            return roleSearchModels;
        }

        /// <summary>
        /// Cascade set RoleSearchModel Result
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static RoleSearchModel SetRoleSearchModelCascade(IdentityRole role)
        {
            ShadingContext context = new ShadingContext();
            if (role == null) return null;
            var roleSearchModel = new RoleSearchModel()
            {
               RoleId = role.Id,
               Name = role.Name,
               Users = role.Users.Select(v => UserOperator.SetUserSubSearchModel(context.Users.FirstOrDefault(r => r.Id == v.UserId)))
                

            };
            return roleSearchModel;
        }

        /// <summary>
        /// Cascade set RoleSubSearchModel Results
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static IEnumerable<RoleSubSearchModel> SetRoleSubSearchModel(List<IdentityRole> roles)
        {
            var roleSearchModels = roles.Select(s => SetRoleSubSearchModel(s));
            return roleSearchModels;
        }

        /// <summary>
        /// Cascade set RoleSubSearchModel Result
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static RoleSubSearchModel SetRoleSubSearchModel(IdentityRole role)
        {
            if (role == null) return null;
            var roleSearchModel = new RoleSubSearchModel()
            {
                RoleId = role.Id,
                Name = role.Name
            };
            return roleSearchModel;
        }
    }
}
