using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DpControl.Utility.Authorization
{
    /// <summary>
    /// Restful web api Authorization 
    /// </summary>
    public class APIAuthorizeAttribute:AuthorizationFilterAttribute
    {
        public string Roles { get; set; }

        private AbstractAuthentication _authentication;
        private IMemoryCache _memoryCach;

        
        public override async Task OnAuthorizationAsync(AuthorizationContext context)
        {
            InitServices(context.HttpContext);

            //if allowanonymous
            if (!HasAllowAnonymous(context))
            {
                
                string userName = await _authentication.DoAuthentication(context.HttpContext);
                if (!String.IsNullOrEmpty(userName))
                {
                    if (!await DoAuthorize(context.HttpContext, userName))
                    {
                        ChallengeForAuthorization(context.HttpContext);
                        Fail(context);
                    }
                }
                else
                {
                    //if authentication fail,return HttpUnauthorizedResult
                    _authentication.Challenge(context.HttpContext);
                    Fail(context);

                }
            }


        }

        /// <summary>
        /// 获取依赖注入实例
        /// </summary>
        /// <param name="httpContext"></param>
        private void InitServices(HttpContext httpContext)
        {
            var serviceProveider = httpContext.RequestServices;  // Controller中，当前请求作用域内注册的Service
            _authentication = serviceProveider.GetRequiredService<AbstractAuthentication>();
            _memoryCach = serviceProveider.GetRequiredService<IMemoryCache>();
        }

        /// <summary>
        /// define authorize logic here
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<bool> DoAuthorize(HttpContext httpContext,string username)
        {
            //
            return true;

        }

        protected override void Fail(AuthorizationContext context)
        {
            context.Result = new HttpUnauthorizedResult();
        }
        

        private async void ChallengeForAuthorization(HttpContext context)
        {
            var header = DigestHeader.Unauthorized();
            var parameter = header.ToString();
            context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            string errMessage = ResponseHandler.ReturnError("You have no permission!");
            await context.Response.WriteAsync(errMessage);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userName"></param>
        private void SetIdentity(HttpContext context, string userName)
        {
            //根据用户名，查询用户信息
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim(ClaimTypes.Role, "Allen"));
            var claimsIdentity = new ClaimsIdentity(claims, "DigestAuthentication");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            context.User = claimsPrincipal;
        }
    }
}
