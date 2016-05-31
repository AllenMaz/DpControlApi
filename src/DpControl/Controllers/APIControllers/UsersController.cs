using DpControl.Domain.Entities;
using DpControl.Domain.Execptions;
using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using DpControl.Utility;
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
    [Authorize]
    public class UsersController:BaseAPIController
    {
        private ILoginUserRepository _loginUser;

        public UsersController(ILoginUserRepository loginUser)
        {
            _loginUser = loginUser;
        }

        [FromServices]
        public IUserRepository _userInfoRepository { get; set; }
        [FromServices]
        public ICustomerRepository _customerRepository { get; set; }

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:All<br/>
        /// Description：根据UserId查询用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get current Login user info
        /// </summary>
        /// <returns></returns>
        [EnableQuery(typeof(UserSearchModel))]
        [HttpGet("Current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var loginUser = await _loginUser.GetLoginUserInfoAsync();
            var user = _userInfoRepository.FindByName(loginUser.UserName);
            if (user == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(user);
        }

        /// <summary>
        /// Get current Login user's customer
        /// </summary>
        /// <returns></returns>
        [EnableQuery(typeof(CustomerSearchModel))]
        [HttpGet("Customer")]
        public async Task<IActionResult> GetCustomer()
        {
            var loginUser = await _loginUser.GetLoginUserInfoAsync();
            if (string.IsNullOrEmpty(loginUser.CustomerNo))
                return HttpNotFound();

            var customer = _customerRepository.FindByCustomerNo(loginUser.CustomerNo);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(customer);
        }

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:All<br/>
        /// Description：根据用户名查询用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        //[EnableQuery(typeof(UserSearchModel))]
        //[HttpGet("{userName}", Name = "GetByUserName")]
        //public IActionResult GetByUserName(string userName)
        //{
        //    var user = _userInfoRepository.FindByName(userName);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return new ObjectResult(user);
        //}

        #region Relations
        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:All<br/>
        /// Description：根据用户名获取该用户下的所有Locations
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{userId}/Locations")]
        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsByUserNameAsync(string userId)
        {
            var locations = await _userInfoRepository.GetLocationsByUserId(userId);
            return locations;
        }

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:All<br/>
        /// Description：根据用户名获取该用户下的所有Groups
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{userId}/Groups")]
        public async Task<IEnumerable<GroupSubSearchModel>> GetGroupsByUserNameAsync(string userId)
        {
            var groups = await _userInfoRepository.GetGroupsByUserId(userId);
            return groups;
        }

        /// <summary>
        /// Roles：All<br/>
        /// UserLevel:All<br/>
        /// Description：根据用户名获取该用户下的所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("{userId}/Roles")]
        public async Task<IEnumerable<RoleSubSearchModel>> GetRolesByUserNameAsync(string userId)
        {
            var roles = await _userInfoRepository.GetRolesByUserId(userId);
            return roles;
        }
        #endregion

        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:SuperLevel,CustomerLevel,ProjectLevel<br/>
        /// Description：根据当前登录用户，获取用户列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles =Role.Admin)]
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<UserSearchModel>> GetAllAsync()
        {
            var result = await _userInfoRepository.GetAllAsync(); ;

            return result;
        }

        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:SuperLevel,CustomerLevel,ProjectLevel<br/>
        /// Description：根据当前登录用户，新增用户信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles =Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserAddModel mUserAddModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }
            
            string userId = await _userInfoRepository.AddAsync(mUserAddModel);
            return CreatedAtRoute("GetByUserIdAsync", new { controller = "Users", userId = userId }, mUserAddModel);
        }

        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:All<br/>
        /// Description：ProjectLevel级别下的管理员指定用户下有哪些Locations,Groups;<br/>
        ///       管理员指定用户有哪些角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("{userId}/{navigationProperty}")]
        public async Task<IActionResult> CreateRelationsAsync(string userId, string navigationProperty,
            [FromBody] List<string> navigationPropertyIds)
        {
            if(navigationPropertyIds ==null || navigationPropertyIds.Count ==0 )
            {
                return HttpNotFound();
            }
            var uniqueNavigationPropertyIds = navigationPropertyIds.Distinct().ToList();
            await _userInfoRepository.CreateRelationsAsync(userId, navigationProperty, uniqueNavigationPropertyIds);

            string returnUrl = CreateCustomUrl("GetByUserIdAsync", 
                new { controller = "Users", userId = userId },
                "?expand=" + navigationProperty);

            return Created(returnUrl, null);

        }

        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:All<br/>
        /// Description：ProjectLevel级别下的管理员删除用户下的Locations,Groups;<br/>
        ///       管理员删除用户下的角色
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="navigationProperty"></param>
        /// <param name="navigationPropertyIds"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
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
        /// Roles：All<br/>
        /// UserLevel:All<br/>
        /// Description：修改用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mUser"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] UserUpdateModel mUser)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelStateError());
            }
            var userId = await _userInfoRepository.UpdateByIdAsync(id, mUser);
            var user = await _userInfoRepository.FindByIdAsync(userId);
            return CreatedAtRoute("GetByUserIdName", new { controller = "Users", userName = user.UserName }, mUser);

        }

        /// <summary>
        /// Roles：Admin<br/>
        /// UserLevel:All<br/>
        /// Description：删除用户
        /// </summary>
        /// <param name="userId"></param>
        [Authorize(Roles =Role.Admin)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteByUserIdAsync(string userId)
        {
            await _userInfoRepository.RemoveByIdAsync(userId);
            return Ok();
        }
    }
}
