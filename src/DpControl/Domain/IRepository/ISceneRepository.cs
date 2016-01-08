using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Entities;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ISceneRepository
    {
        Task Add(MScene item, string projectNo);
        Task<IEnumerable<MScene>> GetAll(string projectNo);
        Task<MScene> Find(string sceneNo, string projectNo);
        Task Remove(int Id);
        Task UpdateById(MScene mscene, string projectNo);
    }
}
