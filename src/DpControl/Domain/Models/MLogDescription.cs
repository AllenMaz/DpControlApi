using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class LogDescriptionBaseModel
    {
        [MaxLength(500, ErrorMessage = "Description must be less than 500 characters!")]
        public string Description { get; set; }

        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "DescriptionCode must be an integer")]
        public int DescriptionCode { get; set; }
    }

    public class LogDescriptionAddModel: LogDescriptionBaseModel
    {

    }

    public class LogDescriptionUpdateModel: LogDescriptionBaseModel
    {

    }

    public class LogDescriptionSubSearchModel: LogDescriptionBaseModel
    {
        public int LogDescriptionId { get; set; }
    }

    public class LogDescriptionSearchModel: LogDescriptionSubSearchModel
    {
        public IEnumerable<LogSubSearchModel> Logs { get; set; }
    }
}
