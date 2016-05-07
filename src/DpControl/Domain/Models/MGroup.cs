using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class GroupBaseModel
    {
        [Required(ErrorMessage = "GroupName is required!")]
        [MaxLength(50, ErrorMessage = "GroupName must be less than 50 characters!")]
        public string GroupName { get; set; }

        public int? SceneId { get; set; }
    }

    public class GroupAddModel: GroupBaseModel
    {
        [Required(ErrorMessage = "ProjectId is required!")]
        public int ProjectId { get; set; }
        
    }

    public class GroupUpdateModel: GroupBaseModel
    {

    }

    public class GroupSubSearchModel: GroupBaseModel
    {
        public int GroupId { get; set; }
        public int? ProjectId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    
    public class GroupSearchModel : GroupSubSearchModel
    {
        public ProjectSubSearchModel Project { get; set; }
        public SceneSubSearchModel Scene { get; set; }
        public IEnumerable<LocationSubSearchModel> Locations { get; set; }

        public IEnumerable<UserSubSearchModel> Users { get; set; }
    }

    public static class GroupOperator
    {
        /// <summary>
        /// Cascade set GroupSearchModel Results
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static IEnumerable<GroupSearchModel> SetGroupSearchModelCascade(List<Group> groups)
        {
            var groupSearchModels = groups.Select(s => SetGroupSearchModelCascade(s));
            return groupSearchModels;
        }

        /// <summary>
        /// Cascade set GroupSearchModel Result
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static GroupSearchModel SetGroupSearchModelCascade(Group group)
        {
            if (group == null) return null;
            var groupSearchModel = new GroupSearchModel()
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                ProjectId = group.ProjectId,
                SceneId = group.SceneId,
                Creator = group.Creator,
                CreateDate = group.CreateDate,
                Modifier = group.Modifier,
                ModifiedDate = group.ModifiedDate,
                Project = ProjectOperator.SetProjectSubSearchModel(group.Project),
                Scene = SceneOperator.SetSceneSubSearchModel(group.Scene),
                Locations = group.GroupLocations.Select(v => LocationOperator.SetLocationSearchModelCascade(v.Location)),
                Users = group.UserGroups.Select(v=>UserOperator.SetUserSearchModelCascade(v.User))

            };
            return groupSearchModel;
        }

        /// <summary>
        /// Cascade set GroupSubSearchModel Results
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static IEnumerable<GroupSubSearchModel> SetGroupSubSearchModel(List<Group> groups)
        {
            var groupSearchModels = groups.Select(s => SetGroupSubSearchModel(s));
            return groupSearchModels;
        }

        /// <summary>
        /// Cascade set GroupSubSearchModel Result
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static GroupSubSearchModel SetGroupSubSearchModel(Group group)
        {
            if (group == null) return null;
            var groupSearchModel = new GroupSubSearchModel()
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                ProjectId = group.ProjectId,
                SceneId = group.SceneId,
                Creator = group.Creator,
                CreateDate = group.CreateDate,
                Modifier = group.Modifier,
                ModifiedDate = group.ModifiedDate

            };
            return groupSearchModel;
        }

    }
}
