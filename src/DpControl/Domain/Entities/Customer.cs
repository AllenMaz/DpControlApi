using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
        public List<Project> Projects { get; set; }

        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
