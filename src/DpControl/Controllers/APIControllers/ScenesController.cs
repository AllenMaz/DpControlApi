using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
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
    public class ScenesController:BaseAPIController
    {
        [FromServices]
        public ISceneRepository _sceneRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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
        /// Search data by SceneId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        [FormatReturnType]
        public async Task<IEnumerable<SceneSearchMoodel>> GetAllAsync([FromUri] Query query)
        {

            var result = await _sceneRepository.GetAllAsync(query); ;

            return result;
        }

        /// <summary>
        /// Edit data by SceneId
        /// </summary>
        /// <param name="SceneId"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
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
        /// <param name="customerId"></param>
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{sceneId}")]
        public async Task<IActionResult> DeleteByProjectIdIdAsync(int sceneId)
        {
            await _sceneRepository.RemoveByIdAsync(sceneId);
            return Ok();
        }

    }
}
