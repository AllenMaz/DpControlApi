using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class GroupOperator
    {
        public int GroupId { get; set; }
        public int OperatorId { get; set; }
        public Group Group { get; set; }
        public Operator Operator { get; set; }
    }
}
