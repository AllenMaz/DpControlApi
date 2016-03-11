using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class UserGroupBaseModel
    {
        public int GroupId { get; set; }

        [Required(ErrorMessage = "UserId is required!")]
        public string UserId { get; set; }
    }

    public class UserGroupAddModel: UserGroupBaseModel
    {

    }
    public class UserGroupUpdateModel: UserGroupBaseModel
    {

    }
    public class UserGroupSearchModel: UserGroupBaseModel
    {
        public int UserGroupId { get; set; }
       
    }
}
