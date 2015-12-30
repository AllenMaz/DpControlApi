using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Log
    {
        public int LogId { get; set; }
        public string Comment { get; set; }         // used to express additional information

        #region Relationship
        public int LogDescriptionId { get; set; }
        public LogDescription Description { get; set; }
        public int LocationId { get; set; }
        public Location LogOf { get; set; }
        //public int OperatorId { get; set; }
        //public Operator Operator { get; set; }
        #endregion
        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
