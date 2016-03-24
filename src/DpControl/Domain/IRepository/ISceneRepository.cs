using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ISceneRepository:IBaseRepository<SceneAddModel,SceneUpdateModel,SceneSearchModel>
    {
        #region Relations
        Task<ProjectSubSearchModel> GetProjectBySceneIdAsync(int sceneId);
        Task<IEnumerable<SceneSegmentSubSearchModel>> GetSceneSegmentsBySceneIdAsync(int sceneId);
        #endregion
    }
}
