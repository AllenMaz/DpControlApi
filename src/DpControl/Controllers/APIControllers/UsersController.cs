using DpControl.Domain.Entities;
using DpControl.Domain.Execptions;
using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility.Authorization;
using DpControl.Utility.Filters;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
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
        public IUserRepository _userInfoRepository { get; set; }

        [FromServices]
        private IUrlHelper _urlHelper { get; set; }

        /// <summary>
        /// Search data by UserId
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
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
        [Authorize(Roles = "Admin,Public")]
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
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{userId}/Groups")]
        public async Task<IEnumerable<GroupSubSearchModel>> GetGroupsBySceneIdAsync(string userId)
        {
            var groups = await _userInfoRepository.GetGroupsByUserId(userId);
            return groups;
        }

        /// <summary>
        /// Get Roles Relation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [EnableQuery]
        [HttpGet("{userId}/Roles")]
        public async Task<IEnumerable<RoleSubSearchModel>> GetRolesBySceneIdAsync(string userId)
        {
            var roles = await _userInfoRepository.GetRolesByUserId(userId);
            return roles;
        }
        #endregion

        /// <summary>
        /// Search all data
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Public")]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<UserSearchModel>> GetAllAsync()
        {

            var result = await _userInfoRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserAddModel mUserAddModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            //check UserLevel value
            if (!Enum.IsDefined(typeof(UserLevel), mUserAddModel.UserLevel))
            {
                string userLevelUrl = CreateCustomUrl("GetUserLevel", new { controller = "Utilities"});
                throw new ExpectException("Invalid UserLevel.UserLevel ref :"+ userLevelUrl);

            }

            string userId = await _userInfoRepository.AddAsync(mUserAddModel);
            return CreatedAtRoute("GetByUserIdAsync", new { controller = "Users", userId = userId }, mUserAddModel);
        }

        /// <summary>
        /// Create RelationShips:Roles、Locations、Groups
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [HttpPost("{userId}/{navigationProperty}")]
        public async Task<IActionResult> CreateRelationsAsync(string userId,string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if(navigationPropertyIds ==null || navigationPropertyIds.Count ==0 )
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();
            await _userInfoRepository.CreateRelationsAsync(userId,navigationProperty, uniqueNavigationPropertyIds);

            string returnUrl = CreateCustomUrl("GetByUserIdAsync", 
                new { controller = "Users", userId = userId },
                "?expand=" + navigationProperty);

            return Created(returnUrl, null);

        }

        /// <summary>
        /// Remove RelationShips:Roles、Locations、Groups
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [HttpDelete("{userId}/{navigationProperty}")]
        public async Task<IActionResult> RemoveRelationsAsync(string userId, string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if (navigationPropertyIds == null || navigationPropertyIds.Count == 0)
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();

            await _userInfoRepository.RemoveRelationsAsync(userId, navigationProperty, uniqueNavigationPropertyIds);
            return Ok();
        }

        /// <summary>
        /// Edit data by UserId
        /// </summary>
        /// <param name="customerNo"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] UserUpdateModel mUser)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }

            //check UserLevel value
            if (!Enum.IsDefined(typeof(UserLevel), mUser.UserLevel))
            {
                string userLevelUrl = CreateCustomUrl("GetUserLevel", new { controller = "Utilities" });
                throw new ExpectException("Invalid UserLevel.UserLevel ref :" + userLevelUrl);

            }

            var userId = await _userInfoRepository.UpdateByIdAsync(id, mUser);
            return CreatedAtRoute("GetByUserIdAsync", new { controller = "Users", userId = userId }, mUser);

        }

        /// <summary>
        /// Delete data by UserId
        /// </summary>
        /// <param name="userId"></param>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteByUserIdAsync(string userId)
        {
            await _userInfoRepository.RemoveByIdAsync(userId);
            return Ok();
        }
    }
}
