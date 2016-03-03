using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Holiday
    {
        public int HolidayId { get; set; }
        public int Day { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
