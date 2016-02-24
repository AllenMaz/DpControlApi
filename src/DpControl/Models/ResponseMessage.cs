using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DpControl.Models
{
    public class BaseResponseModel
    {

    }

    /// <summary>
    /// 返回结果是list的消息构造
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListResponseModel<T> : BaseResponseModel
    {
        public T data { get; set; }
    }

    /// <summary>
    /// 返回结果是单条数据的消息构造
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleResponseModel<T> : BaseResponseModel
    {
        public T data { get; set; }
    }
    

    /// <summary>
    /// API错误消息构造
    /// </summary>
    public class ErrorResponseModel : BaseResponseModel
    {

        public int code { get; set; }

        public List<string> errors { get; set; }
    }
    
}
