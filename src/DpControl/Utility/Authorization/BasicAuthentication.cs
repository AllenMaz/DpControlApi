using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpControl.Utility.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicAuthentication:AbstractAuthentication
    {
        public BasicAuthentication()
        {
            base._scheme = "Basic ";
        }

        protected override async Task<string> CheckUserInfo(string headParams,HttpContext httpContext)
        {
            string resultUserName = string.Empty;

            string userstr = Encoding.UTF8.GetString(
                   Convert.FromBase64String(headParams));

            string[] arrUser = userstr.Split(':');
            if (arrUser.Length >= 2)
            {
                string userName = arrUser[0];
                string passWord = arrUser[1];
                
                if (await DoIdentity(userName, passWord))
                {
                    resultUserName = userName;
                }

            }

            return resultUserName;
        }

        public override void Challenge(HttpContext httpContext)
        {
            var header = DigestHeader.Unauthorized();
            var parameter = $"realm=\"DpControl\"";
            base.ApplyChallenge(httpContext, parameter);
        }

        /// <summary>
        /// 校验用户名和密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<bool> DoIdentity(string userName,string password)
        {
            return true;
        }
    }
}
