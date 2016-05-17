﻿using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using DpControl.Domain.IRepository;
using DpControl.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class LoginUserInfo
    {
        public string UserName { get; set; }
        public string CustomerNo { get; set; }
        public string ProjectNo { get; set; }
        public int UserLevel { get; set; }

        public List<string> Roles { get; set; }

        public bool isAdmin
        {
            get
            {
               
                return Roles != null && Roles.Contains(Role.Admin) ? true : false;
            }
            set
            {
                value = Roles != null && Roles.Contains(Role.Admin) ? true : false;
            }
        }

        public bool isSuperLevel
        {
            get
            {

                return (int)DpControl.Domain.Entities.UserLevel.SuperLevel ==this.UserLevel  ? true : false;
            }
            set
            {
                value = (int)DpControl.Domain.Entities.UserLevel.SuperLevel == this.UserLevel ? true : false;
            }
        }

        public bool isCustomerLevel
        {
            get
            {

                return (int)DpControl.Domain.Entities.UserLevel.CustomerLevel == this.UserLevel ? true : false;
            }
            set
            {
                value = (int)DpControl.Domain.Entities.UserLevel.CustomerLevel == this.UserLevel ? true : false;
            }
        }

        public bool isProjectLevel
        {
            get
            {

                return (int)DpControl.Domain.Entities.UserLevel.ProjectLevel == this.UserLevel ? true : false;
            }
            set
            {
                value = (int)DpControl.Domain.Entities.UserLevel.ProjectLevel == this.UserLevel ? true : false;
            }
        }

        public bool hasCustomerNo
        {
            get {
                return string.IsNullOrEmpty(this.CustomerNo) ? false : true;
            }
            set
            {
                value = string.IsNullOrEmpty(this.CustomerNo) ? false : true;
            }
        }

        public bool hasProjectNo
        {
            get{
                return string.IsNullOrEmpty(this.ProjectNo) ? false : true;

            }
            set
            {
                value = string.IsNullOrEmpty(this.ProjectNo) ? false : true;
            }
        }
        
    }

    public class UserBaseModel
    {
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "UserLeveal is required!")]
        public int UserLevel { get; set; }

        public string CustomerNo { get; set; }
        public string ProjectNo { get; set; }
    }
    public class UserSubSearchModel: UserBaseModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        
    }

    public class UserSearchModel:UserSubSearchModel
    {
        public IEnumerable<GroupSubSearchModel> Groups { get; set; }     
        public IEnumerable<LocationSubSearchModel> Locations { get; set; }
        public IEnumerable<RoleSubSearchModel> Roles { get; set; }
    }

    public class UserAddModel : UserBaseModel
    {
        [Required]
        public string UserName { get; set; }
        

        [Required]
        public string Password { get; set; }
    }

    public class UserUpdateModel : UserBaseModel
    {

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
            ShadingContext context = new ShadingContext();
            if (user == null) return null;
            var userSearchModel = new UserSearchModel
            {
                UserId = user.Id,
                UserLevel = user.UserLevel,
                UserName = user.UserName,
                Groups = user.UserGroups.Select(v => GroupOperator.SetGroupSubSearchModel(v.Group)),
                Locations = user.UserLocations.Select(v => LocationOperator.SetLocationSubSearchModel(v.Location)),
                Roles = user.Roles.Select(v=>RoleOperator.SetRoleSubSearchModel(context.Roles.FirstOrDefault(r=>r.Id == v.RoleId)))
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
