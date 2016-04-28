using DpControl.Utility;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace DpControl.Controllers
{
    //版本号为V1的BaseController
    //[GlobalExceptionFilter]
    [Route("v1/[controller]")]
    public class BaseAPIController:Controller
    {
        [NonAction]
        public string ModelStateError()
        {
            string errorMessageStr = string.Empty;

            var errorMessages = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = errorMessages.ToList();
            
            int httpStatusCode = (int)HttpStatusCode.BadRequest;
            string errorMessage = ResponseHandler.ReturnError(httpStatusCode, errorList);

            return errorMessage;
        }

        
    }
}
