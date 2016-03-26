using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class UserInfo
    {
        public string UserName { get; set; }

        public List<string> Roles { get; set; }
    }

    public class UserSubSearchModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    public class UserSearchModel:UserSubSearchModel
    {
        public IEnumerable<GroupSubSearchModel> Groups { get; set; }     
        public IEnumerable<LocationSubSearchModel> Locations { get; set; }
    }

    public static class UserOperator
    {
        /// <summary>
        /// Cascade set UserSearchModel Results
        /// </summary>
        public static IEnumerable<UserSearchModel> SetUserSearchModelCascade(List<ApplicationUser> users)
        {
            var userSearchModels = users.Select(c => SetUserSearchModelCascade(c));

            return userSearchModels;
        }

        /// <summary>
        /// Cascade set UserSearchModel Result
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserSearchModel SetUserSearchModelCascade(ApplicationUser user)
        {
            if (user == null) return null;
            var userSearchModel = new UserSearchModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Groups = user.UserGroups.Select(v => GroupOperator.SetGroupSubSearchModel(v.Group)),
                Locations = user.UserLocations.Select(v => LocationOperator.SetLocationSubSearchModel(v.Location))
            };

            return userSearchModel;
        }

        /// <summary>
        /// Cascade set UserSubSearchModel Results
        /// </summary>
        public static IEnumerable<UserSubSearchModel> SetUserSubSearchModel(List<ApplicationUser> users)
        {
            var userSearchModels = users.Select(c => SetUserSubSearchModel(c));

            return userSearchModels;
        }

        /// <summary>
        /// Cascade set UserSubSearchModel Result
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserSubSearchModel SetUserSubSearchModel(ApplicationUser user)
        {
            if (user == null) return null;
            var userSearchModel = new UserSubSearchModel
            {
                UserId = user.Id,
                UserName = user.UserName
            };

            return userSearchModel;
        }
    }
}
