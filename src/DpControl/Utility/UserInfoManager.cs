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
using Microsoft.AspNet.Identity;

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
        private readonly UserManager<ApplicationUser> _userManager;

        public UserInfoManager(AbstractAuthentication authentication,
             IHttpContextAccessor httpContextAccessor,
             UserManager<ApplicationUser> userManager)
        {
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        
        public LoginUserInfo GetLoginUserInfo()
        {
            var loginUserInfo = this.GetOAuth2LoginUserInfo();
            //var loginUserInfo = this.GetBasicLoginUserInfo();
            return loginUserInfo;
        }

        

        public async Task<LoginUserInfo> GetLoginUserInfoAsync()
        {
            var loginUserInfo = await this.GetOAuth2LoginUserInfoAsync();
            return loginUserInfo;
        }

        private LoginUserInfo GetOAuth2LoginUserInfo()
        {
            LoginUserInfo loginUserInfo = new LoginUserInfo();
            //ClaimsPrincipal 
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var userName = claimsPrincipal.Claims.First(c => c.Type == JwtClaimTypes.Name).Value;

            var user = Task.Run<ApplicationUser>(() => _userManager.FindByNameAsync(userName)).Result;

            loginUserInfo.UserName = userName;
            loginUserInfo.UserLevel = user.UserLevel;
            loginUserInfo.CustomerNo = user.CustomerNo;
            loginUserInfo.ProjectNo = user.ProjectNo;
            loginUserInfo.Roles = claimsPrincipal.Claims.Where(c => c.Type == JwtClaimTypes.Role).Select(c => c.Value).ToList();

            return loginUserInfo;
        }



        private async Task<LoginUserInfo> GetOAuth2LoginUserInfoAsync()
        {
            LoginUserInfo loginUserInfo = new LoginUserInfo();
            //ClaimsPrincipal
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var userName = claimsPrincipal.Claims.First(c => c.Type == JwtClaimTypes.Name).Value;

            var user = await _userManager.FindByNameAsync(userName);
            loginUserInfo.UserName = userName;
            loginUserInfo.UserLevel = user.UserLevel;
            loginUserInfo.CustomerNo = user.CustomerNo;
            loginUserInfo.ProjectNo = user.ProjectNo;
            loginUserInfo.Roles = claimsPrincipal.Claims.Where(c => c.Type == JwtClaimTypes.Role).Select(c => c.Value).ToList();

            return loginUserInfo;
        }

        /// <summary>
        /// Get UserInfo from Http Head
        /// Basic Authorization / Digest Authorization
        /// </summary>
        /// <returns></returns>
        private LoginUserInfo GetBasicLoginUserInfo()
        {
            LoginUserInfo loginUserInfo = new LoginUserInfo();
            //call async method
            var user = Task.Run<ApplicationUser>(() => _authentication.GetUserInfoFromHttpHeadAsync(_httpContextAccessor.HttpContext)).Result;

            if (user != null)
            {
                var userInfo = Task.Run<ApplicationUser>(() => _userManager.FindByNameAsync(user.UserName)).Result;

                loginUserInfo.UserName = userInfo.UserName;
                loginUserInfo.CustomerNo = userInfo.CustomerNo;
                loginUserInfo.ProjectNo = userInfo.ProjectNo;
            }
            else
            {
                loginUserInfo = null;
            }
            return loginUserInfo;
        }

        /// <summary>
        /// Get UserInfo from Http Head
        /// Basic Authorization / Digest Authorization
        /// </summary>
        /// <returns></returns>
        private async Task<LoginUserInfo> GetBasicLoginUserInfoAsync()
        {
            LoginUserInfo loginUserInfo = new LoginUserInfo();
            var user = await _authentication.GetUserInfoFromHttpHeadAsync(_httpContextAccessor.HttpContext);
            if (user != null)
            {
                //Construct UserInfo
                var userInfo = await _userManager.FindByNameAsync(user.UserName);
                loginUserInfo.UserName = userInfo.UserName;
                loginUserInfo.CustomerNo = userInfo.CustomerNo;
                loginUserInfo.ProjectNo = userInfo.ProjectNo;
            }
            else
            {
                loginUserInfo = null;
            }
            return loginUserInfo;
        }


    }
}
