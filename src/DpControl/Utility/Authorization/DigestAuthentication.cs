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
    /// Digest Authentication
    /// </summary>
    public class DigestAuthentication:AbstractAuthentication
    {
        public DigestAuthentication()
        {
            base._scheme = "Digest ";
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override async Task<string> CheckUserInfo(string headParams,HttpContext httpContext)
        {
            
            var header = DigestHeader.Create(headParams, httpContext.Request.Method);
            string userName = header.UserName;

            if (DigestNonce.IsValid(header.Nonce, header.NounceCounter))
            {
                var password = await GetPassword(header.UserName);

                var hash1 = String.Format(
                    "{0}:{1}:{2}",
                    header.UserName,
                    header.Realm,
                    password).ToMd5Hash();

                //查询参数中不能有逗号
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
                ? userName
                : null;
            }
            return null;

        }

        public override void Challenge(HttpContext httpContext)
        {
            var header = DigestHeader.Unauthorized();
            var parameter = header.ToString();
            base.ApplyChallenge(httpContext,parameter);
        }
        

        private async Task<string> GetPassword(string userName)
        {
            return userName;
        }
        
    }
}
