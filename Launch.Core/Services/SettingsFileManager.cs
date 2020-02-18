using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Launch.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Launch.Core.Services
{
    public class SettingsFileManager : ISettingsFileManager
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        private const string APPLICATION_NAME = "app-launcher";
        private const string SETTINGS_FILE_NAME = "launch-user-settings.json";
        private readonly string _settingsFilePath;

        public SettingsFileManager(
            IConfiguration config,
            ILogger<SettingsFileManager> logger
            )
        {
            _configuration = config;
            _logger = logger;

            _settingsFilePath = GetSettingsFilePath();
        }

        public string GetSettingsFilePath()
        {
            var defaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var configuredSettingsFolder = _configuration.GetValue<string>("SettingsFolderPath", defaultFolder);
            var configuredFileName = _configuration.GetValue<string>("SettingsFileName", $"{APPLICATION_NAME}\\{SETTINGS_FILE_NAME}");

            return $"{configuredSettingsFolder}\\{configuredFileName}";
        }

        public async Task<SettingsModel> ParseSettingsFile(CancellationToken cancellationToken = default)
        {
            if (!File.Exists(_settingsFilePath))
            {
                throw new Exception($"The settings file could not be found at location: {_settingsFilePath}");
            }

            try
            {
                _logger.LogTrace($"Reading settings file at: {_settingsFilePath}");
                string settingsFileContents = await File.ReadAllTextAsync(_settingsFilePath, cancellationToken);
                return JsonConvert.DeserializeObject<SettingsModel>(settingsFileContents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error parsing settings file: {_settingsFilePath}");
                throw ex;
            }
        }

        public async Task WriteSettingsFile(SettingsModel model, CancellationToken cancellationToken = default)
        {
            var serializedModel = JsonConvert.SerializeObject(model, Formatting.Indented);
            await File.WriteAllTextAsync(_settingsFilePath, serializedModel, cancellationToken);
        }
    }
}
