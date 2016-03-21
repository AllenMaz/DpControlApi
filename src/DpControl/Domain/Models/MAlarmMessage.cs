using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class AlarmMessageBaseModel
    {
        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "ErrorCode must be an integer")]
        public int ErrorCode { get; set; }

        [MaxLength(500, ErrorMessage = "Message must be less than 500 characters!")]
        public string Message { get; set; }
    }
    public class AlarmMessageAddModel: AlarmMessageBaseModel
    {

    }

    public class AlarmMessageUpdateModel: AlarmMessageBaseModel
    {

    }

    public class AlarmMessageSubSearchModel : AlarmMessageBaseModel
    {
        public int AlarmMessageId { get; set; }

    }
    public class AlarmMessageSearchModel: AlarmMessageSubSearchModel
    {
        public IEnumerable<AlarmSubSearchModel> Alarms { get; set; }

    }
}
