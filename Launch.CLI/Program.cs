using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using Launch.CLI.Commands;
using Launch.Core;
using Colorful;
using Console = Colorful.Console;
using System.Drawing;

namespace Launch.CLI
{
    public partial class Program
    {

        private static async Task Main(string[] args)
        {
            Logger logger = LogManager.GetLogger("Launch");

            var parser = new CommandLineBuilder(LaunchCommand.GetCommand())
                .UseHost((args) => CreateHostBuilder(args))
                .UseDefaults()
                .UseMiddleware(async (context, next) =>
                {
                    await next(context);
                })
                .UseExceptionHandler((ex, context) =>
                {
                    var stackTrace = Configuration.GetValue<bool>("ShowStackTraceOnError")
                        ? ex.StackTrace
                        : "Error details hidden. Enable 'ShowStackTraceOnError' to see more...";

                    logger.Error(ex, $"The global exception handler caught an exception: {ex.Message}{Environment.NewLine}{stackTrace}");
                    logger.Info($"Press any key to close...");
                    Console.ReadKey();
                })
                .Build();

            // display some startup info

            Console.WriteAscii("Launch", Color.FromArgb(204,102,0));
            Console.Write($"Version: ");
            parser.Parse("--version").Invoke();

            // parse the arguments
            await parser.InvokeAsync(args);
        }

        public static IConfiguration Configuration { get; set; }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(GetBasePath());
                    Configuration = config.Build();
                })
                .ConfigureLogging((hostContext, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();

                    // register NLog
                    loggingBuilder.AddNLog(); // adds ILoggerFactory
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // this is critical for CipherService... wondering how I can add that to the module
                    services.AddDataProtection()
                        .SetApplicationName("app-launcher")
                        .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
                        {
                            EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
                            ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                        })
                    ;
                })
                .ConfigureContainer<ContainerBuilder>((hostContext, builder) =>
                {
                    builder.RegisterModule(new LaunchCoreModule());
                })
            ;

        private static string GetBasePath()
        {
            using var processModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
    }
}
