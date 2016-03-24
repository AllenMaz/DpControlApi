using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ISceneSegmentRepository:IBaseRepository<SceneSegmentAddModel,SceneSegmentUpdateModel,SceneSegmentSearchModel>
    {
        #region Relations
        Task<SceneSubSearchModel> GetSceneBySceneSegmentIdAsync(int sceneSegmentId);
        #endregion
    }
}
