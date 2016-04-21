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
        private readonly ILoginUserRepository _loginUser;

        #region Constructors
        public ProjectRepository()
        {
        }

        public ProjectRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        public ProjectRepository(ShadingContext dbContext, ILoginUserRepository loginUser)
        {
            _context = dbContext;
            _loginUser = loginUser;
        }

        #endregion

        public int Add(ProjectAddModel project)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == project.CustomerId);
            if (customer == null)
                throw new ExpectException("Could not find Customer data which CustomerId equal to " + project.CustomerId);

            //ProjectNo must be unique
            var checkData = _context.Projects.Where(p => p.ProjectNo == project.ProjectNo).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which ProjectNo equal to '" + project.ProjectNo + "' already exist in system");

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            var model = new Project
            {
                CustomerId = project.CustomerId,
                ProjectName = project.ProjectName,
                ProjectNo = project.ProjectNo,
                Creator = user.UserName ,
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

            //ProjectNo must be unique
            var checkData = await _context.Projects.Where(p =>p.ProjectNo == project.ProjectNo).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which ProjectNo equal to '" + project.ProjectNo + "' already exist in system");

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            var model = new Project
            {
                CustomerId = project.CustomerId,
                ProjectName = project.ProjectName,
                ProjectNo = project.ProjectNo,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Projects.Add(model);
            await _context.SaveChangesAsync();
            return model.ProjectId;
        }

        public ProjectSearchModel FindById(int projectId)
        {
            var result = _context.Projects.Where(v => v.ProjectId == projectId);
            result = (IQueryable<Project>)ExpandOperator.ExpandRelatedEntities<Project>(result);

            var project = result.FirstOrDefault();
            var projectSearch = ProjectOperator.SetProjectSearchModelCascade(project);

            return projectSearch;
        }

        public async Task<ProjectSearchModel> FindByIdAsync(int projectId)
        {
            var result = _context.Projects.Where(v => v.ProjectId == projectId);
            result = (IQueryable<Project>)ExpandOperator.ExpandRelatedEntities<Project>(result);

            var project = await result.FirstOrDefaultAsync();
            var projectSearch = ProjectOperator.SetProjectSearchModelCascade(project);
            
            return projectSearch;
        }

        public IEnumerable<ProjectSearchModel> GetAll()
        {
            var queryData = from P in _context.Projects
                            select P;

            var result = QueryOperate<Project>.Execute(queryData);
            result = (IQueryable<Project>)ExpandOperator.ExpandRelatedEntities<Project>(result);

            //以下执行完后才会去数据库中查询
            var projects = result.ToList();

            var projectSearch = ProjectOperator.SetProjectSearchModelCascade(projects);

            return projectSearch;
        }

        public async Task<IEnumerable<ProjectSearchModel>> GetAllAsync()
        {
            var queryData = from P in _context.Projects
                            select P;

            var result = QueryOperate<Project>.Execute(queryData);
            result = (IQueryable<Project>)ExpandOperator.ExpandRelatedEntities<Project>(result);

            //以下执行完后才会去数据库中查询
            var projects = await result.ToListAsync();

            var projectSearch = ProjectOperator.SetProjectSearchModelCascade(projects);

            return projectSearch;

        }

        #region Relations
        public async Task<CustomerSubSearchModel> GetCustomerByProjectIdAsync(int projectId)
        {
            var project = await _context.Projects.Include(p => p.Customer)
                .Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();

            var customer = project == null ? null : project.Customer;

            var customerSearch = CustomerOperator.SetCustomerSubSearchModel(customer);
            return customerSearch;
        }

        public async Task<IEnumerable<GroupSubSearchModel>> GetGroupsByProjectIdAsync(int projectId)
        {
            var queryData = _context.Groups.Where(g => g.ProjectId == projectId);
            var result = QueryOperate<Group>.Execute(queryData);
            var groups = await result.ToListAsync();

            var groupsSearch = GroupOperator.SetGroupSubSearchModel(groups);
            return groupsSearch;
        }

        public async Task<IEnumerable<HolidaySubSearchModel>> GetHolidaysByProjectIdAsync(int projectId)
        {
            var queryData = _context.Holidays.Where(g => g.ProjectId == projectId);
            var result = QueryOperate<Holiday>.Execute(queryData);
            var holidays = await result.ToListAsync();

            var holidaysSearch = HolidayOperator.SetHolidaySubSearchModel(holidays);
            return holidaysSearch;
        }

        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsByProjectIdAsync(int projectId)
        {
            var queryData = _context.Locations.Where(g => g.ProjectId == projectId);
            var result = QueryOperate<Location>.Execute(queryData);
            var locations = await result.ToListAsync();

            var locationsSearch = LocationOperator.SetLocationSubSearchModel(locations);
            return locationsSearch;
        }

        public async Task<IEnumerable<SceneSubSearchModel>> GetScenesByProjectIdAsync(int projectId)
        {
            var queryData = _context.Scenes.Where(g => g.ProjectId == projectId);
            var result = QueryOperate<Scene>.Execute(queryData);
            var scenes = await result.ToListAsync();

            var scenesSearch = SceneOperator.SetSceneSubSearchModel(scenes);
            return scenesSearch;
        }
        #endregion

        public void RemoveById(int projectId)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == projectId);
            if (project == null)
                throw new ExpectException("Could not find data which ProjectId equal to " + projectId);

            _context.Projects.Remove(project);
            #region Cascade Delete dependent entities


            #endregion
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int projectId)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == projectId);
            if (project == null)
                throw new ExpectException("Could not find data which ProjectId equal to " + projectId);

            _context.Projects.Remove(project);
            #region Cascade Delete dependent entities


            #endregion
            await _context.SaveChangesAsync();

        }

        public int UpdateById(int projectId, ProjectUpdateModel mproject)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == projectId);
            if (project == null)
                throw new ExpectException("Could not find data which ProjectId equal to " + projectId);

            //ProjectNo must be unique
            var checkData = _context.Projects.Where(p => p.ProjectNo == mproject.ProjectNo
                                                        && p.ProjectId != projectId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which ProjectNo '" + mproject.ProjectNo + "' already exist in system");

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            project.ProjectName = mproject.ProjectName;
            project.ProjectNo = mproject.ProjectNo;
            project.Completed = mproject.Completed;
            project.Modifier = user.UserName;
            project.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
            return project.ProjectId;
        }

        public async Task<int> UpdateByIdAsync(int projectId, ProjectUpdateModel mproject)
        {
            var project = _context.Projects.FirstOrDefault(c => c.ProjectId == projectId);
            if (project == null)
                throw new ExpectException("Could not find data which ProjectId equal to " + projectId);

            //ProjectNo must be unique
            var checkData = _context.Projects.Where(p => p.ProjectNo == mproject.ProjectNo
                                                        && p.ProjectId != projectId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which ProjectNo '" + mproject.ProjectNo + "' already exist in system");

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            project.ProjectName = mproject.ProjectName;
            project.ProjectNo = mproject.ProjectNo;
            project.Completed = mproject.Completed;
            project.Modifier = user.UserName;
            project.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return project.ProjectId;
        }
        

    }
}
