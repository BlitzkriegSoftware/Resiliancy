using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Controllers
{
    /// <summary>
    /// Controller: Entity Framework Resiliancy
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("v1/ef")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class EfController : _ControllerBase
    {

        #region "Properties and Helpers"
        
        private readonly ILoggerFactory _factory;

        #endregion

        #region "CTOR"

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="factory">ILoggerFactory</param>
        /// <param name="logger">ILogger</param>
        public EfController(ILoggerFactory factory,  ILogger<EfController> logger) : base(logger)
        {
            this._factory = factory;
        }

        #endregion

        /// <summary>
        /// EF Example of Customer Order Product
        /// </summary>
        /// <returns></returns>
        [HttpGet("customerorderproduct2")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public IActionResult CustomerOrderProduct2()
        {
            using(var ctx = new Ef.BicycleContext(_factory, Program.SqlConnectionString))
            {
                var q1 = (from c in ctx.Customer
                          join o in ctx.Order
                          on c.CustomerId equals o.CustomerId
                          join od in ctx.OrderDetail 
                          on o.OrderId equals od.OrderId
                          join p in ctx.Product
                          on od.ProductId equals p.ProductId
                          select new
                          {
                              c.CustomerId,
                              c.NameFirst,
                              c.NameLast,
                              o.OrderDate,
                              od.Quantity,
                              p.ProductId,
                              p.Name,
                              p.Description,
                              p.Price
                          }
                    );

                var results = q1.ToList();

                return this.Ok(results);
            }
        }

    }
}
