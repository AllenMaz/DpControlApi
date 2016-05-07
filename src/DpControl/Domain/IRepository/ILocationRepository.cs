using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ILocationRepository:IBaseRepository<LocationAddModel,LocationUpdateModel,LocationSearchModel>
        ,IRelationsRepository<int>
    {
        #region Relations
        Task<ProjectSubSearchModel> GetProjectByLocationIdAsync(int locationId);
        Task<DeviceSubSearchModel> GetDeviceByLocationIdAsync(int locationId);
        Task<IEnumerable<LogSubSearchModel>> GetLogsByLocationIdAsync(int locationId);
        Task<IEnumerable<AlarmSubSearchModel>> GetAlarmsByLocationIdAsync(int locationId);
        Task<IEnumerable<GroupSubSearchModel>> GetGroupsByLocationIdAsync(int locationId);
        #endregion

        
    }
}
