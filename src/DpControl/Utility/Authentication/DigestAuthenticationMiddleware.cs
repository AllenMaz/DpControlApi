using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DpControl.Utility.Authentication
{
    /// <summary>
    /// Implement AuthenticationMiddleware
    /// </summary>
    public class DigestAuthenticationMiddleware: AuthenticationMiddleware<DigestAuthenticationOptions>
    {
        public DigestAuthenticationMiddleware(
            RequestDelegate next,
            IOptions<DigestAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder)
            : base(next, options, loggerFactory,encoder)
        { }

        protected override AuthenticationHandler<DigestAuthenticationOptions> CreateHandler()
        {
            return new DigestAuthenticationHandler();
        }
    }
}
