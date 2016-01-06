using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers.ExceptionHandler
{
    /// <summary>
    /// 自定义程序异常
    /// </summary>
    public class ProcedureException:Exception 
    {
        public ProcedureException()
        {

        }  
        public ProcedureException(string message)
            : base(message)
        {

        }  
        public ProcedureException(string message, Exception inner)
            : base(message, inner) { }  

    }
}
