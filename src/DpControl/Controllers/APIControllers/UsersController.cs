using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility.Authorization;
using DpControl.Utility.Filters;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers.APIControllers
{
    public class UsersController:BaseAPIController
    {
        [FromServices]
        public IUserInfoRepository _userInfoRepository { get; set; }

        /// <summary>
        /// Search data by UserId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery(typeof(UserSearchModel))]
        [HttpGet("{userId}", Name = "GetByUserIdAsync")]
        public async Task<IActionResult> GetByUserIdAsync(string userId)
        {
            var user = await _userInfoRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(user);
        }

        #region Relations
        /// <summary>
        /// Get Locations Relation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{userId}/Locations")]
        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsBySceneIdAsync(string userId)
        {
            var locations = await _userInfoRepository.GetLocationsByUserId(userId);
            return locations;
        }

        /// <summary>
        /// Get Groups Relation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{userId}/Groups")]
        public async Task<IEnumerable<GroupSubSearchModel>> GetGroupsBySceneIdAsync(string userId)
        {
            var groups = await _userInfoRepository.GetGroupsByUserId(userId);
            return groups;
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [APIAuthorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<UserSearchModel>> GetAllAsync()
        {

            var result = await _userInfoRepository.GetAllAsync(); ;

            return result;
        }
    }
}
