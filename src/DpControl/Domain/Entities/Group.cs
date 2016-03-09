using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        #region relationship
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
        public virtual List<GroupDeviceLocation> GroupDeviceLocations { get; set; }     // many-to-many
        public List<UserGroup> UserGroups { get; set; }       // many-to-many
        public int? SceneId { get; set; }
        public virtual Scene Scene { get; set; }         // one-to-many: one Scene can have multi-group; but one group only belong to a single scene.
        #endregion


        public Group()
        {
            this.GroupDeviceLocations = new List<GroupDeviceLocation>();
            this.UserGroups = new List<UserGroup>();
        }
    }
}
