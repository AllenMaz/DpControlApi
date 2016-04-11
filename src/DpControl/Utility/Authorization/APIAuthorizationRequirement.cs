using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DpControl.Utility.Authorization
{
    #region doc
    /*
    to user this handler you must add :
       services.AddAuthorization(options =>
            {
                //Add API Authorization
                options.AddPolicy("APIPolicy",
                    policy => {
                        //policy.RequireAuthenticatedUser();
                        policy.Requirements.Add(new APIAuthorizationRequirement());
                    }
                 );
            });
    then you can user like [Authorize(Policy ="APIAuthorize")]

    */
    #endregion

    /// <summary>
    /// API Authorization
    //  then you can user like [Authorize(Policy ="APIAuthorize")]
    /// </summary>
    public class APIAuthorizationRequirement : AuthorizationHandler<APIAuthorizationRequirement>, IAuthorizationRequirement
    {
        protected override void Handle(AuthorizationContext context, APIAuthorizationRequirement requirement)
        {
            //ChallengeForAuthorization();
            context.Succeed(requirement);

        }

        private async void ChallengeForAuthorization(HttpContext context)
        {
            int httpStatusCode = (int)HttpStatusCode.MethodNotAllowed;
            context.Response.StatusCode = httpStatusCode;
            string errMessage = ResponseHandler.ReturnError(httpStatusCode, new List<string>() { "You have no permission!" });
            await context.Response.WriteAsync(errMessage);

        }

    }
}
