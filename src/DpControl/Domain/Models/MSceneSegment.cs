using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class MSceneSegment
    {
        public int SceneSegmentId { get; set; }
        public int SequenceNo { get; set; }
        public string StartTime { get; set; }       //format: hhmm
        public int Volumn { get; set; }
    }
}
