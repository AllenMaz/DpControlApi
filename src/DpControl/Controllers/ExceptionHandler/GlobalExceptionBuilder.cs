using Microsoft.AspNet.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Diagnostics;
using System.Net;
using Microsoft.AspNet.Http;
using DpControl.Domain.Models;
using System.Text;
using DpControl.Models;
using DpControl.Utility;

namespace DpControl.Controllers.ExceptionHandler
{
    public class GlobalExceptionBuilder 
    {
        /// <summary>
        /// 判断不同的异常并将不同类型的异常转换为HttpStatusCode
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder ExceptionBuilder(IApplicationBuilder builder)
        {
            builder.Run(async context =>
            {

                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var exceptionType = error.Error.GetType();
                    var exceptionMessage = error.Error.Message;

                    ErrResponseMessage errResponse = new ErrResponseMessage();
                    

                    if(exceptionType == typeof(Exception))
                    {
                        //系统异常
                        errResponse.code = 500;
                        errResponse.error = exceptionMessage;
                    }
                    else
                    {
                        //系统异常
                        errResponse.code = 500;
                        errResponse.error = "System is abnormal ！Error：" + exceptionMessage;
                        
                    }

                    string errMessage = ResponseHandler.ConstructErrResponse(errResponse);
                    await context.Response.WriteAsync(errMessage, Encoding.UTF8);
                }
                //await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
            });
            

            return builder;

        }
        
    }
}
