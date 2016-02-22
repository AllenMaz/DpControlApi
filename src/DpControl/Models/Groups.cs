using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Models
{
    public class Groups
    {
        public string GroupName { get; set; }

        [Required(ErrorMessage ="ProjectName Required")]
        public string ProjectName { get; set; }
    }
}
