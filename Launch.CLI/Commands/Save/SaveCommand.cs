using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using Launch.Core.Domain;
using Launch.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Launch.CLI.Commands
{
    public static class SaveCommand
    {
        public static Command GetCommand()
        {
            var command = new Command("save", "Save or update a credential or application");
            command.AddCommand(SaveCredCommand.GetCommand());
            command.AddCommand(SaveAppCommand.GetCommand());

            return command;
        }
    }

    public static class SaveCredCommand
    {
        public static Command GetCommand()
        {
            var command = new Command("cred", "Save or Update your credentials for a particular domain")
            {
                new Option<string>(new [] {"-n", "--name"})
                {
                    Description = "Short name of the credential to be used when launching an app",
                    Required = true,
                },

                new Option<string>(new [] {"-d", "--domain"})
                {
                    Description = "The domain of this credential",
                    Required = true,
                },

                new Option<string>(new [] {"-u", "--username"})
                {
                    Description = "The username associated with this credential",
                    Required = true,
                },

                new Option<string>(new [] {"-p", "--password"})
                {
                    Description = "The password associated with this credential",
                    Required = true,
                },
            };

            command.Handler = CommandHandler.Create(async (SaveCredRequest request, IHost host) =>
            {
                var settingsManager = host.Services.GetRequiredService<ISettingsManager>();

                var credential = new Credential
                {
                    Name = request.Name,
                    Domain = request.Domain,
                    UserName = request.UserName,
                    Password = request.Password,
                };

                await settingsManager.SaveCredential(credential);
            });

            return command;
        }
    }

    public static class SaveAppCommand
    {
        public static Command GetCommand()
        {
            var command = new Command("app", "Save or Update the filepaths associated with an application")
            {
                new Option<string>(new [] {"-n", "--name"})
                {
                    Description = "A name or alias for this app so you can easily reference it",
                    Required = true,
                },
                
                new Option<string>(new [] {"-t", "--target"})
                {
                    Description = "The full path to the executable (most important argument)",
                    Required = true,
                },

                new Option<string>(new [] {"-w", "--working-directory"})
                {
                    Description = "The directory that the app should be executed from. (optional)",
                    Required = false,
                },
            };

            command.Handler = CommandHandler.Create(async (SaveAppRequest request, IHost host) =>
            {
                var settingsManager = host.Services.GetRequiredService<ISettingsManager>();

                var application = new Application
                {
                    Name = request.Name,
                    Target = request.Target,
                    WorkingDirectory = request.WorkingDirectory,
                    IconPath = request.IconPath,
                    RunAsAdmin = request.AdminMode,
                };

                await settingsManager.SaveApplication(application);
            });

            return command;
        }
    }
}
