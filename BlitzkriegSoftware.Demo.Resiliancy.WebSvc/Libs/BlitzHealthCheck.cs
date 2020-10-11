using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Libs
{
    /// <summary>
    /// Example: Custom Health Check
    /// </summary>
    public class BlitzHealthCheck : IHealthCheck
    {
        private readonly ILogger _logger;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        public BlitzHealthCheck(ILogger<BlitzHealthCheck> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Sample Health Check
        /// </summary>
        /// <param name="context">HealthCheckContext</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>HealthCheckResult</returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            // Note: These are the minimum fields desired
            var info = new Dictionary<string, object>
            {
                { "Product", Program.ProgramMetadata.Product },
                { "SemanticVersion", Program.ProgramMetadata.SemanticVersion },
                { "TeamEMail", Program.ProgramMetadata.TeamEMail },
                { "TeamName", Program.ProgramMetadata.TeamName },
                { "ReleaseNotesUrl", Program.ProgramMetadata.ReleaseNotesUrl },
            };

            var deps = new Dictionary<string, object> {
                    { "Dependancy-1", "OK" },
                    { "Dependancy-2", "Timeout" },
                    { "Dependancy-3", "OK" }
            };

            var data = new Dictionary<string, object>
            {
                { "Info" , info },
                { "Dependencies", deps }
            };

            // TODO: Capture any exceptions (if any)
            Exception ex = null;

            var hcr = (ex == null) ?
                new HealthCheckResult(HealthStatus.Healthy, $"{Program.ProgramMetadata.Product} is healthy", null, data) :
                new HealthCheckResult(HealthStatus.Unhealthy, $"{Program.ProgramMetadata.Product} is unhealthy", ex, data);

            var msg = JsonConvert.SerializeObject(hcr);

            if (ex == null)
            {
                this._logger.LogInformation(msg);
            }
            else
            {
                this._logger.LogWarning(ex, msg);
            }

            return Task.FromResult(hcr);
        }

    }
}
