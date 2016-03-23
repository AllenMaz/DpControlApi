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
        public int? LogDescriptionId { get; set; }
        public LogDescription LogDescription { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }

        #endregion
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
