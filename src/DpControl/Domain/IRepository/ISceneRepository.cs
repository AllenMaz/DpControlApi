using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ISceneRepository
    {
        Task Add(string sceneName, string projectNo);
        Task<IEnumerable<MScene>> GetAll(string projectNo);
        Task Remove(int Id);
        Task UpdateById(MScene mscene, string projectNo);
    }
}
