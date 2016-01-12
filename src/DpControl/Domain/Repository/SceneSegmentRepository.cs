using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;

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

        public async Task Add(int sceneId, string projectNo, List<MSceneSegment> segments)
        {
            if (sceneId == 0 || string.IsNullOrEmpty(projectNo) || segments == null)
            {
                throw new ArgumentNullException();
            }

            int _sceneId;

            // get scene with projectNo = projectNo and sceneNo
            var query = await GetCustomerByProjectNo(projectNo, sceneId);
            if (query == null)
            {
                throw new KeyNotFoundException();
            }

            _sceneId = query.SceneId;

            segments = segments.OrderBy(s => s.SequenceNo).ToList();
            // create new Group
            for (int i= 0;i< segments.Count; i++)
            {
                _context.SceneSegments.Add(new SceneSegment
                {
                    SequenceNo=i+1,
                    StartTime=segments.ElementAt(i).StartTime,
                    Volumn=segments.ElementAt(i).Volumn,
                    SceneId = _sceneId,
                    ModifiedDate = DateTime.Now
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MSceneSegment>> GetAll(int Id, string projectNo)
        {
            var query = await GetCustomerByProjectNo(projectNo, Id);

            return query.SceneSegments.OrderBy(s=>s.SequenceNo).Select(s => new MSceneSegment
            {
                SceneSegmentId=s.SceneSegmentId,
                SequenceNo =s.SequenceNo,
                StartTime=s.StartTime,
                Volumn=s.Volumn
            })
            .ToList<MSceneSegment>();
        }

        public async Task Remove(int Id)
        {
            if (Id == 0)
            {
                throw new Exception("The group does not exist.");
            }

            var toDelete = new SceneSegment { SceneId = Id };
            _context.SceneSegments.Attach(toDelete);
            _context.SceneSegments.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msceneSegment"></param>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public async Task UpdateById(MSceneSegment msceneSegment, int sceneId)
        {
            var _single = _context.SceneSegments.Where(s => s.SceneSegmentId == msceneSegment.SceneSegmentId).Single();
            _single.SequenceNo = msceneSegment.SequenceNo;
            _single.StartTime = msceneSegment.StartTime;
            _single.Volumn = msceneSegment.Volumn;
            _single.SceneId = sceneId;
            await _context.SaveChangesAsync();
        }
        async Task<Scene> GetCustomerByProjectNo(string projectNo, int Id )
        {
            var query = await _context.Customers
                        .Include(c => c.Scenes)
                        .Where(c => c.ProjectNo == projectNo)
                        .SingleAsync();
            if (query == null)
            {
                throw new KeyNotFoundException();
            }
            return query.Scenes.Where(s=>s.SceneId==Id).Single();
        }

    }
}
