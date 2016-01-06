﻿using Microsoft.AspNet.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Diagnostics;
using System.Net;
using Microsoft.AspNet.Http;
using DpControl.Domain.Utility;
using DpControl.Domain.Models;
using System.Text;

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

                    ErrResponse errResponse = new ErrResponse();
                    errResponse.message = exceptionMessage; 
                    string errMessage = ResponseUtility.ConstructErrResponse(errResponse);
                    //程序异常
                    if (exceptionType == typeof(ProcedureException))
                    {
                        // This error would not normally be exposed to the client
                        await context.Response.WriteAsync(errMessage,Encoding.UTF8);
                    }
                    else
                    {
                        //系统异常
                        await context.Response.WriteAsync(errMessage, Encoding.UTF8);
                    }

                }
                await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
            });
            

            return builder;

        }
        
    }
}
