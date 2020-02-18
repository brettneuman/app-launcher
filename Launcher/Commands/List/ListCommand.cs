using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using Launch.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Launch.CLI.Commands
{
    public class ListCommand
    {
        public static Command GetCommand()
        {
            var command = new Command("list", "Print out a list of credentials or applications currently configured.");
            command.AddCommand(ListCredsCommand.GetCommand());
            command.AddCommand(ListAppsCommand.GetCommand());

            return command;
        }

        public class ListCredsCommand
        {
            public static Command GetCommand()
            {
                var command = new Command("creds", "Print out the list of credentials currently configured.")
                {
                    Handler = CommandHandler.Create(async (IHost host, IConsole console) =>
                    {
                        var settingsManager = host.Services.GetRequiredService<ISettingsManager>();
                        var creds = await settingsManager.GetCredentials();

                        foreach (var cred in creds)
                        {
                            console.Out.Write($"Credential: '{cred.Name}'{Environment.NewLine}Domain: '{cred.Domain}'{Environment.NewLine}UserName: '{cred.UserName}'{Environment.NewLine}");
                            console.Out.Write($"{Environment.NewLine}");
                        }
                    })
                };

                return command;
            }
        }

        public class ListAppsCommand
        {
            public static Command GetCommand()
            {
                var command = new Command("apps", "Print out the list of applications currently configured.")
                {
                    Handler = CommandHandler.Create(async (IHost host, IConsole console) =>
                    {
                        var settingsManager = host.Services.GetRequiredService<ISettingsManager>();
                        var apps = await settingsManager.GetApplications();

                        foreach (var app in apps)
                        {
                            console.Out.Write($"Application: '{app.Name}'{Environment.NewLine}");
                            console.Out.Write($"Target: '{app.Target}'{Environment.NewLine}");
                            console.Out.Write($"Working Directory: '{app.WorkingDirectory}'{Environment.NewLine}");
                            console.Out.Write($"{Environment.NewLine}");
                        }
                    })
                };

                return command;
            }
        }
    }
}
