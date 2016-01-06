using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IGroupRepository
    {
        void Add(MGroup item, string ProjectNo);
        Task<IEnumerable<MGroup>> GetAll(string projectNo);
        Task<MGroup> Find(string group, string projectNo);
        Task Remove(string key, string projectNo);
        void UpdateById(MGroup mGroup, string projectNo);
    }
}
