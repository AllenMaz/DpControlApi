using DpControl.Domain.Entities;
using DpControl.Utility.Filters;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers.APIControllers
{
    public class UtilitiesController:BaseAPIController
    {
        [HttpGet("UserLevel",Name = "GetUserLevel")]
        public IActionResult GetUserLevel()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var userLevelValues = Enum.GetValues(typeof(UserLevel));
            foreach (var value in userLevelValues)
            {
                result.Add(Enum.GetName(typeof(UserLevel),value),(int)value);

            }
            return new ObjectResult(result);
        }

    }
}
