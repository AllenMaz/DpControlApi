using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using DpControl.Domain.Execptions;
using DpControl.Domain.Entities;
using Microsoft.Data.Entity;

namespace DpControl.Domain.Repository
{
    public class LogRepository : ILogRepository
    {
        ShadingContext _context;
        private readonly ILoginUserRepository _loginUser;

        #region
        public LogRepository()
        {

        }
        public LogRepository(ShadingContext context)
        {
            _context = context;
        }

        public LogRepository(ShadingContext context, ILoginUserRepository loginUser)
        {
            _context = context;
            _loginUser = loginUser;
        }
        #endregion

        public int Add(LogAddModel mLog)
        {
            var location = _context.Locations.FirstOrDefault(a => a.LocationId == mLog.LocationId);
            if (location == null)
                throw new ExpectException("Could not find Location data which LocationId equal to " + mLog.LocationId);

            var logDescription = _context.LogDescriptions.FirstOrDefault(a => a.LogDescriptionId == mLog.LogDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find LogDescription data which LogDescriptionId equal to " + mLog.LogDescriptionId);

            var user = _loginUser.GetLoginUserInfo();

            var model = new Log
            {
                Comment = mLog.Comment,
                LocationId = mLog.LocationId,
                LogDescriptionId = mLog.LogDescriptionId,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Logs.Add(model);
            _context.SaveChanges();
            return model.LogId;
        }

        public async Task<int> AddAsync(LogAddModel mLog)
        {
            var location = _context.Locations.FirstOrDefault(a => a.LocationId == mLog.LocationId);
            if (location == null)
                throw new ExpectException("Could not find Location data which LocationId equal to " + mLog.LocationId);

            var logDescription = _context.LogDescriptions.FirstOrDefault(a => a.LogDescriptionId == mLog.LogDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find LogDescription data which LogDescriptionId equal to " + mLog.LogDescriptionId);

            var user = _loginUser.GetLoginUserInfo();

            var model = new Log
            {
                Comment = mLog.Comment,
                LocationId = mLog.LocationId,
                LogDescriptionId = mLog.LogDescriptionId,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Logs.Add(model);
            await _context.SaveChangesAsync();
            return model.LogId;
        }

        public LogSearchModel FindById(int logId)
        {
            var result = _context.Logs.Where(v => v.LogId == logId);
            result = (IQueryable<Log>)ExpandOperator.ExpandRelatedEntities<Log>(result);

            var log = result.FirstOrDefault();
            var logSearch = LogOperator.SetLogSearchModelCascade(log);
            return logSearch;
        }

        public async Task<LogSearchModel> FindByIdAsync(int logId)
        {
            var result = _context.Logs.Where(v => v.LogId == logId);
            result = (IQueryable<Log>)ExpandOperator.ExpandRelatedEntities<Log>(result);

            var log = await result.FirstOrDefaultAsync();
            var logSearch = LogOperator.SetLogSearchModelCascade(log);
            return logSearch;
        }

        public IEnumerable<LogSearchModel> GetAll()
        {
            var queryData = from L in _context.Logs
                            select L;

            var result = QueryOperate<Log>.Execute(queryData);
            result = (IQueryable<Log>)ExpandOperator.ExpandRelatedEntities<Log>(result);

            //以下执行完后才会去数据库中查询
            var logs = result.ToList();
            var logsSearch = LogOperator.SetLogSearchModelCascade(logs);

            return logsSearch;
        }

        public async Task<IEnumerable<LogSearchModel>> GetAllAsync()
        {
            var queryData = from L in _context.Logs
                            select L;

            var result = QueryOperate<Log>.Execute(queryData);
            result = (IQueryable<Log>)ExpandOperator.ExpandRelatedEntities<Log>(result);

            //以下执行完后才会去数据库中查询
            var logs = await result.ToListAsync();
            var logsSearch = LogOperator.SetLogSearchModelCascade(logs);

            return logsSearch;
        }

        public async Task<LocationSubSearchModel> GetLocationByLogIdAsync(int logId)
        {
            var log = await _context.Logs.Include(l => l.Location)
                .Where(l => l.LogId == logId).FirstOrDefaultAsync();
            var location = log == null ? null : log.Location;
            var locationSearch = LocationOperator.SetLocationSubSearchModel(location);
            return locationSearch;
        }

        public async Task<LogDescriptionSubSearchModel> GetLogDescriptionByLogIdAsync(int logId)
        {
            var log = await _context.Logs.Include(l => l.LogDescription)
               .Where(l => l.LogId == logId).FirstOrDefaultAsync();
            var logDescription = log == null ? null : log.LogDescription;
            var logDescriptionSearch = LogDescriptionOperator.SetLogDescriptionSubSearchModel(logDescription);
            return logDescriptionSearch;
        }

        public void RemoveById(int logId)
        {
            var log = _context.Logs.FirstOrDefault(c => c.LogId == logId);
            if (log == null)
                throw new ExpectException("Could not find data which LogId equal to " + logId);

            _context.Remove(log);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int logId)
        {
            var log = _context.Logs.FirstOrDefault(c => c.LogId == logId);
            if (log == null)
                throw new ExpectException("Could not find data which LogId equal to " + logId);

            _context.Remove(log);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int logId, LogUpdateModel mLog)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateByIdAsync(int logId, LogUpdateModel mLog)
        {
            throw new NotImplementedException();
        }
    }
}
