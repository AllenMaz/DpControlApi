using DpControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Utility
{
    public static class ResponseHandler
    {
        /// <summary>
        /// 构造返回值类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseMessage<T> ConstructResponse<T>(T data)
        {
            ResponseMessage<T> responseData = new ResponseMessage<T>();
            responseData.data = data;

            return responseData;
        }

        public static string ConstructErrResponse(ErrResponseMessage errResponse)
        {
            string errJson = Json.ToJson(errResponse);

            return errJson;
        }
    }
}
