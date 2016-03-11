using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class LogBaseModel
    {
        [MaxLength(500, ErrorMessage = "Comment must be less than 500 characters!")]
        public string Comment { get; set; }
        public int LogDescriptionId { get; set; }
        public int LocationId { get; set; }
    }
    public class LogAddModel: LogBaseModel
    {

    }
    public class LogUpdateModel: LogBaseModel
    {

    }
    public class LogSearchModel
    {
        public int LogId { get; set; }

        [MaxLength(50, ErrorMessage = "Comment must be less than 500 characters!")]
        public string Comment { get; set; }        
        
        public int? LogDescriptionId { get; set; }
        public int? LocationId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
