using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Launch.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Launch.Core.Services
{
    public class SettingsManager : ISettingsManager
    {
        private readonly ISettingsFileManager _fileManager;
        private readonly ICipherService _cipherService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        private SettingsModel _settings;

        public SettingsManager(
            ISettingsFileManager fileManager,
            ICipherService cipherService,
            IConfiguration configuration,
            ILogger<SettingsManager> logger
            )
        {
            _fileManager = fileManager;
            _cipherService = cipherService;
            _configuration = configuration;
            _logger = logger;
        }

        protected async Task<SettingsModel> ParseSettings(CancellationToken cancellationToken = default)
        {
            _settings = await _fileManager.ParseSettingsFile(cancellationToken);
            if (_settings == null)
            {
                _settings = new SettingsModel();
            }

            return _settings;
        }

        public async Task<IEnumerable<Credential>> GetCredentials(CancellationToken cancellationToken = default)
        {
            if (_settings == null)
            {
                await ParseSettings(cancellationToken);
            }

            return _settings?.Credentials;
        }

        public async Task<Credential> GetCredential(string cred, CancellationToken cancellationToken = default)
        {
            if (_settings == null)
            {
                await ParseSettings(cancellationToken);
            }

            return _settings
                ?.Credentials
                ?.FirstOrDefault(x => x.Name.Equals(cred, StringComparison.OrdinalIgnoreCase))
                ;
        }

        public async Task SaveCredential(Credential credential, CancellationToken cancellationToken = default)
        {
            if (_settings == null)
            {
                await ParseSettings(cancellationToken);
            }

            try
            {
                _logger.LogInformation($"Saving credential...");

                var creds = await GetCredential(credential.Name, cancellationToken);
                if (creds == null)
                {
                    _logger.LogDebug($"Credential with the name: '{credential.Name}' was not found, appending to file");

                    var encryptedCredentials = new Credential
                    {
                        Name = credential.Name,
                        Domain = credential.Domain,
                        UserName = credential.UserName,
                        Password = _cipherService.Encrypt(credential.Password),
                    };

                    _settings.Credentials.Add(encryptedCredentials);
                }
                else
                {
                    _logger.LogDebug($"Credential with the name: '{credential.Name}' was found. Updating the file");
                    creds.Domain = credential.Domain;
                    creds.UserName = credential.UserName;
                    creds.Password = _cipherService.Encrypt(credential.Password);
                }

                await _fileManager.WriteSettingsFile(_settings, cancellationToken);
                _logger.LogInformation($"Credential stored");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving credentials");
                throw;
            }
        }

        public async Task<IEnumerable<Application>> GetApplications(CancellationToken cancellationToken = default)
        {
            if (_settings == null)
            {
                await ParseSettings(cancellationToken);
            }

            return _settings?.Applications;
        }

        public async Task<Application> GetApplication(string app, CancellationToken cancellationToken = default)
        {
            if (_settings == null)
            {
                await ParseSettings(cancellationToken);
            }

            return _settings
                ?.Applications
                ?.FirstOrDefault(x => x.Name.Equals(app, StringComparison.OrdinalIgnoreCase))
                ;
        }

        public async Task SaveApplication(Application application, CancellationToken cancellationToken = default)
        {
            if (_settings == null)
            {
                await ParseSettings(cancellationToken);
            }

            try
            {
                _logger.LogInformation($"Saving application...");

                var app = await GetApplication(application.Name);
                if (app == null)
                {
                    _logger.LogDebug($"Application with name: '{application.Name}' was not found, appending to settings");
                    _settings.Applications.Add(application);
                }
                else
                {
                    _logger.LogDebug($"Application with name: '{application.Name}' was found. Updating the file");

                    app.Target = application.Target;
                    app.WorkingDirectory = application.WorkingDirectory;
                    app.IconPath = application.IconPath;
                    app.RunAsAdmin = application.RunAsAdmin;
                }

                await _fileManager.WriteSettingsFile(_settings, cancellationToken);
                _logger.LogDebug($"Application stored");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving application");
                throw;
            }
        }
    }
}
