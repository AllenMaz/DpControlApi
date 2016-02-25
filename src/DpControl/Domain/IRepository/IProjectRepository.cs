using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IProjectRepository
    {
        int Add(ProjectAddModel project);
        Task<int> AddAsync(ProjectAddModel project);
        IEnumerable<ProjectSearchModel> GetAll();
        Task<IEnumerable<ProjectSearchModel>> GetAllAsync();
        ProjectSearchModel FindByProjectId(int projectId);
        Task<ProjectSearchModel> FindByProjectIdAsync(int projectId);
        int UpdateById(int projectId, ProjectUpdateModel project);
        Task<int> UpdateByIdAsync(int projectId, ProjectUpdateModel project);
        void RemoveById(int projectId);
        Task RemoveByIdAsync(int projectId);
    }
}
