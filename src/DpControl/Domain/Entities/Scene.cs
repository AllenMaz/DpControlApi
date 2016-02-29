using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Scene
    {
        public int SceneId { get; set; }
        public String Name { get; set; }
        public bool Enable { get; set; }

        #region relationship
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public virtual List<Group> Groups { get; set; }           // one-to-many: one Scene can have multiple group; but one group only belong to one scene.
        public virtual List<SceneSegment> SceneSegments { get; set; }
        #endregion

        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public Scene()
        {
            this.Groups = new List<Group>();
            this.SceneSegments = new List<SceneSegment>();
        }
    }
}
