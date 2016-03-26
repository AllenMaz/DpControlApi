using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility;
using DpControl.Utility.Authentication;
using DpControl.Utility.Authorization;
using DpControl.Utility.Filters;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DpControl.Controllers.APIControllers
{
    public class ProjectsController:BaseAPIController
    {
        [FromServices]
        public IProjectRepository _projectRepository { get; set; }
        
        /// <summary>
        /// Search data by ProjectId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
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
        /// Get Customer Relation
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
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
        /// Get Groups Relation
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{projectId}/Groups")]
        public async Task<IEnumerable<GroupSubSearchModel>> GetGroupsByProjectIdAsync(int projectId)
        {
            var groups = await _projectRepository.GetGroupsByProjectIdAsync(projectId);
            return groups;
        }

        /// <summary>
        /// Get Locations Relation
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{projectId}/Locations")]
        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsByProjectIdAsync(int projectId)
        {
            var locations = await _projectRepository.GetLocationsByProjectIdAsync(projectId);
            return locations;
        }

        /// <summary>
        /// Get Scenes Relation
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{projectId}/Scenes")]
        public async Task<IEnumerable<SceneSubSearchModel>> GetScenesByProjectIdAsync(int projectId)
        {
            var scenes = await _projectRepository.GetScenesByProjectIdAsync(projectId);
            return scenes;
        }

        /// <summary>
        /// Get Locations Relation
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{projectId}/Holidays")]
        public async Task<IEnumerable<HolidaySubSearchModel>> GetHolidaysByProjectIdAsync(int projectId)
        {
            var holidays = await _projectRepository.GetHolidaysByProjectIdAsync(projectId);
            return holidays;
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<ProjectSearchModel>> GetAllAsync()
        {

            var result = await _projectRepository.GetAllAsync(); 
            
            return result;
        }
        

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProjectAddModel mProject)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var projectId = await _projectRepository.AddAsync(mProject);
            return CreatedAtRoute("GetByProjectIdAsync", new { controller = "Projects", projectId = projectId }, mProject);
        }

        /// <summary>
        /// Edit data by ProjectId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProjectUpdateModel mProject)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var projectId = await _projectRepository.UpdateByIdAsync(id, mProject);
            return CreatedAtRoute("GetByProjectIdAsync", new { controller = "Projects", projectId = projectId }, mProject);

        }

        /// <summary>
        /// Delete data by CustomerNo
        /// </summary>
        /// <param name="customerId"></param>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteByProjectIdIdAsync(int projectId)
        {
            await _projectRepository.RemoveByIdAsync(projectId);
            return Ok();
        }
    }
}
