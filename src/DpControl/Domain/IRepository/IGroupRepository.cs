using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IGroupRepository
    {
        Task Add(string groupName, string ProjectNo);
        Task<IEnumerable<MGroup>> GetAllAsync(string projectNo);
        Task Remove(string key, string projectNo);
        Task Remove(int Id);
        Task UpdateById(MGroup mGroup, string projectNo);
    }
}   
