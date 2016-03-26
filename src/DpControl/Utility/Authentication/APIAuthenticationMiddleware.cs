using DpControl.Domain.Entities;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DpControl.Utility.Authentication
{
    /*  User this Middleware you must add:
             app.UseIdentity();

             //Add API Authentication Middleware
            app.UseMiddleware<APIAuthenticationMiddleware>(
                new APIAuthenticationOptions()
                {
                    Path = "/v1",  //只对API进行身份验证
                    PolicyName = "APIPolicy"
                }
             );
       and set this config after  app.UseIdentity();
    */
    /// <summary>
    /// API Authentication 
    /// </summary>
    public class APIAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private AbstractAuthentication _authentication;
        private IMemoryCache _memoryCache;
        private PathString _path;
        private string _policyName;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public APIAuthenticationMiddleware(RequestDelegate next, 
            APIAuthenticationOptions options,
            AbstractAuthentication authentication,
            IMemoryCache memoryCache,
            UserManager<ApplicationUser> userManager)
        {
            _next = next;
            _authentication = authentication;
            _memoryCache = memoryCache;
            _path = options.Path;
            _policyName = options.PolicyName;
            _userManager = userManager;

        }
        public async Task Invoke(HttpContext httpContext, IAuthorizationService authorizationService)
        {
            if (httpContext.Request.Path.StartsWithSegments(_path))
            {
                //Authentication
                string userName = await _authentication.DoAuthentication(httpContext);
                if (string.IsNullOrEmpty(userName))
                {
                    _authentication.Challenge(httpContext);
                    return;
                }
                

            }

            await _next(httpContext);
        }

        private void SetIdentity(HttpContext context, string userName)
        {
            //根据用户名，查询用户信息
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim(ClaimTypes.Role, userName));
            var claimsIdentity = new ClaimsIdentity(claims, "DigestAuthentication");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            context.User = claimsPrincipal;
        }

        private async Task SiginIn(HttpContext context,string userName)
        {
            //根据用户名，查询用户信息
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim(ClaimTypes.Role, "Allen"));
            var claimsIdentity = new ClaimsIdentity(claims, "DigestAuthentication");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await context.Authentication.SignInAsync("APIAuthentication", claimsPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                        IsPersistent = false,
                        AllowRefresh = false
                    });
        }

    }
}
