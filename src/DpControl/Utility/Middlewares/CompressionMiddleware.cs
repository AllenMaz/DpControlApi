using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Utility.Middlewares
{
    public class CompressionMiddleware
    {
        private readonly RequestDelegate _next;
        const string _header = "Accept-Encoding";

        public CompressionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpHeads = httpContext.Request.Headers;
            if (httpHeads.ContainsKey(_header))
            {
                var acceptEncoding = httpContext.Request.Headers[_header];
                if (acceptEncoding.ToString().IndexOf
                ("gzip", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var stream = httpContext.Response.Body;
                        httpContext.Response.Body = memoryStream;
                        await _next(httpContext);
                        using (var compressedStream = new GZipStream(stream, CompressionLevel.Optimal))
                        {
                            httpContext.Response.Headers.Add("Content-Encoding", new string[] { "gzip" });
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            await memoryStream.CopyToAsync(compressedStream);

                        }
                    }
                }
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
