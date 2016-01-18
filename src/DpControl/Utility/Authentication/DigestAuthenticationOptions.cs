using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;

namespace DpControl.Utility.Authentication
{
    public class DigestAuthenticationOptions: AuthenticationOptions
    {   
        public DigestAuthenticationOptions()
        {
            base.AuthenticationScheme = "Digest ";
            
        }
    }
}
