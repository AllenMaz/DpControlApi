using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Utility.Authentication;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System.Threading;

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

    public class UserInfoManager : IUserInfoRepository
    {
        private readonly AbstractAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserInfoManager(AbstractAuthentication authentication
            , IHttpContextAccessor httpContextAccessor)
        {
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get UserInfo
        /// </summary>
        /// <returns></returns>
        public UserInfo GetUserInfo()
        {
            UserInfo userInfo = new UserInfo();
            //call async method
            var user = Task.Run<ApplicationUser>(() => _authentication.GetUserInfo(_httpContextAccessor.HttpContext)).Result;
            
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
        /// Get UserInfo
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfo> GetUserInfoAsync()
        {
            UserInfo userInfo = new UserInfo();
            var user = await _authentication.GetUserInfo(_httpContextAccessor.HttpContext);
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
