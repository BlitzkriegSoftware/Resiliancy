using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polly;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Libs
{
    /// <summary>
    /// Example
    /// <c>https://github.com/App-vNext/Polly</c>
    /// </summary>
    public static class AdoPollyPolicy
    {
		/// <summary>
		/// Polly: SQL Retry Policy
		/// </summary>
		/// <param name="logger">ILogger</param>
		/// <returns>Policy</returns>
		public static Polly.Policy SqlRetryPolicy(ILogger logger)
		{
			return Policy
				  .Handle<SqlException>()
				  .WaitAndRetry(new[]
				  {
					TimeSpan.FromMilliseconds(100),
					TimeSpan.FromMilliseconds(300),
					TimeSpan.FromMilliseconds(500)
				  }, (exception, timeSpan, context) => {
					  logger.LogWarning($"SQL Retry Because of `{exception.Message}, for {timeSpan.TotalMilliseconds} ms, Context: {string.Join(";", context.Select(x => x.Key + "=" + x.Value).ToArray())}");
				  });
		}
	}
}
