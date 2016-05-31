using DpControl.Domain.EFContext;
using DpControl.Domain.Execptions;
using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility;
using DpControl.Utility.Authentication;
using DpControl.Utility.Authorization;
using DpControl.Utility.Filters;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DpControl.Controllers.APIControllers
{
    [Authorize]
    public class ProjectsController:BaseAPIController
    {
        [FromServices]
        public IProjectRepository _projectRepository { get; set; }

        private ILoginUserRepository _loginUser;

        public ProjectsController(ILoginUserRepository loginUser)
        {
            _loginUser = loginUser;
        }

        /// <summary>
        /// Get Project by id
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery(typeof(ProjectSearchModel))]
        [HttpGet("{projectId}", Name = "GetByProjectIdAsync")]
        public async Task<IActionResult> GetByProjectIdAsync(int projectId)
        {
            var project = await _projectRepository.FindByIdAsync(projectId);
            if (project == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(project);
        }

        #region Relations
        /// <summary>
        /// Get Customer by ProjectId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery(typeof(CustomerSubSearchModel))]
        [HttpGet("{projectId}/Customer")]
        public async Task<IActionResult> GetCustomerByProjectIdAsync(int projectId)
        {
            var customer = await _projectRepository.GetCustomerByProjectIdAsync(projectId);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(customer);
        }

        /// <summary>
        /// Get Groups by ProjectId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{projectId}/Groups")]
        public async Task<IEnumerable<GroupSubSearchModel>> GetGroupsByProjectIdAsync(int projectId)
        {
            var groups = await _projectRepository.GetGroupsByProjectIdAsync(projectId);
            return groups;
        }

        /// <summary>
        /// Get Locations by ProjectId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{projectId}/Locations")]
        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsByProjectIdAsync(int projectId)
        {
            var locations = await _projectRepository.GetLocationsByProjectIdAsync(projectId);
            return locations;
        }

        /// <summary>
        /// Get Scenes by ProjectId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{projectId}/Scenes")]
        public async Task<IEnumerable<SceneSubSearchModel>> GetScenesByProjectIdAsync(int projectId)
        {
            var scenes = await _projectRepository.GetScenesByProjectIdAsync(projectId);
            return scenes;
        }

        /// <summary>
        /// Get Holidays by ProjectId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{projectId}/Holidays")]
        public async Task<IEnumerable<HolidaySubSearchModel>> GetHolidaysByProjectIdAsync(int projectId)
        {
            var holidays = await _projectRepository.GetHolidaysByProjectIdAsync(projectId);
            return holidays;
        }
        #endregion

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:SuperLevel,CustomerLevel<br/>
        /// Description：根据当前登录用户所属级别获取所有Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<ProjectSearchModel>> GetAllAsync()
        {
            var loginUser = _loginUser.GetLoginUserInfo();
            if (loginUser.isProjectLevel)
                throw new UnauthorizedException();

            var result = await _projectRepository.GetAllAsync(); 
            
            return result;
        }


        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:SuperLevel,CustomerLevel<br/>
        /// Description：新增Project
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles =Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProjectAddModel mProject)
        {
            var loginUser = _loginUser.GetLoginUserInfo();
            if (loginUser.isProjectLevel)
                throw new UnauthorizedException();

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var projectId = await _projectRepository.AddAsync(mProject);
            return CreatedAtRoute("GetByProjectIdAsync", new { controller = "Projects", projectId = projectId }, mProject);
        }

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:SuperLevel,CustomerLevel<br/>
        /// Description：修改Project
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProjectUpdateModel mProject)
        {
            var loginUser = _loginUser.GetLoginUserInfo();
            if (loginUser.isProjectLevel)
                throw new UnauthorizedException();


            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var projectId = await _projectRepository.UpdateByIdAsync(id, mProject);
            return CreatedAtRoute("GetByProjectIdAsync", new { controller = "Projects", projectId = projectId }, mProject);

        }

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:SuperLevel,CustomerLevel<br/>
        /// Description：删除Project
        /// </summary>
        /// <param name="customerId"></param>
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteByProjectIdIdAsync(int projectId)
        {
            var loginUser = _loginUser.GetLoginUserInfo();
            if (loginUser.isProjectLevel)
                throw new UnauthorizedException();

            await _projectRepository.RemoveByIdAsync(projectId);
            return Ok();
        }
    }
}
