using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IAlarmRepository:IBaseRepository<AlarmAddModel,AlarmUpdateModel,AlarmSearchModel>
    {
        #region Relations
        Task<LocationSubSearchModel> GetLocationByAlarmIdAsync(int alarmId);
        Task<AlarmMessageSubSearchModel> GetAlarmMessageByAlarmIdAsync(int alarmId);
        #endregion
    }
}
