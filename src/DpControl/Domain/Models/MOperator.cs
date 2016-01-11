using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class MOperator
    {
        public int OperatorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
    }
}
