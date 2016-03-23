using DpControl.Domain.Entities;
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

    public class SceneSegmentSubSearchModel: SceneSegmentBaseModel
    {
        public int SceneSegmentId { get; set; }
        public int SceneId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    
    public class SceneSegmentSearchModel: SceneSegmentSubSearchModel
    {
        public SceneSubSearchModel Scene { get; set; }
    }

    public static class SceneSegmentOperator
    {
        /// <summary>
        /// Cascade set SceneSegmentSearchModel Results
        /// </summary>
        public static IEnumerable<SceneSegmentSearchModel> SetSceneSegmentSearchModelCascade(List<SceneSegment> sceneSegments)
        {
            var sceneSegmentSearchModels = sceneSegments.Select(c => SetSceneSegmentSearchModelCascade(c));

            return sceneSegmentSearchModels;
        }

        /// <summary>
        /// Cascade set SceneSegmentSearchModel Result
        /// </summary>
        /// <param name="sceneSegment"></param>
        /// <returns></returns>
        public static SceneSegmentSearchModel SetSceneSegmentSearchModelCascade(SceneSegment sceneSegment)
        {
            if (sceneSegment == null) return null;
            var sceneSegmentSearchModel = new SceneSegmentSearchModel
            {
                SceneSegmentId = sceneSegment.SceneSegmentId,
                SceneId = sceneSegment.SceneId,
                SequenceNo = sceneSegment.SequenceNo,
                StartTime = sceneSegment.StartTime,
                Volumn = sceneSegment.Volumn,
                Creator = sceneSegment.Creator,
                CreateDate = sceneSegment.CreateDate,
                Modifier = sceneSegment.Modifier,
                ModifiedDate = sceneSegment.ModifiedDate,
                Scene = SceneOperator.SetSceneSubSearchModel(sceneSegment.Scene)
            };

            return sceneSegmentSearchModel;
        }

        /// <summary>
        /// Cascade set SceneSubSegmentSearchModel Results
        /// </summary>
        public static IEnumerable<SceneSegmentSubSearchModel> SetSceneSegmentSubSearchModel(List<SceneSegment> sceneSegments)
        {
            var sceneSegmentSearchModels = sceneSegments.Select(c => SetSceneSegmentSubSearchModel(c));

            return sceneSegmentSearchModels;
        }

        /// <summary>
        /// Cascade set SceneSegmentSearchModel Result
        /// </summary>
        /// <param name="sceneSegment"></param>
        /// <returns></returns>
        public static SceneSegmentSubSearchModel SetSceneSegmentSubSearchModel(SceneSegment sceneSegment)
        {
            if (sceneSegment == null) return null;
            var sceneSegmentSearchModel = new SceneSegmentSubSearchModel
            {
                SceneSegmentId = sceneSegment.SceneSegmentId,
                SceneId = sceneSegment.SceneId,
                SequenceNo = sceneSegment.SequenceNo,
                StartTime = sceneSegment.StartTime,
                Volumn = sceneSegment.Volumn,
                Creator = sceneSegment.Creator,
                CreateDate = sceneSegment.CreateDate,
                Modifier = sceneSegment.Modifier,
                ModifiedDate = sceneSegment.ModifiedDate
            };

            return sceneSegmentSearchModel;
        }
    }
}
