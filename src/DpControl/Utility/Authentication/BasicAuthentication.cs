using DpControl.Domain.Entities;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpControl.Utility.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicAuthentication:AbstractAuthentication
    {
        public readonly UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public BasicAuthentication(UserManager<ApplicationUser> userManager)
        {
            base._scheme = "Basic ";
            _userManager = userManager;
        }

        /// <summary>
        /// 获取依赖注入实例
        /// </summary>
        /// <param name="httpContext"></param>
        private void InitServices(HttpContext httpContext)
        {
            var serviceProveider = httpContext.RequestServices;  // Controller中，当前请求作用域内注册的Service
            _signInManager = serviceProveider.GetRequiredService<SignInManager<ApplicationUser>>();

        }

        protected override async Task<bool> Login(string headParams, HttpContext httpContext)
        {
            bool loginSuccess = false;
            string userstr = Encoding.UTF8.GetString(
                   Convert.FromBase64String(headParams));

            string[] arrUser = userstr.Split(':');
            if (arrUser.Length >= 2)
            {
                string userName = arrUser[0];
                string passWord = arrUser[1];

                InitServices(httpContext);
                // sign out first
                await _signInManager.SignOutAsync();
                //sign in 
                var result = await _signInManager.PasswordSignInAsync(userName, passWord,isPersistent:false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    loginSuccess = true;
                }

            }

            return loginSuccess;
        }

        protected override async Task<string> CheckUserInfo(string headParams,HttpContext httpContext)
        {
            string resultUserName = string.Empty;

            string userstr = Encoding.UTF8.GetString(
                   Convert.FromBase64String(headParams));

            string[] arrUser = userstr.Split(':');
            if (arrUser.Length >= 2)
            {
                string userName = arrUser[0];
                string passWord = arrUser[1];

                var user =await _userManager.FindByNameAsync(userName);
                bool isRightPassword = await _userManager.CheckPasswordAsync(user,passWord);
                if (isRightPassword)
                {
                    resultUserName = userName;
                }

            }

            return resultUserName;
        }

        protected override async Task<ApplicationUser> GetUserInfo(string headParams, HttpContext httpContext)
        {
            ApplicationUser currentUser = null;

            string userstr = Encoding.UTF8.GetString(
                   Convert.FromBase64String(headParams));

            string[] arrUser = userstr.Split(':');
            if (arrUser.Length >= 2)
            {
                string userName = arrUser[0];
                string passWord = arrUser[1];

                currentUser = await _userManager.FindByNameAsync(userName);
               
            }

            return currentUser;
        }

        public override void Challenge(HttpContext httpContext)
        {
            var parameter = $"realm=\"Need Authentication\"";
            base.ApplyChallenge(httpContext, parameter);
        }

       
    }
}
