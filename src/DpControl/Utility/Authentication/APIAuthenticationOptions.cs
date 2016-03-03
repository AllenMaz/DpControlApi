using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Utility.Authentication
{
    public class APIAuthenticationOptions
    {
        public PathString Path { get; set; }
        public string PolicyName { get; set; }
    }
}
