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
    /// Common Controller Base
    /// </summary>
    [Route("/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "common")]
#pragma warning disable IDE1006 // Naming Styles
    public class _CommonBase : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
    {
        private readonly ILogger _logger;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger"></param>
        public _CommonBase(ILogger logger)
        {
            this._logger = logger;
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
            this._logger.LogInformation($"Semantic Version: {Program.ProgramMetadata.SemanticVersion}");
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
            return this.Ok(Program.ProgramMetadata);
        }

    }
}
