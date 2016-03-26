using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Device
    {
        public int DeviceId { get; set; }
        public float Voltage { get; set; }
        public float Diameter { get; set; }
        public float Torque { get; set; }

        public virtual List<Location> Locations { get; set; }

        public byte[] RowVersion { get; set; }

        public Device()
        {
            this.Locations = new List<Location>();
        }

    }
}
