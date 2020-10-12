using System.Data;
using System.Linq;
using BlitzkriegSoftware.AdoSqlHelper;
using BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Libs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Controllers
{
    /// <summary>
    /// ADO Controller
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("v1/ado")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public class AdoController : _ControllerBase
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="config">IConfiguration</param>
        public AdoController(ILogger<AdoController> logger, IConfiguration config): base(logger)
        {
            this._config = config;
        }

        private string _sqlconnection;

        /// <summary>
        /// SQL Connection String
        /// </summary>
        public string SqlConnection
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_sqlconnection))
                {
                    foreach(var c in this._config.AsEnumerable())
                    {
                        if(c.Key.Contains("SQL"))
                        {
                            this._sqlconnection = c.Value;
                            break;
                        }
                    }
                    
                }
                return _sqlconnection;
            }
        }

        /// <summary>
        /// Get a sample Customer, Order, Product
        /// </summary>
        /// <returns>Results in JSON</returns>
        [HttpGet("customerorderproduct")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public IActionResult CustomerOrderProduct()
        {
            string sql = "SELECT TOP (1) c.[CustomerId] ,c.[NameLast] ,c.[NameFirst] ,c.[Company] ,c.[EMail] , o.[OrderDate] , p.[Name] , p.[Description] , p.[Price] FROM [Bicycle].[store].[Customer] c inner join [Bicycle].[store].[Order] o on c.[CustomerId] = o.[CustomerId] inner join [Bicycle].[store].[OrderDetail] od on o.[OrderId] = od.[OrderId] inner join [Bicycle].[store].[Product] p on od.[ProductId] = p.[ProductId]";

            DataTable dt = null;
            var polly = Libs.AdoPollyPolicy.SqlRetryPolicy(this.Logger);

            // See: Execute SP with a retry policy via Polly.
            polly.Execute(() =>
            {
                dt = SqlHelper.ExecuteSqlWithParametersToDataTable(this.SqlConnection, sql, null);
            });

            var d = dt.ToDictionary();

            return this.Ok(d);
        }


    }
}
