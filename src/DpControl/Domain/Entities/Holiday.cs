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
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
