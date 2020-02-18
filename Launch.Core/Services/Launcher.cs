using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Launch.Core.Foundation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Launch.Core.Foundation.RunAs;

namespace Launch.Core.Services
{

    public class Launcher : ILauncher
    {
        private readonly ICipherService _cipherService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public Launcher(
            ICipherService cipherService,
            IConfiguration configuration,
            ILogger<Launcher> logger
            )
        {
            _cipherService = cipherService;
            _configuration = configuration;
            _logger = logger;
        }

        public Task<Process> Launch(LaunchOptions options)
        {
            _logger.LogInformation($"Launching app: '{options.FilePath}' using credentials for: '{options.Domain}'");

            StartUpInfo startupInfo = new StartUpInfo();
            startupInfo.cb = Marshal.SizeOf(startupInfo);
            startupInfo.lpTitle = null;
            startupInfo.dwFlags = (int)StartUpInfoFlags.UseCountChars;
            startupInfo.dwYCountChars = 50;

            var workingDirectory = !string.IsNullOrWhiteSpace(options.WorkingDirectory)
                ? options.WorkingDirectory
                : Path.GetDirectoryName(options.FilePath);

            var password = _cipherService.Decrypt(options.Password);

            var process = RunAs.StartProcess(
                userName: options.UserName,
                domain: options.Domain,
                password: password,
                logonFlags: RunAs.LogonFlags.NetworkCredentialsOnly,
                applicationName: null,
                commandLine: options.FilePath,
                creationFlags: RunAs.CreationFlags.NewConsole,
                environment: IntPtr.Zero,
                currentDirectory: workingDirectory,
                startupInfo: ref startupInfo,
                processInfo: out _
            );

            return Task.FromResult(process);
        }
    }
}
