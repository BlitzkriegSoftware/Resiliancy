using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Libs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc
{
    /// <summary>
    /// Host
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// Main Entry Point
        /// </summary>
        /// <param name="args">Command line args</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Host Builder
        /// </summary>
        /// <param name="args">Command line args</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    var env = context.HostingEnvironment;
                    configBuilder
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json")
                       .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                       // Note: Environment vars should always come AFTER the json files
                       .AddEnvironmentVariables();

                    // Do this LAST
                    Program.Configuration = configBuilder.Build();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    _ = webBuilder.UseStartup<Startup>();
                });

        #region "Configuration and Helpers"

        /// <summary>
        /// IServiceCollection
        /// </summary>
        public static IServiceProvider Services { get; set; }

        /// <summary>
        /// IConfiguration Root
        /// </summary>
        public static IConfigurationRoot Configuration { get; set; }

        private static string _sqlconnectionstring;
        private static string _resturl;

        /// <summary>
        /// SQL Connection String
        /// </summary>
        public static string SqlConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sqlconnectionstring))
                {
                    foreach (var c in Configuration.AsEnumerable())
                    {
                        if (c.Key.Contains("SQL"))
                        {
                            _sqlconnectionstring = c.Value;
                            break;
                        }
                    }

                }
                return _sqlconnectionstring;
            }
        }


        /// <summary>
        /// Rest URL
        /// </summary>
        public static string RestUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_resturl))
                {
                    foreach (var c in Configuration.AsEnumerable())
                    {
                        if (c.Key.Contains("REST"))
                        {
                            _resturl = c.Value;
                            break;
                        }
                    }

                }
                return _resturl;
            }
        }

        #endregion

        #region "Assembly Metadata"

        private static Models.AssemblyVersionMetadata _assemblyVersionMetadata;

        /// <summary>
        /// Gets semantic Version, etc from Assembly Metadata
        /// </summary>
        public static Models.AssemblyVersionMetadata ProgramMetadata
        {
            get
            {
                if (_assemblyVersionMetadata == null)
                {
                    _assemblyVersionMetadata = new Models.AssemblyVersionMetadata();
                    var assembly = typeof(Program).Assembly;
                    foreach (var attribute in assembly.GetCustomAttributesData())
                    {
                        if (!attribute.TryParse(out var value))
                        {
                            value = string.Empty;
                        }

                        var name = attribute.AttributeType.Name;
                        System.Diagnostics.Trace.WriteLine($"{name}, {value}");
                        _assemblyVersionMetadata.PropertySet(name, value);
                    }
                }

                return _assemblyVersionMetadata;
            }
        }

        #endregion

    }
}
