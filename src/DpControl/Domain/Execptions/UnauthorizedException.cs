using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Execptions
{
    public class UnauthorizedException:Exception
    {
        public UnauthorizedException()
        {

        }
        public UnauthorizedException(string message)
            : base(message)
        {
        }
        public UnauthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
