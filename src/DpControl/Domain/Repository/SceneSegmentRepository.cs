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
        private readonly IUserInfoRepository _userInfo;

        #region Constructors
        public SceneSegmentRepository()
        {
        }

        public SceneSegmentRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        public SceneSegmentRepository(ShadingContext dbContext, IUserInfoRepository userInfo)
        {
            _context = dbContext;
            _userInfo = userInfo;
        }
        #endregion

        public int Add(SceneSegmentAddModel mSceneSegment)
        {
            var scene = _context.Scenes.FirstOrDefault(c => c.SceneId == mSceneSegment.SceneId);
            if (scene == null)
                throw new ExpectException("Could not find Scene data which SceneId equal to " + mSceneSegment.SceneId);
            
            //SequenceNo must be unique
            var checkData = _context.SceneSegments.Where(s => s.SequenceNo == mSceneSegment.SequenceNo).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which SequenceNo equal to '" + mSceneSegment.SequenceNo + "' already exist in system");


            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            var model = new SceneSegment
            {
                SceneId = mSceneSegment.SceneId,
                SequenceNo = mSceneSegment.SequenceNo,
                StartTime = mSceneSegment.StartTime,
                Volumn = mSceneSegment.Volumn,
                Creator = user.UserName,
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

            //SequenceNo must be unique
            var checkData = await _context.SceneSegments.Where(s => s.SequenceNo == mSceneSegment.SequenceNo).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which SequenceNo equal to '" + mSceneSegment.SequenceNo + "' already exist in system");


            //Get UserInfo
            var user =await _userInfo.GetUserInfoAsync();
            var model = new SceneSegment
            {
                SceneId = mSceneSegment.SceneId,
                SequenceNo = mSceneSegment.SequenceNo,
                StartTime = mSceneSegment.StartTime,
                Volumn = mSceneSegment.Volumn,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.SceneSegments.Add(model);
            await _context.SaveChangesAsync();
            return model.SceneSegmentId;
        }

        public SceneSegmentSearchModel FindById(int sceneSegmentId)
        {
            var result = _context.SceneSegments.Where(v => v.SceneSegmentId == sceneSegmentId);
            result = (IQueryable<SceneSegment>)ExpandOperator.ExpandRelatedEntities<SceneSegment>(result);

            var sceneSegment = result.FirstOrDefault();
            var sceneSegmentSearch = SceneSegmentOperator.SetSceneSegmentSearchModelCascade(sceneSegment);

            return sceneSegmentSearch;
        }

        public async Task<SceneSegmentSearchModel> FindByIdAsync(int sceneSegmentId)
        {
            var result = _context.SceneSegments.Where(v => v.SceneSegmentId == sceneSegmentId);
            result = (IQueryable<SceneSegment>)ExpandOperator.ExpandRelatedEntities<SceneSegment>(result);

            var sceneSegment = await result.FirstOrDefaultAsync();
            var sceneSegmentSearch = SceneSegmentOperator.SetSceneSegmentSearchModelCascade(sceneSegment);

            return sceneSegmentSearch;
            
        }

        public IEnumerable<SceneSegmentSearchModel> GetAll()
        {
            var queryData = from S in _context.SceneSegments
                            select S;

            var result = QueryOperate<SceneSegment>.Execute(queryData);
            result = (IQueryable<SceneSegment>)ExpandOperator.ExpandRelatedEntities<SceneSegment>(result);

            //以下执行完后才会去数据库中查询
            var sceneSegments = result.ToList();
            var sceneSegmentsSearch = SceneSegmentOperator.SetSceneSegmentSearchModelCascade(sceneSegments);

            return sceneSegmentsSearch;
        }

        public async Task<IEnumerable<SceneSegmentSearchModel>> GetAllAsync()
        {
            var queryData = from S in _context.SceneSegments
                            select S;

            var result = QueryOperate<SceneSegment>.Execute(queryData);
            result = (IQueryable<SceneSegment>)ExpandOperator.ExpandRelatedEntities<SceneSegment>(result);

            //以下执行完后才会去数据库中查询
            var sceneSegments = await result.ToListAsync();

            var sceneSegmentsSearch = SceneSegmentOperator.SetSceneSegmentSearchModelCascade(sceneSegments);

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

            //SequenceNo must be unique
            var checkData = _context.SceneSegments.Where(s => s.SequenceNo == mSceneSegment.SequenceNo
                                                        && s.SceneSegmentId != sceneSegmentId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which SequenceNo '" + mSceneSegment.SequenceNo + "' already exist in system");


            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            sceneSegment.SequenceNo = mSceneSegment.SequenceNo;
            sceneSegment.StartTime = mSceneSegment.StartTime;
            sceneSegment.Volumn = mSceneSegment.Volumn;
            sceneSegment.Modifier = user.UserName;
            sceneSegment.ModifiedDate = DateTime.Now;

             _context.SaveChanges();
            return sceneSegment.SceneSegmentId;
        }

        public async Task<int> UpdateByIdAsync(int sceneSegmentId, SceneSegmentUpdateModel mSceneSegment)
        {
            var sceneSegment = _context.SceneSegments.FirstOrDefault(c => c.SceneSegmentId == sceneSegmentId);
            if (sceneSegment == null)
                throw new ExpectException("Could not find data which SceneSegmentId equal to " + sceneSegmentId);

            //SequenceNo must be unique
            var checkData = await _context.SceneSegments.Where(s => s.SequenceNo == mSceneSegment.SequenceNo
                                                        && s.SceneSegmentId != sceneSegmentId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which SequenceNo '" + mSceneSegment.SequenceNo + "' already exist in system");


            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            sceneSegment.SequenceNo = mSceneSegment.SequenceNo;
            sceneSegment.StartTime = mSceneSegment.StartTime;
            sceneSegment.Volumn = mSceneSegment.Volumn;
            sceneSegment.Modifier = user.UserName;
            sceneSegment.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return sceneSegment.SceneSegmentId;
        }
       


    }
}
