using DpControl.Controllers.ExceptionFilter;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers
{
    [GlobalExceptionFilter]
    public class BaseController:Controller
    {
    }
}
