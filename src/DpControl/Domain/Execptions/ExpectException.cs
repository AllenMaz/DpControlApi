using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Execptions
{
    /// <summary>
    /// Expect Exception which use to catch error tip
    /// </summary>
    public class ExpectException:Exception
    {
        public ExpectException()
        {

        }
        public ExpectException(string message)
            : base(message)
        {
        }
        public ExpectException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
