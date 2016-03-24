using DpControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.IRepository
{
    public interface ILogRepository:IBaseRepository<LogAddModel,LogUpdateModel,LogSearchModel>
    {
        #region Relations
        Task<LocationSubSearchModel> GetLocationByLogIdAsync(int logId);
        Task<LogDescriptionSubSearchModel> GetLogDescriptionByLogIdAsync(int logId);
        #endregion

    }
}
