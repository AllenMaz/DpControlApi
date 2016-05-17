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
            var result = this.ConstructEnumDict(typeof(UserLevel));
            return new ObjectResult(result);
        }

        [HttpGet("Orientation", Name = "GetOrientation")]
        public IActionResult GetOrientation()
        {
            var result = this.ConstructEnumDict(typeof(Orientation));
            return new ObjectResult(result);
        }

        [HttpGet("DeviceType", Name = "GetDeviceType")]
        public IActionResult GetDeviceType()
        {
            var result = this.ConstructEnumDict(typeof(DeviceType));
            return new ObjectResult(result);
        }

        [HttpGet("CommMode", Name = "GetCommMode")]
        public IActionResult GetCommMode()
        {
            var result = this.ConstructEnumDict(typeof(CommMode));
            return new ObjectResult(result);
        }



        [NonAction]
        private Dictionary<string,int> ConstructEnumDict(Type enumType)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var enumValues = Enum.GetValues(enumType);
            foreach (var value in enumValues)
            {
                result.Add(Enum.GetName(enumType, value), (int)value);

            }
            return result;
        }
    }
}
