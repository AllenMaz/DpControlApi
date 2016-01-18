using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DpControl.Utility.Authentication
{
    public class DigestAuthenticationHandler : AuthenticationHandler<DigestAuthenticationOptions>
    {
        
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                string schemeName = Options.AuthenticationScheme;

                StringValues authHeader;
                if (Request.Headers.TryGetValue("Authorization", out authHeader) &&
                    authHeader.Any() &&
                    authHeader[0].StartsWith(Options.AuthenticationScheme))
                {

                    var headParams = authHeader.First().Substring(schemeName.Length);
                    var header = DigestHeader.Create(headParams,Request.Method);
                    if (DigestAuthentication(header))
                    {
                        string userName = header.UserName;
                        //Constuct ClaimIdentity
                        //根据用户名，查询用户信息
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, "bbbb"));
                        claims.Add(new Claim(ClaimTypes.Role, "Allen"));
                        var claimsIdentity = new ClaimsIdentity(claims, "DigestAuthentication");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


                        var ticket = new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties() { }, Options.AuthenticationScheme);
                        var authenticateResult = AuthenticateResult.Success(ticket);
                        return Task.FromResult(authenticateResult);
                    }
                    else
                    {
                        //授权失败
                        var authenticateResult1 = AuthenticateResult.Fail("AuthenticationFail");
                        return Task.FromResult(authenticateResult1);
                    }

                }

                //返回
                var authenticateResult2 = AuthenticateResult.Fail("AuthenticationFail2");
                return Task.FromResult(authenticateResult2);

            } catch (SecurityException e)
            {
                var authenticateResult = AuthenticateResult.Fail(e);
                return Task.FromResult(authenticateResult);
            }

            
        }

        /// <summary>
        /// Digest Authentication
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private bool DigestAuthentication(DigestHeader header)
        {
            if (DigestNonce.IsValid(header.Nonce, header.NounceCounter))
            {
                var password = UserManager.GetPassword(header.UserName);

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
    }
}
