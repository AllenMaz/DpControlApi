using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class GroupLocationBaseModel
    {
        [Required(ErrorMessage = "GroupId is required!")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "LocationId is required!")]
        public int LocationId { get; set; }
    }

    public class GroupLocationAddModel: GroupLocationBaseModel
    {

    }

    public class GroupLocationUpdateModel
    {
    }

    public class GroupLocationSearchModel: GroupLocationBaseModel
    {
        public int GroupLocationId { get; set; }
    }
}
