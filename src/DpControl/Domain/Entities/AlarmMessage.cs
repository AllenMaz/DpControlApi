using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class AlarmMessage
    {
        public int AlarmMessageId { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<Alarm> Alarms { get; set; }
    }
}
