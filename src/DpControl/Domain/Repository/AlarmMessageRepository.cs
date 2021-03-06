﻿using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;

namespace DpControl.Domain.Repository
{
    public class AlarmMessageRepository : IAlarmMessageRepository
    {
        private ShadingContext _context;

        #region Constructors
        public AlarmMessageRepository()
        {
        }

        public AlarmMessageRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        #endregion

        public int Add(AlarmMessageAddModel mAlarmMessage)
        {
            //ErrorCode must be unique
            var checkData = _context.AlarmMessages
                .Where(c => c.ErrorCode == mAlarmMessage.ErrorCode).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which ErrorCode equal to " + mAlarmMessage.ErrorCode + " already exist in system");


            var model = new AlarmMessage
            {
                ErrorCode = mAlarmMessage.ErrorCode,
                Message = mAlarmMessage.Message
            };
            _context.AlarmMessages.Add(model);
            _context.SaveChanges();
            return model.AlarmMessageId;
        }

        public async Task<int> AddAsync(AlarmMessageAddModel mAlarmMessage)
        {
            //ErrorCode must be unique
            var checkData =await _context.AlarmMessages
                .Where(c => c.ErrorCode == mAlarmMessage.ErrorCode).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which ErrorCode equal to " + mAlarmMessage.ErrorCode + " already exist in system");


            var model = new AlarmMessage
            {
                ErrorCode =  mAlarmMessage.ErrorCode,
                Message = mAlarmMessage.Message
            };
            _context.AlarmMessages.Add(model);
            await _context.SaveChangesAsync();
            return model.AlarmMessageId;
        }

        public AlarmMessageSearchModel FindById(int alarmMessageId)
        {
            var result = _context.AlarmMessages
               .Where(v => v.AlarmMessageId == alarmMessageId);
            result = (IQueryable<AlarmMessage>)ExpandOperator.ExpandRelatedEntities<AlarmMessage>(result);

            var alarmMessage = result.FirstOrDefault();
            var alarmMessageSearch = AlarmMessageOperator.SetAlarmMessageSearchModelCascade(alarmMessage);
            return alarmMessageSearch;
        }

        public async Task<AlarmMessageSearchModel> FindByIdAsync(int alarmMessageId)
        {
            var result = _context.AlarmMessages
               .Where(v => v.AlarmMessageId == alarmMessageId);
            result = (IQueryable<AlarmMessage>)ExpandOperator.ExpandRelatedEntities<AlarmMessage>(result);

            var alarmMessage = await result.FirstOrDefaultAsync();
            var alarmMessageSearch = AlarmMessageOperator.SetAlarmMessageSearchModelCascade(alarmMessage);
            return alarmMessageSearch;
        }

        public async Task<IEnumerable<AlarmSubSearchModel>> GetAlarmsByAlarmMessageIdAsync(int alarmMessageId)
        {
            var queryData = _context.Alarms.Where(a=>a.AlarmMessageId == alarmMessageId);
            var result = QueryOperate<Alarm>.Execute(queryData);
            var alarms = await result.ToListAsync();
            var alarmsSearch = AlarmOperator.SetAlarmSubSearchModel(alarms);
            return alarmsSearch;
        }

        public IEnumerable<AlarmMessageSearchModel> GetAll()
        {
            var queryData = from A in _context.AlarmMessages
                            select A;

            var result = QueryOperate<AlarmMessage>.Execute(queryData);
            result = (IQueryable<AlarmMessage>)ExpandOperator.ExpandRelatedEntities<AlarmMessage>(result);

            //以下执行完后才会去数据库中查询
            var alarmMessages = result.ToList();
            var alarmMessagesSearch = AlarmMessageOperator.SetAlarmMessageSearchModelCascade(alarmMessages);

            return alarmMessagesSearch;
        }

        public async Task<IEnumerable<AlarmMessageSearchModel>> GetAllAsync()
        {
            var queryData = from A in _context.AlarmMessages
                            select A;

            var result = QueryOperate<AlarmMessage>.Execute(queryData);
            result = (IQueryable<AlarmMessage>)ExpandOperator.ExpandRelatedEntities<AlarmMessage>(result);

            //以下执行完后才会去数据库中查询
            var alarmMessages = await result.ToListAsync();
            var alarmMessagesSearch = AlarmMessageOperator.SetAlarmMessageSearchModelCascade(alarmMessages);

            return alarmMessagesSearch;
        }
       
        public void RemoveById(int alarmMessageId)
        {
            var alarmMessage = _context.AlarmMessages.FirstOrDefault(c => c.AlarmMessageId == alarmMessageId);
            if (alarmMessage == null)
                throw new ExpectException("Could not find data which AlarmMessageId equal to " + alarmMessageId);

            _context.Remove(alarmMessage);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int alarmMessageId)
        {
            var alarmMessage = _context.AlarmMessages.FirstOrDefault(c => c.AlarmMessageId == alarmMessageId);
            if (alarmMessage == null)
                throw new ExpectException("Could not find data which AlarmMessageId equal to " + alarmMessageId);

            _context.Remove(alarmMessage);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int alarmMessageId, AlarmMessageUpdateModel mAlarmMessage)
        {
            var alarmMessage = _context.AlarmMessages.FirstOrDefault(c => c.AlarmMessageId == alarmMessageId);
            if (alarmMessage == null)
                throw new ExpectException("Could not find data which AlarmMessageId equal to " + alarmMessageId);

            //ErrorCode must be unique
            var checkData = _context.AlarmMessages
                .Where(c => c.ErrorCode == mAlarmMessage.ErrorCode
                && c.AlarmMessageId != alarmMessageId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which ErrorCode equal to " + mAlarmMessage.ErrorCode + " already exist in system");


            alarmMessage.ErrorCode = mAlarmMessage.ErrorCode;
            alarmMessage.Message = mAlarmMessage.Message;

            _context.SaveChanges();
            return alarmMessage.AlarmMessageId;
        }

        public async Task<int> UpdateByIdAsync(int alarmMessageId, AlarmMessageUpdateModel mAlarmMessage)
        {
            var alarmMessage = _context.AlarmMessages.FirstOrDefault(c => c.AlarmMessageId == alarmMessageId);
            if (alarmMessage == null)
                throw new ExpectException("Could not find data which AlarmMessageId equal to " + alarmMessageId);

            //ErrorCode must be unique
            var checkData = await _context.AlarmMessages
                .Where(c => c.ErrorCode == mAlarmMessage.ErrorCode
                && c.AlarmMessageId != alarmMessageId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which ErrorCode equal to " + mAlarmMessage.ErrorCode + " already exist in system");



            alarmMessage.ErrorCode = mAlarmMessage.ErrorCode;
            alarmMessage.Message = mAlarmMessage.Message;

            await _context.SaveChangesAsync();
            return alarmMessage.AlarmMessageId;
        }

        
    }
}
