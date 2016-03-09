﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class UserDeviceLocation
    {
        public int DeviceLocationId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DeviceLocation DeviceLocation { get; set; }
    }
}
