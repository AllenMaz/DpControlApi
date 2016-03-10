﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Alarm
    {
        public int AlarmId { get; set; }

        #region relationship
        public int AlarmMessageId { get; set; }
        public AlarmMessage AlarmMessage { get; set; }
        public int? DeviceLocationId { get; set; }
        public Location DeviceLocation { get; set; }
        #endregion

        public DateTime ModifiedDate { get; set; }      // alarms generated time
        public byte[] RowVersion { get; set; }

    }
}
