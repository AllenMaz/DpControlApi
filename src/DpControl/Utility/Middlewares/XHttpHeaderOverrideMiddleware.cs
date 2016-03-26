using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DpControl.Utility.Middlewares
{
    /// <summary>
    /// 允许通过POST方法覆盖http方法
    /// </summary>
    public class XHttpHeaderOverrideMiddleware
    {
        private readonly RequestDelegate _next;
        private PathString _path;
        readonly string[] _methods = { "DELETE", "HEAD", "PUT" };
        const string _header = "X-HTTP-Method-Override";

        public XHttpHeaderOverrideMiddleware(RequestDelegate next,MiddlewareOptions options)
        {
            _next = next;
            _path = options.Path;
        }
        public Task Invoke(HttpContext httpContext)
        {
            //如果是post请求，且请求头包含X-HTTP-Method-Override
            if (httpContext.Request.Path.StartsWithSegments(_path)
                && httpContext.Request.Method == HttpMethod.Post.Method
                && httpContext.Request.Headers.ContainsKey(_header))
            {
                string headerValue = httpContext.Request.Headers[_header];
                if (_methods.Contains(headerValue.ToUpper()))
                {
                    // Change the request method.
                    httpContext.Request.Method = headerValue.ToUpper();
                }
            }

            return _next.Invoke(httpContext);
        }
    }
}
