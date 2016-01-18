using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
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
    /// 授权
    /// </summary>
    public class DigestAuthenticationAttribute:AuthorizationFilterAttribute
    {
        private readonly string Scheme = "Digest ";


        public override void OnAuthorization(AuthorizationContext context)
        {
            //如果用户已经被验证了，则不需要再次验证
            if (context.HttpContext.User != null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            if (!HasAllowAnonymous(context))
            {
                var userName = AuthorizAndReturnUserName(context.HttpContext);
                if (!String.IsNullOrEmpty(userName))
                {
                    //构造ClaimIdentity
                    SetIdentity(context.HttpContext, userName);

                }
                else
                {
                    Challenge(context.HttpContext);
                    //校验失败则返回HttpUnauthorizedResult
                    Fail(context);

                }
            }


        }

        /// <summary>
        /// Authoriz User
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private string AuthorizAndReturnUserName(HttpContext context)
        {
            StringValues authHeader;
            if (context.Request.Headers.TryGetValue("Authorization", out authHeader) &&
                authHeader.Any() &&
                authHeader[0].StartsWith(Scheme))
            {
                var headParams = authHeader.First().Substring(Scheme.Length);
                var header = DigestHeader.Create(headParams, context.Request.Method);
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
                    ? header.UserName
                    : null;
                }
            }
            return null;

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
            context.Response.Headers.Add("WWW-Authenticate", new[] { Scheme + parameter });

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
