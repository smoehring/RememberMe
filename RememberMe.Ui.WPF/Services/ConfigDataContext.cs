using System.IO;
using System.Text.Json;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Snoval.Dev.RememberMe.Ui.WPF.Models;

namespace Snoval.Dev.RememberMe.Ui.WPF.Services;

public class ConfigDataContext(ILogger<ConfigDataContext> logger, IConfiguration configuration)
{
    private const string AppName = "snoval.dev.rememberme";
    private string? _configFileName = "config.json";
    private string? _appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
    private JsonSerializerOptions _jsonOptions = new() {WriteIndented = false, IgnoreNullValues = true, AllowTrailingCommas = true, PropertyNameCaseInsensitive = true};
    
    public string ConfigFilePath => Path.Combine(_appdataFolder, _configFileName);
    
    public ConfigModel Config { get; set; } = new();

    private void CheckPrerequisite()
    {
        _appdataFolder = configuration["Files:ConfigFilePath"];
        if (string.IsNullOrWhiteSpace(_appdataFolder))
        {
           _appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
           logger.LogWarning("ConfigFilePath not set, using default path '{path}'", _appdataFolder);
        }
        
        _configFileName = configuration["Files:ConfigFileName"];
        if (string.IsNullOrWhiteSpace(_configFileName))
        {
            _configFileName = "config.json";
            logger.LogWarning("ConfigFileName not set, using default name '{name}'", _configFileName);
        }
        
        if (!Directory.Exists(_appdataFolder))
        {
            logger.LogWarning("Create Appfolder at '{path}'", _appdataFolder);
            Directory.CreateDirectory(_appdataFolder);
        }
        
        if (!File.Exists(ConfigFilePath))
        {
            logger.LogWarning("Create Config File at '{path}'", ConfigFilePath);
            var configString = JsonSerializer.Serialize(Config, _jsonOptions);
            File.WriteAllText(ConfigFilePath, configString);
        }
    }

    /// <summary>
    /// Load the Config File
    /// </summary>
    public void Load()
    {
        CheckPrerequisite();
        logger.LogInformation("Read Config File at '{path}'", ConfigFilePath);
        var configString = File.ReadAllText(ConfigFilePath);
        Config = JsonSerializer.Deserialize<ConfigModel>(configString);
    }

    /// Write the Config File
    public void Write()
    {
        CheckPrerequisite();
        logger.LogInformation("Write Config File at '{path}'", ConfigFilePath);
        var configString = JsonSerializer.Serialize(Config, _jsonOptions);
        File.WriteAllText(ConfigFilePath, configString);
    }

    /// <summary>
    /// Quickly sets the LastContact Date of an Contact
    /// </summary>
    /// <param name="uuid"></param>
    public void UpdateLastContact(Guid uuid)
    {
        Load();
        var contact = Config.Contacts.FirstOrDefault(c => c.Uuid == uuid);
        if (contact == null)
        {
            logger.LogError("Trying to update LastContact of non-existing Contact Guid {guid}", uuid);
            return;
        }
        contact.LastContact = DateTime.Now;
        Write();
    }
}