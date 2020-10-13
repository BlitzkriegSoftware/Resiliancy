using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Controllers
{
    /// <summary>
    /// Common Controller
    /// </summary>
    [Route("v1/common")]
    [ApiController]
    public class CommonController : _ControllerBase
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        public CommonController(ILogger<CommonController> logger): base(logger)
        {
        }

        /// <summary>
        /// Get Semantic Version
        /// </summary>
        /// <returns>String of Semantic Version</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public IActionResult VersionGet()
        {
            this.Logger.LogInformation($"Semantic Version: {Program.ProgramMetadata.SemanticVersion}");
            return this.Ok(Program.ProgramMetadata.SemanticVersion);
        }

        /// <summary>
        /// Return Program Metadata and Version Info
        /// </summary>
        /// <returns>Program Metadata and Version Info</returns>
        [HttpGet("version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public IActionResult VersionInfo()
        {
            this.Logger.LogInformation($"{Program.ProgramMetadata}");
            return this.Ok(Program.ProgramMetadata);
        }

        /// <summary>
        /// Health Check
        /// </summary>
        /// <returns></returns>
        [HttpGet("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<HealthCheckResult> Health2()
        {
            var ctx = new Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext();
            var hc = new Libs.BlitzHealthCheck(this.Logger);
            return await hc.CheckHealthAsync(ctx).ConfigureAwait(false);
        }
    }
}
