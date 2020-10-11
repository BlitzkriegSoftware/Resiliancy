using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Libs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc
{
    /// <summary>
    /// Host
    /// </summary>
    [ExcludeFromCodeCoverage]
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
    public class Program
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
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
                    configBuilder.Build();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

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
