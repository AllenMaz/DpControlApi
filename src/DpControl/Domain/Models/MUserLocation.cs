using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class UserLocationBaseModel
    {
        
        public int LocationId { get; set; }

        [Required(ErrorMessage = "UserId is required!")]
        public string UserId { get; set; }
    }

    public class UserLocationAddModel: UserLocationBaseModel
    {

    }
    public class UserLocationUpdateModel: UserLocationBaseModel
    {

    }

    public class UserLocationSearchModel: UserLocationBaseModel
    {
        public int UserLocationId { get; set; }
    }
}
