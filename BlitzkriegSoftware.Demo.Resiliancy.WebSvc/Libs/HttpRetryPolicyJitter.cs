using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Libs
{
    /// <summary>
    /// Polly HttpClient Expo. Retry w. Jitter
    /// </summary>
    public static class HttpRetryPolicyJitter
    {
        // Weak Crypto is not an issue here
        static Random jitterer = new Random();

        /// <summary>
        /// Gives back a nice standard policy
        /// </summary>
        /// <returns>Polly Policy</returns>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // exponential back-off plus some jitter
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                .WaitAndRetryAsync(5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                  + TimeSpan.FromMilliseconds(jitterer.Next(0, 100))
                );
        }
    }
}
