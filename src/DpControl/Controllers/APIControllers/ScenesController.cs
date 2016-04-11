using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
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
    public class ScenesController:BaseAPIController
    {
        [FromServices]
        public ISceneRepository _sceneRepository { get; set; }

        
        /// <summary>
        /// Search data by SceneId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(SceneSearchModel))]
        [HttpGet("{sceneId}", Name = "GetBySceneIdAsync")]
        public async Task<IActionResult> GetBySceneIdAsync(int sceneId)
        {
            var scene = await _sceneRepository.FindByIdAsync(sceneId);
            if (scene == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(scene);
        }

        #region Relations
        /// <summary>
        /// Get Project Relation
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(ProjectSubSearchModel))]
        [HttpGet("{sceneId}/Project")]
        public async Task<IActionResult> GetProjectBySceneIdAsync(int sceneId)
        {
            var project = await _sceneRepository.GetProjectBySceneIdAsync(sceneId);
            if (project == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(project);
        }

        /// <summary>
        /// Get SceneSegments Relation
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{sceneId}/SceneSegments")]
        public async Task<IEnumerable<SceneSegmentSubSearchModel>> GetSceneSegmentsBySceneIdAsync(int sceneId)
        {
            var sceneSegments = await _sceneRepository.GetSceneSegmentsBySceneIdAsync(sceneId);
            return sceneSegments;
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<SceneSearchModel>> GetAllAsync()
        {

            var result = await _sceneRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] SceneAddModel mScene)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var sceneId = await _sceneRepository.AddAsync(mScene);
            return CreatedAtRoute("GetBySceneIdAsync", new { controller = "Scenes", sceneId = sceneId }, mScene);
        }

        /// <summary>
        /// Edit data by SceneId
        /// </summary>
        /// <param name="SceneId"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SceneUpdateModel mScene)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var sceneId = await _sceneRepository.UpdateByIdAsync(id, mScene);
            return CreatedAtRoute("GetBySceneIdAsync", new { controller = "Scenes", sceneId = sceneId }, mScene);

        }

        /// <summary>
        /// Delete data by SceneId
        /// </summary>
        /// <param name="SceneId"></param>
        [Authorize(Roles = "Admin,Public")]
        [HttpDelete("{sceneId}")]
        public async Task<IActionResult> DeleteBySceneIdAsync(int sceneId)
        {
            await _sceneRepository.RemoveByIdAsync(sceneId);
            return Ok();
        }

    }
}
