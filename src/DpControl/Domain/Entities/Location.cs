using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Location
    {
        public int LocationId { get; set; }           // one-to-one relation with DeviceAlarm requires the primary key of dependent table used as the foreign key
        public string Building { get; set; }
        public string Floor { get; set; }
        public string RoomNo { get; set; }
        public Orientation Orientation { get; set; }
        public int InstallationNumber { get; set; }

        public int CurrentPosition { get; set; }
        public string CommAddress { get; set; }
        public string DeviceSerialNo { get; set; }
        public ControllerType DeviceType { get; set; }
        public CommMode CommMode { get; set; }
        public int FavorPositionFirst { get; set; }
        public int FavorPositionrSecond { get; set; }
        public int FavorPositionThird { get; set; }

        #region Realtionship
        public virtual List<GroupLocation> GroupLocations { get; set; }                // many-to-many： 
        public virtual List<OperatorLocation> OperatorLocations { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual List<Alarm> Alarms { get; set; }
        public virtual List<Log> Logs { get; set; }

        #endregion

        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public Location()
        {
            this.GroupLocations = new List<GroupLocation>();
            this.OperatorLocations = new List<OperatorLocation>();
            this.Alarms = new List<Alarm>();
            this.Logs = new List<Log>();
        }
    }

    public enum Orientation
    {
        Null,       // for unidentified
        East,
        South,
        West,
        North,
        Southeast,
        Southwest,
        Northeast,
        Northwest
    }

    public enum ControllerType
    {
        Null,
        Controller,
        TouchScreen,
        DryContact,
        RF,
        Tubular
    }

    public enum CommMode
    {
        Null,
        RS485,
        ZigBee,
        Bluetooth,
        WiFi
    }
}
