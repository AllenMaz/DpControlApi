using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.Entities;
using DpControl.Utility.Authentication;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System.Threading;
using DpControl.Domain.IRepository;
using System.Security.Claims;
using IdentityModel.Constants;

namespace DpControl.Utility
{
    static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return _myTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            _myTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }
    }


    public class UserInfoManager : ILoginUserRepository
    {
        private readonly AbstractAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserInfoManager(AbstractAuthentication authentication
            , IHttpContextAccessor httpContextAccessor)
        {
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
        }

        public UserInfo GetLoginUserInfo()
        {
            UserInfo userInfo = new UserInfo();
            //ClaimsPrincipal
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var aa = claimsPrincipal.Claims.First(c => c.Type == JwtClaimTypes.Name);

            userInfo.UserName = claimsPrincipal.Claims.First(c=>c.Type == JwtClaimTypes.Name).Value;
            userInfo.Roles = claimsPrincipal.Claims.Where(c => c.Type == JwtClaimTypes.Role).Select(c=>c.Value).ToList();

            return userInfo;
        }
        

        /// <summary>
        /// Get UserInfo from Http Head
        /// Basic Authorization / Digest Authorization
        /// </summary>
        /// <returns></returns>
        private UserInfo GetUserInfoFromHttpHead()
        {
            UserInfo userInfo = new UserInfo();
            //call async method
            var user = Task.Run<ApplicationUser>(() => _authentication.GetUserInfoFromHttpHeadAsync(_httpContextAccessor.HttpContext)).Result;
            
            if (user != null)
            {
                //Construct UserInfo
                userInfo.UserName = user.UserName;

            }
            else
            {
                userInfo = null;
            }
            return userInfo;
        }

        /// <summary>
        /// Get UserInfo from Http Head
        /// Basic Authorization / Digest Authorization
        /// </summary>
        /// <returns></returns>
        private async Task<UserInfo> GetUserInfoFromHttpHeadAsync()
        {
            UserInfo userInfo = new UserInfo();
            var user = await _authentication.GetUserInfoFromHttpHeadAsync(_httpContextAccessor.HttpContext);
            if (user != null)
            {
                //Construct UserInfo
                userInfo.UserName = user.UserName;

            }
            else
            {
                userInfo = null;
            }
            return userInfo;
        }


        
    }
}
