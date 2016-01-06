using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{

    public class BaseResponseData
    {

    }

    /// <summary>
    /// API消息返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseData<T>: BaseResponseData
    {

        public T data { get; set; }
    }

    /// <summary>
    /// API错误消息返回
    /// </summary>
    public class ErrResponse : BaseResponseData
    {

        public string code { get; set; }

        public string message { get; set; }
    }
}
