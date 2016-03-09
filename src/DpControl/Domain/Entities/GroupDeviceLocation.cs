using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class GroupDeviceLocation
    {
        
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int? DeviceLocationId { get; set; }
        public DeviceLocation DeviceLocation { get; set; }
    }
}
