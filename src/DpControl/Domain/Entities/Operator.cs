using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Operator
    {
        public int OperatorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }

        #region relationship
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual List<GroupOperator> GroupOperators { get; set; }        //many-to-many: operator can contorl multiple-location motors
        public virtual List<OperatorLocation> OperatorLocations { get; set; }      //many-to-many: operator can contorl multiple-location motors
//        public virtual List<Log> Logs { get; set; }

        #endregion
        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
