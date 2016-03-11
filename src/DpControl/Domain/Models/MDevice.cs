using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class DeviceBaseModel
    {
        public float Voltage { get; set; }
        public float Diameter { get; set; }
        public float Torque { get; set; }
    }

    public class DeviceAddModel: DeviceBaseModel
    {

    }

    public class DeviceUpdateModel: DeviceBaseModel
    {

    }

    public class DeviceSearchModel: DeviceBaseModel
    {
        public int DeviceId { get; set; }
        
    }
}
