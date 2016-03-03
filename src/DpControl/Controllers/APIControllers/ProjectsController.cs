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
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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
        /// Search data by ProjectId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        [FormatReturnType]
        public async Task<IEnumerable<ProjectSearchModel>> GetAllAsync([FromUri] Query query)
        {

            var result = await _projectRepository.GetAllAsync(query); ;
            
            return result;
        }

        /// <summary>
        /// Edit data by ProjectId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteByProjectIdIdAsync(int projectId)
        {
            await _projectRepository.RemoveByIdAsync(projectId);
            return Ok();
        }
    }
}
