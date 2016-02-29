using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class GroupBaseModel
    {
        [Required(ErrorMessage = "GroupName is required!")]
        [MaxLength(50, ErrorMessage = "GroupName must be less than 50 characters!")]
        public string GroupName { get; set; }

        public int? SceneId { get; set; }
    }

    public class GroupAddModel: GroupBaseModel
    {
        [Required(ErrorMessage = "ProjectId is required!")]
        public int ProjectId { get; set; }
        
    }

    public class GroupUpdateModel: GroupBaseModel
    {

    }

    public class GroupSearchModel:GroupBaseModel
    {
        public int GroupId { get; set; }
        public int ProjectId { get; set; }

        private DateTime _createDate;
        public string CreateDate
        {
            get; set;
        }
    }
}
