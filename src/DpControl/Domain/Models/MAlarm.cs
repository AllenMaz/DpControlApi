using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class AlarmBaseModel
    {
        
    }

    public class AlarmAddModel
    {
        public int AlarmMessageId { get; set; }
        public int LocationId { get; set; }
    }

    public class AlarmUpdateModel
    {

        
    }

    public class AlarmSearchModel
    {
        public int AlarmId { get; set; }
        public int? AlarmMessageId { get; set; }
        public int? LocationId { get; set; }
        public DateTime CreateDate { get; set; }
    }

}
