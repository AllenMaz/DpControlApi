using DpControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.IRepository
{
    public interface IBaseRepository<TAddModel,TUpdateModel,TSearchModel>
    {
        int Add(TAddModel item);
        Task<int> AddAsync(TAddModel item);
        IEnumerable<TSearchModel> GetAll(Query query);
        Task<IEnumerable<TSearchModel>> GetAllAsync(Query query);
        TSearchModel FindById(int itemId);
        Task<TSearchModel> FindByIdAsync(int itemId);
        int UpdateById(int itemId, TUpdateModel item);
        Task<int> UpdateByIdAsync(int projectId, TUpdateModel item);
        void RemoveById(int itemId);
        Task RemoveByIdAsync(int itemId);
    }
}
