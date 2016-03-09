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

    public class SceneSearchMoodel: SceneBaseModel
    {
        public int SceneId { get; set; }

        public int? ProjectId { get; set; }

        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
