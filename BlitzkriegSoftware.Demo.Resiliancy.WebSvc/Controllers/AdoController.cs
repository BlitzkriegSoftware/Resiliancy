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

        #region "Boilerplate"

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        public AdoController(ILogger<AdoController> logger): base(logger)
        {
        }

        #endregion

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
                dt = SqlHelper.ExecuteSqlWithParametersToDataTable(Program.SqlConnectionString, sql, null);
            });

            var d = dt.ToDictionary();

            return this.Ok(d);
        }

        /// <summary>
        /// Inits ETL by calling SP
        /// </summary>
        /// <returns>(nothing)</returns>
        [HttpGet("initetl")]
        [ProducesResponseType(200)]
        public IActionResult InitEtl()
        {
            string sql = "[etl].[p00_InitEtl]";

            var polly = Libs.AdoPollyPolicy.SqlRetryPolicy(this.Logger);

            // See: Execute SP with a retry policy via Polly.
            polly.Execute(() =>
            {
                SqlHelper.ExecuteStoredProcedureWithNoReturn(Program.SqlConnectionString, sql, null);
            });

            return this.Ok();
        }

    }
}
