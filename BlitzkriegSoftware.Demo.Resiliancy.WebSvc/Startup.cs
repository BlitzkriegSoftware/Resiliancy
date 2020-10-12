using System;
using System.IO;
using System.Reflection;
using BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Libs;
using BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Svcs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc
{
    /// <summary>
    /// Host Configuration Class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Common
        /// </summary>
        public const string CommonVersion = "common";

        /// <summary>
        /// Current
        /// </summary>
        public const string MajorVersionCurrent = "v1";

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// IConfiguration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
#pragma warning disable CA1822 // Mark members as static
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CA1822 // Mark members as static
        {
            _ = services.AddHealthChecks().AddCheck<Libs.BlitzHealthCheck>("Health Check");

            _ = services.AddControllers();

            _ = services.AddCors();

            // See: We inject HttpClientFactory w. a Retry Policy
            _ = services.AddHttpClient<IHttpCaller, HttpCaller>()
                                   .SetHandlerLifetime(TimeSpan.FromSeconds(1))
                                   .AddPolicyHandler(HttpRetryPolicyJitter.GetRetryPolicy());

            _ = services.AddMvc(
                config =>
                {
                    config.Filters.Add(typeof(Libs.GlobalExceptionFilter));
                });

            _ = services.AddHsts(options =>
             {
                 options.IncludeSubDomains = true;
                 options.MaxAge = TimeSpan.FromMilliseconds(31536000);
             });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Startup.MajorVersionCurrent, MakeOpenApiInfo(Program.ProgramMetadata.Product, Startup.MajorVersionCurrent, "Current API", Program.ProgramMetadata.ReleaseNotesUrl));

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

        }

        private static OpenApiInfo MakeOpenApiInfo(string title, string version, string description, Uri releaseNotes)
        {
            var oai = new OpenApiInfo { Title = title, Version = version, Contact = new OpenApiContact { Email = Program.ProgramMetadata.TeamEMail, Name = Program.ProgramMetadata.TeamName }, Description = description };
            if (releaseNotes != null) oai.Contact.Url = releaseNotes;
            return oai;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IWebHostEnvironment</param>
#pragma warning disable CA1822 // Mark members as static
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore CA1822 // Mark members as static
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                // Prohibited Headers
                _ = context.Response.Headers.Remove("splitsdkversion");
                _ = context.Response.Headers.Remove("x-aspnet-version");
                _ = context.Response.Headers.Remove("x-powered-by");
                _ = context.Response.Headers.Remove("server");

                // Required Headers
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Xss-Protection", "1;mode=block");

                await next().ConfigureAwait(false);
            });
            
            app.UseCors();

            app.UseHsts();

            app.UseHttpsRedirection();

            #region "Swagger"

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{MajorVersionCurrent}/swagger.json", $"API {MajorVersionCurrent}");
            });

            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/health");
        }

    }
}
