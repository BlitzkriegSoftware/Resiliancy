using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Controllers
{
    /// <summary>
    /// Common Controller Base
    /// </summary>
    [ApiController]
#pragma warning disable IDE1006 // Naming Styles
    public class _ControllerBase : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger Logger
        {
            get
            {
                return _logger;
            }
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger"></param>
        public _ControllerBase(ILogger logger)
        {
            this._logger = logger;
        }

    }
}
