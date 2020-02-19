using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using Launch.Core.Domain;
using Launch.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Launch.CLI.Commands
{
    public static class LaunchCommand
    {
        public static Command GetCommand()
        {
            var command = new RootCommand("Launch any executable using a different set of credentials than what you used to log into your computer")
            {
                new Option<string>(new[] { "-c", "--credential" })
                {
                    Description = "The name of the credential to use to launch the application.",
                    Required = true,
                },

                new Option<string>(new[] { "-a", "--application", "--app" })
                {
                    Description = "The name of the application to use.",
                    Required = false,
                },

                new Option<string>(new[] { "-t", "--target" })
                {
                    Description = "The full path to an executable to launch. (optional - use instead of --application)",
                    Required = false,
                },
            };

            command.AddCommand(SaveCommand.GetCommand());
            command.AddCommand(ListCommand.GetCommand());
            command.AddCommand(InteractiveCommand.GetCommand());

            command.Handler = CommandHandler.Create(async (LaunchRequest request, IHost host) =>
            {
                if (string.IsNullOrWhiteSpace(request.Application) && string.IsNullOrWhiteSpace(request.Target))
                {
                    throw new Exception($"Must either provide an application or a target.");
                }

                var settingsManager = host.Services.GetRequiredService<ISettingsManager>();
                var launcher = host.Services.GetRequiredService<ILauncher>();

                var credentials = await settingsManager.GetCredential(request.Credential);
                var application = (!string.IsNullOrWhiteSpace(request.Application))
                    ? await settingsManager.GetApplication(request.Application)
                    : new Application(request.Target);

                if (credentials == null)
                {
                    throw new Exception($"Credential with name: {request.Credential} not found. List the available credentials using the 'list creds' command.");
                }

                if (application == null)
                {
                    throw new Exception($"Application or Target not found. List the available applications using the 'list apps' command.");
                }

                var options = new LaunchOptions
                {
                    Domain = credentials.Domain,
                    UserName = credentials.UserName,
                    Password = credentials.Password,
                    FilePath = application.Target,
                    WorkingDirectory = application.WorkingDirectory,
                };

                await launcher.Launch(options);
            });

            return command;
        }
    }
}
