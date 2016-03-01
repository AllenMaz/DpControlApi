using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class SceneSegmentBaseModel
    {
        [Required(ErrorMessage = "SequenceNo is required!")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "SequenceNo must be a positive integer")]
        public int SequenceNo { get; set; }

        [Required(ErrorMessage = "StartTime is required!")]
        [RegularExpression(@"^([0-1][0-9]|[2][0-3]):([0-5][0-9])$", ErrorMessage = "StartTime format error,The correct format is 'hh:mm',eg:12:00")]
        public string StartTime { get; set; }       //format: hhmm

        [Required(ErrorMessage = "Volumn is required!")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Volumn must be a positive integer")]
        public int Volumn { get; set; }
    }

    public class SceneSegmentAddModel:SceneSegmentBaseModel
    {
        [Required(ErrorMessage = "Volumn is required!")]
        public int SceneId { get; set; }
        
    }

    public class SceneSegmentUpdateModel: SceneSegmentBaseModel
    {

    }

    public class SceneSegmentSearchModel: SceneSegmentBaseModel
    {
        public int SceneSegmentId { get; set; }
        public int SceneId { get; set; }
        public string CreateDate { get; set; }
    }
}
