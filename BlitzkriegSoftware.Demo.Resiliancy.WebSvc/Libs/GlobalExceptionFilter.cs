using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Libs
{
    /// <summary>
    /// Global Exception Filter
    /// <para>See: www.talkingdotnet.com/global-exception-handling-in-aspnet-core-webapi/</para>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        /// <summary>
        /// Field: ILogger
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionFilter"/> class.
        /// CTOR
        /// </summary>
        /// <param name="loggerFactory">Logger to inject</param>
        public GlobalExceptionFilter(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this._logger = loggerFactory.CreateLogger("Global Exception Filter");
        }

        #region "Dispose"

        // Flag: Has Dispose already been called?
        private bool disposed = false;

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">bool</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // ---
            }

            this.disposed = true;
        }

        #endregion

        /// <summary>
        /// Handle Exception
        /// </summary>
        /// <param name="context">ExceptionContext</param>
        public void OnException(ExceptionContext context)
        {
            if (context == null)
            {
                return;
            }

            var data = new Dictionary<string, string>();
            var statusCode = HttpStatusCode.InternalServerError;
            var message = string.Empty;

            var ex = context.Exception;

            TypeSwitch.Do(
                    ex,
                    TypeSwitch.Case<ArgumentException>(() => { statusCode = HttpStatusCode.BadRequest; }),
                    TypeSwitch.Case<ArgumentNullException>(() => { statusCode = HttpStatusCode.BadRequest; }),
                    TypeSwitch.Case<ArgumentOutOfRangeException>(() => { statusCode = HttpStatusCode.BadRequest; }),
                    TypeSwitch.Case<KeyNotFoundException>(() => { statusCode = HttpStatusCode.NotFound; }));

            var response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";

            var err = new Models.ErrorPayload()
            {
                Data = data,
                StackTrace = ex.StackTrace,
                Message = ex.Message,
                StatusCode = (int)statusCode,
            };

            this._logger?.LogError(err.ToString());

            response.WriteAsync(JsonConvert.SerializeObject(err));
            response.CompleteAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
