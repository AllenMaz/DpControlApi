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
    public class DigestAuthentication
    {
        private string _scheme;

        public DigestAuthentication(string scheme)
        {
            _scheme = scheme;
        }

        /// <summary>
        /// Do Authentication and return userName;
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task<string> DoAuthentication(HttpContext httpContext)
        {
            string userName = string.Empty;
            StringValues authHeader;
            if (httpContext.Request.Headers.TryGetValue("Authorization", out authHeader) &&
                authHeader.Any() &&
                authHeader[0].StartsWith(_scheme))
            {
                var headParams = authHeader.First().Substring(_scheme.Length);
                var header = DigestHeader.Create(headParams, httpContext.Request.Method);
                if (await CheckUserInfo(header))
                {
                    userName = header.UserName;
                }

            }
            return userName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private async Task<bool> CheckUserInfo(DigestHeader header)
        {
            if (DigestNonce.IsValid(header.Nonce, header.NounceCounter))
            {
                var password = await GetPassword(header.UserName);

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

        private async Task<string> GetPassword(string userName)
        {
            return userName;
        }
        
    }
}
