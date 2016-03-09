using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class ApplicationUser:IdentityUser
    {
        #region relationship
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public virtual List<UserGroup> UserGroups { get; set; }        //many-to-many: operator can contorl multiple-location motors
        public virtual List<UserDeviceLocation> UserDeviceLocations { get; set; }      //many-to-many: operator can contorl multiple-location motors
        public virtual List<Log> Logs { get; set; }

        #endregion

        public ApplicationUser()
        {
            this.UserGroups = new List<UserGroup>();
            this.UserDeviceLocations = new List<UserDeviceLocation>();
            this.Logs = new List<Log>();
        }

    }
}
