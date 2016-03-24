using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IProjectRepository:IBaseRepository<ProjectAddModel,ProjectUpdateModel,ProjectSearchModel>
    {
        #region Entity Relations
        Task<CustomerSubSearchModel> GetCustomerByProjectIdAsync(int projectId);
        Task<IEnumerable<SceneSubSearchModel>> GetScenesByProjectIdAsync(int projectId);
        Task<IEnumerable<GroupSubSearchModel>> GetGroupsByProjectIdAsync(int projectId);
        Task<IEnumerable<LocationSubSearchModel>> GetLocationsByProjectIdAsync(int projectId);
        Task<IEnumerable<HolidaySubSearchModel>> GetHolidaysByProjectIdAsync(int projectId);

        #endregion
    }
}
