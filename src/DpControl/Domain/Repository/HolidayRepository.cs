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
    public class HolidayRepository : IHolidayRepository
    {
        ShadingContext _context;
        private readonly IUserInfoManagerRepository _userInfoManager;

        #region Constructors
        public HolidayRepository()
        {
        }

        public HolidayRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        public HolidayRepository(ShadingContext dbContext, IUserInfoManagerRepository userInfoManager)
        {
            _context = dbContext;
            _userInfoManager = userInfoManager;
        }
        #endregion

        public int Add(HolidayAddModel mHoliday)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == mHoliday.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mHoliday.ProjectId);

            //Get UserInfo
            var user = _userInfoManager.GetUserInfoFromHttpHead();

            var model = new Holiday
            {
                ProjectId = mHoliday.ProjectId,
                Day = mHoliday.Day,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Holidays.Add(model);
            _context.SaveChanges();
            return model.HolidayId;
        }

        public async Task<int> AddAsync(HolidayAddModel mHoliday)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == mHoliday.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mHoliday.ProjectId);

            //Get UserInfo
            var user = await _userInfoManager.GetUserInfoFromHttpHeadAsync();

            var model = new Holiday
            {
                ProjectId = mHoliday.ProjectId,
                Day = mHoliday.Day,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Holidays.Add(model);
            await _context.SaveChangesAsync();
            return model.HolidayId;
        }

        public HolidaySearchModel FindById(int holidayId)
        {
            var result = _context.Holidays.Where(v => v.HolidayId == holidayId);
            result = (IQueryable<Holiday>)ExpandOperator.ExpandRelatedEntities<Holiday>(result);

            var holiday = result.FirstOrDefault();
            var holidaySearch = HolidayOperator.SetHolidaySearchModelCascade(holiday);

            return holidaySearch;
        }

        public async Task<HolidaySearchModel> FindByIdAsync(int holidayId)
        {
            var result = _context.Holidays.Where(v => v.HolidayId == holidayId);
            result = (IQueryable<Holiday>)ExpandOperator.ExpandRelatedEntities<Holiday>(result);

            var holiday = await result.FirstOrDefaultAsync();
            var holidaySearch = HolidayOperator.SetHolidaySearchModelCascade(holiday); 

            return holidaySearch;
        }

        public IEnumerable<HolidaySearchModel> GetAll()
        {
            var queryData = from H in _context.Holidays
                            select H;

            var result = QueryOperate<Holiday>.Execute(queryData);
            result = (IQueryable<Holiday>)ExpandOperator.ExpandRelatedEntities<Holiday>(result);

            //以下执行完后才会去数据库中查询
            var holidays = result.ToList();
            var holidaysSearch = HolidayOperator.SetHolidaySearchModelCascade(holidays);
            
            return holidaysSearch;
        }

        public async Task<IEnumerable<HolidaySearchModel>> GetAllAsync()
        {
            var queryData = from H in _context.Holidays
                            select H;

            var result = QueryOperate<Holiday>.Execute(queryData);
            result = (IQueryable<Holiday>)ExpandOperator.ExpandRelatedEntities<Holiday>(result);

            //以下执行完后才会去数据库中查询
            var holidays = await result.ToListAsync();
            var holidaysSearch = HolidayOperator.SetHolidaySearchModelCascade(holidays);

            return holidaysSearch;
        }

        public async Task<ProjectSubSearchModel> GetProjectByHolidayIdAsync(int holidayId)
        {
            var holiday = await _context.Holidays.Include(h => h.Project)
                .Where(h => h.HolidayId == holidayId).FirstOrDefaultAsync();
            var project = holiday == null ? null : holiday.Project;
            var projectSearch = ProjectOperator.SetProjectSubSearchModel(project);
            return projectSearch;
        }

        public void RemoveById(int holidayId)
        {
            var holiday = _context.Holidays.FirstOrDefault(c => c.HolidayId == holidayId);
            if (holiday == null)
                throw new ExpectException("Could not find data which HolidayId equal to " + holidayId);

            _context.Holidays.Remove(holiday);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int holidayId)
        {
            var holiday = _context.Holidays.FirstOrDefault(c => c.HolidayId == holidayId);
            if (holiday == null)
                throw new ExpectException("Could not find data which HolidayId equal to " + holidayId);

            _context.Holidays.Remove(holiday);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int holidayId, HolidayUpdateModel mHoliday)
        {
            var holiday = _context.Holidays.FirstOrDefault(c => c.HolidayId == holidayId);
            if (holiday == null)
                throw new ExpectException("Could not find data which HolidayId equal to " + holidayId);

            //Get UserInfo
            var user = _userInfoManager.GetUserInfoFromHttpHead();

            holiday.Day = mHoliday.Day;
            holiday.Modifier = user.UserName;
            holiday.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
            return holiday.HolidayId;
        }

        public async Task<int> UpdateByIdAsync(int holidayId, HolidayUpdateModel mHoliday)
        {
            var holiday = _context.Holidays.FirstOrDefault(c => c.HolidayId == holidayId);
            if (holiday == null)
                throw new ExpectException("Could not find data which HolidayId equal to " + holidayId);

            //Get UserInfo
            var user =await _userInfoManager.GetUserInfoFromHttpHeadAsync();

            holiday.Day = mHoliday.Day;
            holiday.Modifier = user.UserName;
            holiday.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return holiday.HolidayId;
        }
    }
}
