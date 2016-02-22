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
        Task<IEnumerable<MGroup>> GetAll(string projectNo);
        Task RemoveByName(string groupName, string projectNo);
        Task RemoveById(int Id);
        Task Update(MGroup mGroup, string projectNo);
        Task AddLocationToGroup(int locationId, int groupId);
    }
}   
