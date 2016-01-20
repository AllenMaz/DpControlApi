using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DpControl.Utility.Authorization
{
    /// <summary>
    /// Restful web api Authorization 
    /// </summary>
    public class APIAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public string Roles { get; set; }
        
        
        public override async Task OnAuthorizationAsync(AuthorizationContext context)
        {
            //if allowanonymous
            if (!HasAllowAnonymous(context))
            {
                ClaimsPrincipal claimsPrincipal = context.HttpContext.User;
                #region Authorize logic

                if (!await DoAuthorize(context.HttpContext))
                {
                    Challenge(context.HttpContext);
                    Fail(context);
                }
                #endregion
               
            }
            
        }

        private async Task<bool> DoAuthorize(HttpContext httpContext)
        {
            bool passAuthorize = false;

            ClaimsPrincipal claimsPrincipal = httpContext.User;
            #region Authorize logic
            string username = claimsPrincipal.GetUserName();
            if (username == Roles)
            {
                passAuthorize = true;
            }
            
            #endregion
            return passAuthorize;
        }


        private async void Challenge(HttpContext context)
        {
            int httpStatusCode = (int)HttpStatusCode.MethodNotAllowed;
            context.Response.StatusCode = httpStatusCode;
            string errMessage = ResponseHandler.ReturnError(httpStatusCode,"You have no permission!");
            await context.Response.WriteAsync(errMessage);

        }
        
    }
}
