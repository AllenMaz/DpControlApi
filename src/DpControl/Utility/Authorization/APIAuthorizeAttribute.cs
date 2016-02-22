using DpControl.Domain.Models;
using DpControl.Utility.Authentication;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DpControl.Utility.Authorization
{
    /// <summary>
    /// Restful web api Authorization 
    /// </summary>
    public class APIAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private AbstractAuthentication _authentication;
        private IMemoryCache _memoryCache;
        private UserManager<ApplicationUser> _userManager;


        /// <summary>
        /// 获取依赖注入实例
        /// </summary>
        /// <param name="httpContext"></param>
        private void InitServices(HttpContext httpContext)
        {
            var serviceProveider = httpContext.RequestServices;  // Controller中，当前请求作用域内注册的Service
            _authentication = serviceProveider.GetRequiredService<AbstractAuthentication>();
            _memoryCache = serviceProveider.GetRequiredService<IMemoryCache>();
            _userManager = serviceProveider.GetRequiredService<UserManager<ApplicationUser>>();

        }


        public string Roles { get; set; }
        
        
        public override async Task OnAuthorizationAsync(AuthorizationContext context)
        {
            //if allowanonymous
            if (!HasAllowAnonymous(context))
            {
                HttpContext httpContext = context.HttpContext;
                InitServices(httpContext);

                string userName = await _authentication.DoAuthentication(httpContext);
                if (!string.IsNullOrEmpty(userName))
                {
                   
                    if (!await DoAuthorize(userName))
                    {
                        Challenge(context.HttpContext);
                        Fail(context);
                    }
                }
                else
                {
                    //if authentication fail,return HttpUnauthorizedResult
                    _authentication.Challenge(httpContext);
                    Fail(context);
                }
            }
            
        }

        private async Task<bool> DoAuthorize(string userName)
        {
            bool passAuthorize = true;
            if (!string.IsNullOrEmpty(Roles))
            {
                string[] allowRoles = this.Roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var user = await _userManager.FindByNameAsync(userName);
                IList<string> userRoles = await _userManager.GetRolesAsync(user);

                //如果该用户的角色中有任意一个角色存在授权中，则允许访问
                if (!userRoles.Any(v=>allowRoles.Contains(v)))
                {
                    passAuthorize = false;
                }
            }
            return passAuthorize;
        }


        private async void Challenge(HttpContext context)
        {
            int httpStatusCode = (int)HttpStatusCode.MethodNotAllowed;
            context.Response.StatusCode = httpStatusCode;
            string errMessage = ResponseHandler.ReturnError(httpStatusCode,"You have no permission!");
            await context.Response.WriteAsync(errMessage);

        }
        
    }
}
