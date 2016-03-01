using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;

namespace DpControl.Domain.Repository
{
    public class SceneSegmentRepository : ISceneSegmentRepository
    {
        ShadingContext _context;

        #region Constructors
        public SceneSegmentRepository()
        {
        }

        public SceneSegmentRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        #endregion

        public int Add(SceneSegmentAddModel mSceneSegment)
        {
            var scene = _context.Scenes.FirstOrDefault(c => c.SceneId == mSceneSegment.SceneId);
            if (scene == null)
                throw new ExpectException("Could not find Scene data which SceneId equal to " + mSceneSegment.SceneId);


            var model = new SceneSegment
            {
                SceneId = mSceneSegment.SceneId,
                SequenceNo = mSceneSegment.SequenceNo,
                StartTime = mSceneSegment.StartTime,
                Volumn = mSceneSegment.Volumn,
                CreateDate = DateTime.Now
            };
            _context.SceneSegments.Add(model);
            _context.SaveChanges();
            return model.SceneSegmentId;
        }

        public async Task<int> AddAsync(SceneSegmentAddModel mSceneSegment)
        {
            var scene = _context.Scenes.FirstOrDefault(c => c.SceneId == mSceneSegment.SceneId);
            if (scene == null)
                throw new ExpectException("Could not find Scene data which SceneId equal to " + mSceneSegment.SceneId);
            

            var model = new SceneSegment
            {
                SceneId = mSceneSegment.SceneId,
                SequenceNo = mSceneSegment.SequenceNo,
                StartTime = mSceneSegment.StartTime,
                Volumn = mSceneSegment.Volumn,
                CreateDate = DateTime.Now
            };
            _context.SceneSegments.Add(model);
            await _context.SaveChangesAsync();
            return model.SceneSegmentId;
        }

        public SceneSegmentSearchModel FindById(int sceneSegmentId)
        {
            var sceneSegment = _context.SceneSegments.Where(v => v.SceneSegmentId == sceneSegmentId)
               .Select(v => new SceneSegmentSearchModel()
               {
                   SceneSegmentId = v.SceneSegmentId,
                   SceneId = v.SceneId,
                   SequenceNo = v.SequenceNo,
                   StartTime = v.StartTime,
                   Volumn = v.Volumn,
                   CreateDate = v.CreateDate.ToString()
               }).FirstOrDefault();

            return sceneSegment;
        }

        public async Task<SceneSegmentSearchModel> FindByIdAsync(int sceneSegmentId)
        {
            var sceneSegment = await _context.SceneSegments.Where(v => v.SceneSegmentId == sceneSegmentId)
               .Select(v => new SceneSegmentSearchModel()
               {
                   SceneSegmentId = v.SceneSegmentId,
                   SceneId = v.SceneId,
                   SequenceNo = v.SequenceNo,
                   StartTime = v.StartTime,
                   Volumn = v.Volumn,
                   CreateDate = v.CreateDate.ToString()
               }).FirstOrDefaultAsync();

            return sceneSegment;
        }

        public IEnumerable<SceneSegmentSearchModel> GetAll(Query query)
        {
            var queryData = from S in _context.SceneSegments
                            select S;

            var result = QueryOperate<SceneSegment>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var sceneSegments = result.ToList();

            var sceneSegmentsSearch = sceneSegments.Select(v => new SceneSegmentSearchModel
            {
                SceneSegmentId = v.SceneSegmentId,
                SceneId = v.SceneId,
                SequenceNo = v.SequenceNo,
                StartTime = v.StartTime,
                Volumn = v.Volumn,
                CreateDate = v.CreateDate.ToString()
            });

            return sceneSegmentsSearch;
        }

        public async Task<IEnumerable<SceneSegmentSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from S in _context.SceneSegments
                            select S;

            var result = QueryOperate<SceneSegment>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var sceneSegments = await result.ToListAsync();

            var sceneSegmentsSearch = sceneSegments.Select(v => new SceneSegmentSearchModel
            {
                SceneSegmentId = v.SceneSegmentId,
                SceneId = v.SceneId,
                SequenceNo = v.SequenceNo,
                StartTime = v.StartTime,
                Volumn = v.Volumn,
                CreateDate = v.CreateDate.ToString()
            });

            return sceneSegmentsSearch;
        }

        public void RemoveById(int sceneSegmentId)
        {
            var sceneSegment = _context.SceneSegments.FirstOrDefault(c => c.SceneSegmentId == sceneSegmentId);
            if (sceneSegment == null)
                throw new ExpectException("Could not find data which SceneSegmentId equal to " + sceneSegmentId);

            _context.SceneSegments.Remove(sceneSegment);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int sceneSegmentId)
        {
            var sceneSegment = _context.SceneSegments.FirstOrDefault(c => c.SceneSegmentId == sceneSegmentId);
            if (sceneSegment == null)
                throw new ExpectException("Could not find data which SceneSegmentId equal to " + sceneSegmentId);

            _context.SceneSegments.Remove(sceneSegment);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int sceneSegmentId, SceneSegmentUpdateModel mSceneSegment)
        {
            var sceneSegment = _context.SceneSegments.FirstOrDefault(c => c.SceneSegmentId == sceneSegmentId);
            if (sceneSegment == null)
                throw new ExpectException("Could not find data which SceneSegmentId equal to " + sceneSegmentId);


            sceneSegment.SequenceNo = mSceneSegment.SequenceNo;
            sceneSegment.StartTime = mSceneSegment.StartTime;
            sceneSegment.Volumn = mSceneSegment.Volumn;

             _context.SaveChanges();
            return sceneSegment.SceneSegmentId;
        }

        public async Task<int> UpdateByIdAsync(int sceneSegmentId, SceneSegmentUpdateModel mSceneSegment)
        {
            var sceneSegment = _context.SceneSegments.FirstOrDefault(c => c.SceneSegmentId == sceneSegmentId);
            if (sceneSegment == null)
                throw new ExpectException("Could not find data which SceneSegmentId equal to " + sceneSegmentId);


            sceneSegment.SequenceNo = mSceneSegment.SequenceNo;
            sceneSegment.StartTime = mSceneSegment.StartTime;
            sceneSegment.Volumn = mSceneSegment.Volumn;

            await _context.SaveChangesAsync();
            return sceneSegment.SceneSegmentId;
        }
       


    }
}
