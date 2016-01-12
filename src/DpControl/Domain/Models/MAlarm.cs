using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class MAlarm
    {
        public int AlarmId { get; set; }
        public string  AlarmMessage { get; set; }
        public string Location { get; set; }
        public DateTime ALarmDate { get; set; }
    }
}
