using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DpControl.Utility.Authorization
{
    /// <summary>
    /// 身份验证中间件
    /// </summary>
    public class DigestAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private PathString _path;
        private string _scheme = "Digest ";
        public DigestAuthenticationMiddleware(RequestDelegate next, AuthenticationOptions options)
        {
            _next = next;
            _path = options.Path;

        }
        public Task Invoke(HttpContext httpContext)
        {
            
            if (httpContext.Request.Path.StartsWithSegments(_path))
            {
                string userName = string.Empty;
                StringValues authHeader;
                if (httpContext.Request.Headers.TryGetValue("Authorization", out authHeader) &&
                    authHeader.Any() &&
                    authHeader[0].StartsWith(_scheme))
                {
                    var headParams = authHeader.First().Substring(_scheme.Length);
                    var header = DigestHeader.Create(headParams, httpContext.Request.Method);
                    if (CheckUserInfo(header))
                    {
                        userName = header.UserName;

                    }

                }
                if (!string.IsNullOrEmpty(userName))
                {
                    //设置ClaimsIdentity
                    SetIdentity(httpContext, userName);
                }
                else
                {
                    //httpContext.Authentication.ChallengeAsync();
                    Challenge(httpContext);
                    return Task.FromResult(new HttpUnauthorizedResult());
                }
            }

            return _next.Invoke(httpContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private bool CheckUserInfo(DigestHeader header)
        {
            if (DigestNonce.IsValid(header.Nonce, header.NounceCounter))
            {
                var password = GetPassword(header.UserName);

                var hash1 = String.Format(
                    "{0}:{1}:{2}",
                    header.UserName,
                    header.Realm,
                    password).ToMd5Hash();

                var hash2 = String.Format(
                    "{0}:{1}",
                    header.Method,
                    header.Uri).ToMd5Hash();

                var computedResponse = String.Format(
                    "{0}:{1}:{2}:{3}:{4}:{5}",
                    hash1,
                    header.Nonce,
                    header.NounceCounter,
                    header.Cnonce,
                    "auth",
                    hash2).ToMd5Hash();

                return header.Response.Equals(computedResponse, StringComparison.Ordinal)
                ? true
                : false;
            }
            return false;

        }

        private string GetPassword(string userName)
        {
            return userName;
        }

        private void Challenge(HttpContext context)
        {
            var header = DigestHeader.Unauthorized();
            var parameter = header.ToString();
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.Headers.Add("WWW-Authenticate", new[] { _scheme + parameter });

        }


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
