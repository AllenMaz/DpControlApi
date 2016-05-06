using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        #region relationship

        /// <summary>
        /// there are three levels:
        /// SuperLevel:this level CustomerNo and ProjectNo must be empty
        /// CustomerLevel:this level CustomerNo is required and ProjectNo must be empty
        /// ProjectLevel:this level ProjectNo is required
        /// </summary>
        public int UserLevel { get; set; }

        public string CustomerNo { get; set; }

        public string ProjectNo { get; set; }

        public virtual List<UserGroup> UserGroups { get; set; }        //many-to-many: operator can contorl multiple-location motors
        public virtual List<UserLocation> UserLocations { get; set; }      //many-to-many: operator can contorl multiple-location motors


        #endregion

        public ApplicationUser()
        {
            this.UserGroups = new List<UserGroup>();
            this.UserLocations = new List<UserLocation>();
        }

    }

    
}
