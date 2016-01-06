using DpControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Utility
{
    public static class ResponseUtility
    {
        /// <summary>
        /// 构造返回值类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseData<T> ConstructResponse<T>(T data)
        {
            ResponseData<T> responseData = new ResponseData<T>();
            responseData.data = data;

            return responseData;
        }

        public static string ConstructErrResponse(ErrResponse errResponse)
        {
            string errJson = JSON.ToJson(errResponse);

            return errJson;
        }
    }
}
