using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class GroupLocation
    {
        public int GroupId { get; set; }
        public int LocationId { get; set; }
        public Group Group { get; set; }
        public Location Location { get; set; }

    }
}
