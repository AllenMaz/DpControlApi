using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DpControl.Controllers
{
    //版本号为V1的BaseController
    //[GlobalExceptionFilter]
    [Route("v1/[controller]")]
    public class BaseAPIController:Controller
    {
    }
}
