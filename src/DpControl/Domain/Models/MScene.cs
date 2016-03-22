using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class SceneBaseModel
    {
        [Required(ErrorMessage = "SceneName is required!")]
        [MaxLength(50, ErrorMessage = "SceneName must be less than 50 characters!")]
        public String SceneName { get; set; }

        [Required(ErrorMessage = "Enable is required!")]
        public bool Enable { get; set; }
    }
    public class SceneAddModel: SceneBaseModel
    {
        [Required(ErrorMessage = "ProjectId is required!")]
        public int ProjectId { get; set; }
        
    }

    public class SceneUpdateModel: SceneBaseModel
    {

    }

    public class SceneSearchModel : SceneBaseModel
    {
        public int SceneId { get; set; }

        public int? ProjectId { get; set; }

        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public IEnumerable<GroupSearchModel> Groups { get; set; }
        public IEnumerable<SceneSegmentSearchModel> SceneSegments { get; set; }
    }

    public static class SceneOperator
    {
        /// <summary>
        /// Cascade set SceneSearchModel Results
        /// </summary>
        /// <param name="scenes"></param>
        /// <returns></returns>
        public static IEnumerable<SceneSearchModel> SetSceneSearchModelCascade(List<Scene> scenes)
        {
            var sceneSearchModels = scenes.Select(s => SetSceneSearchModelCascade(s));
            return sceneSearchModels;
        }

        /// <summary>
        /// Cascade set SceneSearchModel Result
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static SceneSearchModel SetSceneSearchModelCascade(Scene scene)
        {
            if (scene == null) return null;
            var sceneSearchModel = new SceneSearchModel()
            {
                SceneId = scene.SceneId,
                SceneName = scene.SceneName,
                ProjectId = scene.ProjectId,
                Enable = scene.Enable,
                Creator = scene.Creator,
                CreateDate = scene.CreateDate,
                Modifier = scene.Modifier,
                ModifiedDate = scene.ModifiedDate,
                SceneSegments = SceneSegmentOperator.SetSceneSegmentSearchModelCascade(scene.SceneSegments),
                Groups = GroupOperator.SetGroupSearchModelCascade(scene.Groups)

            };
            return sceneSearchModel;
        }

    }
}
