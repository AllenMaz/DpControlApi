﻿using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using DpControl.Domain.Execptions;
using Microsoft.Data.Entity;
using DpControl.Domain.Entities;

namespace DpControl.Domain.Repository
{
    public class LogDescriptionRepository : ILogDescriptionRepository
    {

        ShadingContext _context;

        #region
        public LogDescriptionRepository()
        {

        }
        public LogDescriptionRepository(ShadingContext context)
        {
            _context = context;
        }
        
        #endregion

        public int Add(LogDescriptionAddModel mLogDescription)
        {
            //DescriptionCode must be unique
            var checkData = _context.LogDescriptions
                .Where(c => c.DescriptionCode == mLogDescription.DescriptionCode).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DescriptioncODE equal to " + mLogDescription.DescriptionCode + " already exist in system");


            var model = new LogDescription
            {
                DescriptionCode = mLogDescription.DescriptionCode,
                Description = mLogDescription.Description
            };
            _context.LogDescriptions.Add(model);
            _context.SaveChanges();
            return model.LogDescriptionId;
        }

        public async Task<int> AddAsync(LogDescriptionAddModel mLogDescription)
        {
            //DescriptionCode must be unique
            var checkData = await _context.LogDescriptions
                .Where(c => c.DescriptionCode == mLogDescription.DescriptionCode).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DescriptioncODE equal to " + mLogDescription.DescriptionCode + " already exist in system");


            var model = new LogDescription
            {
                DescriptionCode = mLogDescription.DescriptionCode,
                Description = mLogDescription.Description
            };
            _context.LogDescriptions.Add(model);
            await _context.SaveChangesAsync();
            return model.LogDescriptionId;
        }

        public LogDescriptionSearchModel FindById(int logDescriptionId)
        {
            var result = _context.LogDescriptions
               .Where(v => v.LogDescriptionId == logDescriptionId);
            result = (IQueryable<LogDescription>)ExpandOperator.ExpandRelatedEntities<LogDescription>(result);

            var logDescription = result.FirstOrDefault();
            var logDescriptionSearch = LogDescriptionOperator.SetLogDescriptionSearchModelCascade(logDescription);

            return logDescriptionSearch;
        }

        public async Task<LogDescriptionSearchModel> FindByIdAsync(int logDescriptionId)
        {
            var result = _context.LogDescriptions
               .Where(v => v.LogDescriptionId == logDescriptionId);

            result = (IQueryable<LogDescription>)ExpandOperator.ExpandRelatedEntities<LogDescription>(result);

            var logDescription = await result.FirstOrDefaultAsync();
            var logDescriptionSearch = LogDescriptionOperator.SetLogDescriptionSearchModelCascade(logDescription);

            return logDescriptionSearch;
        }

        public IEnumerable<LogDescriptionSearchModel> GetAll()
        {
            var queryData = from L in _context.LogDescriptions
                            select L;

            var result = QueryOperate<LogDescription>.Execute(queryData);
            result = (IQueryable<LogDescription>)ExpandOperator.ExpandRelatedEntities<LogDescription>(result);

            //以下执行完后才会去数据库中查询
            var logDescriptions = result.ToList();
            var logDescriptionsSearch = LogDescriptionOperator.SetLogDescriptionSearchModelCascade(logDescriptions);

            return logDescriptionsSearch;
        }

        public async Task<IEnumerable<LogDescriptionSearchModel>> GetAllAsync()
        {
            var queryData = from L in _context.LogDescriptions
                            select L;

            var result = QueryOperate<LogDescription>.Execute(queryData);
            result = (IQueryable<LogDescription>)ExpandOperator.ExpandRelatedEntities<LogDescription>(result);

            //以下执行完后才会去数据库中查询
            var logDescriptions = await result.ToListAsync();
            var logDescriptionsSearch = LogDescriptionOperator.SetLogDescriptionSearchModelCascade(logDescriptions);

            return logDescriptionsSearch;
        }

        public async Task<IEnumerable<LogSubSearchModel>> GetLogsByLogDescriptionIdAsync(int logDescriptionId)
        {
            var queryData = _context.Logs.Where(l=>l.LogDescriptionId == logDescriptionId);
            var result = QueryOperate<Log>.Execute(queryData);
            var logs = await result.ToListAsync();
            var logsSearch = LogOperator.SetLogSubSearchModel(logs);
            return logsSearch;
        }

        public void RemoveById(int logDescriptionId)
        {
            var logDescription = _context.LogDescriptions.FirstOrDefault(c => c.LogDescriptionId == logDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find data which LogDescriptionId equal to " + logDescriptionId);

            _context.Remove(logDescription);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int logDescriptionId)
        {
            var logDescription = _context.LogDescriptions.FirstOrDefault(c => c.LogDescriptionId == logDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find data which LogDescriptionId equal to " + logDescriptionId);

            _context.Remove(logDescription);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int logDescriptionId, LogDescriptionUpdateModel mLogDescription)
        {
            var logDescription = _context.LogDescriptions.FirstOrDefault(c => c.LogDescriptionId == logDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find data which LogDescriptionId equal to " + logDescriptionId);

            //DescriptionCode must be unique
            var checkData = _context.LogDescriptions
                .Where(c => c.DescriptionCode == mLogDescription.DescriptionCode
                && c.LogDescriptionId != logDescriptionId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DescriptionCode equal to " + mLogDescription.DescriptionCode + " already exist in system");



            logDescription.DescriptionCode = mLogDescription.DescriptionCode;
            logDescription.Description = mLogDescription.Description;

            _context.SaveChanges();
            return logDescription.LogDescriptionId;
        }

        public async Task<int> UpdateByIdAsync(int logDescriptionId, LogDescriptionUpdateModel mLogDescription)
        {
            var logDescription = _context.LogDescriptions.FirstOrDefault(c => c.LogDescriptionId == logDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find data which LogDescriptionId equal to " + logDescriptionId);

            //DescriptionCode must be unique
            var checkData = await _context.LogDescriptions
                .Where(c => c.DescriptionCode == mLogDescription.DescriptionCode
                && c.LogDescriptionId != logDescriptionId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DescriptionCode equal to " + mLogDescription.DescriptionCode + " already exist in system");



            logDescription.DescriptionCode = mLogDescription.DescriptionCode;
            logDescription.Description = mLogDescription.Description;

            await _context.SaveChangesAsync();
            return logDescription.LogDescriptionId;
        }

        
    }
}
