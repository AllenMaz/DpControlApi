using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class ProjectBaseModel
    {
        [Required(ErrorMessage = "ProjectName is required!")]
        [MaxLength(50, ErrorMessage = "ProjectName must be less than 50 characters!")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "ProjectNo is required!")]
        [MaxLength(50, ErrorMessage = "ProjectNo must be less than 50 characters!")]
        public string ProjectNo { get; set; }
    }

    public class ProjectAddModel: ProjectBaseModel
    {
        [Required(ErrorMessage = "CustomerId is required!")]
        public int? CustomerId { get; set; }

    }

    public class ProjectUpdateModel: ProjectBaseModel
    {
        [Required(ErrorMessage = "Completed is required!")]
        public bool Completed { get; set; }
    }

    public class ProjectSubSearchModel: ProjectBaseModel
    {
        public int? CustomerId { get; set; }
        public int ProjectId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Completed { get; set; }
    }

    public class ProjectSearchModel : ProjectSubSearchModel
    {
        public CustomerSubSearchModel Customer { get; set; }
        public IEnumerable<LocationSubSearchModel> Locations { get; set; }          
        public IEnumerable<GroupSubSearchModel> Groups { get; set; }
        public IEnumerable<SceneSubSearchModel> Scenes { get; set; }
        public IEnumerable<HolidaySubSearchModel> Holidays { get; set; }
    }

    public static class ProjectOperator
    {
        /// <summary>
        /// Cascade set ProjectSearchModel Results
        /// </summary>
        /// <param name="projects"></param>
        /// <returns></returns>
        public static IEnumerable<ProjectSearchModel> SetProjectSearchModelCascade(List<Project> projects)
        {
            var projectSearchModels = projects.Select(p => SetProjectSearchModelCascade(p));
            return projectSearchModels;
        }

        /// <summary>
        /// Cascade set ProjectSearchModel Result
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static ProjectSearchModel SetProjectSearchModelCascade(Project project)
        {
            if (project == null) return null;
            var projectSearchModel = new ProjectSearchModel()
            {
                ProjectId = project.ProjectId,
                ProjectNo = project.ProjectNo,
                ProjectName = project.ProjectName,
                CustomerId = project.CustomerId,
                Completed = project.Completed,
                Creator = project.Creator,
                CreateDate = project.CreateDate,
                Modifier = project.Modifier,
                ModifiedDate = project.ModifiedDate,
                Customer = CustomerOperator.SetCustomerSubSearchModel(project.Customer),
                Scenes = SceneOperator.SetSceneSearchModelCascade(project.Scenes),
                Groups = GroupOperator.SetGroupSearchModelCascade(project.Groups),
                Locations = LocationOperator.SetLocationSearchModelCascade(project.Locations),
                Holidays = HolidayOperator.SetHolidaySearchModelCascade(project.Holidays)
            };
            return projectSearchModel;
            
            
        }

        /// <summary>
        /// Cascade set ProjectSubSearchModel Results
        /// </summary>
        /// <param name="projects"></param>
        /// <returns></returns>
        public static IEnumerable<ProjectSubSearchModel> SetProjectSubSearchModel(List<Project> projects)
        {
            var projectSearchModels = projects.Select(p => SetProjectSubSearchModel(p));
            return projectSearchModels;
        }

        /// <summary>
        /// Cascade set ProjectSubSearchModel Result
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static ProjectSubSearchModel SetProjectSubSearchModel(Project project)
        {
            if (project == null) return null;
            var projectSearchModel = new ProjectSubSearchModel()
            {
                ProjectId = project.ProjectId,
                ProjectNo = project.ProjectNo,
                ProjectName = project.ProjectName,
                CustomerId = project.CustomerId,
                Completed = project.Completed,
                Creator = project.Creator,
                CreateDate = project.CreateDate,
                Modifier = project.Modifier,
                ModifiedDate = project.ModifiedDate
            };
            return projectSearchModel;


        }
    }
}
