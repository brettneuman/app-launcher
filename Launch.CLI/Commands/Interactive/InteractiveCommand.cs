using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Launch.CLI.Commands
{
    public class InteractiveCommand
    {
        public static Command GetCommand()
        {
            var command = new Command("interactive", "Run multiple commands in the same session")
            {
                Handler = CommandHandler.Create(async (IHost host, IConsole console, InvocationContext context) =>
                {
                    console.Out.Write($">> entering interactive mode, type 'exit' to exit.{Environment.NewLine}");

                    var interactive = true;
                    while (interactive)
                    {
                        console.Out.Write(">> ");
                        var arguments = Console.ReadLine();

                        if (arguments.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        {
                            interactive = false;
                        }
                        else
                        {
                            await context.Parser.InvokeAsync(arguments);
                        }
                    }
                })
            };

            return command;
        }
    }
}
