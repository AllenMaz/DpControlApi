using DpControl.Domain.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DpControl.Utility.Authentication
{
    public abstract class AbstractAuthentication
    {
        public string _scheme;
       

        /// <summary>
        /// Do Authentication and return userName;
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public string DoAuthentication(HttpContext httpContext)
        {
            string userName = string.Empty;
            StringValues authHeader;
            if (httpContext.Request.Headers.TryGetValue("Authorization", out authHeader) &&
                authHeader.Any() &&
                authHeader[0].StartsWith(_scheme))
            {
                var headParams = authHeader.First().Substring(_scheme.Length);
                userName = CheckUserInfo(headParams,httpContext);

            }
            return userName;
        }

        public abstract void Challenge(HttpContext httpContext);

        protected void ApplyChallenge(HttpContext context,string parameter)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.Headers.Add("WWW-Authenticate", new[] { _scheme + parameter });

        }
        

        protected abstract string CheckUserInfo(string headParams, HttpContext httpContext);
    }
}
