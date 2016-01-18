using DpControl.Utility.Authentication;
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
using System.Threading.Tasks;

namespace DpControl.Utility.Filters
{
    public class DigestAuthorizationAttribute: AuthorizationFilterAttribute
    {
        private readonly string Scheme = "Digest ";
        

        public override void OnAuthorization(AuthorizationContext context)
        {
            if (!HasAllowAnonymous(context))
            {
                var userName = AuthorizAndReturnUserName(context.HttpContext);
                if (!String.IsNullOrEmpty(userName))
                {
                    //SetIdentity(actionContext, userName);

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
        /// 校验是否允许匿名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool HasAllowAnonymous(AuthorizationContext context)
        {
            
            bool hasAlloAnonymous = context.Filters.Any(item => item is Microsoft.AspNet.Mvc.Filters.AllowAnonymousFilter);

            //hasAlloAnonymous =  context.Filters.Any(item => item is Microsoft.AspNet.Authorization.AllowAnonymousAttribute);
            return hasAlloAnonymous;
        }

        protected override void Fail(AuthorizationContext context)
        {
            context.Result = new HttpUnauthorizedResult();
        }

        /// <summary>
        /// Authoriz User
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private string AuthorizAndReturnUserName(HttpContext context)
        {
            StringValues authHeader ;
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

    }
}
