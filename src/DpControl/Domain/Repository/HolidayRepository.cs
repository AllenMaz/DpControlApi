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

        #region Constructors
        public HolidayRepository()
        {
        }

        public HolidayRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        #endregion

        public int Add(HolidayAddModel mHoliday)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == mHoliday.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mHoliday.ProjectId);


            var model = new Holiday
            {
                ProjectId = mHoliday.ProjectId,
                Day = mHoliday.Day,
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


            var model = new Holiday
            {
                ProjectId = mHoliday.ProjectId,
                Day = mHoliday.Day,
                CreateDate = DateTime.Now
            };
            _context.Holidays.Add(model);
            await _context.SaveChangesAsync();
            return model.HolidayId;
        }

        public HolidaySearchModel FindById(int holidayId)
        {
            var holiday = _context.Holidays.Where(v => v.HolidayId == holidayId)
               .Select(v => new HolidaySearchModel()
               {
                   HolidayId = v.HolidayId,
                   ProjectId = v.ProjectId,
                   Day = v.Day,
                   CreateDate = v.CreateDate.ToString()
               }).FirstOrDefault();

            return holiday;
        }

        public async Task<HolidaySearchModel> FindByIdAsync(int holidayId)
        {
            var holiday = await _context.Holidays.Where(v => v.HolidayId == holidayId)
               .Select(v => new HolidaySearchModel()
               {
                   HolidayId = v.HolidayId,
                   ProjectId = v.ProjectId,
                   Day =v.Day,
                   CreateDate = v.CreateDate.ToString()
               }).FirstOrDefaultAsync();

            return holiday;
        }

        public IEnumerable<HolidaySearchModel> GetAll(Query query)
        {
            var queryData = from H in _context.Holidays
                            select H;

            var result = QueryOperate<Holiday>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var holidays = result.ToList();

            var holidaysSearch = holidays.Select(v => new HolidaySearchModel
            {
                HolidayId = v.HolidayId,
                ProjectId = v.ProjectId,
                Day = v.Day,
                CreateDate = v.CreateDate.ToString()
            });

            return holidaysSearch;
        }

        public async Task<IEnumerable<HolidaySearchModel>> GetAllAsync(Query query)
        {
            var queryData = from H in _context.Holidays
                            select H;

            var result = QueryOperate<Holiday>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var holidays = await result.ToListAsync();

            var holidaysSearch = holidays.Select(v => new HolidaySearchModel
            {
                HolidayId = v.HolidayId,
                ProjectId = v.ProjectId,
                Day = v.Day,
                CreateDate = v.CreateDate.ToString()
            });

            return holidaysSearch;
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


            holiday.Day = mHoliday.Day;

            _context.SaveChanges();
            return holiday.HolidayId;
        }

        public async Task<int> UpdateByIdAsync(int holidayId, HolidayUpdateModel mHoliday)
        {
            var holiday = _context.Holidays.FirstOrDefault(c => c.HolidayId == holidayId);
            if (holiday == null)
                throw new ExpectException("Could not find data which HolidayId equal to " + holidayId);


            holiday.Day = mHoliday.Day;

            await _context.SaveChangesAsync();
            return holiday.HolidayId;
        }
    }
}
