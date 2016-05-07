using DpControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DpControl.Utility
{
    public static class ResponseHandler
    {
        /// <summary>
        /// 返回数据是List的消息构造
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ListResponseModel<T> ListResponse<T>(T data)
        {
            ListResponseModel<T> responseData = new ListResponseModel<T>();
            responseData.data = data;

            return responseData;
        }

        /// <summary>
        /// 返回数据是单条数据的消息构造
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SingleResponseModel<T> SingleResponse<T>(T data)
        {
            SingleResponseModel<T> responseData = new SingleResponseModel<T>();
            responseData.data = data;

            return responseData;
        }

        public static string ReturnBadRequestError(string error)
        {
            int httpStatusCode = (int)HttpStatusCode.BadRequest;

            return ReturnError(httpStatusCode,new List<string>() { error});
        }

        /// <summary>
        /// 返回错误消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ReturnError(int httpStatusCode,List<string> errors)
        {
            ErrorResponseModel errResponse = new ErrorResponseModel();
            errResponse.code = httpStatusCode;
            errResponse.errors = errors;
            
            string errJson = JsonHandler.ToJson(errResponse);

            //URL 
            errJson = errJson.Replace("\\/","/");
            return errJson;
        }
    }
}
