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
        private readonly IUserInfoRepository _userInfo;

        #region Constructors
        public ProjectRepository()
        {
        }

        public ProjectRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        public ProjectRepository(ShadingContext dbContext,IUserInfoRepository userInfo)
        {
            _context = dbContext;
            _userInfo = userInfo;
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
            var user = _userInfo.GetUserInfo();

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
            var user = await _userInfo.GetUserInfoAsync();

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
            result = result.Include(p => p.Scenes);
            result = result.Include(p => p.Groups);
            result = result.Include(p => p.Locations);
            result = result.Include(p => p.Holidays);

            var project = result.FirstOrDefault();
            var projectSearch = ProjectOperator.SetProjectSearchModelCascade(project);

            return projectSearch;
        }

        public async Task<ProjectSearchModel> FindByIdAsync(int projectId)
        {
            var result = _context.Projects.Where(v => v.ProjectId == projectId);
            result = result.Include(p => p.Scenes);
            result = result.Include(p => p.Groups);
            result = result.Include(p => p.Locations);
            result = result.Include(p => p.Holidays);

            var project = await result.FirstOrDefaultAsync();
            var projectSearch = ProjectOperator.SetProjectSearchModelCascade(project);
            
            return projectSearch;
        }

        public IEnumerable<ProjectSearchModel> GetAll(Query query)
        {
            var queryData = from P in _context.Projects
                            select P;

            var result = QueryOperate<Project>.Execute(queryData, query);
            result = ExpandRelatedEntities(result, query.expand);

            //以下执行完后才会去数据库中查询
            var projects = result.ToList();

            var projectSearch = ProjectOperator.SetProjectSearchModelCascade(projects);

            return projectSearch;
        }

        public async Task<IEnumerable<ProjectSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from P in _context.Projects
                            select P;

            var result = QueryOperate<Project>.Execute(queryData, query);
            result = ExpandRelatedEntities(result,query.expand);

            //以下执行完后才会去数据库中查询
            var projects = await result.ToListAsync();

            var projectSearch = ProjectOperator.SetProjectSearchModelCascade(projects);

            return projectSearch;

        }

        

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
            var user = _userInfo.GetUserInfo();

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
            var user = await _userInfo.GetUserInfoAsync();

            project.ProjectName = mproject.ProjectName;
            project.ProjectNo = mproject.ProjectNo;
            project.Completed = mproject.Completed;
            project.Modifier = user.UserName;
            project.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return project.ProjectId;
        }

        /// <summary>
        /// Expand Related Entities
        /// </summary>
        /// <param name="result"></param>
        /// <param name="expandParams"></param>
        /// <returns></returns>
        private IQueryable<Project> ExpandRelatedEntities(IQueryable<Project> result, string[] expandParams)
        {
            var needExpandScenes = ExpandOperator.NeedExpand("Scenes", expandParams);
            var needExpandGroups = ExpandOperator.NeedExpand("Groups", expandParams);
            var needExpandLocations = ExpandOperator.NeedExpand("Locations", expandParams);
            var needExpandHolidays = ExpandOperator.NeedExpand("Holidays", expandParams);

            if (needExpandScenes)
                result = result.Include(p => p.Scenes);
            if (needExpandGroups)
                result = result.Include(p => p.Groups);
            if (needExpandLocations)
                result = result.Include(p => p.Locations);
            if (needExpandHolidays)
                result = result.Include(p => p.Holidays);

            return result;
        }

    }
}
