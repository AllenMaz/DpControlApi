using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IGroupRepository:IBaseRepository<GroupAddModel,GroupUpdateModel,GroupSearchModel>
    {
        #region Relations
        Task<SceneSubSearchModel> GetSceneByGroupIdAsync(int groupId);
        Task<ProjectSubSearchModel> GetProjectByGroupIdAsync(int groupId);
        Task<IEnumerable<LocationSubSearchModel>> GetLocationsByGroupIdAsync(int groupId);
        #endregion
    }
}   
