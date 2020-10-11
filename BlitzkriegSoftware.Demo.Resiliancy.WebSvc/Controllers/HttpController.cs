using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Controllers
{
    /// <summary>
    /// Controller: Http Resiliancy
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("/v1/http")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class HttpController : _CommonBase
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        public HttpController(ILogger<HttpController> logger): base(logger)
        {
        }

    }
}
