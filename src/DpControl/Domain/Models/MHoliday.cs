using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class HoliadyBaseModel
    {
        [Required(ErrorMessage = "Day is required!")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Day must be a positive integer")]
        public int Day { get; set; }
    }
    public class HolidayAddModel: HoliadyBaseModel
    {
        [Required(ErrorMessage = "ProjectId is required!")]
        public int ProjectId { get; set; }
    }

    public class HolidayUpdateModel: HoliadyBaseModel
    {
    }

    public class HolidaySearchModel: HoliadyBaseModel
    {
        public int HolidayId { get; set; }

        public int ProjectId { get; set; }

        public string Creator { get; set; }
        public string CreateDate { get; set; }
        public string Modifier { get; set; }
        public string ModifiedDate { get; set; }
    }
}
