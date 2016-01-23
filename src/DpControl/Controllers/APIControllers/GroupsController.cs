using DpControl.Domain.IRepository;
using DpControl.Domain.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers.APIControllers
{
    public class GroupsController:BaseAPIController
    {
        [FromServices]
        public IGroupRepository _groupRepository { get; set; }


        /// <summary>
        /// Find Group by ProjectNo
        /// </summary>
        /// <param name="projectNo"></param>
        /// <returns></returns>
        [HttpGet("{projectNo}", Name = "GetByProjectNo")]
        public async Task<IActionResult> GetByProjectNo(string projectNo)
        {
            var group = await _groupRepository.GetByProjectNoAsync(projectNo);
            if (group == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(group);
        }
    }
}
