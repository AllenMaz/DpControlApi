using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Models;
using System.Reflection;
using DpControl.Domain.Execptions;


namespace DpControl.Domain.Repository
{
    public class ProjectRepository : IProjectRepository
    {

        private ShadingContext _context;

        #region Constructors
        public ProjectRepository()
        {
        }

        public ProjectRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        #endregion

        public int Add(ProjectAddModel project)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == project.CustomerId);
            if (customer == null)
                throw new ExpectException("Could not find Customer data which CustomerId equal to " + project.CustomerId);

            var model = new Project
            {
                CustomerId = project.CustomerId,
                ProjectName = project.ProjectName,
                ProjectNo = project.ProjectNo,
                CreateDate = DateTime.Now
            };
            _context.Projects.Add(model);
            _context.SaveChanges();
            return model.ProjectId;

        }

        public async Task<int> AddAsync(ProjectAddModel project)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == project.CustomerId);
            if (customer == null)
                throw new ExpectException("Could not find Customer data which CustomerId equal to " + project.CustomerId);

            var model = new Project
            {
                CustomerId = project.CustomerId,
                ProjectName = project.ProjectName,
                ProjectNo = project.ProjectNo,
                CreateDate = DateTime.Now
            };
            _context.Projects.Add(model);
            await _context.SaveChangesAsync();
            return model.ProjectId;
        }

        public ProjectSearchModel FindByProjectId(int projectId)
        {
            var projects = _context.Projects.Where(v => v.ProjectId == projectId)
                .Select(v=>new ProjectSearchModel() {
                    ProjectId = v.ProjectId,
                    ProjectNo = v.ProjectNo,
                    ProjectName = v.ProjectName,
                    CustomerId = v.CustomerId,
                    CreateDate = v.CreateDate,
                    Completed = v.Completed
                }).FirstOrDefault();

            return projects;
        }

        public async Task<ProjectSearchModel> FindByProjectIdAsync(int projectId)
        {
            var projects = _context.Projects.Where(v => v.ProjectId == projectId)
                .Select(v => new ProjectSearchModel()
                {
                    ProjectId = v.ProjectId,
                    ProjectNo = v.ProjectNo,
                    ProjectName = v.ProjectName,
                    CustomerId = v.CustomerId,
                    CreateDate = v.CreateDate,
                    Completed = v.Completed
                }).FirstOrDefaultAsync();

            return await projects;
        }

        public IEnumerable<ProjectSearchModel> GetAll()
        {
            var projects = _context.Projects.Select(v => new ProjectSearchModel
            {
                ProjectId = v.ProjectId,
                ProjectNo = v.ProjectNo,
                ProjectName = v.ProjectName,
                CustomerId = v.CustomerId,
                CreateDate = v.CreateDate,
                Completed = v.Completed
            })
                .OrderBy(v =>v.ProjectNo )
                .ToList<ProjectSearchModel>();

            return projects;
        }

        public async Task<IEnumerable<ProjectSearchModel>> GetAllAsync()
        {
            var projects = await _context.Projects.Select(v => new ProjectSearchModel
            {
                ProjectId = v.ProjectId,
                ProjectNo = v.ProjectNo,
                ProjectName = v.ProjectName,
                CustomerId = v.CustomerId,
                CreateDate = v.CreateDate,
                Completed = v.Completed
            })
               .OrderBy(v => v.ProjectNo)
               .ToListAsync<ProjectSearchModel>();

            return projects;
        }

        public void RemoveById(int projectId)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == projectId);
            if (project == null)
                throw new ExpectException("Could not find data which ProjectId equal to " + projectId);

            _context.Projects.Remove(project);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int projectId)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == projectId);
            if (project == null)
                throw new ExpectException("Could not find data which ProjectId equal to " + projectId);

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int projectId, ProjectUpdateModel mproject)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == projectId);
            if (project == null)
                throw new ExpectException("Could not find data which ProjectId equal to " + projectId);

            project.ProjectName = mproject.ProjectName;
            project.ProjectNo = mproject.ProjectNo;
            project.Completed = mproject.Completed;

            _context.SaveChanges();
            return project.ProjectId;
        }

        public async Task<int> UpdateByIdAsync(int projectId, ProjectUpdateModel mproject)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == projectId);
            if (project == null)
                throw new ExpectException("Could not find data which ProjectId equal to " + projectId);

            project.ProjectName = mproject.ProjectName;
            project.ProjectNo = mproject.ProjectNo;
            project.Completed = mproject.Completed;

            await _context.SaveChangesAsync();
            return project.ProjectId;
        }

       

    }
}
