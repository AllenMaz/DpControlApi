using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNo { get; set; }
        public bool Completed { get; set; }
        #region relationship
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }
        public virtual List<Location> DeviceLocations { get; set; }           // Devices contained in a company accessed through Locations
        public virtual List<Group> Groups { get; set; }
        public virtual List<Scene> Scenes { get; set; }
        public virtual List<Holiday> Holidays { get; set; }
        #endregion

        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public Project()
        {
            this.Users = new List<ApplicationUser>();
            this.DeviceLocations = new List<Location>();
            this.Groups = new List<Group>();
            this.Scenes = new List<Scene>();
            this.Holidays = new List<Holiday>();
        }
    }
}
