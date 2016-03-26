using DpControl.Models;
using DpControl.Utility;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers
{
    public class BaseController : Controller
    {
        public string ModelStateError()
        {
            string errorMessageStr = string.Empty;

            var errorMessages = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

            var errorList = errorMessages.ToList();

            string errorMessage = string.Empty;
            foreach (string error in errorList)
            {
                errorMessage += error + "<br/>";
            }
            
            return errorMessage;
        }
    }
}
