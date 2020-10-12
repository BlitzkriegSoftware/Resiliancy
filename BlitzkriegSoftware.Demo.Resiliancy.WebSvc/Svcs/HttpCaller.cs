using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Svcs
{
    /// <summary>
    /// Implementation: HttpCaller
    /// </summary>
    public class HttpCaller : IHttpCaller
    {
        private HttpClient _client;

        private HttpCaller() { }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="client">(DI Injected)</param>
        public HttpCaller(HttpClient client) => this._client = client;

        /// <summary>
        /// Get the contents of a URI
        /// </summary>
        /// <param name="fromWhere">Uri</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> GetIt(Uri fromWhere)
        {
            return await this._client.GetAsync(fromWhere).ConfigureAwait(false);
        }
    }
}
