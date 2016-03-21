using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
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
    public class SceneSegmentsController:BaseAPIController
    {
        [FromServices]
        public ISceneSegmentRepository _sceneSegmentRepository { get; set; }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] SceneSegmentAddModel mSceneSegment)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var sceneSegmentId = await _sceneSegmentRepository.AddAsync(mSceneSegment);
            return CreatedAtRoute("GetBySceneSegmentIdAsync", new { controller = "SceneSegments", sceneSegmentId = sceneSegmentId }, mSceneSegment);
        }
        /// <summary>
        /// Search data by sceneSegmentId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [EnableQuery(true,typeof(SceneSegmentSearchModel))]
        [HttpGet("{sceneSegmentId}", Name = "GetBySceneSegmentIdAsync")]
        public async Task<IActionResult> GetBySceneSegmentIdAsync(int sceneSegmentId)
        {
            var sceneSegment = await _sceneSegmentRepository.FindByIdAsync(sceneSegmentId);
            if (sceneSegment == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(sceneSegment);
        }

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<SceneSegmentSearchModel>> GetAllAsync([FromUri] Query query)
        {

            var result = await _sceneSegmentRepository.GetAllAsync(query); ;

            return result;
        }

        /// <summary>
        /// Edit data by sceneSegmentId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin")]
        [HttpPut("{sceneSegmentId}")]
        public async Task<IActionResult> UpdateAsync(int sceneSegmentId, [FromBody] SceneSegmentUpdateModel mSceneSegment)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            var id = await _sceneSegmentRepository.UpdateByIdAsync(sceneSegmentId, mSceneSegment);
            return CreatedAtRoute("GetBySceneSegmentIdAsync", new { controller = "SceneSegments", sceneSegmentId = id }, mSceneSegment);

        }

        /// <summary>
        /// Delete data by SceneSegmentId
        /// </summary>
        /// <param name="customerId"></param>
        [APIAuthorize(Roles = "Admin")]
        [HttpDelete("{sceneSegmentId}")]
        public async Task<IActionResult> DeleteBySceneSegmentIdAsync(int sceneSegmentId)
        {
            await _sceneSegmentRepository.RemoveByIdAsync(sceneSegmentId);
            return Ok();
        }
    }
}
