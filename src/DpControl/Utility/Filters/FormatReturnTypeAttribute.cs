using DpControl.Models;
using DpControl.Utility;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Utility.Filters
{

    /// <summary>
    /// 格式化List数据返回类型
    /// </summary>
    public class FormatReturnTypeAttribute : ActionFilterAttribute
    {
        private string _actionReturnType;
        

        /// <summary>
        /// 在controller action result执行之前调用 
        /// 统一数据返回类型
        /// </summary>
        /// <param name="context"></param>
        /// 
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;
            ListResponseModel<object> responseData = ResponseHandler.ListResponse<object>(result.Value);
            result.Value = responseData;
            
        }
        
    }
}
