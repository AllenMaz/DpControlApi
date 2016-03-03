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
            //Check whether the ProjectNo already exist
            var checkData = _context.Projects.Where(p => p.ProjectNo == project.ProjectNo).ToList();
            if (checkData.Count > 0)
                throw new ExpectException(project.ProjectNo + " already exist in system.");

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

            //Check whether the ProjectNo already exist
            var checkData = await _context.Projects.Where(p =>p.ProjectNo == project.ProjectNo).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException(project.ProjectNo + " already exist in system.");

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

        public ProjectSearchModel FindById(int projectId)
        {
            var projects = _context.Projects.Where(v => v.ProjectId == projectId)
                .Select(v=>new ProjectSearchModel() {
                    ProjectId = v.ProjectId,
                    ProjectNo = v.ProjectNo,
                    ProjectName = v.ProjectName,
                    CustomerId = v.CustomerId,
                    CreateDate = v.CreateDate.ToString(),
                    Completed = v.Completed
                }).FirstOrDefault();

            return projects;
        }

        public async Task<ProjectSearchModel> FindByIdAsync(int projectId)
        {
            var projects =await _context.Projects.Where(v => v.ProjectId == projectId)
                .Select(v => new ProjectSearchModel()
                {
                    ProjectId = v.ProjectId,
                    ProjectNo = v.ProjectNo,
                    ProjectName = v.ProjectName,
                    CustomerId = v.CustomerId,
                    CreateDate = v.CreateDate.ToString(),
                    Completed = v.Completed
                }).FirstOrDefaultAsync();

            return projects;
        }

        public IEnumerable<ProjectSearchModel> GetAll(Query query)
        {
            var queryData = from P in _context.Projects
                            select P;

            var result = QueryOperate<Project>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var projects = result.ToList();

            var projectSearch = projects.Select(v => new ProjectSearchModel
            {
                ProjectId = v.ProjectId,
                ProjectNo = v.ProjectNo,
                ProjectName = v.ProjectName,
                CustomerId = v.CustomerId,
                CreateDate = v.CreateDate.ToString(),
                Completed = v.Completed
            });

            return projectSearch;
        }

        public async Task<IEnumerable<ProjectSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from P in _context.Projects
                            select P;

            var result = QueryOperate<Project>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var projects = await result.ToListAsync();

            var projectSearch = projects.Select(v => new ProjectSearchModel
            {
                ProjectId = v.ProjectId,
                ProjectNo = v.ProjectNo,
                ProjectName = v.ProjectName,
                CustomerId = v.CustomerId,
                CreateDate = v.CreateDate.ToString(),
                Completed = v.Completed
            });

            return projectSearch;

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

            var checkData = _context.Projects.Where(p => p.ProjectNo == mproject.ProjectNo
                                                        && p.ProjectId != projectId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException(mproject.ProjectNo + " already exist in system.");

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

            var checkData = _context.Projects.Where(p => p.ProjectNo == mproject.ProjectNo
                                                        && p.ProjectId != projectId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException(mproject.ProjectNo + " already exist in system.");

            project.ProjectName = mproject.ProjectName;
            project.ProjectNo = mproject.ProjectNo;
            project.Completed = mproject.Completed;

            await _context.SaveChangesAsync();
            return project.ProjectId;
        }

       

    }
}
