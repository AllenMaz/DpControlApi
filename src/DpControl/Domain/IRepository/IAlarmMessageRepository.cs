using DpControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.IRepository
{
    public interface IAlarmMessageRepository:IBaseRepository<AlarmMessageAddModel,AlarmMessageUpdateModel,AlarmMessageSearchModel>
    {
        #region Relations
        Task<IEnumerable<AlarmSubSearchModel>> GetAlarmsByAlarmMessageIdAsync(int alarmMessageId);
        #endregion
    }
}
