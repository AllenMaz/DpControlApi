using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DpControl.Utility.Authorization
{
    #region doc
    /*
    to user this handler you must add :
        services.AddAuthorization(options =>
        {
            //Add API Authorization
            options.AddPolicy("APIAuthorize", policy => policy.Requirements.Add(new APIAuthorizationHandler()));
        });
    then you can user like [Authorize(Policy ="APIAuthorize")]

    */
    #endregion

    /// <summary>
    /// API Authorization
    //  then you can user like [Authorize(Policy ="APIAuthorize")]
    /// </summary>
    public class APIAuthorizationHandler : AuthorizationHandler<APIAuthorizationHandler>, IAuthorizationRequirement
    {
        protected override void Handle(AuthorizationContext context, APIAuthorizationHandler requirement)
        {
            //ChallengeForAuthorization();
            context.Succeed(requirement);

        }

        private async void ChallengeForAuthorization(HttpContext context)
        {
            int httpStatusCode = (int)HttpStatusCode.MethodNotAllowed;
            context.Response.StatusCode = httpStatusCode;
            string errMessage = ResponseHandler.ReturnError(httpStatusCode, "You have no permission!");
            await context.Response.WriteAsync(errMessage);

        }

    }
}
