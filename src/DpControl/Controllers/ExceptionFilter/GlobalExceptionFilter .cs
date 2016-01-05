using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Http;
using System.Text;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace DpControl.Controllers.ExceptionFilter
{
    /// <summary>
    /// 全局捕获异常
    /// 把异常转换为HTTP消息
    /// </summary>
    public class GlobalExceptionFilter  : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {

            //if (context.Exception is NotImplementedException)
            //{
            bool aa = context.HttpContext.Response.HasStarted;
            context.HttpContext.Response.StatusCode = 404;
            var data = Encoding.UTF8.GetBytes("异常测试");
            //context.Response.Body.WriteAsync(data, 0, data.Length);
            //var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            //{
            //    Content = new StringContent(string.Format("No product with ID = {0}", "aaa")),
            //    ReasonPhrase = "Product ID Not Found"
            //};
            //throw new HttpResponseException(resp);

            //}
        }
    }
}
