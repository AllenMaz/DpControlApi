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
        private readonly ILoginUserRepository _loginUser;

        #region Constructors
        public HolidayRepository()
        {
        }

        public HolidayRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        public HolidayRepository(ShadingContext dbContext, ILoginUserRepository loginUser)
        {
            _context = dbContext;
            _loginUser = loginUser;
        }
        #endregion

        public int Add(HolidayAddModel mHoliday)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == mHoliday.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mHoliday.ProjectId);

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

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
            var user =  _loginUser.GetLoginUserInfo();

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

            #region filter by login user
            var loginUser = _loginUser.GetLoginUserInfo();
            if (loginUser.isCustomerLevel)
            {
                var customer = _context.Customers
                    .Include(c => c.Projects)
                    .Where(c => c.CustomerNo == loginUser.CustomerNo).FirstOrDefault();
                var projectIds = customer.Projects.Select(p => p.ProjectId);
                queryData = queryData.Where(s => projectIds.Contains((int)s.ProjectId));

            }
            else if (loginUser.isProjectLevel)
            {
                var project = _context.Projects.Where(p => p.ProjectNo == loginUser.ProjectNo).FirstOrDefault();
                queryData = queryData.Where(s => s.ProjectId == project.ProjectId);
            }
            #endregion

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

            #region filter by login user
            var loginUser = _loginUser.GetLoginUserInfo();
            if (loginUser.isCustomerLevel)
            {
                var customer = _context.Customers
                    .Include(c => c.Projects)
                    .Where(c => c.CustomerNo == loginUser.CustomerNo).FirstOrDefault();
                var projectIds = customer.Projects.Select(p => p.ProjectId);
                queryData = queryData.Where(s => projectIds.Contains((int)s.ProjectId));

            }
            else if (loginUser.isProjectLevel)
            {
                var project = _context.Projects.Where(p => p.ProjectNo == loginUser.ProjectNo).FirstOrDefault();
                queryData = queryData.Where(s => s.ProjectId == project.ProjectId);
            }
            #endregion

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
            var user = _loginUser.GetLoginUserInfo();

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
            var user = _loginUser.GetLoginUserInfo();

            holiday.Day = mHoliday.Day;
            holiday.Modifier = user.UserName;
            holiday.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return holiday.HolidayId;
        }
    }
}
