using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IGroupRepository
    {
        int Add(GroupAddModel group);
        Task<int> AddAsync(GroupAddModel group);
        IEnumerable<GroupSearchModel> GetAll(Query query);
        Task<IEnumerable<GroupSearchModel>> GetAllAsync(Query query);
        GroupSearchModel FindByGroupId(int groupId);
        Task<GroupSearchModel> FindByGroupIdAsync(int groupId);
        int UpdateById(int groupId, GroupUpdateModel mgroup);
        Task<int> UpdateByIdAsync(int groupId, GroupUpdateModel mgroup);
        void RemoveById(int groupId);
        Task RemoveByIdAsync(int groupId);
    }
}   
