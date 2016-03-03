using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class SceneSegment
    {
        public int SceneSegmentId { get; set; }
        public int SequenceNo { get; set; }
        public string StartTime { get; set; }       //format: hhmm
        public int Volumn { get; set; }

        #region relationship
        public int SceneId { get; set; }
        public virtual Scene Scene { get; set; }
        #endregion
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
