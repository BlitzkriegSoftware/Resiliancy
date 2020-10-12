using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Svcs
{
    /// <summary>
    /// Interface: IHttpCaller
    /// </summary>
    public interface IHttpCaller
    {
        /// <summary>
        /// Get the contents of a URI
        /// </summary>
        /// <param name="fromWhere">Uri</param>
        /// <returns>HttpResponseMessage</returns>
        Task<HttpResponseMessage> GetIt(Uri fromWhere);
    }
}
