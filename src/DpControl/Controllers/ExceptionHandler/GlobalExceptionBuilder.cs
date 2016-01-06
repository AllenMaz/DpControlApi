using Microsoft.AspNet.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Diagnostics;
using System.Net;
using Microsoft.AspNet.Http;

namespace DpControl.Controllers.ExceptionHandler
{
    public class GlobalExceptionBuilder 
    {
        public static IApplicationBuilder ExceptionBuilder(IApplicationBuilder builder)
        {
            builder.Run(async context =>
            {

                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    //判断不同的异常并将不同类型的异常转换为HttpStatusCode
                    var exceptionType = error.Error.GetType();
                    if (exceptionType == typeof(NotImplementedException))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        // This error would not normally be exposed to the client
                        await context.Response.WriteAsync(error.Error.Message);
                    }

                }
                await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
            });
            

            return builder;

        }
        
    }
}
