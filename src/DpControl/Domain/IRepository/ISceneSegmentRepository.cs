using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ISceneSegmentRepository
    {
        Task Add(int sceneId, string projectNo, List<MSceneSegment> sceneSegment);
        Task<IEnumerable<MSceneSegment>> GetAll(int Id, string projectNo);
        Task Remove(int Id);
        Task UpdateById(MSceneSegment msceneSegment, int sceneId);
    }
}
