using DpControl.Domain.Models;
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
    /// <summary>
    /// API Authentication 
    /// </summary>
    public class APIAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private AbstractAuthentication _authentication;
        private IMemoryCache _memoryCache;
        private PathString _path;
        private readonly UserManager<ApplicationUser> _userManager;

        public APIAuthenticationMiddleware(RequestDelegate next, 
            AuthenticationOptions options,
            AbstractAuthentication authentication,
            IMemoryCache memoryCache,
            UserManager<ApplicationUser> userManager)
        {
            _next = next;
            _authentication = authentication;
            _memoryCache = memoryCache;
            _path = options.Path;
            _userManager = userManager;

        }
        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments(_path))
            {
                string userName =  _authentication.DoAuthentication(httpContext);
                if (!string.IsNullOrEmpty(userName))
                {
                    SetIdentity(httpContext,userName);
                }
                else
                {
                    //if authentication fail,return HttpUnauthorizedResult
                    _authentication.Challenge(httpContext);
                    return Task.FromResult(new HttpUnauthorizedResult());

                }
            }

            return  _next.Invoke(httpContext);
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
