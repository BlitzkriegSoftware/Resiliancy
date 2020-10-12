using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class HttpController : _ControllerBase
    {
        private readonly IHttpClientFactory _factory;

        /// <summary>
        /// Client Factory
        /// </summary>
        private IHttpClientFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="factory">IHttpClientFactory</param>
        public HttpController(ILogger<HttpController> logger, IHttpClientFactory factory): base(logger)
        {
            _factory = factory;
        }

        /// <summary>
        /// Employees
        /// </summary>
        /// <returns></returns>
        [HttpGet("employees")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public string Employees()
        {
            var url = "http://dummy.restapiexample.com/api/v1/employees";

            var client = this.Factory.CreateClient();

            var result = client.GetAsync(new Uri(url)).GetAwaiter().GetResult();

            var json = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return json;
        }

    }
}
