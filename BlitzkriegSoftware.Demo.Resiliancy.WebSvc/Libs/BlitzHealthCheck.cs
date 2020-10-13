using BlitzkriegSoftware.AdoSqlHelper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
        public BlitzHealthCheck(ILogger logger)
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
            Exception ex = null;

            var info = new Dictionary<string, object>
            {
                { "Product", Program.ProgramMetadata.Product },
                { "SemanticVersion", Program.ProgramMetadata.SemanticVersion },
                { "TeamEMail", Program.ProgramMetadata.TeamEMail },
                { "TeamName", Program.ProgramMetadata.TeamName },
                { "ReleaseNotesUrl", Program.ProgramMetadata.ReleaseNotesUrl },
            };

            var deps = new Dictionary<string, string>();

            try {
                CheckSql();
                deps.Add("SQL Connection", "Ok");
            }
            catch (SqlException e)
            {
                ex = e;
                deps.Add( "SQL Connection", $"Not OK: {e.Message}" );
            }

            try
            {
                CheckRest();
                deps.Add("REST API", "Ok");
            } catch (System.Net.Http.HttpRequestException e)
            {
                ex = e;
                deps.Add("REST API", $"Not OK: {e.Message}");
            }

            var data = new Dictionary<string, object>
            {
                { "Info" , info },
                { "Dependencies", deps }
            };

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


        private static void CheckSql()
        {
            var cs = Program.SqlConnectionString;
            var sql = "select count(1) from [store].[Customer]";
            _ = SqlHelper.ExecuteSqlWithParametersToScaler<int>(cs, sql, null);
        }

        private static void CheckRest()
        {
            var uri = new Uri(Program.RestUrl + "/api/v1/employee/1");
            var factory = Program.Services.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient();
            _ = client.GetAsync(uri).GetAwaiter().GetResult();
        }

    }
}
