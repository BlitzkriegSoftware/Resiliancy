using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Controllers
{
    /// <summary>
    /// Controller: Entity Framework Resiliancy
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("/v1/ef")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public class EfController : _CommonBase
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        public EfController(ILogger<EfController> logger) : base(logger)
        {
        }


    }
}
