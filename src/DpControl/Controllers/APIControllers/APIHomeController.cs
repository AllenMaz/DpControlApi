using DpControl.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ApiExplorer;
using Microsoft.AspNet.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers.APIControllers
{
    [Route("v1")]
    public class APIHomeController:Controller
    {
        private IApiDescriptionGroupCollectionProvider _description;
        public APIHomeController(IApiDescriptionGroupCollectionProvider description)
        {
            _description = description;
        }

        /// <summary>
        /// Get api list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Link> GetApiStartList()
        {
            IEnumerable<Link> links = new List<Link>();
            foreach (var group in _description.ApiDescriptionGroups.Items)
            {
                foreach (var each in group.Items)
                {
                    var bb = each.ActionDescriptor as ControllerActionDescriptor;
                    var cc = bb.MethodInfo.CustomAttributes.FirstOrDefault(v => v.AttributeType == typeof(AuthorizeAttribute));
                    var aa = HttpContext.Request.Host + each.RelativePath;
                }
            }
            return links;
        }
    }
}
