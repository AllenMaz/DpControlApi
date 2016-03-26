using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
