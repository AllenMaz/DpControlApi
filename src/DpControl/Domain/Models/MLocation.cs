using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Entities;

namespace DpControl.Domain.Models
{
    public class MLocation
    {
        public string ProjectNo { get; set; }
        public int DeviceLocationId { get; set; }           // one-to-one relation with DeviceAlarm requires the primary key of dependent table used as the foreign key
        public string Building { get; set; }
        public string Floor { get; set; }
        public string RoomNo { get; set; }
        public Orientation Orientation { get; set; }
        public int InstallationNumber { get; set; }

        public ControllerType DeviceType { get; set; }
        public CommMode CommMode { get; set; }
        public string DeviceSerialNo { get; set; }
        public string CommAddress { get; set; }
        public int CurrentPosition { get; set; }
        public int FavorPositionFirst { get; set; }
        public int FavorPositionrSecond { get; set; }
        public int FavorPositionThird { get; set; }

    }
}
