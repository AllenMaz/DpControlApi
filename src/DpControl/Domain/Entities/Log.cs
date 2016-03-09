using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int? DeviceLocationId { get; set; }
        public DeviceLocation DeviceLocation { get; set; }
       
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        #endregion
        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
