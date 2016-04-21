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
using MimeKit;

namespace DpControl.Domain.Repository
{
    public class AlarmRepository : IAlarmRepository
    {
        private ShadingContext _context;

        #region Constructors
        public AlarmRepository()
        {
        }

        public AlarmRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        #endregion

        public int Add(AlarmAddModel mAlarm)
        {
            var location = _context.Locations.FirstOrDefault(a => a.LocationId == mAlarm.LocationId);
            if (location == null)
                throw new ExpectException("Could not find Location data which LocationId equal to " + mAlarm.LocationId);

            var alarmMessage = _context.AlarmMessages.FirstOrDefault(a => a.AlarmMessageId == mAlarm.AlarmMessageId);
            if (alarmMessage == null)
                throw new ExpectException("Could not find AlarmMessage data which AlarmMessageId equal to " + mAlarm.AlarmMessageId);


            var model = new Alarm
            {
                AlarmMessageId = mAlarm.AlarmMessageId,
                LocationId = mAlarm.LocationId,
                CreateDate = DateTime.Now
            };
            _context.Alarms.Add(model);
            _context.SaveChanges();
            return model.AlarmId;
        }

        public async Task<int> AddAsync(AlarmAddModel mAlarm)
        {
            var location = _context.Locations.FirstOrDefault(a=>a.LocationId == mAlarm.LocationId);
            if (location == null)
                throw new ExpectException("Could not find Location data which LocationId equal to " + mAlarm.LocationId);

            var alarmMessage = _context.AlarmMessages.FirstOrDefault(a=>a.AlarmMessageId == mAlarm.AlarmMessageId);
            if (alarmMessage == null)
                throw new ExpectException("Could not find AlarmMessage data which AlarmMessageId equal to " + mAlarm.AlarmMessageId);
            

            var model = new Alarm
            {
                AlarmMessageId = mAlarm.AlarmMessageId,
                LocationId = mAlarm.LocationId,
                CreateDate = DateTime.Now
            };
            _context.Alarms.Add(model);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(mAlarm.Email))
            {
                //Send Mail
                MailSend mailSend = new MailSend();
                mailSend.To.Add(new MailboxAddress("", mAlarm.Email));
                mailSend.Subject = "AlarmMessage";

                var builder = new BodyBuilder();

                // Set the plain-text version of the message text
                builder.HtmlBody = string.Format(@"
<!DOCTYPE html><html><head><meta charset='utf - 8'><title>MailAlarm</title><style></style></head><body id='preview'>
<p><font style='font-weight:bold;'>Location Information</font></p>
<p>DeviceSerialNo：{0}</p>
<p><font style='font-weight:bold;'>Alarm Message</font></p>
<p>ErrorCode：{1}</p>
<p>Message：{2}</p>
<br/>
<br/>
<br/>
--This is a system email 
</ body ></ html > ", location.DeviceSerialNo, alarmMessage.ErrorCode, alarmMessage.Message);


                mailSend.Body = builder.ToMessageBody();
                await mailSend.SendAsync();
            }
           

            return model.AlarmId;
        }

        public AlarmSearchModel FindById(int alarmId)
        {
            var result = _context.Alarms.Where(v => v.AlarmId == alarmId);
            result = (IQueryable<Alarm>)ExpandOperator.ExpandRelatedEntities<Alarm>(result);

            var alarm = result.FirstOrDefault();
            var alarmSearch = AlarmOperator.SetAlarmSearchModelCascade(alarm);
            return alarmSearch;
        }

        public async Task<AlarmSearchModel> FindByIdAsync(int alarmId)
        {
            var result = _context.Alarms.Where(v => v.AlarmId == alarmId);
            result = (IQueryable<Alarm>)ExpandOperator.ExpandRelatedEntities<Alarm>(result);

            var alarm = await result.FirstOrDefaultAsync();
            var alarmSearch = AlarmOperator.SetAlarmSearchModelCascade(alarm);
            return alarmSearch;
        }

        

        public IEnumerable<AlarmSearchModel> GetAll()
        {
            var queryData = from A in _context.Alarms
                            select A;

            var result = QueryOperate<Alarm>.Execute(queryData);
            result = (IQueryable<Alarm>)ExpandOperator.ExpandRelatedEntities<Alarm>(result);

            //以下执行完后才会去数据库中查询
            var alarms = result.ToList();
            var alarmsSearch = AlarmOperator.SetAlarmSearchModelCascade(alarms);

            return alarmsSearch;
        }

        public async Task<IEnumerable<AlarmSearchModel>> GetAllAsync()
        {
            var queryData = from A in _context.Alarms
                            select A;

            var result = QueryOperate<Alarm>.Execute(queryData);
            result = (IQueryable<Alarm>)ExpandOperator.ExpandRelatedEntities<Alarm>(result);

            //以下执行完后才会去数据库中查询
            var alarms = await result.ToListAsync();
            var alarmsSearch = AlarmOperator.SetAlarmSearchModelCascade(alarms);

            return alarmsSearch;
        }

        public async Task<LocationSubSearchModel> GetLocationByAlarmIdAsync(int alarmId)
        {
            var alarm = await _context.Alarms
                .Include(a => a.Location).Where(a => a.AlarmId == alarmId).FirstOrDefaultAsync();
            var location = alarm == null ? null : alarm.Location;
            var locationSearch = LocationOperator.SetLocationSubSearchModel(location);
            return locationSearch;
        }

        public async Task<AlarmMessageSubSearchModel> GetAlarmMessageByAlarmIdAsync(int alarmId)
        {
            var alarm = await _context.Alarms
                .Include(a => a.AlarmMessage).Where(a => a.AlarmId == alarmId).FirstOrDefaultAsync();
            var alarmMessage = alarm == null ? null : alarm.AlarmMessage;
            var alarmMessageSearch = AlarmMessageOperator.SetAlarmMessageSubSearchModel(alarmMessage);
            return alarmMessageSearch;
        }

        public void RemoveById(int alarmId)
        {
            var alarm = _context.Alarms.FirstOrDefault(c => c.AlarmId == alarmId);
            if (alarm == null)
                throw new ExpectException("Could not find data which AlarmId equal to " + alarmId);

            _context.Remove(alarm);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int alarmId)
        {
            var alarm = _context.Alarms.FirstOrDefault(c => c.AlarmId == alarmId);
            if (alarm == null)
                throw new ExpectException("Could not find data which AlarmId equal to " + alarmId);

            _context.Remove(alarm);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int alarmId, AlarmUpdateModel mAlarm)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateByIdAsync(int itemId, AlarmUpdateModel item)
        {
            throw new NotImplementedException();
        }
    }
}
