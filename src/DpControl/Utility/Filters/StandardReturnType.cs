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
    /// 统一数据返回格式
    /// </summary>
    public class StandardReturnType : ActionFilterAttribute
    {
        private string _actionReturnType;

        /// <summary>
        /// Action数据返回类型，用于判断返回何种格式的数据
        /// </summary>
        /// <param name="actionType"></param>
        public StandardReturnType(string actionReturnType)
        {
            this._actionReturnType = actionReturnType;
        }

        /// <summary>
        /// 在controller action result执行之前调用 
        /// 统一数据返回类型
        /// </summary>
        /// <param name="context"></param>
        /// 
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;
            var responseData = new object();
            switch (_actionReturnType)
            {
                case Common.ActionReturnType_GetList:

                    //获取返回的结果
                    
                    //统一查询返回格式
                    responseData = ResponseHandler.ConstructResponse<object>(result.Value);
                    result.Value = responseData;

                    break;
                case Common.ActionReturnType_GetSingle:
                    
                    //统一查询返回格式
                    responseData = ResponseHandler.ConstructResponse<object>(result.Value);
                    result.Value = responseData;

                    break;
                case Common.ActionReturnType_Post:

                    //统一查询返回格式
                    //responseData = ResponseHandler.ConstructResponse<object>(result.Value);
                    //result.Value = responseData;

                    break;
                case Common.ActionReturnType_Put:
                    break;
                case Common.ActionReturnType_Delete:
                    break;
                default:
                    break;
            }
            
        }

        private object StandardGetListFromat(ResultExecutingContext context)
        {
            //获取返回的结果
            var result = context.Result as ObjectResult;
            //统一查询返回格式
            var responseData = ResponseHandler.ConstructResponse<object>(result.Value);
            return responseData;
        }
    }
}
