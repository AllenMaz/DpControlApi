using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class LogDescription
    {
        public int LogDescriptionId { get; set; }
        public string Description { get; set; }
        public int DescriptionCode { get; set; }
        public List<Log> Logs { get; set; }
    }
}
