using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DpControl.Models
{
    public class BaseResponseMessage
    {

    }

    /// <summary>
    /// API消息返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseMessage<T> : BaseResponseMessage
    {

        public T data { get; set; }
    }

    /// <summary>
    /// API错误消息返回
    /// </summary>
    public class ErrResponseMessage : BaseResponseMessage
    {

        public int code { get; set; }

        public string error { get; set; }
    }
    
}
